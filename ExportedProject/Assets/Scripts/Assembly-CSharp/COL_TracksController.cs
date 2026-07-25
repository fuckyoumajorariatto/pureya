using System.Collections.Generic;
using UnityEngine;

public class COL_TracksController : MonoBehaviour
{
	public WheelController wheelController;

	public SUR_WheelController SUR_wheelController;

	public COLController controller;

	public PCK_UnlockAssets unlockAssets;

	public GameObject tracksMenu;

	public GameObject colMenu;

	public COL_IndividualSkinsController individualSkinsController;

	public GameObject patternBG;

	public GameObject darkPatternBG;

	public Sprite shadow_big;

	public Sprite shadow_half_big;

	public Sprite shadow_small;

	public Sprite shadow_half_small;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Sprite[] smallRotationOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public List<int> unlockedTracks;

	public Color white;

	public SpriteRenderer enabledSR;

	public SpriteRenderer enabledSR_s;

	public SpriteRenderer[] middleNumbers;

	public SpriteRenderer[] leftNumbers;

	public SpriteRenderer[] rightNumbers;

	public Sprite trackIconSmall;

	public Sprite trackIconBig;

	private bool inputActive;

	private int currentTrack;

	private bool numbersChanged;

	private AudioSource currentAudioSource;

	private float currentMusicTime;

	private float previousMusicTime;

	private bool skipTrackCheck;

	private void Start()
	{
		PreInitialize();
	}

	private void Update()
	{
		if (controller.currentMenu == "tracks")
		{
			ManageAccept();
			if (currentTrack != wheelController.selection && !skipTrackCheck)
			{
				currentTrack = wheelController.selection;
				UpdateInfo();
				PlayTrack();
			}
			if (wheelController.moving && !numbersChanged)
			{
				numbersChanged = true;
			}
			else if (!wheelController.movingAnimationFinished && !wheelController.moving && numbersChanged)
			{
				numbersChanged = false;
			}
			ManageMusic();
		}
	}

	private void ManageMusic()
	{
		if (!(currentAudioSource == null) && !(currentAudioSource.clip == null) && wheelController.selection != GlobalControl.globalControl.tracks.Length)
		{
			currentMusicTime = (float)currentAudioSource.timeSamples / (float)currentAudioSource.clip.frequency;
			if (currentMusicTime < previousMusicTime)
			{
				currentMusicTime = previousMusicTime + Time.deltaTime;
			}
			previousMusicTime = currentMusicTime;
			if (currentMusicTime >= 10f)
			{
				currentAudioSource = GlobalAudio.globalAudio.PlayMusic(GlobalControl.globalControl.tracks[wheelController.selection], 0.8f, 1f, 0f);
				previousMusicTime = 0f;
			}
		}
	}

	public void Initialize()
	{
		wheelController.ResetNextFrameInput();
		colMenu.SetActive(false);
		tracksMenu.SetActive(true);
		patternBG.SetActive(false);
		darkPatternBG.SetActive(true);
		currentTrack = 0;
		skipTrackCheck = false;
		PlayTrack();
		UpdateInfo();
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void PreInitialize()
	{
		optionsToSkip = new bool[GlobalControl.globalControl.tracks.Length + 1];
		smallIcons = new Sprite[optionsToSkip.Length];
		smallIconsOverlays = new Sprite[smallIcons.Length];
		bigIcons = new Sprite[smallIcons.Length];
		bigIconsOverlays = new Sprite[smallIcons.Length];
		iconColors = new Color[smallIcons.Length];
		for (int i = 0; i < GlobalControl.globalControl.tracks.Length; i++)
		{
			if (!GlobalControl.globalControl.unlockedTracks[i])
			{
				optionsToSkip[i] = true;
				if (GlobalControl.globalControl.trackPieces[i] == 0)
				{
					smallIcons[i] = shadow_small;
					smallIconsOverlays[i] = null;
					bigIcons[i] = shadow_big;
					bigIconsOverlays[i] = null;
					iconColors[i] = new Color(0f, 0f, 0f, 0f);
				}
				else
				{
					smallIcons[i] = unlockAssets.tracks_small[i];
					smallIconsOverlays[i] = shadow_half_small;
					bigIcons[i] = unlockAssets.tracks_big[i];
					bigIconsOverlays[i] = shadow_half_big;
					iconColors[i] = white;
				}
			}
			else
			{
				smallIcons[i] = unlockAssets.tracks_small[i];
				smallIconsOverlays[i] = null;
				bigIcons[i] = unlockAssets.tracks_big[i];
				bigIconsOverlays[i] = null;
				iconColors[i] = white;
			}
		}
		smallIcons[smallIcons.Length - 1] = SUR_wheelController.backIconSmall;
		bigIcons[smallIcons.Length - 1] = SUR_wheelController.backIconBig;
		iconColors[smallIcons.Length - 1] = SUR_wheelController.backIconColor;
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		skipTrackCheck = true;
	}

	private void ManageAccept()
	{
		if (!inputActive || wheelController.moving || controller.currentMenu != "tracks")
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			if (wheelController.selection == optionsToSkip.Length - 1)
			{
				controller.ResetNextFrameInput();
				controller.PlayAccept();
				BackToCollection();
			}
			else
			{
				controller.ResetNextFrameInput();
				controller.PlayAccept();
				ToggleEnabled();
			}
		}
		else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
		{
			controller.ResetNextFrameInput();
			controller.PlayCancel();
			BackToCollection();
		}
	}

	private void BackToCollection()
	{
		inputActive = false;
		if (currentAudioSource != null)
		{
			currentAudioSource.Stop();
		}
		controller.ResetNextFrameInput();
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("ShowCollectionMenu", 1f / 6f);
	}

	private void ShowCollectionMenu()
	{
		GlobalControl.globalControl.SaveData();
		GlobalAudio.globalAudio.PlayMainMenuMusic();
		wheelController.selection = 0;
		tracksMenu.SetActive(false);
		colMenu.SetActive(true);
		patternBG.SetActive(true);
		darkPatternBG.SetActive(false);
		controller.Initialize();
		skipTrackCheck = true;
	}

	private void ToggleEnabled()
	{
		GlobalControl.globalControl.enabledTracks[wheelController.selection] = !GlobalControl.globalControl.enabledTracks[wheelController.selection];
		UpdateInfo();
	}

	private void UpdateInfo()
	{
		if (wheelController.selection == optionsToSkip.Length - 1)
		{
			enabledSR.sprite = null;
			enabledSR_s.sprite = null;
			return;
		}
		if (GlobalControl.globalControl.enabledTracks[wheelController.selection])
		{
			enabledSR.sprite = individualSkinsController.enabledSprites[1];
			enabledSR.color = individualSkinsController.enabledColors[1];
		}
		else
		{
			enabledSR.sprite = individualSkinsController.enabledSprites[0];
			enabledSR.color = individualSkinsController.enabledColors[0];
		}
		enabledSR_s.sprite = enabledSR.sprite;
	}

	private void PlayTrack()
	{
		if (wheelController.selection == GlobalControl.globalControl.tracks.Length)
		{
			GlobalAudio.globalAudio.ClearMusic();
			previousMusicTime = 0f;
		}
		else if (!GlobalControl.globalControl.unlockedTracks[wheelController.selection])
		{
			GlobalAudio.globalAudio.ClearMusic();
			previousMusicTime = 0f;
		}
		else
		{
			GlobalAudio.globalAudio.ClearMusic();
			currentAudioSource = GlobalAudio.globalAudio.PlayMusic(GlobalControl.globalControl.tracks[wheelController.selection], 0.8f, 1f, 0f);
			previousMusicTime = 0f;
		}
	}
}
