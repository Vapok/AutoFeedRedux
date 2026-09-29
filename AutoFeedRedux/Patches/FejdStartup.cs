using HarmonyLib;

namespace AutoFeedRedux.Patches;

internal static class FejdStartupPatches
{

    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Awake))]
    [HarmonyAfter("vapok.common.LocalizationManager", "org.bepinex.helpers.LocalizationManager")]
    [HarmonyBefore("vapok.common.ItemManager", "org.bepinex.helpers.ItemManager")]
    static class FejdStartupAwakePatch
    {
        static void Prefix()
        {
            AutoFeedRedux.Waiter.ValheimIsAwake(true);
        }
    }

}