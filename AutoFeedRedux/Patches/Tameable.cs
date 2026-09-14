using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public static class TameablePatches
{
    [HarmonyPatch(typeof(Tameable), nameof(Tameable.Awake))]
    static class TameableAwakePatch
    {
        static void Postfix(Tameable __instance)
        {
            if (!__instance.gameObject.TryGetComponent<Forager>(out var forager))
            {
                forager = __instance.gameObject.AddComponent<Forager>();
            }
            forager.Tame = __instance;
            forager.Animal = __instance.gameObject.GetComponent<Humanoid>();
            forager.MonsterAI = __instance.gameObject.GetComponent<MonsterAI>();
        }
    }
}
