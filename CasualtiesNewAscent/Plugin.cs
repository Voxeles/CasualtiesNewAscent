using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace CasualtiesNewAscent;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
	public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
	public const string ModName = MyPluginInfo.PLUGIN_NAME;
	public const string ModVersion = MyPluginInfo.PLUGIN_VERSION;

	internal new static ManualLogSource Logger = null!;
	private readonly Harmony _harmony = new(ModGuid);
	public static Plugin Instance { get; private set; } = null!;

	public static AudioClip MusicIntro;
	public static AudioClip MusicLoop;

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
		return biomeDepth is 10 or 13 or 14 or 15 or 16;
	}

	public static bool CheckIfLastLayer(int biomeDepth)
	{
		return biomeDepth is 10 or 16;
	}

	public static bool CheckIfNewLayer(int biomeDepth)
	{
		return biomeDepth is 13 or 14 or 15 or 16;
	}

	private static IEnumerator LoadAudio()
	{
		yield return LoadAudioClip("mus-intro.ogg", clip => MusicIntro = clip);
		yield return LoadAudioClip("mus-loop.ogg", clip => MusicLoop = clip);
	}

	private static IEnumerator LoadAudioClip(string assetName, Action<AudioClip> onLoaded)
	{
		using var stream = typeof(Plugin).Assembly.GetManifestResourceStream($"{ModName}.assets.{assetName}");
		if (stream == null)
		{
			Logger.LogError($"Embedded audio missing!");
			yield break;
		}

		string path = Path.Combine(Path.GetTempPath(), $"{ModName}-{assetName}");
		try
		{
			using (var output = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
				stream.CopyTo(output);

			using var request = UnityWebRequestMultimedia.GetAudioClip(new Uri(path).AbsoluteUri, AudioType.OGGVORBIS);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Logger.LogError($"Could not load audio file!");
				yield break;
			}

			AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
			onLoaded(clip);
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
}
