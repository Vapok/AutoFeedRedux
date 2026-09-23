using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

internal static class MonsterAIPatches
{
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.UpdateConsumeItem))]
    static class MonsterAIUpdateConsumeItemPatch
    {
        static bool Prefix(MonsterAI __instance, Humanoid humanoid, float dt, ref bool __result)
        {
            if (__instance.m_consumeTarget != null)
                return true;

            if (__instance.TryGetComponent<Forager>(out Forager forager))
            {
                if (forager.UpdateAutoFeed(__instance, humanoid, dt, ref __result))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
