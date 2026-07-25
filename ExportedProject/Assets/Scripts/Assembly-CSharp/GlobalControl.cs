using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
	public static GameObject control;

	public static GlobalControl globalControl;

	public string version;

	public bool testmode;

	public bool mobile;

	public bool steam;

	private float time;

	public int totalPuzzles;

	public Dictionary<string, bool> puzzles;

	public GameLevel gameLevel;

	public GameLevel temporalGameLevel;

	public int storyLevel;

	public int pckLightsLevel;

	public string mode;

	public int pachinkoDesign;

	public float[] pachinkoHole;

	public int rank;

	public float slotsProgress;

	public int profile;

	public int playtime;

	public int timesSaved;

	public bool playing;

	public bool golden;

	public List<string> playedMinigames;

	public int survivalSelection;

	public int marbles;

	public int goldenMarbles;

	public bool goldenIndicator;

	public int alternateMarbles;

	public int remainingPrizes;

	public int jackpots;

	public bool backFromMinigames;

	public List<string> minigamesList;

	public List<string> unlockedMinigames;

	public List<string> justUnlockedMinigames;

	public Dictionary<string, int> minigamesPieces;

	public Dictionary<string, int[]> minigamesScores;

	public Dictionary<string, bool[]> enabledSkins;

	public Dictionary<string, bool> enabledMinigames;

	public int[] MIXscores;

	public int[] MAXscores;

	public AudioClip[] tracks;

	public bool[] unlockedTracks;

	public bool[] enabledTracks;

	public int[] trackPieces;

	public bool usingKeyboard;

	public int changeMMUselectionTo;

	public bool firstPCKMusicPlayed;

	private bool pauseWhenTransitionEnds;

	public Dictionary<string, int[]> skinsDictionary;

	public Dictionary<string, int> lastUnlockedSkins;

	private int[] defaultSkins;

	public float minigamesResetTime;

	private bool pauseAudioNextFrame;

	public bool initialDataLoaded;

	public bool _111;

	public bool _222;

	public bool _333;

	public bool _444;

	public bool _555;

	public bool _666;

	public bool _777;

	public bool _888;

	public bool _999;

	public GameObject steamManagerPrefab;

	public int[] recentCinematics = new int[2];

	private void Awake()
	{
		if (control == null)
		{
			control = base.gameObject;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (globalControl != base.gameObject)
		{
			Object.Destroy(base.gameObject);
		}
		InitlializeLists();
	}

	private void Start()
	{
		globalControl = this;
		mobile = false;
		testmode = false;
		InitializeDifficultyLevel();
		if (steam)
		{
			Object.Instantiate(steamManagerPrefab);
			UnlockAchievements();
		}
	}

	private void Update()
	{
		if (pauseAudioNextFrame)
		{
			pauseAudioNextFrame = false;
			GlobalAudio.globalAudio.PauseAll();
		}
		if (pauseWhenTransitionEnds)
		{
			PauseWhenTransitionEnds();
		}
	}

	private void EnsureDataLoadedFromAnyScene()
	{
		if (!initialDataLoaded)
		{
			InitialLoadData();
		}
	}

	public void CountPlayTime()
	{
		if (playtime < 3599940)
		{
			time += Time.deltaTime;
			if (time > 1f)
			{
				time -= 1f;
				playtime++;
			}
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		ManageFocus(hasFocus);
	}

	public void ManageFocus(bool hasFocus)
	{
		if (!(PauseController.pauseController == null))
		{
			if (!hasFocus && !PauseController.pauseController.paused && SceneChangeController.sceneChangeController.pausableScene && !SceneChangeController.sceneChangeController.transitioning)
			{
				SaveData();
				PauseController.pauseController.TogglePause();
			}
			else if (!hasFocus && SceneChangeController.sceneChangeController.transitioning)
			{
				pauseWhenTransitionEnds = true;
			}
			else if (hasFocus && PauseController.pauseController.paused && !SceneChangeController.sceneChangeController.transitioning)
			{
				pauseAudioNextFrame = true;
			}
			if (PauseController.pauseController.invokeCall == "TogglePause" || PauseController.pauseController.invokeCall == "Unpause")
			{
				PauseController.pauseController.ForcePause();
			}
			if (hasFocus)
			{
				ScreenResolutionController.screenResolutionController.updateResolutionNextFrame = true;
			}
		}
	}

	private void PauseWhenTransitionEnds()
	{
		if (!SceneChangeController.sceneChangeController.transitioning && SceneChangeController.sceneChangeController.pausableScene && !PauseController.pauseController.paused)
		{
			pauseWhenTransitionEnds = false;
			PauseController.pauseController.TogglePause();
		}
		else if (!SceneChangeController.sceneChangeController.pausableScene)
		{
			pauseWhenTransitionEnds = false;
		}
	}

	public void InitialLoadData()
	{
		if (SaveLoad.saveLoad.IsInitialized())
		{
			SaveLoadManager.saveLoadManager.LoadDefaultSave();
			if (profile == 0)
			{
				profile = 1;
				PlayerPrefs.SetInt("profile", 1);
			}
			LoadData();
		}
		else
		{
			Invoke("InitialLoadData", 0.1f);
		}
	}

	public void LoadData()
	{
		ResetAchievements();
		justUnlockedMinigames = new List<string>();
		SaveLoadManager.saveLoadManager.Load(profile);
		DataChecks();
	}

	public void SaveData()
	{
		SavingIconsController.savingIconsController.ShowAndHideSaving();
		SaveLoadManager.saveLoadManager.Save(profile);
	}

	public void ResetData()
	{
		ResetAchievements();
		justUnlockedMinigames = new List<string>();
		SavingIconsController.savingIconsController.ShowAndHideSaving();
		SaveLoadManager.saveLoadManager.ResetSave(profile);
	}

	private void DataChecks()
	{
		if (storyLevel == 0)
		{
			marbles = 10;
		}
		if (goldenMarbles > 0 || storyLevel > 1)
		{
			golden = true;
		}
		else
		{
			golden = false;
		}
		AchievementsChecks();
		initialDataLoaded = true;
	}

	private void ResetAchievements()
	{
		_111 = false;
		_222 = false;
		_333 = false;
		_444 = false;
		_555 = false;
		_666 = false;
		_777 = false;
		_888 = false;
		_999 = false;
	}

	private void AchievementsChecks()
	{
		if (jackpots > 0)
		{
			_111 = true;
		}
		bool flag = true;
		for (int i = 0; i < minigamesList.Count; i++)
		{
			if (minigamesScores[minigamesList[i]][0] > 99)
			{
				_444 = true;
			}
			else
			{
				flag = false;
			}
			if (_444)
			{
				continue;
			}
			for (int j = 0; j < 9; j++)
			{
				if (skinsDictionary[minigamesList[i]][j] > 8)
				{
					_444 = true;
					break;
				}
			}
		}
		if (MIXscores[0] > 99 || MAXscores[0] > 99)
		{
			_444 = true;
		}
		if (marbles >= 999)
		{
			_555 = true;
		}
		if (storyLevel >= 2)
		{
			_666 = true;
		}
		for (int k = 0; k < minigamesList.Count; k++)
		{
			if (skinsDictionary[minigamesList[k]][8] > 0)
			{
				_777 = true;
				break;
			}
		}
		if (flag && MIXscores[0] > 99 && MAXscores[0] > 99)
		{
			_888 = true;
		}
		UnlockAchievements();
	}

	public void UnlockAchievements()
	{
		if (steam && AchievementsController.achievementsController != null)
		{
			AchievementsController.achievementsController.UnlockAchievements();
		}
	}

	public void InitlializeLists()
	{
		minigamesResetTime = 1.5f;
		minigamesList = new List<string>
		{
			"MTO", "SQD", "PLU", "CNN", "CBL", "BAT", "SPR", "BAL", "GOT", "PPR",
			"SNW", "MNK", "ZIP", "FTL", "SQR", "FLA", "FRO", "PNG", "SEA", "SWO",
			"BEE", "DUC", "CRA", "WGN", "SHI", "HEL", "RAT", "PLA", "SNK", "SKI",
			"BWL", "TOP"
		};
		playedMinigames = new List<string>();
		defaultSkins = new int[9];
		skinsDictionary = new Dictionary<string, int[]>();
		minigamesScores = new Dictionary<string, int[]>();
		minigamesPieces = new Dictionary<string, int>();
		trackPieces = new int[tracks.Length];
		enabledSkins = new Dictionary<string, bool[]>();
		enabledMinigames = new Dictionary<string, bool>();
		lastUnlockedSkins = new Dictionary<string, int>();
		int[] array = new int[2];
		bool[] value = new bool[9] { true, true, true, true, true, true, true, true, true };
		MIXscores = array;
		MAXscores = array;
		foreach (string minigames in minigamesList)
		{
			skinsDictionary.Add(minigames, defaultSkins);
			minigamesScores.Add(minigames, array);
			minigamesPieces.Add(minigames, 0);
			enabledSkins.Add(minigames, value);
			enabledMinigames.Add(minigames, true);
			lastUnlockedSkins.Add(minigames, -1);
		}
		unlockedTracks = new bool[tracks.Length];
		enabledTracks = new bool[tracks.Length];
		for (int i = 0; i < tracks.Length; i++)
		{
			enabledTracks[i] = true;
		}
		pachinkoDesign = 1;
		pachinkoHole = new float[2] { 0f, -3f };
		justUnlockedMinigames = new List<string>();
	}

	private void InitializeUnlocksCheat()
	{
		unlockedMinigames = minigamesList;
	}

	private void InitializePuzzles()
	{
		totalPuzzles = 3;
		puzzles = new Dictionary<string, bool>();
		for (int i = 0; i < 10; i++)
		{
			puzzles.Add("001_" + i, false);
			puzzles.Add("002_" + i, false);
			puzzles.Add("003_" + i, false);
		}
	}

	private void InitializeDifficultyLevel()
	{
		gameLevel = new GameLevel();
	}

	public void UnlockInitialMinigames()
	{
		UnlockMinigame("MTO");
		UnlockMinigame("SQD");
		UnlockMinigame("PLU");
		UnlockMinigame("CNN");
		UnlockMinigame("CBL");
		UnlockMinigame("BAL");
		UnlockMinigame("GOT");
		UnlockMinigame("PPR");
		UnlockMinigame("SNW");
		UnlockMinigame("MNK");
		UnlockMinigame("PNG");
		UnlockMinigame("SEA");
		for (int i = 0; i < 20; i++)
		{
			unlockedTracks[i] = true;
			trackPieces[i] = 2;
		}
	}

	public void UnlockMinigame(string minigame)
	{
		if (unlockedMinigames.Contains(minigame))
		{
			Debug.Log("ERROR: Attempted to unlock " + minigame + ", but it was already unlocked.");
			return;
		}
		unlockedMinigames.Add(minigame);
		minigamesPieces[minigame] = 4;
		justUnlockedMinigames.Add(minigame);
		if (skinsDictionary[minigame][0] == 0)
		{
			skinsDictionary[minigame][0] = 1;
		}
	}

	public void UnlockAllCheat()
	{
		UnlockAllMinigames();
		UnlockAllTracks();
		DataChecks();
		SaveData();
	}

	private void UnlockAllMinigames()
	{
		for (int i = 0; i < minigamesList.Count; i++)
		{
			if (!unlockedMinigames.Contains(minigamesList[i]))
			{
				UnlockMinigame(minigamesList[i]);
			}
		}
	}

	private void UnlockAllTracks()
	{
		for (int i = 0; i < unlockedTracks.Length; i++)
		{
			UnlockTrack(i);
		}
	}

	public void UnlockTrack(int trackNumber)
	{
		unlockedTracks[trackNumber] = true;
		trackPieces[trackNumber] = 2;
		enabledTracks[trackNumber] = true;
	}

	public void ResetUnlocks()
	{
		unlockedMinigames = new List<string>();
		minigamesPieces = new Dictionary<string, int>();
		recentCinematics = new int[2];
		InitlializeLists();
	}

	public void ReduceXp()
	{
		if (mode == "normal")
		{
			gameLevel.ReduceXp();
		}
		else if (mode == "endless")
		{
			temporalGameLevel.ReduceXp();
		}
	}

	public void AddXp()
	{
		if (mode == "normal")
		{
			gameLevel.AddXp();
		}
		else
		{
			temporalGameLevel.AddXp();
		}
	}

	public void SetLevel(int l)
	{
		gameLevel.SetLevel(l);
		temporalGameLevel.SetLevel(l);
	}

	public int GetLevel()
	{
		if (mode == "normal")
		{
			return gameLevel.GetLevel();
		}
		return temporalGameLevel.GetLevel();
	}

	public int GetActualLevel()
	{
		if (mode == "normal")
		{
			return gameLevel.GetActualLevel();
		}
		return temporalGameLevel.GetLevel();
	}

	public int[] GetUnlockedSkins(string minigameName)
	{
		int[] array = skinsDictionary[minigameName];
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] > 0)
			{
				text = text + i + "_";
			}
		}
		if (text != string.Empty)
		{
			text = text.Substring(0, text.Length - 1);
			string[] array2 = text.Split("_"[0]);
			array = new int[array2.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array[j] = int.Parse(array2[j]);
			}
		}
		else
		{
			array = new int[1];
		}
		return array;
	}

	public int[] GetUnlockedAndEnabledSkins(string minigameName)
	{
		int[] array = skinsDictionary[minigameName];
		List<int> list = new List<int>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] > 0 && enabledSkins[minigameName][i])
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			array = new int[list.Count];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = list[j];
			}
		}
		array = new int[list.Count];
		for (int k = 0; k < list.Count; k++)
		{
			array[k] = list[k];
		}
		return array;
	}

	public int HowManyUnlockedSkins()
	{
		int num = 0;
		foreach (string minigames in minigamesList)
		{
			int[] array = skinsDictionary[minigames];
			foreach (int num2 in array)
			{
				if (num2 > 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int HowManyTotalSkins()
	{
		int num = 0;
		foreach (string minigames in minigamesList)
		{
			int[] array = skinsDictionary[minigames];
			foreach (int num2 in array)
			{
				num++;
			}
		}
		return num;
	}

	public int HowManyLockedNormalSkins()
	{
		int num = 0;
		foreach (string minigames in minigamesList)
		{
			for (int i = 0; i < 8; i++)
			{
				num += 9 - skinsDictionary[minigames][i];
			}
		}
		return num;
	}

	public int HowManyLockedGoldenSkins()
	{
		int num = 0;
		foreach (string minigames in minigamesList)
		{
			num += 9 - skinsDictionary[minigames][8];
		}
		return num;
	}

	public int HowManyUnlockedMedals()
	{
		int num = 0;
		foreach (string minigames in minigamesList)
		{
			int[] array = skinsDictionary[minigames];
			foreach (int num2 in array)
			{
				if (num2 == 10)
				{
					num++;
				}
			}
		}
		return num;
	}

	public bool AreAllMinigamesUnlocked()
	{
		if (unlockedMinigames.Count >= minigamesList.Count)
		{
			return true;
		}
		return false;
	}

	public int HowManyUnlockedTracks()
	{
		int num = 0;
		for (int i = 0; i < unlockedTracks.Length; i++)
		{
			if (unlockedTracks[i])
			{
				num++;
			}
		}
		return num;
	}

	public bool AreAllTracksUnlocked()
	{
		for (int i = 0; i < unlockedTracks.Length; i++)
		{
			if (!unlockedTracks[i])
			{
				return false;
			}
		}
		return true;
	}

	public bool AreAllSkinsCollected()
	{
		foreach (string minigames in minigamesList)
		{
			for (int i = 0; i < 9; i++)
			{
				if (skinsDictionary[minigames][i] < 9)
				{
					return false;
				}
			}
		}
		return true;
	}

	public Color32 HexToColor(string hex)
	{
		hex = hex.Replace("0x", string.Empty);
		hex = hex.Replace("#", string.Empty);
		byte a = byte.MaxValue;
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		if (hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
		}
		return new Color32(r, g, b, a);
	}

	public int CalculateProgress()
	{
		int num = minigamesList.Count * 9 * 9;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int count = minigamesList.Count;
		int count2 = unlockedMinigames.Count;
		int num6 = unlockedTracks.Length;
		int num7 = 0;
		int num8 = minigamesScores.Count + 2;
		int num9 = 0;
		foreach (string minigames in minigamesList)
		{
			num4 += 72;
			for (int i = 0; i < 9; i++)
			{
				if (skinsDictionary[minigames][i] > 1)
				{
					num5 += skinsDictionary[minigames][i] - 1;
				}
				num3++;
				if (skinsDictionary[minigames][i] > 0)
				{
					num2++;
				}
			}
			if (minigamesScores[minigames][0] >= 99)
			{
				num9++;
			}
		}
		if (MIXscores[0] >= 99)
		{
			num9++;
		}
		if (MAXscores[0] >= 99)
		{
			num9++;
		}
		for (int j = 0; j < num6; j++)
		{
			if (unlockedTracks[j])
			{
				num7++;
			}
		}
		float num10 = (float)count2 * 50f / (float)count;
		num10 -= 20f;
		float num11 = (float)num7 * 30f / (float)num6;
		num11 -= 10f;
		float num12 = (float)num9 * 10f / (float)num8;
		float num13 = (float)num2 * 30f / (float)num3;
		float num14 = (float)num5 * 10f / (float)num4;
		float num15 = num10 + num11 + num12 + num13 + num14;
		int num16 = 0;
		if (num15 < 1f && num15 > 0f)
		{
			num16 = 1;
		}
		else
		{
			if (num15 < 0f)
			{
				num15 = 0f;
			}
			num15 = Mathf.Floor(num15);
			num16 = (int)num15;
		}
		if (num9 > 0 && !_444)
		{
			if (!_444)
			{
				_444 = true;
				UnlockAchievements();
				SaveData();
			}
			else
			{
				_444 = true;
				UnlockAchievements();
			}
		}
		if (num9 >= num8 && !_888)
		{
			if (!_888)
			{
				_888 = true;
				UnlockAchievements();
				SaveData();
			}
			else
			{
				_888 = true;
				UnlockAchievements();
			}
		}
		if (num2 >= num3 && num7 >= num6 && count2 >= count)
		{
			if (!_999)
			{
				_999 = true;
				UnlockAchievements();
				SaveData();
			}
			else
			{
				_999 = true;
				UnlockAchievements();
			}
		}
		return num16;
	}
}
