using HarmonyLib;

namespace AutoFeedRedux.Patches;

internal static class FejdStartupPatches
{

    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Awake))]
    [HarmonyAfter("org.bepinex.helpers.LocalizationManager")]
    [HarmonyBefore("org.bepinex.helpers.ItemManager")]
    static class FejdStartupAwakePatch
    {
        static void Prefix()
        {
            AutoFeedRedux.Waiter.ValheimIsAwake(true);
        }
    }

}