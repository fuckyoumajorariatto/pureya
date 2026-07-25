using System;
using UnityEngine;

public class PCK_Controller : MonoBehaviour
{
	public static PCK_Controller PCK_controller;

	public Camera RenderCamera;

	private bool initialized;

	public int marbles;

	private bool ended;

	public bool marbleRefilling = true;

	public int jackpot;

	public bool managingJackpot;

	private bool callUnlocks;

	private bool marbleRefillingChecked;

	private GameObject[] pausedMarbles;

	private AudioSource aS;

	public AudioClip pachinkoMusic;

	private int r;

	private bool checkEndingNextFrame;

	public bool playingTrack;

	public PCK_SlotsProgressController slotsProgressController;

	public PCK_GoldenIndicatorController goldenIndicatorController;

	private float dontEndPachinkoTimer;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(188, 104, 0, byte.MaxValue);
		PCK_controller = this;
		GlobalInput.globalInput.gameInputActive = true;
		InitializeMarbles();
		GlobalControl.globalControl.rank = 0;
		GlobalControl.globalControl.playing = true;
		Screen.sleepTimeout = -1;
	}

	private void Update()
	{
		if (!initialized)
		{
			InitializeMarbles();
			if (GlobalControl.globalControl.storyLevel > 0)
			{
				PCK_RankController.PCK_rankController.ShowRank();
			}
			else
			{
				PCK_CounterController.PCK_counterController.ShowNumber(marbles);
				PCK_InputController.PCK_inputController.inputActive = true;
				PCK_controller.marbleRefilling = false;
			}
			if (marbles > 0 && GlobalControl.globalControl.storyLevel > 0)
			{
				PlayMusic();
			}
			else
			{
				aS = null;
			}
		}
		if (callUnlocks)
		{
			LoadUnlocks();
		}
		CheckIfPachinkoEnded();
		if (!marbleRefillingChecked && GlobalControl.globalControl != null)
		{
			marbleRefillingChecked = true;
		}
		if (aS == null && playingTrack)
		{
			playingTrack = false;
		}
		if (playingTrack && !PauseController.pauseController.paused && aS.clip != null)
		{
			float num = (float)aS.timeSamples / (float)aS.clip.frequency;
			if (num > 11f)
			{
				PlayMusic();
				playingTrack = false;
			}
		}
		else if (playingTrack && aS.clip == null)
		{
			playingTrack = false;
		}
		if (PauseController.pauseController.skip && !PauseController.pauseController.inCinematic)
		{
			if (PCK_InputController.PCK_inputController.marblesRemaining >= 100)
			{
				GlobalControl.globalControl.rank = -10 + Mathf.FloorToInt((float)PCK_InputController.PCK_inputController.marblesRemaining / 10f);
				PCK_HoleStarsController.PCK_holeStarsController.ShowStar();
			}
			else if (PCK_InputController.PCK_inputController.marblesRemaining >= 30)
			{
				Invoke("AddProgress", 0.5f);
			}
			GlobalControl.globalControl.goldenMarbles = 0;
			PCK_SlotsController.PCK_slotsController.marblesSkipped = PCK_InputController.PCK_inputController.marblesRemaining;
			if (PCK_SlotsController.PCK_slotsController.marblesSkipped >= 100)
			{
				PCK_SlotsController.PCK_slotsController.AddToQueue();
			}
			else if (PCK_SlotsController.PCK_slotsController.marblesSkipped >= 10)
			{
				dontEndPachinkoTimer = 1.5f;
			}
			PCK_InputController.PCK_inputController.marblesRemaining = 0;
			PCK_InputController.PCK_inputController.visualMarblesRemaining = 0;
			PCK_InputController.PCK_inputController.UpdateMarblesCounter();
			if (PCK_SlotsController.PCK_slotsController.marblesSkipped >= 10)
			{
				PCK_CounterController.PCK_counterController.SignalCounter();
			}
			PauseController.pauseController.skip = false;
		}
	}

	private void AddProgress()
	{
		slotsProgressController.AddProgress(false);
	}

	public void PlayingTrack(AudioSource trackAS)
	{
		aS = trackAS;
		playingTrack = true;
	}

	public void FadePachinkoMusic()
	{
		GlobalAudio.globalAudio.FadeMusic(aS, 1f);
	}

	public void PlayMusic()
	{
		GlobalAudio.globalAudio.ClearMusic();
		aS = GlobalAudio.globalAudio.PlayMusic(pachinkoMusic, 0.9f, 1f, 0f);
		aS.loop = true;
		playingTrack = false;
		if (!GlobalControl.globalControl.firstPCKMusicPlayed)
		{
			GlobalControl.globalControl.firstPCKMusicPlayed = true;
			return;
		}
		r = UnityEngine.Random.Range(0, 4);
		if (r == 1)
		{
			aS.timeSamples = 8 * pachinkoMusic.frequency;
		}
		else if (r == 2)
		{
			aS.timeSamples = 20 * pachinkoMusic.frequency;
		}
		else if (r == 3)
		{
			aS.timeSamples = 28 * pachinkoMusic.frequency;
		}
	}

	private void InitializeMarbles()
	{
		if (!(GlobalControl.globalControl == null) && !(PCK_RankController.PCK_rankController == null) && !(PCK_InputController.PCK_inputController == null))
		{
			marbles = GlobalControl.globalControl.marbles;
			if (marbles > 999)
			{
				marbles = 999;
			}
			PCK_InputController.PCK_inputController.marblesRemaining = marbles;
			if (GlobalControl.globalControl.storyLevel == 0)
			{
				marbles = 10;
				PCK_InputController.PCK_inputController.marblesRemaining = 10;
				GlobalControl.globalControl.rank = 0;
				GlobalControl.globalControl.pckLightsLevel = 0;
				slotsProgressController.ResetProgress();
				PCK_RankController.PCK_rankController.UpdateRankCorner();
			}
			slotsProgressController.UpdateSlotsProgressAdjusted();
			PCK_InputController.PCK_inputController.visualMarblesRemaining = marbles;
			if (PCK_InputController.PCK_inputController.marblesRemaining > 0)
			{
				PauseController.pauseController.inPCK = true;
			}
			initialized = true;
		}
	}

	public void CheckIfPachinkoEnded()
	{
		if (dontEndPachinkoTimer > 0f)
		{
			dontEndPachinkoTimer -= Time.deltaTime;
		}
		else if (PCK_InputController.PCK_inputController.marblesRemaining <= 0 && !ended && !managingJackpot && !marbleRefilling && !PCK_SlotsController.PCK_slotsController.AreSlotsWorking() && !PauseController.pauseController.exiting)
		{
			if (!checkEndingNextFrame)
			{
				checkEndingNextFrame = true;
			}
			else if (!PCK_SlotsController.PCK_slotsController.AreSlotsWorking() && GameObject.FindGameObjectsWithTag("marble").Length == 0)
			{
				PCK_CounterController.PCK_counterController.SignalCounter();
				GlobalControl.globalControl.marbles = 0;
				GlobalControl.globalControl.goldenMarbles = 0;
				GC.Collect();
				ended = true;
				if (aS != null)
				{
					GlobalAudio.globalAudio.FadeMusic(aS, 0.5f);
				}
				Invoke("End", 0.5f);
			}
			else
			{
				checkEndingNextFrame = false;
			}
		}
		else
		{
			checkEndingNextFrame = false;
		}
	}

	private void End()
	{
		if (PauseController.pauseController.exiting || SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		if (GlobalControl.globalControl.storyLevel > 0 && GlobalControl.globalControl.storyLevel < 3)
		{
			if (GlobalControl.globalControl.storyLevel == 1 && GlobalControl.globalControl.AreAllMinigamesUnlocked() && GlobalControl.globalControl.AreAllTracksUnlocked() && GlobalControl.globalControl.justUnlockedMinigames.Count == 0)
			{
				PCK_CinematicsController.PCK_cinematicsController.Ending1();
			}
			else if (GlobalControl.globalControl.storyLevel == 2 && GlobalControl.globalControl.AreAllSkinsCollected())
			{
				PCK_CinematicsController.PCK_cinematicsController.Ending2();
			}
			else
			{
				MinigamesController.minigamesController.StartMinigames(true);
			}
		}
		else if (GlobalControl.globalControl.storyLevel == 0)
		{
			PCK_CinematicsController.PCK_cinematicsController.Intro();
		}
		else
		{
			MinigamesController.minigamesController.StartMinigames(true);
		}
		GlobalAudio.globalAudio.ClearMusic();
	}

	public void ManageJackpot(int givenJackpot)
	{
		PCK_InputController.PCK_inputController.inputActive = false;
		managingJackpot = true;
		PauseController.pauseController.inPCK = false;
		Invoke("ActivateCallUnlocks", 1f);
		jackpot = givenJackpot;
	}

	private void ActivateCallUnlocks()
	{
		callUnlocks = true;
	}

	public void EndJackpot()
	{
		PCK_InputController.PCK_inputController.inputActive = true;
		managingJackpot = false;
		if (PCK_InputController.PCK_inputController.marblesRemaining > 0)
		{
			PauseController.pauseController.inPCK = true;
		}
		PCK_SlotsController.PCK_slotsController.ManageHole();
		PCK_SlotsController.PCK_slotsController.slotsProgress.ResetProgress();
		PCK_SlotsController.PCK_slotsController.inJackpot = false;
		goldenIndicatorController.LightOff();
	}

	private void LoadUnlocks()
	{
		callUnlocks = false;
		PCK_UnlocksController.PCK_unlocksController.remainingPrizes = jackpot;
		GlobalControl.globalControl.remainingPrizes = jackpot;
		PCK_UnlocksController.PCK_unlocksController.ShowDispenser();
	}

	public void ResumeUnlocks()
	{
		managingJackpot = true;
		PCK_UnlocksController.PCK_unlocksController.remainingPrizes = GlobalControl.globalControl.remainingPrizes;
		PCK_UnlocksController.PCK_unlocksController.ShowDispenser();
	}

	public void PauseMarbles()
	{
		pausedMarbles = GameObject.FindGameObjectsWithTag("marble");
		for (int i = 0; i < pausedMarbles.Length; i++)
		{
			pausedMarbles[i].SetActive(false);
		}
	}

	public void UnpauseMarbles()
	{
		for (int i = 0; i < pausedMarbles.Length; i++)
		{
			pausedMarbles[i].SetActive(true);
		}
		pausedMarbles = null;
	}
}
