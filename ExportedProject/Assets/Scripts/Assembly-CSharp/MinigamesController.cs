using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesController : MonoBehaviour
{
	public static MinigamesController minigamesController;

	public string[] recentlyUsedMinigames;

	public string[] selectedMinigames;

	private int totalSelectedMinigames;

	public bool playing;

	public int remainingMinigames;

	public int currentMinigameNumber;

	private AudioSource currentAudioSource;

	public int[] availableTracks;

	private int previousTrackN;

	private int currentTrackN;

	private AudioClip currentTrack;

	public AudioClip blankTrack;

	public GameObject transition;

	private bool transitionInPlayed;

	public GameObject luckBonus;

	public AudioClip luckSound;

	public int survivedMinigames;

	public int[] brokenHearts;

	public bool endSurvival;

	private string preloadedMinigame = string.Empty;

	public bool mix;

	public bool max;

	private float currentMusicTime;

	private float previousMusicTime;

	private int r;

	public bool firstMusic;

	private bool fisrtMusicOfThisBatch;

	private bool activateMinigameNextFrame;

	private bool activateOnTransitionClosed;

	private AsyncOperation async;

	private List<string> justUnlockedMinigames;

	private string minigame;

	private List<string> minigames;

	private int unlockedAndEnabledMinigames;

	private bool validMinigame;

	private int control;

	private void Start()
	{
		minigamesController = this;
		ResetRecentlyUsedMinigames();
	}

	public void ResetRecentlyUsedMinigames()
	{
		recentlyUsedMinigames = new string[9];
	}

	private void Update()
	{
		if (!playing || !(currentAudioSource != null) || currentAudioSource.clip == null)
		{
			return;
		}
		currentMusicTime = (float)currentAudioSource.timeSamples / (float)currentAudioSource.clip.frequency;
		if (currentMusicTime < previousMusicTime)
		{
			currentMusicTime = previousMusicTime + Time.deltaTime;
		}
		previousMusicTime = currentMusicTime;
		if (currentMusicTime >= 9.75f && !transitionInPlayed && !PauseController.pauseController.paused)
		{
			if (GlobalControl.globalControl.mode == "normal")
			{
				ConsoleController.consoleController.AddText("Clip at 9.75.");
				if (remainingMinigames > 0)
				{
					TransitionIn("minigame");
					PreloadMinigame(GetNextMinigame());
				}
				else
				{
					UpdateLightsLevel();
					TransitionIn("title");
					PreloadMinigame("PCK");
				}
			}
			else if (GlobalControl.globalControl.mode == "endless")
			{
				if (unlockedAndEnabledMinigames <= 1)
				{
					transitionInPlayed = true;
				}
				else
				{
					TransitionIn("title");
					PreloadMinigame(ChooseNextMinigame());
				}
			}
			else if (GlobalControl.globalControl.mode == "survival" && mix && !endSurvival)
			{
				if (unlockedAndEnabledMinigames <= 1)
				{
					transitionInPlayed = true;
				}
				else
				{
					TransitionIn("title");
					PreloadMinigame(ChooseNextMinigame());
				}
			}
			else if (GlobalControl.globalControl.mode == "survival" && !mix)
			{
				transitionInPlayed = true;
			}
		}
		else if (currentMusicTime >= 10f && !SceneChangeController.sceneChangeController.inTransition)
		{
			if (GlobalControl.globalControl.mode == "normal")
			{
				ConsoleController.consoleController.AddText("Clip at 10.");
				if (remainingMinigames > 0)
				{
					survivedMinigames++;
					PreactivateMinigame();
					remainingMinigames--;
					currentMinigameNumber++;
				}
				else if (playing)
				{
					GlobalControl.globalControl.backFromMinigames = true;
					ActivateNonMinigameScene();
				}
			}
			else if (GlobalControl.globalControl.mode == "endless")
			{
				if (unlockedAndEnabledMinigames <= 1)
				{
					ChangeMusic();
				}
				else
				{
					PreactivateMinigame();
				}
			}
			else if (GlobalControl.globalControl.mode == "survival" && mix && !endSurvival)
			{
				if (unlockedAndEnabledMinigames <= 1)
				{
					ChangeMusic();
				}
				else
				{
					PreactivateMinigame();
				}
			}
			else if (GlobalControl.globalControl.mode == "survival" && !endSurvival)
			{
				ChangeMusic();
			}
		}
		if (activateMinigameNextFrame)
		{
			ActivateMinigame();
		}
	}

	private void Cheats()
	{
		if (Input.GetKeyDown("1"))
		{
			GlobalControl.globalControl.SetLevel(GlobalControl.globalControl.GetLevel() - 1);
		}
		else if (Input.GetKeyDown("2"))
		{
			GlobalControl.globalControl.SetLevel(GlobalControl.globalControl.GetLevel() + 1);
		}
	}

	public void StartMinigames(bool stats)
	{
		if (!PauseController.pauseController.exiting)
		{
			GlobalControl.globalControl.SaveData();
			fisrtMusicOfThisBatch = true;
			ResetBrokenHearts();
			currentMinigameNumber = 1;
			totalSelectedMinigames = 9;
			selectedMinigames = new string[totalSelectedMinigames];
			for (int i = 0; i < totalSelectedMinigames; i++)
			{
				selectedMinigames[i] = ChooseNextMinigame();
				UpdateRecentlyUsedMinigames(selectedMinigames[i]);
			}
			playing = true;
			survivedMinigames = 1;
			remainingMinigames = 9;
			GetNextMinigame();
			UpdateRecentlyUsedMinigames(preloadedMinigame);
			if (stats)
			{
				SceneChangeController.sceneChangeController.LoadAndChangeScene(preloadedMinigame, "title", 0.5f);
				Invoke("ChangeMusic", 0.65f);
				Invoke("ReduceRemainingMinigames", 1f);
			}
			else
			{
				SceneChangeController.sceneChangeController.LoadAndChangeScene(preloadedMinigame, "title", 1f);
				Invoke("ChangeMusic", 1.15f);
				Invoke("ReduceRemainingMinigames", 1.5f);
			}
			ManageIgnoreXp();
			GetAvailableTracks();
			previousMusicTime = 0f;
			transitionInPlayed = false;
			activateMinigameNextFrame = false;
			activateOnTransitionClosed = false;
		}
	}

	public void StartEndlessMode()
	{
		playing = true;
		fisrtMusicOfThisBatch = true;
		GlobalControl.globalControl.temporalGameLevel = new GameLevel();
		DifficultyGaugeController.difficultyGaugeController.ResetLevel();
		SceneChangeController.sceneChangeController.LoadAndChangeScene(ChooseNextMinigame(), "title", 0f);
		UpdateRecentlyUsedMinigames(preloadedMinigame);
		previousMusicTime = 0f;
		transitionInPlayed = false;
		ResetBrokenHearts();
		GetAvailableTracks();
		Invoke("ChangeMusic", 1f / 6f);
	}

	public void StartSurvival(string mg)
	{
		GlobalControl.globalControl.mode = "survival";
		playing = true;
		fisrtMusicOfThisBatch = true;
		GlobalControl.globalControl.temporalGameLevel = new GameLevel();
		DifficultyGaugeController.difficultyGaugeController.ResetLevel();
		GlobalControl.globalControl.alternateMarbles = 0;
		endSurvival = false;
		SceneChangeController.sceneChangeController.LoadAndChangeScene(mg, "title", 0f);
		previousMusicTime = 0f;
		transitionInPlayed = false;
		ResetBrokenHearts();
		GetAvailableTracks();
		Invoke("ChangeMusic", 1f / 6f);
	}

	public void StartSurvivalMix()
	{
		GlobalControl.globalControl.mode = "survival";
		playing = true;
		fisrtMusicOfThisBatch = true;
		GlobalControl.globalControl.temporalGameLevel = new GameLevel();
		DifficultyGaugeController.difficultyGaugeController.ResetLevel();
		GlobalControl.globalControl.alternateMarbles = 0;
		endSurvival = false;
		ChooseNextMinigame();
		UpdateRecentlyUsedMinigames(preloadedMinigame);
		SceneChangeController.sceneChangeController.LoadAndChangeScene(preloadedMinigame, "title", 0f);
		previousMusicTime = 0f;
		transitionInPlayed = false;
		ResetBrokenHearts();
		activateMinigameNextFrame = false;
		activateOnTransitionClosed = false;
		GetAvailableTracks();
		Invoke("ChangeMusic", 1f / 6f);
	}

	private void ReduceRemainingMinigames()
	{
		remainingMinigames--;
	}

	private void TransitionIn(string type)
	{
		ConsoleController.consoleController.AddText("Transition in from minigames controller.");
		SceneChangeController.sceneChangeController.ManualTransitionIn(type);
		transitionInPlayed = true;
	}

	private void PreactivateMinigame()
	{
		if (!PauseController.pauseController.exiting)
		{
			ChangeMusic();
			activateMinigameNextFrame = true;
			activateOnTransitionClosed = true;
		}
	}

	private void ActivateMinigame()
	{
		if (activateOnTransitionClosed)
		{
			activateOnTransitionClosed = false;
			SceneChangeController.sceneChangeController.ActivateOnTransitionClosed();
		}
		if (async.isDone)
		{
			ConsoleController.consoleController.AddText("Activate minigame. Preloaded: " + preloadedMinigame);
			activateMinigameNextFrame = false;
			if (!PauseController.pauseController.exiting)
			{
				HeartController.heartController.CancelAnimation();
				UpdateRecentlyUsedMinigames(preloadedMinigame);
				transitionInPlayed = false;
				ManageIgnoreXp();
				GlobalControl.globalControl.playing = true;
			}
		}
	}

	private void ActivateNonMinigameScene()
	{
		if (!PauseController.pauseController.exiting)
		{
			if (GlobalControl.globalControl.marbles >= 70)
			{
				GlobalControl.globalControl._333 = true;
				GlobalControl.globalControl.UnlockAchievements();
				GlobalControl.globalControl.CalculateProgress();
			}
			if (survivedMinigames >= 9 && GlobalControl.globalControl.marbles >= 50)
			{
				GlobalControl.globalControl._222 = true;
				GlobalControl.globalControl.UnlockAchievements();
				GlobalControl.globalControl.CalculateProgress();
			}
			GlobalControl.globalControl.SaveData();
			StopPlay();
			transitionInPlayed = false;
			ConsoleController.consoleController.AddText("pry- Activate non minigame scene.");
			SceneChangeController.sceneChangeController.ActivateOnTransitionClosed();
		}
	}

	public bool Hit()
	{
		if (GlobalControl.globalControl.mode == "survival")
		{
			EndSurvival();
			return false;
		}
		return true;
	}

	private void PreloadMinigame(string minigame)
	{
		if (!PauseController.pauseController.exiting)
		{
			preloadedMinigame = minigame;
			async = SceneChangeController.sceneChangeController.DirectlyLoadSceneInBackground(minigame);
		}
	}

	public void StopPlay()
	{
		playing = false;
		if (currentAudioSource != null)
		{
			currentAudioSource.Stop();
		}
		remainingMinigames = 0;
		HeartController.heartController.CancelAnimation();
	}

	private void ManageIgnoreXp()
	{
		if (GlobalControl.globalControl.mode == "normal")
		{
			if (!GlobalControl.globalControl.playedMinigames.Contains(preloadedMinigame))
			{
				GlobalControl.globalControl.playedMinigames.Add(preloadedMinigame);
				GlobalControl.globalControl.gameLevel.IgnoreXp(true);
			}
			else
			{
				GlobalControl.globalControl.gameLevel.IgnoreXp(false);
			}
		}
	}

	private void EndSurvival()
	{
		if (!endSurvival)
		{
			endSurvival = true;
			StopPlay();
			GlobalControl.globalControl.backFromMinigames = true;
			GC.Collect();
			if (!PauseController.pauseController.exiting)
			{
				Invoke("LoadSUR", 1f);
			}
		}
	}

	private void LoadMMU()
	{
		if (!PauseController.pauseController.exiting)
		{
			activateMinigameNextFrame = false;
			activateOnTransitionClosed = false;
			SceneChangeController.sceneChangeController.LoadAndChangeScene("MMU", "title", 0f);
		}
	}

	private void LoadSUR()
	{
		endSurvival = false;
		if (!PauseController.pauseController.exiting && !SceneChangeController.sceneChangeController.transitioning)
		{
			activateMinigameNextFrame = false;
			activateOnTransitionClosed = false;
			SceneChangeController.sceneChangeController.LoadAndChangeScene("SUR", "marbles", 1f);
			Invoke("PlayMenuMusic", 1.4f);
		}
	}

	private void PlayMenuMusic()
	{
		GlobalAudio.globalAudio.PlayMainMenuMusic();
	}

	private string ChooseNextMinigame()
	{
		bool flag = false;
		unlockedAndEnabledMinigames = 0;
		justUnlockedMinigames = GlobalControl.globalControl.justUnlockedMinigames;
		if (justUnlockedMinigames.Count > 0 && GlobalControl.globalControl.mode == "normal")
		{
			while (justUnlockedMinigames.Count > 0)
			{
				minigame = justUnlockedMinigames[0];
				justUnlockedMinigames.Remove(minigame);
				if (!GlobalControl.globalControl.enabledMinigames[minigame])
				{
					continue;
				}
				minigames = new List<string>();
				for (int i = 0; i < GlobalControl.globalControl.unlockedMinigames.Count; i++)
				{
					if (GlobalControl.globalControl.enabledMinigames[GlobalControl.globalControl.unlockedMinigames[i]])
					{
						minigames.Add(GlobalControl.globalControl.unlockedMinigames[i]);
						unlockedAndEnabledMinigames++;
					}
				}
				preloadedMinigame = minigame;
				return minigame;
			}
		}
		unlockedAndEnabledMinigames = 0;
		minigames = new List<string>();
		if (!max)
		{
			for (int j = 0; j < GlobalControl.globalControl.unlockedMinigames.Count; j++)
			{
				if (GlobalControl.globalControl.enabledMinigames[GlobalControl.globalControl.unlockedMinigames[j]])
				{
					minigames.Add(GlobalControl.globalControl.unlockedMinigames[j]);
					unlockedAndEnabledMinigames++;
				}
			}
		}
		else
		{
			for (int k = 0; k < GlobalControl.globalControl.unlockedMinigames.Count; k++)
			{
				minigames.Add(GlobalControl.globalControl.unlockedMinigames[k]);
			}
		}
		for (int l = 0; l < recentlyUsedMinigames.Length; l++)
		{
			for (int m = 0; m < minigames.Count; m++)
			{
				if (recentlyUsedMinigames[l] == minigames[m])
				{
					minigames.RemoveAt(m);
					break;
				}
			}
		}
		if (minigames.Count > 0)
		{
			r = UnityEngine.Random.Range(0, minigames.Count);
			flag = true;
		}
		if (minigames.Count < 9 && !flag)
		{
			for (int n = 0; n < recentlyUsedMinigames.Length - 1; n++)
			{
				if (recentlyUsedMinigames[n] != null && GlobalControl.globalControl.enabledMinigames[recentlyUsedMinigames[n]] && !minigames.Contains(recentlyUsedMinigames[n]) && recentlyUsedMinigames[n] != recentlyUsedMinigames[recentlyUsedMinigames.Length - 1])
				{
					minigames.Add(recentlyUsedMinigames[n]);
				}
			}
		}
		if (minigames.Count > 0 && !flag)
		{
			r = UnityEngine.Random.Range(0, minigames.Count);
			flag = true;
		}
		if (recentlyUsedMinigames[recentlyUsedMinigames.Length - 1] != null && GlobalControl.globalControl.enabledMinigames[recentlyUsedMinigames[recentlyUsedMinigames.Length - 1]] && !flag)
		{
			minigames.Add(recentlyUsedMinigames[recentlyUsedMinigames.Length - 1]);
			r = 0;
			flag = true;
		}
		if (minigames.Count == 0 && !flag)
		{
			minigames.Add("NOT");
			flag = true;
			preloadedMinigame = "NOT";
			return preloadedMinigame;
		}
		preloadedMinigame = minigames[r];
		return minigames[r];
	}

	private string GetNextMinigame()
	{
		preloadedMinigame = selectedMinigames[(remainingMinigames - totalSelectedMinigames) * -1];
		return preloadedMinigame;
	}

	private void UpdateRecentlyUsedMinigames(string minigame)
	{
		if (!(minigame == "NOT"))
		{
			for (int i = 1; i < recentlyUsedMinigames.Length; i++)
			{
				recentlyUsedMinigames[i - 1] = recentlyUsedMinigames[i];
			}
			recentlyUsedMinigames[recentlyUsedMinigames.Length - 1] = minigame;
		}
	}

	private void GetAvailableTracks()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < GlobalControl.globalControl.unlockedTracks.Length; i++)
		{
			if (GlobalControl.globalControl.unlockedTracks[i] && GlobalControl.globalControl.enabledTracks[i])
			{
				list.Add(i);
			}
		}
		availableTracks = new int[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			availableTracks[j] = list[j];
		}
	}

	public void ChangeMusic()
	{
		ChooseNextMusic();
		float vol = 0.5f;
		if (GlobalControl.globalControl.mobile)
		{
			vol = 0.6f;
		}
		currentAudioSource = GlobalAudio.globalAudio.PlayMusic(currentTrack, vol, 1f, 0f);
		previousMusicTime = 0f;
	}

	private void ChooseNextMusic()
	{
		if (availableTracks.Length == 0)
		{
			currentTrack = blankTrack;
			previousTrackN = currentTrackN;
			currentTrackN = 0;
			return;
		}
		if (availableTracks.Length == 1)
		{
			previousTrackN = currentTrackN;
			currentTrackN = availableTracks[0];
			currentTrack = GlobalControl.globalControl.tracks[availableTracks[0]];
			return;
		}
		if (availableTracks.Length == 2 || availableTracks.Length == 3)
		{
			r = UnityEngine.Random.Range(0, availableTracks.Length);
			while (availableTracks[r] == currentTrackN)
			{
				r++;
				if (r >= availableTracks.Length)
				{
					r = 0;
				}
			}
			previousTrackN = currentTrackN;
			currentTrackN = availableTracks[r];
			currentTrack = GlobalControl.globalControl.tracks[availableTracks[r]];
			return;
		}
		r = UnityEngine.Random.Range(0, availableTracks.Length);
		while (availableTracks[r] == currentTrackN || availableTracks[r] == previousTrackN || (availableTracks[r] == 3 && fisrtMusicOfThisBatch))
		{
			r++;
			if (r >= availableTracks.Length)
			{
				r = 0;
			}
		}
		previousTrackN = currentTrackN;
		currentTrackN = availableTracks[r];
		currentTrack = GlobalControl.globalControl.tracks[currentTrackN];
		if (firstMusic && GlobalControl.globalControl.enabledTracks[3])
		{
			firstMusic = false;
			currentTrack = GlobalControl.globalControl.tracks[3];
		}
		else
		{
			firstMusic = false;
		}
		fisrtMusicOfThisBatch = false;
	}

	private void ResetBrokenHearts()
	{
		GlobalControl.globalControl.pckLightsLevel = 0;
		brokenHearts = new int[9];
	}

	private void UpdateLightsLevel()
	{
		GlobalControl.globalControl.pckLightsLevel = 7;
		for (int i = 0; i < brokenHearts.Length; i++)
		{
			if (brokenHearts[i] > 0)
			{
				GlobalControl.globalControl.pckLightsLevel--;
			}
		}
		if (GlobalControl.globalControl.pckLightsLevel < 0)
		{
			GlobalControl.globalControl.pckLightsLevel = 0;
		}
	}
}
