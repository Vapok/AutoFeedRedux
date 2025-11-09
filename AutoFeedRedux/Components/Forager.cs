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

    private MoveInfo moveInfo = null;
    private List<Container> _nearbyContainers;
    private HashSet<Collider> _nearbyColliders;
    private void Awake()
    {
        Feeder = AutoFeeder.Instance;
        _nearbyContainers = new List<Container>();
        
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateContainers), 0f, 60f);
    }

    public void UpdateContainers()
    {
        _nearbyContainers = GetNearbyContainers(gameObject.transform.position, ConfigRegistry.FeedRange.Value);
        AutoFeedRedux.Log.Debug($"Tameable {gameObject.name} with Tameness {Tame.GetTameness()} has {_nearbyContainers.Count} nearby Containers");
    }
    private void OnEnable()
    {
        Feeder.RegisterForager(this);
    }

    private void OnDisable()
    {
        Feeder.UnregisterForager(this);
    }

    private void MoveToTrough()
    {
        if (moveInfo == null)
        {
            CancelInvoke(nameof(MoveToTrough));
            return;
        }
        var success = moveInfo.MonsterAI.MoveTo(0.05f, moveInfo.ClosestPoint, 0.0f, Vector3.Distance(moveInfo.ClosestPoint, moveInfo.Position) > 10f );
        if (success)
        {
            moveInfo.MonsterAI.StopMoving();
            CancelInvoke(nameof(MoveToTrough));
        }
    }
    public ItemDrop FeedFromContainers(MonsterAI monsterAI)
    {
        if (_nearbyContainers.Count == 0)
            return monsterAI.m_consumeTarget;

        var disallowList = ConfigRegistry.DisallowAnimal.Value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).ToList();
        if (disallowList.Any(animal => monsterAI.name.ToUpper().StartsWith(animal.ToUpper())))
        {
            AutoFeedRedux.Log.Debug($"Skipping Feed on {monsterAI.m_character.name} due to disallow list");
            return monsterAI.m_consumeTarget;
        }
        
        foreach (var container in _nearbyContainers)
        {
            var inventory = container.GetInventory();
            if (inventory == null)
                continue;
            foreach (var food in monsterAI.m_consumeItems)
            {
                if (ConfigRegistry.DisallowFeed.Value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).ToList().Any(x => x.ToUpper().Equals(food.name.ToUpper())))
                {
                    AutoFeedRedux.Log.Debug($"Skipping Feed of {food.name} due to disallow food list");
                    continue;
                }
                    
                
                if (!inventory.ContainsItemByName(food.m_itemData.m_shared.m_name))
                    continue;

                if (ConfigRegistry.RequireMove.Value)
                {

                    var position = Animal.transform.position;
                    var closestPoint = container.m_piece.FindClosestPoint(position);
                    var itemData = monsterAI.SelectBestAttack(Animal,0.05f);
                    if (itemData == null)
                        goto doexit;
                    AutoFeedRedux.Log.Debug($"Moving to Feed Trough - Distance: {Vector3.Distance(closestPoint, position)}");                    
                    if (Vector3.Distance(closestPoint, position) < ConfigRegistry.MoveProximity.Value && monsterAI.CanSeeTarget(container.m_piece))
                    {
                        monsterAI.LookAt(container.m_piece.GetCenter());
                        if (monsterAI.IsLookingTowards(position, itemData.m_shared.m_aiAttackMaxAngle))
                        {
                            goto feed;
                        }
                        
                        monsterAI.StopMoving();
                    }
                    else
                    {
                        moveInfo = new MoveInfo()
                            { ClosestPoint = closestPoint, MonsterAI = monsterAI, Position = position };
                        if (!IsInvoking(nameof(MoveToTrough)))
                            InvokeRepeating(nameof(MoveToTrough),0f,2f);
                    }
                    goto doexit;
                }
                
                feed:
                if (IsInvoking(nameof(MoveToTrough)))
                    CancelInvoke(nameof(MoveToTrough));
                
                var invItem = inventory.GetItem(food.m_itemData.m_shared.m_name);

                if (invItem != null && food.m_itemData != null && Animal != null && Animal.transform != null && inventory.RemoveOneItem(invItem))
                {
                    monsterAI.m_consumeTarget = ItemDrop.DropItem(invItem, 1, Animal.transform.position,Animal.transform.rotation);
                    AutoFeedRedux.Log.Debug($"Found {food.name} in Container {container.name}");
                }
                else
                {
                    AutoFeedRedux.Log.Debug($"invItem != null: {invItem != null}");
                    AutoFeedRedux.Log.Debug($"Food: {food.m_itemData.m_shared.m_name}");
                    AutoFeedRedux.Log.Debug($"Food Not Null {food.m_itemData != null}");
                    AutoFeedRedux.Log.Debug($"Animal Not Null {Animal != null}");
                    AutoFeedRedux.Log.Debug($"Animal.transform Not Null {Animal.transform != null}");
                    
                }
                doexit:
                return monsterAI.m_consumeTarget;
            }
        }

        return monsterAI.m_consumeTarget;
    }

    private List<Container> GetNearbyContainers(Vector3 center, float range)
    {
        var containers = new List<Container>();
        var colliders = Physics.OverlapSphere(center, Mathf.Max(range, 0), Feeder.ContainerLayer);
        _nearbyColliders = new HashSet<Collider>();

        foreach (var collider in colliders)
        {
            var container = collider.gameObject.GetComponentInParent<Container>();
            if (container != null)
            {
                containers.Add(container);
                _nearbyColliders.Add(collider);
            }
        }

        return containers;
    }
}
public class MoveInfo
{
    public MonsterAI MonsterAI;
    public Vector3 ClosestPoint;
    public Vector3 Position;
}