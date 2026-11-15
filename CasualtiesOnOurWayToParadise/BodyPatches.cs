using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace CasualtiesOnOurWayToParadise;

[HarmonyPatch(typeof(Body), nameof(Body.Jump))]
public static class BodyPatches
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Body), nameof(Body.hasCargo))),
                new CodeMatch(OpCodes.Brfalse),
                new CodeMatch(OpCodes.Ldloc_0),
                new CodeMatch(OpCodes.Ldc_R4, 0.25f))
            .ThrowIfInvalid($"{nameof(BodyPatches)}.{nameof(Transpiler)} - could not find a match!")
            .Advance(4)
            .RemoveInstruction()
            .Insert(
                new CodeInstruction(OpCodes.Ldc_R4, 2f))
            .InstructionEnumeration();
    }
}

[HarmonyPatch(typeof(Body), nameof(Body.HandleBody))]
public static class MakeCaffeinated
{
    public static void Prefix(Body __instance)
    {
        if (!__instance.hasCargo)
            return;
        __instance.caffeinated = 200f;
        __instance.energy = 100f;
        __instance.stamina = 100f;
        __instance.hunger = Mathf.Max(100f, __instance.hunger);
        __instance.thirst = Mathf.Max(100f, __instance.thirst);
    }
}

[HarmonyPatch(typeof(Limb), nameof(Limb.ImpactDamage))]
public static class LimbImpactDamageReduce
{
    public static void Prefix(Limb __instance, ref float force)
    {
        if (__instance.body.hasCargo)
            force /= 3f;
    }
}