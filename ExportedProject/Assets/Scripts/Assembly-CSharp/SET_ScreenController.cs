using UnityEngine;

public class SET_ScreenController : MonoBehaviour
{
	public SETController SETcontroller;

	public SET_ScreenResolutionController resolutionController;

	public WheelController wheelController;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public Sprite[] fsSmallIcons;

	public Sprite[] fsBigIcons;

	public Sprite[] buttonsSmallIcons;

	public Sprite[] buttonsBigIcons;

	public SpriteRenderer titleIcon;

	public SpriteRenderer titleIcon_s;

	public SpriteRenderer titleIconOverlay;

	public Sprite titleIconSprite;

	public Sprite titleIconOverlaySprite;

	public Color titleIconColor;

	public Color titleIconOverlayColor;

	private bool inputActive;

	private bool fullscreen;

	private void Update()
	{
		ManageAccept();
		if (inputActive && fullscreen != Screen.fullScreen)
		{
			fullscreen = Screen.fullScreen;
			UpdateResolutionIcons();
		}
	}

	public void Initialize()
	{
		SETcontroller.wheel.SetActive(true);
		wheelController.ResetNextFrameInput();
		SETcontroller.wheelController.ChangeSelectionAuto(0);
		titleIcon.sprite = titleIconSprite;
		titleIcon_s.sprite = titleIconSprite;
		titleIcon.color = titleIconColor;
		titleIconOverlay.sprite = titleIconOverlaySprite;
		titleIconOverlay.color = titleIconOverlayColor;
		optionsToSkip = new bool[bigIcons.Length];
		SETcontroller.wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, null, iconColors, optionsToSkip);
		UpdateResolutionIcons();
		UpdateButtonsIcons();
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void UpdateResolutionIcons()
	{
		if (PlayerPrefs.GetInt("fullscreen") == 1)
		{
			fullscreen = true;
			wheelController.smallIcons[1] = fsSmallIcons[1];
			wheelController.bigIcons[1] = fsBigIcons[1];
		}
		else
		{
			fullscreen = false;
			wheelController.smallIcons[1] = fsSmallIcons[0];
			wheelController.bigIcons[1] = fsBigIcons[0];
		}
		wheelController.UpdateIcons();
	}

	private void UpdateButtonsIcons()
	{
		if (PlayerPrefs.GetInt("buttons") == 1)
		{
			wheelController.smallIcons[2] = buttonsSmallIcons[1];
			wheelController.bigIcons[2] = buttonsBigIcons[1];
		}
		else
		{
			wheelController.smallIcons[2] = buttonsSmallIcons[0];
			wheelController.bigIcons[2] = buttonsBigIcons[0];
		}
		wheelController.UpdateIcons();
	}

	private void ManageAccept()
	{
		if (!inputActive || wheelController.moving)
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			if (SETcontroller.wheelController.selection == 0)
			{
				inputActive = false;
				SETcontroller.ResetNextFrameInput();
				PlayAccept();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowResolutionSelection", 1f / 6f);
			}
			else if (SETcontroller.wheelController.selection == 1)
			{
				inputActive = false;
				SETcontroller.ResetNextFrameInput();
				PlayAccept();
				ToggleFullScreen();
				Invoke("ReactivateInput", 0.1f);
			}
			else if (SETcontroller.wheelController.selection == 2)
			{
				inputActive = false;
				SETcontroller.ResetNextFrameInput();
				PlayAccept();
				ToggleShowButtons();
				Invoke("ReactivateInput", 0.1f);
			}
			else if (SETcontroller.wheelController.selection == 3)
			{
				inputActive = false;
				PlayAccept();
				BackToSettings();
			}
		}
		else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
		{
			inputActive = false;
			PlayCancel();
			BackToSettings();
		}
	}

	private void ReactivateInput()
	{
		inputActive = true;
	}

	private void ShowResolutionSelection()
	{
		SETcontroller.wheel.SetActive(false);
		resolutionController.Initialize();
	}

	private void BackToSettings()
	{
		inputActive = false;
		SETcontroller.ResetNextFrameInput();
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("ShowSettingsMenu", 1f / 6f);
	}

	private void ShowSettingsMenu()
	{
		SETcontroller.Initialize();
	}

	private void ToggleFullScreen()
	{
		if (Screen.fullScreen)
		{
			Screen.fullScreen = false;
			PlayerPrefs.SetInt("fullscreen", 0);
		}
		else
		{
			Screen.fullScreen = true;
			PlayerPrefs.SetInt("fullscreen", 1);
		}
		UpdateResolutionIcons();
	}

	private void ToggleShowButtons()
	{
		if (PlayerPrefs.GetInt("buttons") == 1)
		{
			PlayerPrefs.SetInt("buttons", 0);
			ButtonLeftController.buttonLeftController.HideButton();
			ButtonRightController.buttonRightController.HideButton();
		}
		else
		{
			PlayerPrefs.SetInt("buttons", 1);
			ButtonLeftController.buttonLeftController.ShowButton();
			ButtonRightController.buttonRightController.ShowButton();
		}
		UpdateButtonsIcons();
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
