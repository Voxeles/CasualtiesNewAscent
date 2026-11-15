using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace CasualtiesOnOurWayToParadise;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
	public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
	public const string ModName = MyPluginInfo.PLUGIN_NAME;
	public const string ModVersion = MyPluginInfo.PLUGIN_VERSION;

	internal new static ManualLogSource Logger = null!;
	private readonly Harmony _harmony = new(ModGuid);
	public static Plugin Instance { get; private set; } = null!;

	public static AudioClip MusicClip;

	public void Awake()
	{
		Logger = base.Logger;
		Instance = this;

		StartCoroutine(LoadAudio());

		_harmony.PatchAll();

		Logger.LogInfo($"Plugin {ModName} is loaded!");
	}

	public static bool CheckIfAscent(int biomeDepth)
	{
		return biomeDepth is 10 or 13 or 14 or 15;
	}

	public static bool CheckIfLastLayer(int biomeDepth)
	{
		return biomeDepth is 10 or 15;
	}

	public static bool CheckIfNewLayer(int biomeDepth)
	{
		return biomeDepth is 13 or 14 or 15;
	}

	private static IEnumerator LoadAudio()
	{
		using var stream = typeof(Plugin).Assembly.GetManifestResourceStream($"{ModName}.assets.mus.bin");
		if (stream == null)
		{
			Logger.LogError($"Embedded audio missing!");
			yield break;
		}

		string path = Path.Combine(Path.GetTempPath(), $"{ModName}-music.ogg");
		try
		{
			using (var output = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
			using (var gz = new GZipStream(stream, CompressionMode.Decompress))
				gz.CopyTo(output);

			var p = new Uri(path).AbsoluteUri;

			using var request = UnityWebRequestMultimedia.GetAudioClip(p, AudioType.OGGVORBIS);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Logger.LogError($"Could not load audio file!");
				yield break;
			}

			MusicClip = DownloadHandlerAudioClip.GetContent(request);
		}
		finally
		{
			try
			{
				File.Delete(path);
			}
			catch (Exception)
			{
				// ignored
			}
		}
	}

	/*
	private static void CompressAudio()
	{
		using var stream = typeof(Plugin).Assembly.GetManifestResourceStream($"{ModName}.assets.ext.ogg");
		if (stream == null)
		{
			Logger.LogError($"Embedded audio missing!");
			return;
		}

		string path = Path.Combine(Path.GetTempPath(), $"{ModName}-music-compressed.bin");
		using (var output = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
		using (var gz = new GZipStream(output, CompressionLevel.Optimal))
		{
			stream.CopyTo(gz);
		}

		Plugin.Logger.LogMessage("Done.");
	}
	*/
}
