using UnityEngine;

namespace AutoFeedRedux.Extensions;

public static class GameObjectExtensions
{
    public static bool HasComponent<T>(this GameObject go, object search)
    {
        var component = go.GetComponent<T>();
        
        return component != null && component.Equals(search);
    }
}