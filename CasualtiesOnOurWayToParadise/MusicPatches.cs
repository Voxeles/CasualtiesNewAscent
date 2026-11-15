using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CasualtiesOnOurWayToParadise;

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
        Playing = true;
        inst.source.Stop();

        var source = inst.gameObject.AddComponent<AudioSource>();
        source.bypassReverbZones = true;
        source.dopplerLevel = 0.0f;
        source.spatialBlend = 0.0f;
        source.loop = false;
        source.playOnAwake = false;
        source.volume = 100f;
        source.clip = Plugin.MusicClip;
        source.PlayDelayed(5f);
        while (true)
        {
            if (Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth) && !WorldGeneration.world.cutsceneActive)
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
