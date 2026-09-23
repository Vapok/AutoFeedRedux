using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

internal static class ContainerPatches
{
    [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
    static class ContainerAwakePatch
    {
        static void Postfix(Container __instance)
        {
            AutoFeeder.Queue(__instance);
        }
    }

    [HarmonyPatch(typeof(Container), nameof(Container.OnDestroyed))]
    static class ContainerOnDestroyedPatch
    {
        static void Prefix(Container __instance)
        {
            if (AutoFeeder.Instance != null)
            {
                AutoFeeder.Instance.RemoveContainer(__instance);
            }
        }
    }    
}
