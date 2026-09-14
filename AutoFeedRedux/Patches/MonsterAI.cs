using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public static class MonsterAIPatches
{
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.UpdateConsumeItem))]
    static class MonsterAIUpdateConsumeItemPatch
    {
        static bool Prefix(MonsterAI __instance, Humanoid humanoid, float dt, ref bool __result)
        {
            // If vanilla already has a ground item target, let vanilla handle it
            if (__instance.m_consumeTarget != null)
                return true;

            if (__instance.TryGetComponent<Forager>(out var forager))
            {
                if (forager.UpdateAutoFeed(__instance, humanoid, dt, ref __result))
                {
                    return false; // AutoFeed handled navigation or feeding from container
                }
            }

            return true; // Fall back to vanilla search for dropped items on the ground
        }
    }
}
