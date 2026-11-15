using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace CasualtiesOnOurWayToParadise;

[HarmonyPatch(typeof(EndLabScript), nameof(EndLabScript.ScientistCutscene), MethodType.Enumerator)]
public static class MakeUnchippedEndingAlwaysPossible
{
    private static readonly MethodInfo Getter = AccessTools.PropertyGetter(typeof(WorldGeneration), nameof(WorldGeneration.unchipped));

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var ins in instructions)
        {
            if (ins.Calls(Getter))
            {
                ins.opcode = OpCodes.Ldc_I4_1;
                ins.operand = null;
            }
            yield return ins;
        }
    }
}