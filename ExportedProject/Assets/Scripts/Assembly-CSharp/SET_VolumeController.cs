using UnityEngine;

public class SET_VolumeController : MonoBehaviour
{
	public SETController SETcontroller;

	public SET_MusicVolumeController musicController;

	public SET_SoundsVolumeController soundsController;

	public WheelController wheelController;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public SpriteRenderer titleIcon;

	public SpriteRenderer titleIcon_s;

	public SpriteRenderer titleIconOverlay;

	public Sprite titleIconSprite;

	public Color titleIconColor;

	private bool inputActive;

	private void Update()
	{
		ManageAccept();
	}

	public void Initialize(bool comingBackFromSounds)
	{
		SETcontroller.wheel.SetActive(true);
		wheelController.ResetNextFrameInput();
		if (comingBackFromSounds)
		{
			wheelController.ChangeSelectionAuto(1);
		}
		else
		{
			wheelController.ChangeSelectionAuto(0);
		}
		titleIcon.sprite = titleIconSprite;
		titleIcon_s.sprite = titleIconSprite;
		titleIcon.color = titleIconColor;
		titleIconOverlay.sprite = null;
		optionsToSkip = new bool[bigIcons.Length];
		SETcontroller.wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, null, iconColors, optionsToSkip);
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void ManageAccept()
	{
		if (inputActive && !wheelController.moving)
		{
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
			{
				DoAccept();
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				inputActive = false;
				PlayCancel();
				BackToSettings();
			}
		}
	}

	private void DoAccept()
	{
		inputActive = false;
		SETcontroller.ResetNextFrameInput();
		if (wheelController.selection == 0)
		{
			PlayAccept();
			SETcontroller.ResetNextFrameInput();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowMusicVolumeSelection", 1f / 6f);
		}
		else if (wheelController.selection == 1)
		{
			PlayAccept();
			SETcontroller.ResetNextFrameInput();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowSoundsVolumeSelection", 1f / 6f);
		}
		else if (wheelController.selection == 2)
		{
			inputActive = false;
			PlayAccept();
			BackToSettings();
		}
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

	private void ShowMusicVolumeSelection()
	{
		SETcontroller.wheel.SetActive(false);
		musicController.Initialize();
	}

	private void ShowSoundsVolumeSelection()
	{
		SETcontroller.wheel.SetActive(false);
		soundsController.Initialize();
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
