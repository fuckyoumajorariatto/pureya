using UnityEngine;

public class SETController : MonoBehaviour
{
	public Sprite[] numberSprites;

	public WheelController wheelController;

	public GameObject wheel;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Sprite[] smallRotationOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	public Sprite[] profileNumbersSmall;

	public Sprite[] profileNumbersBig;

	private bool[] optionsToSkip;

	public SpriteRenderer titleIconSR;

	public SpriteRenderer titleIconSR_s;

	public SpriteRenderer titleIconOverlaySR;

	public Sprite settingsIcon;

	public Color titleColor;

	public string currentMenu = "settings";

	public SET_ProfileController profileController;

	public SET_VolumeController volumeController;

	public SET_ScreenController screenController;

	public SET_CreditsController creditsController;

	private bool inputActive;

	private bool callResetNextFrameInput;

	private void Start()
	{
		optionsToSkip = new bool[bigIcons.Length];
		if (GlobalControl.globalControl.mobile)
		{
			optionsToSkip[1] = true;
		}
		else
		{
			optionsToSkip[2] = true;
		}
		if (GlobalControl.globalControl.profile == 0)
		{
			GlobalControl.globalControl.profile = 1;
		}
		smallIconsOverlays[0] = profileNumbersSmall[GlobalControl.globalControl.profile - 1];
		bigIconsOverlays[0] = profileNumbersBig[GlobalControl.globalControl.profile - 1];
		if (wheelController != null)
		{
			wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		}
		UpdateRotationIcons();
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	public void Initialize()
	{
		Invoke("ActivateInput", 0.5f);
		wheel.SetActive(true);
		wheelController.ResetNextFrameInput();
		smallIconsOverlays[0] = profileNumbersSmall[GlobalControl.globalControl.profile - 1];
		bigIconsOverlays[0] = profileNumbersBig[GlobalControl.globalControl.profile - 1];
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		UpdateRotationIcons();
		titleIconSR.sprite = settingsIcon;
		titleIconSR_s.sprite = settingsIcon;
		titleIconSR.color = titleColor;
		titleIconOverlaySR.sprite = null;
		if (currentMenu == "profile")
		{
			wheelController.ChangeSelectionAuto(0);
		}
		else if (currentMenu == "screen")
		{
			wheelController.ChangeSelectionAuto(1);
		}
		else if (currentMenu == "volume")
		{
			wheelController.ChangeSelectionAuto(3);
		}
		else if (currentMenu == "credits")
		{
			wheelController.ChangeSelectionAuto(4);
		}
		currentMenu = "settings";
	}

	private void Update()
	{
		if (currentMenu == "settings" && inputActive)
		{
			ManageInput();
		}
		if (callResetNextFrameInput)
		{
			ResetNextFrameInput();
		}
	}

	private void UpdateRotationIcons()
	{
		if (!(wheelController == null))
		{
			if (Screen.autorotateToPortrait)
			{
				wheelController.smallIconsOverlays[2] = smallRotationOverlays[1];
				wheelController.bigIconsOverlays[2] = bigRotationOverlays[1];
			}
			else
			{
				wheelController.smallIconsOverlays[2] = smallRotationOverlays[0];
				wheelController.bigIconsOverlays[2] = bigRotationOverlays[0];
			}
			wheelController.UpdateIcons();
		}
	}

	private void ManageInput()
	{
		if (!(wheelController == null) && !wheelController.selectionMade && !wheelController.moving)
		{
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
			{
				ManageAccept();
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				PlayCancel();
				BackToMMU();
			}
		}
	}

	private void ManageAccept()
	{
		if (wheelController.selection == 0)
		{
			currentMenu = "profile";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowProfileMenu", 1f / 6f);
		}
		else if (wheelController.selection == 1)
		{
			currentMenu = "screen";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowScreenMenu", 1f / 6f);
		}
		else if (wheelController.selection == 2)
		{
			PlayAccept();
			PauseController.pauseController.ToggleRotation();
			UpdateRotationIcons();
		}
		else if (wheelController.selection == 3)
		{
			currentMenu = "volume";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowVolumeMenu", 1f / 6f);
		}
		else if (wheelController.selection == 4)
		{
			currentMenu = "credits";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowCreditsMenu", 1f / 6f);
		}
		else if (wheelController.selection == 5 && !SceneChangeController.sceneChangeController.transitioning)
		{
			PlayAccept();
			BackToMMU();
		}
	}

	private void BackToMMU()
	{
		inputActive = false;
		currentMenu = string.Empty;
		GlobalControl.globalControl.changeMMUselectionTo = 5;
		SceneChangeController.sceneChangeController.LoadAndChangeScene("MMU", "title", 0f);
	}

	public void ResetNextFrameInput()
	{
		callResetNextFrameInput = false;
		wheelController.ResetNextFrameInput();
	}

	private void ShowProfileMenu()
	{
		inputActive = false;
		profileController.Initialize();
	}

	private void ShowScreenMenu()
	{
		inputActive = false;
		screenController.Initialize();
	}

	private void ShowVolumeMenu()
	{
		inputActive = false;
		volumeController.Initialize(false);
	}

	private void ShowCreditsMenu()
	{
		inputActive = false;
		creditsController.Initialize();
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayCancel()
	{
		GlobalAudio.globalAudio.PlayCancel();
	}
}
