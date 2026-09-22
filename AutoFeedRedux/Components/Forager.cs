using System;
using System.Collections.Generic;
using System.Linq;
using AutoFeedRedux.Configuration;
using UnityEngine;

namespace AutoFeedRedux.Components;

public class Forager : MonoBehaviour
{
    public AutoFeeder Feeder;
    public Tameable Tame;
    public Humanoid Animal;
    public MonsterAI MonsterAI;

    public Container TargetContainer { get; private set; }
    public ItemDrop TargetFoodItem { get; private set; }

    private List<Container> _nearbyContainers = new();
    private HashSet<Collider> _nearbyColliders;
    private float _searchTimer = 0f;

    private void Awake()
    {
        Feeder = AutoFeeder.Instance;
        Animal = GetComponent<Humanoid>();
        Tame = GetComponent<Tameable>();
        MonsterAI = GetComponent<MonsterAI>();
        _nearbyContainers = new List<Container>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateContainers), 0f, 30f);
    }

    public void UpdateContainers()
    {
        if (gameObject == null || !gameObject)
            return;

        _nearbyContainers = GetNearbyContainers(gameObject.transform.position, ConfigRegistry.FeedRange.Value);
        string tameness = Tame != null ? Tame.GetTameness().ToString() : "N/A";
        AutoFeedRedux.Log.Debug($"Tameable {gameObject.name} with Tameness {tameness} has {_nearbyContainers?.Count ?? 0} nearby Containers");
    }

    private void OnEnable()
    {
        Feeder ??= AutoFeeder.Instance;
        Feeder?.RegisterForager(this);
    }

    private void OnDisable()
    {
        Feeder ??= AutoFeeder.Instance;
        Feeder?.UnregisterForager(this);
        ClearTarget();
    }

    public void ClearTarget()
    {
        TargetContainer = null;
        TargetFoodItem = null;
    }

    public bool UpdateAutoFeed(MonsterAI monsterAI, Humanoid humanoid, float dt, ref bool result)
    {
        if (!ConfigRegistry.Enabled.Value)
            return false;

        if (Tame != null && !Tame.IsHungry())
        {
            ClearTarget();
            return false;
        }

        if (monsterAI.m_consumeItems == null || monsterAI.m_consumeItems.Count == 0)
        {
            ClearTarget();
            return false;
        }

        string[] disallowAnimalList = ConfigRegistry.DisallowAnimal.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (disallowAnimalList.Any(animal => monsterAI.name.StartsWith(animal.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            ClearTarget();
            return false;
        }

        // Validate existing target container
        if (TargetContainer != null)
        {
            if (!IsContainerValidWithFood(TargetContainer, TargetFoodItem, monsterAI))
            {
                ClearTarget();
            }
        }

        // If no active target, search periodically for a container with food
        if (TargetContainer == null)
        {
            _searchTimer += dt;
            if (_searchTimer < monsterAI.m_consumeSearchInterval)
            {
                return false;
            }

            _searchTimer = 0f;

            if (_nearbyContainers == null || _nearbyContainers.Count == 0)
            {
                UpdateContainers();
            }

            if (_nearbyContainers == null || _nearbyContainers.Count == 0)
                return false;

            if (!FindClosestContainerWithFood(monsterAI, out Container closestContainer, out ItemDrop foodItem))
            {
                return false;
            }

            if (!ConfigRegistry.RequireMove.Value)
            {
                // Feed on the spot without moving to chest
                if (ConsumeFromContainer(closestContainer, foodItem, monsterAI, humanoid))
                {
                    result = true;
                    return true;
                }
                return false;
            }

            // Set target container for moving
            TargetContainer = closestContainer;
            TargetFoodItem = foodItem;
            AutoFeedRedux.Log.Debug($"{monsterAI.name} targeting container {TargetContainer.name} to eat {foodItem.name}");
        }

        // Move towards target container and consume when in range
        if (TargetContainer != null)
        {
            Vector3 myPos = Animal != null ? Animal.transform.position : transform.position;
            Vector3 targetPos = TargetContainer.m_piece != null
                ? TargetContainer.m_piece.FindClosestPoint(myPos)
                : TargetContainer.transform.position;

            float proximity = ConfigRegistry.MoveProximity.Value;
            float distance = Vector3.Distance(myPos, targetPos);

            bool reached = monsterAI.MoveTo(dt, targetPos, proximity, false);
            if (reached || distance <= proximity)
            {
                Vector3 lookTarget = TargetContainer.m_piece != null ? TargetContainer.m_piece.GetCenter() : targetPos;
                monsterAI.LookAt(lookTarget);
                if (monsterAI.IsLookingAt(lookTarget, 35f))
                {
                    monsterAI.StopMoving();
                    ConsumeFromContainer(TargetContainer, TargetFoodItem, monsterAI, humanoid);
                    ClearTarget();
                }
            }

            result = true;
            return true;
        }

        return false;
    }

    private bool IsContainerValidWithFood(Container container, ItemDrop foodItem, MonsterAI monsterAI)
    {
        if (container == null || !container || container.GetInventory() == null)
            return false;

        Vector3 myPos = Animal != null ? Animal.transform.position : transform.position;
        if (Vector3.Distance(myPos, container.transform.position) > ConfigRegistry.FeedRange.Value * 1.5f)
            return false;

        if (foodItem == null || foodItem.m_itemData == null || foodItem.m_itemData.m_shared == null)
            return false;

        return container.GetInventory().ContainsItemByName(foodItem.m_itemData.m_shared.m_name);
    }

    private bool FindClosestContainerWithFood(MonsterAI monsterAI, out Container bestContainer, out ItemDrop bestFood)
    {
        bestContainer = null;
        bestFood = null;
        float shortestDist = float.MaxValue;
        Vector3 myPos = Animal != null ? Animal.transform.position : transform.position;

        string[] disallowFoodList = ConfigRegistry.DisallowFeed.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (Container container in _nearbyContainers)
        {
            if (container == null || container.GetInventory() == null)
                continue;

            Inventory inv = container.GetInventory();
            foreach (ItemDrop food in monsterAI.m_consumeItems)
            {
                if (food == null || food.m_itemData == null || food.m_itemData.m_shared == null)
                    continue;

                if (disallowFoodList.Any(x => x.Trim().Equals(food.name, StringComparison.OrdinalIgnoreCase)))
                    continue;

                if (inv.ContainsItemByName(food.m_itemData.m_shared.m_name))
                {
                    float dist = Vector3.Distance(myPos, container.transform.position);
                    if (dist < shortestDist)
                    {
                        shortestDist = dist;
                        bestContainer = container;
                        bestFood = food;
                    }
                    break;
                }
            }
        }

        return bestContainer != null;
    }

    private bool ConsumeFromContainer(Container container, ItemDrop foodItem, MonsterAI monsterAI, Humanoid humanoid)
    {
        if (container == null || container.GetInventory() == null || foodItem == null || foodItem.m_itemData?.m_shared == null)
            return false;

        Inventory inventory = container.GetInventory();
        ItemDrop.ItemData invItem = inventory.GetItem(foodItem.m_itemData.m_shared.m_name);
        if (invItem != null && inventory.RemoveOneItem(invItem))
        {
            AutoFeedRedux.Log.Debug($"{monsterAI?.name ?? "Creature"} consumed {foodItem.name} from {container.name}");

            try
            {
                monsterAI?.m_onConsumedItem?.Invoke(foodItem);
            }
            catch (Exception ex)
            {
                AutoFeedRedux.Log.Warning($"Exception invoking m_onConsumedItem for {monsterAI?.name ?? "Creature"}: {ex.Message}");
            }

            try
            {
                if (humanoid != null && humanoid.m_consumeItemEffects != null)
                {
                    humanoid.m_consumeItemEffects.Create(transform.position, Quaternion.identity, null, 1f, -1, default(ZDOID));
                }
            }
            catch (Exception ex)
            {
                AutoFeedRedux.Log.Warning($"Exception playing consumeItemEffects: {ex.Message}");
            }

            try
            {
                monsterAI?.m_animator?.SetTrigger("consume");
            }
            catch (Exception ex)
            {
                AutoFeedRedux.Log.Warning($"Exception triggering consume animation: {ex.Message}");
            }

            return true;
        }

        return false;
    }

    private List<Container> GetNearbyContainers(Vector3 center, float range)
    {
        List<Container> containers = new();
        Feeder ??= AutoFeeder.Instance;
        if (Feeder == null)
            return containers;

        Collider[] colliders = Physics.OverlapSphere(center, Mathf.Max(range, 0), Feeder.ContainerLayer);
        _nearbyColliders = new HashSet<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider == null || !collider || collider.gameObject == null)
                continue;

            Container container = collider.gameObject.GetComponentInParent<Container>();
            if (container != null && !containers.Contains(container))
            {
                containers.Add(container);
                _nearbyColliders.Add(collider);
            }
        }

        return containers;
    }
}
