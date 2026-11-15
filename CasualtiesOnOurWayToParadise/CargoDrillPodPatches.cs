using HarmonyLib;

namespace CasualtiesOnOurWayToParadise;

[HarmonyPatch(typeof(CargoDrillPod), nameof(ConsoleScript.RegisterAllCommands))]
public static class CargoDrillPodPatches
{
    
}