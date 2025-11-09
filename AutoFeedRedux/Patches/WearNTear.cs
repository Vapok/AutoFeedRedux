using AutoFeedRedux.Components;
using AutoFeedRedux.Configuration;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public class WearNTearPatches
{
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Damage))]
    static class GameAwakePatch
    {
        static bool Prefix(WearNTear __instance, HitData hit)
        {
            if (!ConfigRegistry.ProtectContainers.Value || !ConfigRegistry.Enabled.Value)
                return true;
            
            if (!__instance.m_nview.IsValid() || hit == null)
                return true;
            
            if (__instance.gameObject.TryGetComponent<FeedTrough>(out var trough))
            {
                var attacker = hit.GetAttacker();
                if (attacker != null && attacker.TryGetComponent<Forager>(out var forager))
                {
                    AutoFeedRedux.Log.Debug($"Protecting Container {trough.gameObject.name} from {forager.gameObject.name}");
                    return false;
                }
            }

            return true;
        }
    }

}