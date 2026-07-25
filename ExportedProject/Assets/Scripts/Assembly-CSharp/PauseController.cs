using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
	public static PauseController pauseController;

	public bool inCinematic;

	public bool inCinematicSkipped;

	public bool inPCK;

	public bool skip;

	public bool exiting;

	public GameObject pauseScreen;

	public SpriteRenderer shade;

	public bool paused;

	public GameObject selectionSquares;

	public GameObject buttonScreen;

	public AudioClip acceptSound;

	public AudioClip invalidSound;

	public AudioClip moveSelectionSound;

	public float pauseCooldownTimer;

	private float inputCoolDownTimer = 0.1f;

	private bool controlsResettedAfterInputCooldown;

	private float invokeTime;

	public string invokeCall = string.Empty;

	private Color32 shadeOriginalColor;

	public SpriteRenderer gaugeSR;

	public SpriteRenderer gaugeSR_s;

	public Sprite[] gaugeSprites = new Sprite[4];

	public Sprite[] gaugeSprites_s = new Sprite[4];

	public Sprite[] rotationSprites;

	public SpriteRenderer rotationSR;

	public WheelController wheelController;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Sprite[] smallRotationOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public GameObject elements;

	private Color32 shadeTranslucidColor;

	private void Start()
	{
		pauseController = this;
		shadeOriginalColor = shade.color;
		optionsToSkip = new bool[bigIcons.Length];
		if (!GlobalControl.globalControl.mobile)
		{
			optionsToSkip[3] = true;
		}
		if (!inCinematic && !inPCK)
		{
			optionsToSkip[1] = true;
		}
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		wheelController.ChangeOrderInLayerTo(6);
		UpdateRotationIcons();
	}

	private void Update()
	{
		if (pauseCooldownTimer > 0f && !paused)
		{
			pauseCooldownTimer -= Time.unscaledDeltaTime;
		}
		if (!SceneChangeController.sceneChangeController.pausableScene)
		{
			return;
		}
		if (GlobalInput.globalInput.exitDown && pauseCooldownTimer <= 0f && !wheelController.selectionMade && !paused && (!inCinematic || !inCinematicSkipped))
		{
			TogglePause();
		}
		else if ((GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreen) && !paused && pauseCooldownTimer <= 0f && (!inCinematic || !inCinematicSkipped))
		{
			TogglePause();
		}
		else if (paused)
		{
			ManageInput();
		}
		if (!(invokeTime > 0f))
		{
			return;
		}
		invokeTime -= Time.unscaledDeltaTime;
		if (invokeTime <= 0f)
		{
			invokeTime = 0f;
			if (invokeCall == "Unpause")
			{
				invokeCall = string.Empty;
				Unpause();
			}
			else if (invokeCall == "UnpauseQuick")
			{
				invokeCall = string.Empty;
				UnpauseQuick();
			}
			else if (invokeCall == "Exit")
			{
				invokeCall = string.Empty;
				Exit();
			}
			else if (invokeCall == "LoadMMU")
			{
				invokeCall = string.Empty;
				LoadMMU();
			}
			else if (invokeCall == "TogglePause")
			{
				invokeCall = string.Empty;
				TogglePause();
			}
			else if (invokeCall == "LoadSUR")
			{
				invokeCall = string.Empty;
				LoadSUR();
			}
		}
	}

	public void TogglePause()
	{
		if (!(pauseCooldownTimer > 0f))
		{
			if (paused)
			{
				pauseCooldownTimer = 0.2f;
				GlobalInput.globalInput.gameInputActive = true;
				paused = false;
				ButtonsInputController.buttonsInputController.inheritedTouchControl = true;
				GlobalAudio.globalAudio.UnpauseAll();
				wheelController.selectionMade = false;
				wheelController.selection = 0;
				pauseScreen.SetActive(false);
				Time.timeScale = 1f;
				GlobalControl.globalControl.playing = true;
				wheelController.ResetNextFrameInput();
				Screen.sleepTimeout = -1;
			}
			else if (!SceneChangeController.sceneChangeController.transitioning && !MinigamesController.minigamesController.endSurvival && SceneChangeController.sceneChangeController.loadingScene == string.Empty && !SceneChangeController.sceneChangeController.transitioning)
			{
				paused = true;
				inputCoolDownTimer = 0.1f;
				controlsResettedAfterInputCooldown = false;
				GlobalInput.globalInput.gameInputActive = false;
				ReloadWheel();
				pauseScreen.SetActive(true);
				elements.SetActive(true);
				GlobalAudio.globalAudio.PauseAll();
				shade.color = shadeOriginalColor;
				Time.timeScale = 0f;
				GlobalControl.globalControl.playing = false;
				Screen.sleepTimeout = -2;
				gaugeSR.sprite = gaugeSprites[GlobalControl.globalControl.GetActualLevel()];
				gaugeSR_s.sprite = gaugeSprites_s[GlobalControl.globalControl.GetActualLevel()];
				GlobalControl.globalControl.SaveData();
			}
		}
	}

	public void ForcePause()
	{
		invokeCall = string.Empty;
		paused = false;
		wheelController.selectionMade = false;
		pauseCooldownTimer = 0f;
		TogglePause();
	}

	private void ReloadWheel()
	{
		optionsToSkip = new bool[bigIcons.Length];
		if (!GlobalControl.globalControl.mobile)
		{
			optionsToSkip[3] = true;
		}
		if ((!inCinematic || inCinematicSkipped) && !inPCK)
		{
			optionsToSkip[1] = true;
		}
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		wheelController.ChangeOrderInLayerTo(6);
		UpdateRotationIcons();
	}

	private void ManageInput()
	{
		if (inputCoolDownTimer > 0f)
		{
			inputCoolDownTimer -= Time.unscaledDeltaTime;
			return;
		}
		if (!controlsResettedAfterInputCooldown)
		{
			controlsResettedAfterInputCooldown = true;
		}
		if (wheelController.selectionMade)
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			ManageAccept();
		}
		else
		{
			if (!GlobalInput.globalInput.cancelDown && !GlobalInput.globalInput.exitDown)
			{
				return;
			}
			wheelController.selectionMade = true;
			PlayAccept();
			if (GlobalControl.globalControl.mobile)
			{
				GlobalInput.globalInput.gameInputActive = false;
				Exit();
				invokeTime = 0.25f + Time.unscaledDeltaTime;
				return;
			}
			if (inCinematic || inPCK)
			{
				invokeCall = "UnpauseQuick";
			}
			else
			{
				invokeCall = "Unpause";
			}
			invokeTime = 0.25f + Time.unscaledDeltaTime;
		}
	}

	private void ManageAccept()
	{
		if (wheelController.selectionMade)
		{
			return;
		}
		if (wheelController.selection == 0)
		{
			wheelController.selectionMade = true;
			PlayAccept();
			if (inCinematic || inPCK)
			{
				invokeCall = "UnpauseQuick";
			}
			else
			{
				invokeCall = "Unpause";
			}
			invokeTime = 0.25f + Time.unscaledDeltaTime;
		}
		else if (wheelController.selection == 1)
		{
			if (inCinematic || inPCK)
			{
				inputCoolDownTimer = 1f;
				skip = true;
				if (inCinematic)
				{
					inCinematicSkipped = true;
				}
				PlayAccept();
				invokeCall = "UnpauseQuick";
				invokeTime = 0.25f + Time.unscaledDeltaTime;
			}
		}
		else if (wheelController.selection == 2)
		{
			wheelController.selectionMade = true;
			PlayAccept();
			GlobalInput.globalInput.gameInputActive = false;
			Exit();
			invokeTime = 0.25f + Time.unscaledDeltaTime;
		}
		else if (wheelController.selection == 3)
		{
			if (GlobalControl.globalControl.mobile)
			{
				ToggleRotation();
				PlayAccept();
			}
			else
			{
				PlayInvalid();
			}
		}
	}

	public void ToggleRotation()
	{
		if (Screen.autorotateToPortrait)
		{
			ScreenOrientation orientation = Screen.orientation;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
			Screen.orientation = orientation;
			PlayerPrefs.SetInt("rotation", 0);
		}
		else
		{
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
			Screen.orientation = ScreenOrientation.AutoRotation;
			PlayerPrefs.SetInt("rotation", 1);
		}
		UpdateRotationIcons();
	}

	public void UpdateRotationIcons()
	{
		if (Screen.autorotateToPortrait)
		{
			wheelController.smallIconsOverlays[3] = smallRotationOverlays[1];
			wheelController.bigIconsOverlays[3] = bigRotationOverlays[1];
		}
		else
		{
			wheelController.smallIconsOverlays[3] = smallRotationOverlays[0];
			wheelController.bigIconsOverlays[3] = bigRotationOverlays[0];
		}
		wheelController.UpdateIcons();
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayMoveSelection()
	{
		GlobalAudio.globalAudio.PlayMoveSelection();
	}

	private void PlayInvalid()
	{
		GlobalAudio.globalAudio.PlayInvalid();
	}

	public void Unpause()
	{
		elements.SetActive(false);
		ButtonsInputController.buttonsInputController.inheritedTouchControl = true;
		shadeTranslucidColor = new Color32(0, 0, 0, 50);
		shade.color = shadeTranslucidColor;
		invokeCall = "TogglePause";
		invokeTime = 0.5f + Time.unscaledDeltaTime;
		wheelController.nextFrameInput = 0;
	}

	public void UnpauseQuick()
	{
		elements.SetActive(false);
		ButtonsInputController.buttonsInputController.inheritedTouchControl = true;
		shadeTranslucidColor = new Color32(0, 0, 0, 50);
		shade.color = shadeTranslucidColor;
		invokeCall = "TogglePause";
		invokeTime = Time.unscaledDeltaTime;
		wheelController.nextFrameInput = 0;
	}

	private void Exit()
	{
		exiting = true;
		inCinematic = false;
		inPCK = false;
		ButtonsInputController.buttonsInputController.inheritedTouchControl = true;
		if (GlobalControl.globalControl.mode == "survival")
		{
			GlobalAudio.globalAudio.ClearMusic();
			invokeCall = "LoadSUR";
		}
		else
		{
			if (SceneManager.GetActiveScene().name == "PCK")
			{
				GlobalControl.globalControl.marbles = PCK_InputController.PCK_inputController.marblesRemaining;
				GlobalControl.globalControl.changeMMUselectionTo = 0;
			}
			if (GlobalControl.globalControl.mode == "endless")
			{
				GlobalControl.globalControl.marbles += GlobalControl.globalControl.alternateMarbles;
				if (GlobalControl.globalControl.marbles > 999)
				{
					GlobalControl.globalControl.marbles = 999;
				}
				if (GlobalControl.globalControl.goldenMarbles > 999)
				{
					GlobalControl.globalControl.goldenMarbles = 999;
				}
				GlobalControl.globalControl.alternateMarbles = 0;
				GlobalControl.globalControl.changeMMUselectionTo = 2;
			}
			MinigamesController.minigamesController.survivedMinigames = 0;
			MinigamesController.minigamesController.endSurvival = false;
			GlobalAudio.globalAudio.ClearMusic();
			invokeCall = "LoadMMU";
		}
		GlobalAudio.globalAudio.ResetPaused();
		invokeTime = 0.25f + Time.unscaledDeltaTime;
		Time.timeScale = 1f;
		wheelController.nextFrameInput = 0;
		GlobalControl.globalControl.SaveData();
	}

	private void LoadMMU()
	{
		SceneChangeController.sceneChangeController.LoadAndChangeScene("MMU", "title", 0f);
		DifficultyGaugeController.difficultyGaugeController.ForceHideGuage();
		GlobalInput.globalInput.gameInputActive = true;
		MinigamesController.minigamesController.StopPlay();
		paused = false;
		wheelController.selectionMade = false;
		wheelController.selection = 0;
		Invoke("LoadMMUMusic", 0.25f);
		pauseScreen.SetActive(false);
		wheelController.nextFrameInput = 0;
	}

	private void LoadMMUMusic()
	{
		GlobalAudio.globalAudio.PlayMainMenuMusic();
		GlobalControl.globalControl.firstPCKMusicPlayed = false;
	}

	private void LoadSUR()
	{
		SceneChangeController.sceneChangeController.LoadAndChangeScene("SUR", "marbles", 0f);
		DifficultyGaugeController.difficultyGaugeController.ForceHideGuage();
		GlobalInput.globalInput.gameInputActive = true;
		MinigamesController.minigamesController.StopPlay();
		paused = false;
		wheelController.selectionMade = false;
		wheelController.selection = 0;
		GlobalAudio.globalAudio.PlayMainMenuMusic();
		pauseScreen.SetActive(false);
		wheelController.nextFrameInput = 0;
	}
}
