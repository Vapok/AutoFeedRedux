using System;
using System.Collections.Generic;
using AutoFeedRedux.Configuration;
using AutoFeedRedux.Extensions;
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
        int containerCount = _nearbyContainers != null ? _nearbyContainers.Count : 0;
        AutoFeedRedux.Log.Debug($"Tameable {gameObject.name} with Tameness {tameness} has {containerCount} nearby Containers");
    }

    private void OnEnable()
    {
        if (Feeder == null)
        {
            Feeder = AutoFeeder.Instance;
        }
        if (Feeder != null)
        {
            Feeder.RegisterForager(this);
        }
    }

    private void OnDisable()
    {
        if (Feeder == null)
        {
            Feeder = AutoFeeder.Instance;
        }
        if (Feeder != null)
        {
            Feeder.UnregisterForager(this);
        }
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

        if (monsterAI == null || monsterAI.m_consumeItems == null || monsterAI.m_consumeItems.Count == 0)
        {
            ClearTarget();
            return false;
        }

        HashSet<string> disallowedAnimals = ConfigRegistry.DisallowedAnimals;
        if (disallowedAnimals.Count > 0)
        {
            string creatureName = monsterAI.name;
            foreach (string animal in disallowedAnimals)
            {
                if (creatureName.StartsWith(animal, StringComparison.OrdinalIgnoreCase))
                {
                    ClearTarget();
                    return false;
                }
            }
        }

        if (TargetContainer != null)
        {
            if (!IsContainerValidWithFood(TargetContainer, TargetFoodItem, monsterAI))
            {
                AutoFeedRedux.Log.Debug($"{monsterAI.name} container {TargetContainer.name} no longer has food, resetting target");
                ClearTarget();
            }
        }

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
            {
                AutoFeedRedux.Log.Debug($"{monsterAI.name} is hungry, but 0 nearby containers found within {ConfigRegistry.FeedRange.Value}m");
                return false;
            }

            if (!FindClosestContainerWithFood(monsterAI, out Container closestContainer, out ItemDrop foodItem))
            {
                AutoFeedRedux.Log.Debug($"{monsterAI.name} found {_nearbyContainers.Count} containers, but none contain consumable food");
                return false;
            }

            if (!ConfigRegistry.RequireMove.Value)
            {
                if (ConsumeFromContainer(closestContainer, foodItem, monsterAI, humanoid))
                {
                    result = true;
                    return true;
                }
                return false;
            }

            TargetContainer = closestContainer;
            TargetFoodItem = foodItem;
            AutoFeedRedux.Log.Debug($"{monsterAI.name} targeting container {TargetContainer.name} to eat {foodItem.name}");
        }

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
                if (distance <= proximity * 0.8f || monsterAI.IsLookingAt(lookTarget, 60f))
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
        if (container == null || !container || container.IsInUse() || container.GetInventory() == null)
            return false;

        Vector3 myPos = Animal != null ? Animal.transform.position : transform.position;
        if (Vector3.Distance(myPos, container.transform.position) > ConfigRegistry.FeedRange.Value * 1.5f)
            return false;

        if (foodItem == null || !foodItem || foodItem.m_itemData == null || foodItem.m_itemData.m_shared == null)
            return false;

        return container.GetInventory().ContainsItemByName(foodItem.m_itemData.m_shared.m_name);
    }

    private bool FindClosestContainerWithFood(MonsterAI monsterAI, out Container bestContainer, out ItemDrop bestFood)
    {
        bestContainer = null;
        bestFood = null;
        float shortestDist = float.MaxValue;
        Vector3 myPos = Animal != null ? Animal.transform.position : transform.position;

        HashSet<string> disallowedFoods = ConfigRegistry.DisallowedFoods;

        for (int i = 0; i < _nearbyContainers.Count; i++)
        {
            Container container = _nearbyContainers[i];
            if (container == null || !container || container.IsInUse())
                continue;

            Inventory inv = container.GetInventory();
            if (inv == null)
                continue;

            for (int j = 0; j < monsterAI.m_consumeItems.Count; j++)
            {
                ItemDrop food = monsterAI.m_consumeItems[j];
                if (food == null || !food || food.m_itemData == null || food.m_itemData.m_shared == null)
                    continue;

                if (disallowedFoods.Count > 0 && disallowedFoods.Contains(food.name))
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
        if (container == null || !container || container.IsInUse())
            return false;

        if (foodItem == null || !foodItem || foodItem.m_itemData == null || foodItem.m_itemData.m_shared == null)
            return false;

        Inventory inventory = container.GetInventory();
        if (inventory == null)
            return false;

        ItemDrop.ItemData invItem = inventory.GetItem(foodItem.m_itemData.m_shared.m_name);
        if (invItem == null)
            return false;

        if (container.m_nview != null && container.m_nview.IsValid() && !container.m_nview.IsOwner())
        {
            container.m_nview.ClaimOwnership();
        }

        if (!inventory.RemoveOneItem(invItem))
            return false;

        string creatureName = monsterAI != null ? monsterAI.name : "Creature";
        string containerName = container.name;
        AutoFeedRedux.Log.Debug($"{creatureName} consumed {foodItem.name} from {containerName}");

        bool invokedConsumed = false;
        if (monsterAI != null && monsterAI.m_onConsumedItem != null)
        {
            try
            {
                monsterAI.m_onConsumedItem.Invoke(foodItem);
                invokedConsumed = true;
            }
            catch (Exception ex)
            {
                AutoFeedRedux.Log.Warning($"Exception invoking m_onConsumedItem for {creatureName}: {ex.Message}");
            }
        }

        if (!invokedConsumed && Tame != null)
        {
            Tame.ResetFeedingTimer();
        }

        if (!Jotunn.Managers.GUIManager.IsHeadless())
        {
            if (humanoid != null && humanoid.m_consumeItemEffects != null)
            {
                try
                {
                    humanoid.m_consumeItemEffects.Create(transform.position, Quaternion.identity, null, 1f, -1, default(ZDOID));
                }
                catch (Exception ex)
                {
                    AutoFeedRedux.Log.Warning($"Exception playing consumeItemEffects: {ex.Message}");
                }
            }

            if (monsterAI != null && monsterAI.m_animator != null)
            {
                try
                {
                    monsterAI.m_animator.SetTrigger("consume");
                }
                catch (Exception ex)
                {
                    AutoFeedRedux.Log.Warning($"Exception triggering consume animation: {ex.Message}");
                }
            }
        }

        return true;
    }

    private List<Container> GetNearbyContainers(Vector3 center, float range)
    {
        List<Container> containers = new();
        if (Feeder == null)
        {
            Feeder = AutoFeeder.Instance;
        }
        if (Feeder == null)
            return containers;

        IReadOnlyList<Container> allContainers = Feeder.AllContainers;
        if (allContainers != null)
        {
            float maxDistSq = range * range;
            for (int i = 0; i < allContainers.Count; i++)
            {
                Container c = allContainers[i];
                if (c == null || !c)
                    continue;

                Vector3 pos = c.transform.position;
                if ((pos - center).sqrMagnitude <= maxDistSq)
                {
                    containers.Add(c);
                }
            }
        }

        return containers;
    }
}
