using UnityEngine;

namespace AutoFeedRedux.Components;

public class FeedTrough : MonoBehaviour
{
    private static int _troughCount = 0;
    private Container _container;

    private void Awake()
    {
        _container = GetComponent<Container>();
        _troughCount++;
        AutoFeedRedux.Log.Debug($"Trough Count: {_troughCount}");
    }

    private void OnDestroy()
    {
        _troughCount--;
        AutoFeedRedux.Log.Debug($"Trough Count: {_troughCount}");
        if (AutoFeeder.Instance != null && _container != null)
        {
            AutoFeeder.Instance.RemoveContainer(_container);
        }
    }
}
