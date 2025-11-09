using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public static class GamePatches
{
    [HarmonyPatch(typeof(Game), nameof(Game.Awake))]
    static class GameAwakePatch
    {
        static void Postfix(Game __instance)
        {
            __instance.gameObject.AddComponent<AutoFeeder>();
        }
    }
}