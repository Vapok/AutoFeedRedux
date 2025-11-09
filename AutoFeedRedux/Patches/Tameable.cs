using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public static class TameablePatches
{
    [HarmonyPatch(typeof(Tameable), nameof(Tameable.Awake))]
    static class ContainerAwakePatch
    {
        static void Postfix(Tameable __instance)
        {
            var trough = __instance.gameObject.AddComponent<Forager>();
            trough.Tame = __instance;
            trough.Animal = __instance.gameObject.GetComponent<Humanoid>();
        }
    }
}