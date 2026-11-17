using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CasualtiesNewAscent;

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.Awake))]
public static class NewBiomesSetup
{
    // biomeProfiles
    // biomeTitles (size is 13 for some reason? The rest are 12), automatically filled
    // temperatureCurves
    // backgroundDrones
    // biomeProfileNoise -- based on biomeProfiles
    // layerGenerationImages
    // layerGenerationCharColors

    public static void Postfix(WorldGeneration __instance)
    {
        // New strings generated automatically by WorldGeneration.Start
        Array.Resize(ref __instance.biomeTitles, 17);

        Array.Resize(ref __instance.temperatureCurves, 17);
        __instance.temperatureCurves[12] = __instance.temperatureCurves[11];
        __instance.temperatureCurves[13] = __instance.temperatureCurves[10];
        __instance.temperatureCurves[14] = __instance.temperatureCurves[10];
        __instance.temperatureCurves[15] = __instance.temperatureCurves[10];
        __instance.temperatureCurves[16] = __instance.temperatureCurves[10];

        Array.Resize(ref __instance.backgroundDrones, 17);
        __instance.backgroundDrones[12] = __instance.backgroundDrones[11];
        __instance.backgroundDrones[13] = new AudioClip();
        __instance.backgroundDrones[14] = new AudioClip();
        __instance.backgroundDrones[15] = new AudioClip();
        __instance.backgroundDrones[16] = new AudioClip();

        Array.Resize(ref __instance.biomeProfiles, 17);
        __instance.biomeProfiles[12] = __instance.biomeProfiles[11];
        __instance.biomeProfiles[13] = __instance.biomeProfiles[10];
        __instance.biomeProfiles[14] = __instance.biomeProfiles[10];
        __instance.biomeProfiles[15] = __instance.biomeProfiles[10];
        __instance.biomeProfiles[16] = __instance.biomeProfiles[10];

        Array.Resize(ref __instance.layerGenerationImages, 17);
        __instance.layerGenerationImages[12] = __instance.layerGenerationImages[11];
        __instance.layerGenerationImages[13] = __instance.layerGenerationImages[8];
        __instance.layerGenerationImages[14] = __instance.layerGenerationImages[5];
        __instance.layerGenerationImages[15] = __instance.layerGenerationImages[2];
        __instance.layerGenerationImages[16] = __instance.layerGenerationImages[10];

        Array.Resize(ref __instance.layerGenerationCharColors, 17);
        __instance.layerGenerationCharColors[12] = __instance.layerGenerationCharColors[11];
        __instance.layerGenerationCharColors[13] = __instance.layerGenerationCharColors[8];
        __instance.layerGenerationCharColors[14] = __instance.layerGenerationCharColors[5];
        __instance.layerGenerationCharColors[15] = __instance.layerGenerationCharColors[2];
        __instance.layerGenerationCharColors[16] = __instance.layerGenerationCharColors[10];
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.RegenerateWorld))]
public static class NewRegenAfterCargoDrillPodUsed
{
    public static bool Prefix(WorldGeneration __instance, ref IEnumerator __result)
    {
        if (__instance.biomeDepth < 13)
        {
            var cargoDrill = Object.FindAnyObjectByType<CargoDrillPod>();
            if (cargoDrill is null || !cargoDrill.started)
                return true;
        }

        __result = RegenerateWorldNew(__instance);
        return false;
    }

    private static IEnumerator RegenerateWorldNew(WorldGeneration inst)
    {
        inst.doingRegen = true;
        inst.canStartMercRun = false;
        GlobalDark.main.Darken();
        yield return new WaitUntil(() => !GlobalDark.main.IsDarkening());

        if (inst.biomeDepth < 13)
        {
            inst.biomeDepth = 13;
            inst.totalTraveled = (int) (inst.height * 0.3f * 3.5f);
            inst.lootRarityMultiplier = 0;
            inst.trapRarityMultiplier = 1;
        }
        else if (inst.biomeDepth < 16)
        {
            inst.biomeDepth++;
            inst.totalTraveled -= (int) (inst.height * 0.3f);
            inst.lootRarityMultiplier *= WorldGeneration.GetRunSettingFloat("lootmultiplier");
            inst.trapRarityMultiplier += WorldGeneration.GetRunSettingFloat("trapincrease");
        }
        else
        {
            inst.biomeDepth = 11;
        }

        yield return inst.Clear();

        inst.doingRegen = false;
        yield return inst.InstantiateWorld(true);
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.InstantiateWorld), MethodType.Enumerator)]
public static class RemoveBiomeDepthClamping
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldfld),
                new CodeMatch(OpCodes.Ldc_I4_1),
                new CodeMatch(OpCodes.Sub),
                new CodeMatch(OpCodes.Stfld))
            .ThrowIfInvalid($"{nameof(RemoveBiomeDepthClamping)}.{nameof(Transpiler)} - could not find a match!")
            .RemoveInstructions(6)
            .InstructionEnumeration();
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.Update))]
public static class HandleAscentEndLayer
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(WorldGeneration),nameof(WorldGeneration.biomeDepth))),
                new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)10),
                new CodeMatch(OpCodes.Beq))
            .ThrowIfInvalid($"{nameof(HandleAscentEndLayer)}.{nameof(Transpiler)} - could not find a match (1)!")
            .Advance(2)
            .RemoveInstruction()
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Plugin), nameof(Plugin.CheckIfAscent))),
                new CodeInstruction(OpCodes.Ldc_I4_1))
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(WorldGeneration),nameof(WorldGeneration.biomeDepth))),
                new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)10),
                new CodeMatch(OpCodes.Bne_Un))
            .ThrowIfInvalid($"{nameof(HandleAscentEndLayer)}.{nameof(Transpiler)} - could not find a match (2)!")
            .Advance(2)
            .RemoveInstruction()
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Plugin), nameof(Plugin.CheckIfLastLayer))),
                new CodeInstruction(OpCodes.Ldc_I4_1))
            .InstructionEnumeration();
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> ChangeEndingDepth(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Call, AccessTools.PropertyGetter(typeof(WorldGeneration), nameof(WorldGeneration.body))),
                new CodeMatch(OpCodes.Callvirt),
                new CodeMatch(OpCodes.Callvirt),
                new CodeMatch(OpCodes.Ldfld), // body.transform.position.y
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Call, AccessTools.PropertyGetter(typeof(WorldGeneration), nameof(WorldGeneration.halfHeight))),
                new CodeMatch(OpCodes.Conv_R_Un),
                new CodeMatch(OpCodes.Conv_R4),
                new CodeMatch(OpCodes.Ldc_R4, 5f),
                new CodeMatch(OpCodes.Sub),
                new CodeMatch(OpCodes.Cgt),
                new CodeMatch(OpCodes.Brfalse))
            .ThrowIfInvalid($"{nameof(HandleAscentEndLayer)}.{nameof(ChangeEndingDepth)} - could not find a match!")
            .Advance(5)
            .RemoveInstructions(6)
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HandleAscentEndLayer), nameof(NewAscentEndingDepth))))
            .InstructionEnumeration();
    }

    private static float NewAscentEndingDepth()
    {
        return WorldGeneration.world.biomeDepth == 16 ? 0f : WorldGeneration.world.halfHeight - 5f;
    }

    public static void Postfix()
    {
        if (!Plugin.CheckIfAscent(WorldGeneration.world.biomeDepth))
            return;

        var world = WorldGeneration.world;
        if (!world.savePanel.activeSelf)
            return;

        world.body.forceWalk = false;
        world.savePanel.SetActive(false);
        world.StartCoroutine(world.RegenerateWorld());
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.SurfaceBreachCutscene))]
public static class HealPlayerAtEnd
{
    public static void Postfix()
    {
        PlayerCamera.main.body.Heal(true);
    }
}

[HarmonyPatch(typeof(Body), nameof(Body.PlaceBody))]
public static class HandleAscentPlacePlayer
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(WorldGeneration),nameof(WorldGeneration.world))),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(WorldGeneration),nameof(WorldGeneration.biomeDepth))),
                new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)10),
                new CodeMatch(OpCodes.Ceq))
            .ThrowIfInvalid($"{nameof(HandleAscentPlacePlayer)}.{nameof(Transpiler)} - could not find a match!")
            .Advance(2)
            .RemoveInstructions(2)
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Plugin), nameof(Plugin.CheckIfAscent))))
            .InstructionEnumeration();
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.InstantiateWorld), MethodType.Enumerator)]
public static class OverrideChunkSize
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(WorldGeneration),nameof(WorldGeneration.chunkWidth))),
                new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(WorldGeneration),nameof(WorldGeneration.CHUNKSIZE))),
                new CodeMatch(OpCodes.Mul))
            .ThrowIfInvalid($"{nameof(OverrideChunkSize)}.{nameof(Transpiler)} - could not find a match!")
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(OverrideChunkSize), nameof(Override))))
            .InstructionEnumeration();
    }

    private static uint baseChunkWidth = 0;
    private static uint baseChunkHeight = 0;

    private static void Override()
    {
        var world = WorldGeneration.world;
        if (baseChunkWidth == 0)
        {
            baseChunkWidth = world.chunkWidth;
            baseChunkHeight = world.chunkHeight;
        }

        if (Plugin.CheckIfNewLayer(world.biomeDepth))
        {
            world.chunkWidth = 5;
        }
        else
        {
            world.chunkWidth = baseChunkWidth;
            world.chunkHeight = baseChunkHeight;
        }
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.FinishWorldGeneration), MethodType.Enumerator)]
public static class HideAscentLayerNumberInAlert
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(WorldGeneration), nameof(WorldGeneration.biomeDepth))),
                new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)11),
                new CodeMatch(OpCodes.Bne_Un),
                new CodeMatch(OpCodes.Ldstr),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Locale), nameof(Locale.GetOther))))
            .ThrowIfInvalid($"{nameof(HideAscentLayerNumberInAlert)}.{nameof(Transpiler)} - could not find a match!")
            .Advance(2)
            .RemoveInstruction()
            .InsertAndAdvance(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HideAscentLayerNumberInAlert), nameof(Check))),
                new CodeInstruction(OpCodes.Ldc_I4_1))
            .Advance(1)
            .RemoveInstructions(2)
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HideAscentLayerNumberInAlert), nameof(GetLocalizedTitleString))))
            .InstructionEnumeration();
    }

    private static bool Check(int biomeDepth)
    {
        return biomeDepth == 11 || Plugin.CheckIfNewLayer(biomeDepth);
    }

    private static string GetLocalizedTitleString()
    {
        return Locale.GetOther($"layertitle{WorldGeneration.world.biomeDepth + 1}");
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.FinishWorldGeneration))]
public static class FinishWorldGenerationPatch
{
    public static void Postfix(WorldGeneration __instance, ref IEnumerator __result)
    {
        if (!Plugin.CheckIfNewLayer(__instance.biomeDepth))
        {
            if (ArrowGuideTriple.Child1 != null)
                ArrowGuideTriple.DestroyTriple();
            return;
        }
        __result = Coro(__instance, __result);
    }

    private static IEnumerator Coro(WorldGeneration inst, IEnumerator original)
    {
        while (original.MoveNext())
            yield return original.Current;

        inst.skyColor = new Color32(250, 201, 77, byte.MaxValue);
        inst.skyMaterial.SetColor("_TopColor", inst.skyColor);
        inst.skyMaterial.SetFloat("_RainIntensity", 0.0f);
        inst.crimsonParticle.SetActive(true);
        MusicManager.main.cutsceneVolMult = 1f;
        MusicManager.main.StartCoroutine(MusicManager.main.AscentMusic());
        if (!Plugin.CheckIfLastLayer(inst.biomeDepth))
        {
            if (ArrowGuideTriple.Child1 == null)
                ArrowGuideTriple.MakeTriple();
        }
        else
        {
            // Last layer doesn't have triple arrows
            if (ArrowGuideTriple.Child1 != null)
                ArrowGuideTriple.DestroyTriple();
            WorldGeneration.world.gameObject.AddComponent<SkyBg>();
        }
        if (AscentTimer.Instance == null)
            WorldGeneration.world.gameObject.AddComponent<AscentTimer>();
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.ApplyLayerModifiers))]
public static class DisableLayerModifiers
{
    public static bool Prefix()
    {
        // Old ascent can have layer modifiers
        if (Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth))
            return false;
        return true;
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.InstantiateWorld))]
public static class AddNewBackgrounds
{
    public static void Postfix(WorldGeneration __instance, ref IEnumerator __result)
    {
        if (!Plugin.CheckIfNewLayer(__instance.biomeDepth))
            return;
        __result = AddNewBackgroundsCoro(__instance, __result);
    }

    private static IEnumerator AddNewBackgroundsCoro(WorldGeneration inst, IEnumerator original)
    {
        while (original.MoveNext())
            yield return original.Current;

        if (inst.biomeDepth == 13)
            NewWorldgen.CreateBackground3(inst);
        else if (inst.biomeDepth == 14)
            NewWorldgen.CreateBackground2(inst);
        else if (inst.biomeDepth == 15)
            NewWorldgen.CreateBackground1(inst);
        else if (inst.biomeDepth == 16)
            NewWorldgen.CreateBackground0(inst);
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.WorldGenerateTerrain))]
public static class AddNewTerrain
{
    public static void Postfix(WorldGeneration __instance, ref IEnumerator __result)
    {
        if (!Plugin.CheckIfNewLayer(__instance.biomeDepth))
            return;
        __result = AddNewTerrainCoro(__instance, __result);
    }

    private static IEnumerator AddNewTerrainCoro(WorldGeneration inst, IEnumerator original)
    {
        while (original.MoveNext())
            yield return original.Current;

        if (inst.biomeDepth == 13)
        {
            yield return NewWorldgen.GenerateBiome8(inst);
            yield return NewWorldgen.GenerateBiome7(inst);
            yield return NewWorldgen.GenerateBiome6(inst);
            inst.UpdateWorld();
            yield return null;

            NewWorldgen.FinishBiome8(inst);
            yield return null;
            NewWorldgen.FinishBiome7(inst);
            yield return null;
            NewWorldgen.FinishBiome6(inst);
            yield return null;

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.EntitiesBiome8(inst);
            yield return null;
            NewWorldgen.EntitiesBiome7(inst);
            yield return null;
            NewWorldgen.EntitiesBiome6(inst);
            yield return null;
        }
        else if (inst.biomeDepth == 14)
        {
            yield return NewWorldgen.GenerateBiome5(inst);
            yield return NewWorldgen.GenerateBiome4(inst);
            yield return NewWorldgen.GenerateBiome3(inst);

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.FinishBiome5(inst);
            yield return null;
            NewWorldgen.FinishBiome4(inst);
            yield return null;
            NewWorldgen.FinishBiome3(inst);
            yield return null;

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.EntitiesBiome5(inst);
            yield return null;
            NewWorldgen.EntitiesBiome4(inst);
            yield return null;
            NewWorldgen.EntitiesBiome3(inst);
            yield return null;
        }
        else if (inst.biomeDepth == 15)
        {
            yield return NewWorldgen.GenerateBiome2(inst);
            yield return NewWorldgen.GenerateBiome1(inst);
            yield return NewWorldgen.GenerateBiome0(inst);

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.FinishBiome2(inst);
            yield return null;
            NewWorldgen.FinishBiome1(inst);
            yield return null;
            NewWorldgen.FinishBiome0(inst);
            yield return null;

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.EntitiesBiome2(inst);
            yield return null;
            NewWorldgen.EntitiesBiome1(inst);
            yield return null;
            NewWorldgen.EntitiesBiome0(inst);
            yield return null;
        }
        else if (inst.biomeDepth == 16)
        {
            yield return NewWorldgen.GenerateBiome10(inst);

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.FinishBiome10(inst);
            yield return null;

            inst.UpdateWorld();
            yield return null;

            NewWorldgen.EntitiesBiome10(inst);
            yield return null;
        }
    }
}
