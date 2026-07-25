using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
	public static SaveLoadManager saveLoadManager;

	private int t_saveFile;

	private bool uploadSaveToAndroidCloudNextFrame;

	private bool uploadSaveToiCloudNextFrame;

	private int readAttemps;

	private bool deletingAll;

	private int deleteAllIndex;

	public List<string> unlockedMinigames;

	private bool[] unlockedTracks;

	private bool[] enabledTracks;

	private int[] trackPieces;

	private string[] recentCinematics;

	private void Awake()
	{
		if (saveLoadManager == null)
		{
			saveLoadManager = this;
		}
	}

	private void Update()
	{
		if (uploadSaveToAndroidCloudNextFrame)
		{
			UploadSaveToAndroidCloud(t_saveFile);
		}
		if (uploadSaveToiCloudNextFrame)
		{
			UploadSaveToAndroidCloud(t_saveFile);
		}
		if (deletingAll && !PlayCloudDataManager.Instance.isProcessing)
		{
			PlayCloudDataManager.Instance.Save(deleteAllIndex, string.Empty);
			deleteAllIndex++;
			if (deleteAllIndex > 9)
			{
				deletingAll = false;
				SceneChangeController.sceneChangeController.RestartGame();
			}
		}
	}

	public void Save(int saveFile)
	{
		UpdateTimesSaved();
		SaveData saveData = new SaveData();
		GlobalControl globalControl = GlobalControl.globalControl;
		saveData.profileNumber = globalControl.profile;
		saveData.gameLevel = globalControl.gameLevel.GetActualLevel();
		saveData.storyLevel = globalControl.storyLevel;
		saveData.pachinkoDesign = globalControl.pachinkoDesign;
		saveData.pachinkoHole = FloatArrayToString(globalControl.pachinkoHole);
		saveData.pckLightsLevel = globalControl.pckLightsLevel;
		saveData.marbles = globalControl.marbles;
		saveData.goldenMarbles = globalControl.goldenMarbles;
		if (saveData.marbles > 999)
		{
			saveData.marbles = 999;
		}
		saveData.jackpots = globalControl.jackpots;
		saveData.playtime = globalControl.playtime;
		saveData.MIX_scores = IntArrayToString(globalControl.MIXscores);
		saveData.MAX_scores = IntArrayToString(globalControl.MAXscores);
		if (globalControl.goldenIndicator)
		{
			saveData.goldenIndicator = 1;
		}
		else
		{
			saveData.goldenIndicator = 0;
		}
		saveData.unlockedMinigames = StringListToString(globalControl.unlockedMinigames);
		saveData.unlockedTracks = BoolArrayToString(globalControl.unlockedTracks);
		saveData.enabledTracks = BoolArrayToString(globalControl.enabledTracks);
		saveData.trackPieces = IntArrayToString(globalControl.trackPieces);
		saveData.recentCinematics = globalControl.recentCinematics[0] + "," + globalControl.recentCinematics[1];
		foreach (string minigames in globalControl.minigamesList)
		{
			saveData[minigames + "_skins"] = SkinsArrayToString(globalControl.skinsDictionary[minigames]);
			saveData[minigames + "_pieces"] = globalControl.minigamesPieces[minigames];
			saveData[minigames + "_scores"] = SkinsArrayToString(globalControl.minigamesScores[minigames]);
			saveData[minigames + "_enabledSkins"] = BoolArrayToString(globalControl.enabledSkins[minigames]);
			saveData[minigames + "_enabledMinigame"] = BoolToInt(globalControl.enabledMinigames[minigames]);
		}
		saveData._111 = BoolToInt(globalControl._111);
		saveData._222 = BoolToInt(globalControl._222);
		saveData._333 = BoolToInt(globalControl._333);
		saveData._444 = BoolToInt(globalControl._444);
		saveData._555 = BoolToInt(globalControl._555);
		saveData._666 = BoolToInt(globalControl._666);
		saveData._777 = BoolToInt(globalControl._777);
		saveData._888 = BoolToInt(globalControl._888);
		saveData._999 = BoolToInt(globalControl._999);
		saveData.playedMinigames = StringListToString(globalControl.playedMinigames);
		SaveLoad.saveLoad.WriteData(saveData, saveFile);
		UploadSaveToAndroidCloud(saveFile);
		UploadSaveToiCloud(saveFile);
	}

	private void UploadSaveToAndroidCloud(int saveFile)
	{
	}

	public void UpdateDefaultSaveAndroidCloud(int defaultProfile)
	{
	}

	private void UploadSaveToiCloud(int saveFile)
	{
	}

	public void UpdateDefaultSaveiCloud(int defaultProfile)
	{
	}

	private void UpdateTimesSaved()
	{
		if (GlobalControl.globalControl.timesSaved >= 3599940)
		{
			GlobalControl.globalControl.timesSaved = 0;
		}
		else
		{
			GlobalControl.globalControl.timesSaved++;
		}
		DefaultSave defaultSave = new DefaultSave();
		defaultSave.defaultSave = GlobalControl.globalControl.profile;
		defaultSave.timesSaved = GlobalControl.globalControl.timesSaved;
		SaveLoad.saveLoad.WriteDefaultSave(defaultSave);
	}

	public void Load(int saveFile)
	{
		SaveData saveData = SaveLoad.saveLoad.Load(saveFile);
		if (saveData == null)
		{
			saveData = ResetSave(saveFile);
		}
		ParseLoadedData(saveData, saveFile);
		DefaultSave defaultSave = new DefaultSave();
		defaultSave.defaultSave = saveFile;
		defaultSave.timesSaved = GlobalControl.globalControl.timesSaved;
		SaveLoad.saveLoad.WriteDefaultSave(defaultSave);
	}

	public void ReplaceDefaultSave(int profile, int timesSaved)
	{
		DefaultSave defaultSave = new DefaultSave();
		defaultSave.defaultSave = profile;
		defaultSave.timesSaved = timesSaved;
		SaveLoad.saveLoad.WriteDefaultSave(defaultSave);
	}

	public void LoadDefaultSave()
	{
		DefaultSave defaultSave = SaveLoad.saveLoad.LoadDefaultSave();
		if (defaultSave == null)
		{
			if (PlayerPrefs.GetInt("profile") != 0)
			{
				int num = PlayerPrefs.GetInt("profile");
				if (num == 0)
				{
					num = 1;
				}
				PlayerPrefs.DeleteKey("profile");
				GlobalControl.globalControl.profile = num;
				GlobalControl.globalControl.timesSaved = 0;
			}
			else
			{
				GlobalControl.globalControl.profile = 1;
				GlobalControl.globalControl.timesSaved = 0;
			}
		}
		else
		{
			if (defaultSave.defaultSave != 0)
			{
				GlobalControl.globalControl.profile = defaultSave.defaultSave;
			}
			else
			{
				GlobalControl.globalControl.profile = 1;
			}
			GlobalControl.globalControl.timesSaved = defaultSave.timesSaved;
		}
	}

	private void ParseLoadedData(SaveData data, int saveFile)
	{
		data = CheckDataIntegrity(data, saveFile);
		GlobalControl globalControl = GlobalControl.globalControl;
		globalControl.profile = data.profileNumber;
		globalControl.gameLevel.SetLevel(data.gameLevel);
		globalControl.storyLevel = data.storyLevel;
		globalControl.pachinkoDesign = data.pachinkoDesign;
		globalControl.pachinkoHole = StringToFloatArray(data.pachinkoHole);
		globalControl.pckLightsLevel = data.pckLightsLevel;
		globalControl.marbles = data.marbles;
		globalControl.goldenMarbles = data.goldenMarbles;
		globalControl.jackpots = data.jackpots;
		globalControl.playtime = data.playtime;
		globalControl.MIXscores = StringToIntArray(data.MIX_scores);
		globalControl.MAXscores = StringToIntArray(data.MAX_scores);
		recentCinematics = data.recentCinematics.Split(',');
		if (recentCinematics.Length == 2)
		{
			bool flag = int.TryParse(recentCinematics[0], out globalControl.recentCinematics[0]);
			flag = int.TryParse(recentCinematics[1], out globalControl.recentCinematics[1]);
		}
		if (data.goldenIndicator > 0)
		{
			globalControl.goldenIndicator = true;
		}
		else
		{
			globalControl.goldenIndicator = false;
		}
		unlockedMinigames = LoadStringList(data.unlockedMinigames);
		globalControl.unlockedMinigames = new List<string>();
		for (int i = 0; i < globalControl.minigamesList.Count; i++)
		{
			if (unlockedMinigames.Contains(globalControl.minigamesList[i]))
			{
				globalControl.unlockedMinigames.Add(globalControl.minigamesList[i]);
			}
		}
		if (globalControl.tracks.Length == data.unlockedTracks.Length)
		{
			globalControl.unlockedTracks = StringToBoolArray(data.unlockedTracks);
		}
		else
		{
			globalControl.unlockedTracks = new bool[globalControl.tracks.Length];
			unlockedTracks = StringToBoolArray(data.unlockedTracks);
			for (int j = 0; j < globalControl.unlockedTracks.Length; j++)
			{
				if (unlockedTracks.Length > j)
				{
					globalControl.unlockedTracks[j] = unlockedTracks[j];
				}
				else
				{
					globalControl.unlockedTracks[j] = false;
				}
			}
		}
		if (globalControl.tracks.Length == data.enabledTracks.Length)
		{
			globalControl.enabledTracks = StringToBoolArray(data.enabledTracks);
		}
		else
		{
			globalControl.enabledTracks = new bool[globalControl.tracks.Length];
			enabledTracks = StringToBoolArray(data.enabledTracks);
			for (int k = 0; k < globalControl.enabledTracks.Length; k++)
			{
				if (enabledTracks.Length > k)
				{
					globalControl.enabledTracks[k] = enabledTracks[k];
				}
				else
				{
					globalControl.enabledTracks[k] = false;
				}
			}
		}
		if (globalControl.tracks.Length == data.trackPieces.Length)
		{
			globalControl.trackPieces = StringToIntArray(data.trackPieces);
		}
		else
		{
			globalControl.trackPieces = new int[globalControl.tracks.Length];
			trackPieces = StringToIntArray(data.trackPieces);
			for (int l = 0; l < globalControl.trackPieces.Length; l++)
			{
				if (globalControl.unlockedTracks[l])
				{
					globalControl.trackPieces[l] = 2;
				}
				else if (trackPieces.Length > l)
				{
					globalControl.trackPieces[l] = trackPieces[l];
				}
				else
				{
					globalControl.trackPieces[l] = 0;
				}
			}
		}
		globalControl.enabledMinigames = new Dictionary<string, bool>();
		globalControl.enabledSkins = new Dictionary<string, bool[]>();
		foreach (string minigames in globalControl.minigamesList)
		{
			globalControl.skinsDictionary[minigames] = SkinsStringToArray((string)data[minigames + "_skins"]);
			globalControl.minigamesScores[minigames] = SkinsStringToArray((string)data[minigames + "_scores"]);
			globalControl.minigamesPieces[minigames] = (int)data[minigames + "_pieces"];
			if ((int)data[minigames + "_enabledMinigame"] == 1)
			{
				globalControl.enabledMinigames.Add(minigames, true);
			}
			else
			{
				globalControl.enabledMinigames.Add(minigames, false);
			}
			globalControl.enabledSkins.Add(minigames, StringToBoolArray((string)data[minigames + "_enabledSkins"]));
		}
		bool flag2 = false;
		flag2 = ((data._111 > 0) ? true : false);
		globalControl._111 = flag2;
		flag2 = ((data._222 > 0) ? true : false);
		globalControl._222 = flag2;
		flag2 = ((data._333 > 0) ? true : false);
		globalControl._333 = flag2;
		flag2 = ((data._444 > 0) ? true : false);
		globalControl._444 = flag2;
		flag2 = ((data._555 > 0) ? true : false);
		globalControl._555 = flag2;
		flag2 = ((data._666 > 0) ? true : false);
		globalControl._666 = flag2;
		flag2 = ((data._777 > 0) ? true : false);
		globalControl._777 = flag2;
		flag2 = ((data._888 > 0) ? true : false);
		globalControl._888 = flag2;
		flag2 = ((data._999 > 0) ? true : false);
		globalControl._999 = flag2;
		globalControl.playedMinigames = LoadStringList(data.playedMinigames);
	}

	private SaveData CheckDataIntegrity(SaveData data, int saveFile)
	{
		GlobalControl globalControl = GlobalControl.globalControl;
		SaveData saveData = new SaveData();
		if (globalControl.minigamesList == null)
		{
			globalControl.InitlializeLists();
		}
		saveData.profileNumber = data.profileNumber;
		saveData.gameLevel = data.gameLevel;
		saveData.storyLevel = data.storyLevel;
		saveData.pachinkoDesign = data.pachinkoDesign;
		saveData.pachinkoHole = data.pachinkoHole;
		saveData.pckLightsLevel = data.pckLightsLevel;
		saveData.marbles = data.marbles;
		saveData.goldenMarbles = data.goldenMarbles;
		saveData.goldenIndicator = data.goldenIndicator;
		saveData.jackpots = data.jackpots;
		saveData.unlockedMinigames = data.unlockedMinigames;
		saveData.unlockedTracks = data.unlockedTracks;
		saveData.playtime = data.playtime;
		saveData.enabledTracks = data.enabledTracks;
		saveData.trackPieces = data.trackPieces;
		saveData.recentCinematics = "0,0";
		if (data.recentCinematics != string.Empty && data.recentCinematics != null)
		{
			saveData.recentCinematics = data.recentCinematics;
		}
		saveData.playedMinigames = data.playedMinigames;
		saveData.MIX_scores = data.MIX_scores;
		saveData.MAX_scores = data.MAX_scores;
		saveData._111 = data._111;
		saveData._222 = data._222;
		saveData._333 = data._333;
		saveData._444 = data._444;
		saveData._555 = data._555;
		saveData._666 = data._666;
		saveData._777 = data._777;
		saveData._888 = data._888;
		saveData._999 = data._999;
		if (data.profileNumber == 0)
		{
			saveData.profileNumber = saveFile;
		}
		if (data.pachinkoDesign == 0)
		{
			saveData.pachinkoDesign = 1;
		}
		if (data.pachinkoHole == null)
		{
			saveData.pachinkoHole = FloatArrayToString(globalControl.pachinkoHole);
		}
		if (data.unlockedMinigames == null)
		{
			globalControl.UnlockInitialMinigames();
			saveData.unlockedMinigames = StringListToString(globalControl.unlockedMinigames);
		}
		foreach (string minigames in globalControl.minigamesList)
		{
			if (data[minigames + "_skins"] == null)
			{
				saveData[minigames + "_skins"] = SkinsArrayToString(globalControl.skinsDictionary[minigames]);
			}
			else
			{
				saveData[minigames + "_skins"] = data[minigames + "_skins"];
			}
			if (data[minigames + "_scores"] == null)
			{
				saveData[minigames + "_scores"] = SkinsArrayToString(globalControl.minigamesScores[minigames]);
			}
			else
			{
				saveData[minigames + "_scores"] = data[minigames + "_scores"];
			}
			if (data[minigames + "_pieces"] == null)
			{
				saveData[minigames + "_pieces"] = globalControl.minigamesPieces[minigames];
			}
			else
			{
				saveData[minigames + "_pieces"] = data[minigames + "_pieces"];
			}
			if (data[minigames + "_enabledMinigame"] == null)
			{
				saveData[minigames + "_enabledMinigame"] = 1;
			}
			else
			{
				saveData[minigames + "_enabledMinigame"] = data[minigames + "_enabledMinigame"];
			}
			if (data[minigames + "_enabledSkins"] == null)
			{
				saveData[minigames + "_enabledSkins"] = BoolArrayToString(globalControl.enabledSkins[minigames]);
			}
			else
			{
				saveData[minigames + "_enabledSkins"] = data[minigames + "_enabledSkins"];
			}
		}
		if (data.unlockedTracks == null)
		{
			saveData.unlockedTracks = BoolArrayToString(globalControl.unlockedTracks);
		}
		if (data.enabledTracks == null)
		{
			saveData.enabledTracks = BoolArrayToString(globalControl.enabledTracks);
		}
		if (data.trackPieces == null)
		{
			saveData.trackPieces = IntArrayToString(globalControl.trackPieces);
		}
		if (data.MIX_scores == null)
		{
			saveData.MIX_scores = IntArrayToString(globalControl.MIXscores);
		}
		if (data.MAX_scores == null)
		{
			saveData.MAX_scores = IntArrayToString(globalControl.MAXscores);
		}
		if (data.playedMinigames == null)
		{
			saveData.playedMinigames = string.Empty;
		}
		return saveData;
	}

	public SaveData ResetSave(int saveFile)
	{
		GlobalControl globalControl = GlobalControl.globalControl;
		SaveData saveData = new SaveData();
		saveData.profileNumber = saveFile;
		saveData.playtime = 0;
		saveData.gameLevel = 0;
		saveData.storyLevel = 0;
		saveData.pachinkoDesign = 1;
		saveData.pachinkoHole = "0,-3";
		saveData.pckLightsLevel = 1;
		saveData.marbles = 0;
		saveData.goldenMarbles = 0;
		saveData.goldenIndicator = 0;
		saveData.jackpots = 0;
		globalControl.ResetUnlocks();
		saveData.unlockedMinigames = StringListToString(globalControl.unlockedMinigames);
		saveData.unlockedTracks = BoolArrayToString(globalControl.unlockedTracks);
		saveData.enabledTracks = BoolArrayToString(globalControl.enabledTracks);
		saveData.trackPieces = IntArrayToString(globalControl.trackPieces);
		saveData.recentCinematics = "0,0";
		foreach (string minigames in globalControl.minigamesList)
		{
			saveData[minigames + "_skins"] = SkinsArrayToString(globalControl.skinsDictionary[minigames]);
			saveData[minigames + "_scores"] = SkinsArrayToString(globalControl.minigamesScores[minigames]);
			saveData[minigames + "_pieces"] = globalControl.minigamesPieces[minigames];
			saveData[minigames + "_enabledSkins"] = BoolArrayToString(globalControl.enabledSkins[minigames]);
			saveData[minigames + "_enabledMinigame"] = BoolToInt(globalControl.enabledMinigames[minigames]);
		}
		saveData.MIX_scores = IntArrayToString(globalControl.MIXscores);
		saveData.MAX_scores = IntArrayToString(globalControl.MAXscores);
		saveData._111 = 0;
		saveData._222 = 0;
		saveData._333 = 0;
		saveData._444 = 0;
		saveData._555 = 0;
		saveData._666 = 0;
		saveData._777 = 0;
		saveData._888 = 0;
		saveData._999 = 0;
		saveData.playedMinigames = string.Empty;
		SaveLoad.saveLoad.WriteData(saveData, saveFile);
		UploadSaveToAndroidCloud(saveFile);
		UploadSaveToiCloud(saveFile);
		return saveData;
	}

	public void DeleteAll()
	{
		SaveLoad.saveLoad.DeleteAll();
		deleteAllIndex = 0;
		SceneChangeController.sceneChangeController.RestartGame();
	}

	private string StringListToString(List<string> list)
	{
		string text = string.Empty;
		foreach (string item in list)
		{
			text = ((!(text == string.Empty)) ? (text + "," + item) : (text + item));
		}
		return text;
	}

	private string IntListToString(List<int> list)
	{
		string text = string.Empty;
		foreach (int item in list)
		{
			text = ((!(text == string.Empty)) ? (text + "," + item) : (text + item));
		}
		return text;
	}

	private List<string> LoadStringList(string line)
	{
		List<string> list = new List<string>();
		string[] array = line.Split(new string[1] { "," }, StringSplitOptions.None);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text != string.Empty && text != null)
			{
				list.Add(text);
			}
		}
		return list;
	}

	private List<int> LoadIntList(string line)
	{
		List<int> list = new List<int>();
		string[] array = line.Split(new string[1] { "," }, StringSplitOptions.None);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text != string.Empty && text != null)
			{
				int item = int.Parse(text);
				list.Add(item);
			}
		}
		return list;
	}

	private int[] StringToIntArray(string line)
	{
		if (line == string.Empty)
		{
			return null;
		}
		string[] array = line.Split(new string[1] { "," }, StringSplitOptions.None);
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = int.Parse(array[i]);
		}
		return array2;
	}

	private string IntArrayToString(int[] array)
	{
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text = ((!(text == string.Empty)) ? (text + "," + array[i]) : (text + array[i]));
		}
		return text;
	}

	private float[] StringToFloatArray(string line)
	{
		if (line == string.Empty)
		{
			return null;
		}
		string[] array = line.Split(new string[1] { "," }, StringSplitOptions.None);
		float[] array2 = new float[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = float.Parse(array[i]);
		}
		return array2;
	}

	private string FloatArrayToString(float[] array)
	{
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text = ((!(text == string.Empty)) ? (text + "," + array[i]) : (text + array[i]));
		}
		return text;
	}

	private string SkinsArrayToString(int[] skins)
	{
		string text = string.Empty;
		foreach (int num in skins)
		{
			text = text + num + "_";
		}
		return text.Substring(0, text.Length - 1);
	}

	private int[] SkinsStringToArray(string s)
	{
		string[] array = s.Split("_"[0]);
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = int.Parse(array[i]);
		}
		return array2;
	}

	private string BoolArrayToString(bool[] b)
	{
		string text = string.Empty;
		for (int i = 0; i < b.Length; i++)
		{
			text = ((!b[i]) ? (text + "0_") : (text + "1_"));
		}
		return text.Substring(0, text.Length - 1);
	}

	private bool[] StringToBoolArray(string s)
	{
		string[] array = s.Split("_"[0]);
		bool[] array2 = new bool[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == "0")
			{
				array2[i] = false;
			}
			else
			{
				array2[i] = true;
			}
		}
		return array2;
	}

	private int BoolToInt(bool b)
	{
		if (!b)
		{
			return 0;
		}
		return 1;
	}
}
