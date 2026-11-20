using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CasualtiesNewAscent;

[HarmonyPatch(typeof(MusicManager), nameof(MusicManager.AscentMusic))]
public static class MusicPatches
{
    internal static AudioSource _source;

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

        if (_source != null)
            yield break;

        inst.source.Stop();

        _source = inst.gameObject.AddComponent<AudioSource>();
        _source.bypassReverbZones = true;
        _source.dopplerLevel = 0.0f;
        _source.spatialBlend = 0.0f;
        _source.loop = false;
        _source.playOnAwake = false;
        _source.volume = 100f;
        _source.clip = Plugin.MusicIntro;

        while (true)
        {
            if (WorldGeneration.world.generatingWorld || WorldGeneration.world.instantiatingWorld)
                yield return null;
            else
                break;

            if (!WorldGeneration.world)
            {
                Object.Destroy(_source);
                _source = null;
                yield break;
            }
        }

        _source.PlayDelayed(1f);
        while (true)
        {
            if (_source.isPlaying && WorldGeneration.world && Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth) && !WorldGeneration.world.cutsceneActive && PlayerCamera.main.body.alive)
                yield return null;
            else
                break;
        }

        _source.clip = Plugin.MusicLoop;
        _source.loop = true;
        _source.Play();
        while (true)
        {
            if (WorldGeneration.world && Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth) && !WorldGeneration.world.cutsceneActive && PlayerCamera.main.body.alive)
                yield return null;
            else
                break;
        }
        Object.Destroy(_source);
        _source = null;
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

[HarmonyPatch(typeof(PauseHandler), nameof(PauseHandler.TogglePause))]
public static class QuietDownOnPause
{
    public static void Postfix(PauseHandler __instance)
    {
        if (MusicPatches._source == null)
            return;
        MusicPatches._source.volume = PauseHandler.paused ? 0f : 100f;
    }
}
