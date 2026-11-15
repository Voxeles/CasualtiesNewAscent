using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CasualtiesNewAscent;

[HarmonyPatch(typeof(MusicManager), nameof(MusicManager.AscentMusic))]
public static class MusicPatches
{
    public static bool Playing;

    public static void Postfix(MusicManager __instance, ref IEnumerator __result)
    {
        if (!WorldGeneration.world || !Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth))
            return;
        __result = NewMusicCoro(__instance);
    }

    private static IEnumerator NewMusicCoro(MusicManager inst)
    {
        if (Plugin.MusicIntro == null || Plugin.MusicLoop == null || !WorldGeneration.world)
            yield break;

        Playing = true;
        inst.source.Stop();

        var source = inst.gameObject.AddComponent<AudioSource>();
        source.bypassReverbZones = true;
        source.dopplerLevel = 0.0f;
        source.spatialBlend = 0.0f;
        source.loop = false;
        source.playOnAwake = false;
        source.volume = 100f;
        source.clip = Plugin.MusicIntro;

        while (true)
        {
            if (WorldGeneration.world.generatingWorld || WorldGeneration.world.instantiatingWorld)
                yield return null;
            else
                break;

            if (!WorldGeneration.world)
            {
                Object.Destroy(source);
                Playing = false;
                yield break;
            }
        }

        source.PlayDelayed(1f);
        while (true)
        {
            if (source.isPlaying && WorldGeneration.world && Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth) && !WorldGeneration.world.cutsceneActive)
                yield return null;
            else
                break;
        }

        if (!WorldGeneration.world && !Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth) && WorldGeneration.world.cutsceneActive)
        {
            Object.Destroy(source);
            Playing = false;
            yield break;
        }

        source.clip = Plugin.MusicLoop;
        source.loop = true;
        source.Play();
        while (true)
        {
            if (WorldGeneration.world && Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth) && !WorldGeneration.world.cutsceneActive)
                yield return null;
            else
                break;
        }
        Object.Destroy(source);
        Playing = false;
    }
}

[HarmonyPatch(typeof(MusicManager), nameof(MusicManager.PlayLayerSong))]
public static class DoNotPlayLayerSong
{
    public static bool Prefix(MusicManager __instance)
    {
        var world = WorldGeneration.world;
        if (!world || !Plugin.CheckIfNewLayer(world.biomeDepth))
            return true;
        __instance.timeSinceFinishedPlaying = 0f;
        return false;
    }
}
