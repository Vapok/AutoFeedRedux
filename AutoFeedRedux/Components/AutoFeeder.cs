using System;
using System.Collections.Generic;
using AutoFeedRedux.Configuration;
using AutoFeedRedux.Extensions;
using UnityEngine;

namespace AutoFeedRedux.Components;

public class AutoFeeder : MonoBehaviour
{
    public static AutoFeeder Instance { get; private set; }
    
    public int ContainerLayer => _layer;
    public int ForagerLayer => _foragerLayer;
    public List<Forager> ForagerRegistry => _registry;
    
    private int _layer;
    private int _foragerLayer;
    private List<Forager> _registry = new();
    private Queue<Container> _containerQueue = new();

    private void Awake()
    {
        Instance = this;
        _layer = LayerMask.GetMask(new string[] { "piece" });
        _foragerLayer = LayerMask.GetMask(new string[] { "character" });
        InvokeRepeating(nameof(ProcessContainerQueue), 0f, 1f);
    }

    private void ProcessContainerQueue()
    {
        while (_containerQueue.Count > 0)
        {
            AddContainer(_containerQueue.Dequeue());
        }
    }
    
    public void RegisterForager(Forager forager)
    {
        if (forager != null && !_registry.Contains(forager))
        {
            _registry.Add(forager);
            AutoFeedRedux.Log.Debug($"Add Forager Registry: {_registry.Count}");
        }
    }
    
    public void UnregisterForager(Forager forager)
    {
        if (forager != null)
        {
            _registry.Remove(forager);
            AutoFeedRedux.Log.Debug($"Remove Forager Registry: {_registry.Count}");
        }
    }

    public void QueueContainer(Container container)
    {
        if (container != null)
        {
            _containerQueue.Enqueue(container);
        }
    }
    
    public void AddContainer(Container container)
    {
        if (container != null && container.IsPlayerContainer())
        {
            if (!container.gameObject.TryGetComponent<FeedTrough>(out _))
            {
                container.gameObject.AddComponent<FeedTrough>();
            }
            RefillFeedTroughs();
        }
    }
    
    public void RemoveContainer(Container container)
    {
        if (container != null && container.IsPlayerContainer())
        {
            if (container.gameObject.TryGetComponent<FeedTrough>(out var trough))
            {
                Destroy(trough);
            }
            RefillFeedTroughs();
        }
    }

    private void RefillFeedTroughs()
    {
        foreach (var forager in ForagerRegistry)
        {
            if (forager != null)
            {
                forager.UpdateContainers();
            }
        }
    }
}
