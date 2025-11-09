using System.Collections.Generic;
using AutoFeedRedux.Configuration;
using UnityEngine;

namespace AutoFeedRedux.Components;

public class FeedTrough : MonoBehaviour
{
    private static int _troughCount = 0;
    public AutoFeeder Feeder;
    public List<Forager> Foragers => _nearbyForagers;
    
    private List<Forager> _nearbyForagers;
    private HashSet<Collider> _nearbyColliders;

    private void Awake()
    {
        Feeder = AutoFeeder.Instance;
        _troughCount++;
        AutoFeedRedux.Log.Debug($"Trough Count: {_troughCount}");
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(UpdateContainers), 0f, 60f);
    }
    
    private void OnDestroy()
    {
        _troughCount--;
        AutoFeedRedux.Log.Debug($"Trough Count: {_troughCount}");
    }
    
    public void UpdateContainers()
    {
        _nearbyForagers = GetNearbyForagers(gameObject.transform.position, ConfigRegistry.FeedRange.Value);
        AutoFeedRedux.Log.Debug($"Container {gameObject.name} has {_nearbyForagers.Count} nearby Foragers");
    }
    
    private List<Forager> GetNearbyForagers(Vector3 center, float range)
    {
        var foragers = new List<Forager>();
        var colliders = Physics.OverlapSphere(center, Mathf.Max(range, 0), Feeder.ForagerLayer);
        _nearbyColliders = new HashSet<Collider>();

        foreach (var collider in colliders)
        {
            var container = collider.gameObject.GetComponentInParent<Forager>();
            if (container == null)
            {
                container = collider.gameObject.GetComponent<Forager>();
            }
            
            if (container != null)
            {
                foragers.Add(container);
                _nearbyColliders.Add(collider);
            }
        }

        return foragers;
    }

}
