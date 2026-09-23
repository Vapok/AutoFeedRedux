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
    public IReadOnlyList<Container> AllContainers => _allContainers;
    
    private static readonly Queue<Container> _preInitQueue = new();
    private int _layer;
    private int _foragerLayer;
    private List<Forager> _registry = new();
    private List<Container> _allContainers = new();
    private Queue<Container> _containerQueue = new();

    public static void Queue(Container container)
    {
        if (container == null)
            return;

        if (Instance != null)
        {
            Instance.QueueContainer(container);
        }
        else
        {
            _preInitQueue.Enqueue(container);
        }
    }

    private void Awake()
    {
        Instance = this;
        _layer = LayerMask.GetMask(new string[] { "piece", "piece_nonsolid" });
        _foragerLayer = LayerMask.GetMask(new string[] { "character" });

        while (_preInitQueue.Count > 0)
        {
            Container pending = _preInitQueue.Dequeue();
            if (pending != null && pending)
            {
                _containerQueue.Enqueue(pending);
            }
        }

        InvokeRepeating(nameof(ProcessContainerQueue), 0f, 1f);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(ProcessContainerQueue));
        _allContainers.Clear();
        _preInitQueue.Clear();
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void ProcessContainerQueue()
    {
        bool containerAdded = false;
        while (_containerQueue.Count > 0)
        {
            Container container = _containerQueue.Dequeue();
            if (container != null && container.IsPlayerContainer())
            {
                if (!_allContainers.Contains(container))
                {
                    _allContainers.Add(container);
                    containerAdded = true;
                }
                if (!container.gameObject.TryGetComponent<FeedTrough>(out _))
                {
                    container.gameObject.AddComponent<FeedTrough>();
                }
            }
        }

        if (containerAdded)
        {
            RefillFeedTroughs();
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
            if (!_allContainers.Contains(container))
            {
                _allContainers.Add(container);
            }
            if (!container.gameObject.TryGetComponent<FeedTrough>(out _))
            {
                container.gameObject.AddComponent<FeedTrough>();
                RefillFeedTroughs();
            }
        }
    }
    
    public void RemoveContainer(Container container)
    {
        if (container != null)
        {
            _allContainers.Remove(container);
            if (container.gameObject.TryGetComponent<FeedTrough>(out FeedTrough trough))
            {
                Destroy(trough);
                RefillFeedTroughs();
            }
        }
    }

    private void RefillFeedTroughs()
    {
        for (int i = _registry.Count - 1; i >= 0; i--)
        {
            Forager forager = _registry[i];
            if (forager == null)
            {
                _registry.RemoveAt(i);
                continue;
            }

            forager.UpdateContainers();
        }
    }
}
