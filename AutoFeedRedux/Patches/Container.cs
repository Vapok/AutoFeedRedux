using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public static class ContainerPatches
{
    [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
    static class ContainerAwakePatch
    {
        static void Postfix(Container __instance)
        {
            if (AutoFeeder.Instance == null)
            {
                return;
            }
            AutoFeeder.Instance.QueueContainer(__instance);
        }
    }

    [HarmonyPatch(typeof(Container), nameof(Container.OnDestroyed))]
    static class ContainerOnDestroyedPatch
    {
        static void Prefix(Container __instance)
        {
            AutoFeeder.Instance.RemoveContainer(__instance);
        }
    }    
}