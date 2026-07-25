using UnityEngine;

public class PlayerPrefsLoader : MonoBehaviour
{
	private bool fullscreen;

	private void Start()
	{
		if (PlayerPrefs.GetInt("initialized") == 0)
		{
			InitialPrefs();
		}
		Invoke("ApplyPrefs", 0.5f);
	}

	private void InitialPrefs()
	{
		PlayerPrefs.SetInt("initialized", 1);
		PlayerPrefs.SetInt("musicVolume", 100);
		PlayerPrefs.SetInt("soundsVolume", 100);
		PlayerPrefs.SetInt("musicMute", 0);
		PlayerPrefs.SetInt("soundsMute", 0);
		if (GlobalControl.globalControl.mobile)
		{
			PlayerPrefs.SetInt("rotation", 1);
		}
		else
		{
			PlayerPrefs.SetInt("rotation", 0);
		}
		PlayerPrefs.SetInt("fullscreen", 1);
		PlayerPrefs.SetInt("resolutionX", Screen.currentResolution.width);
		PlayerPrefs.SetInt("resolutionY", Screen.currentResolution.height);
		PlayerPrefs.SetInt("buttons", 1);
	}

	private void ApplyPrefs()
	{
		Invoke("ApplyRotation", 0.5f);
		GlobalAudio.globalAudio.UpdateMusicVolumeTo(PlayerPrefs.GetInt("musicVolume"));
		GlobalAudio.globalAudio.UpdateSoundsVolumeTo(PlayerPrefs.GetInt("soundsVolume"));
		if (PlayerPrefs.GetInt("musicMute") != 0)
		{
			GlobalAudio.globalAudio.MuteMusic();
		}
		if (PlayerPrefs.GetInt("soundsMute") != 0)
		{
			GlobalAudio.globalAudio.MuteSounds();
		}
		if (PlayerPrefs.GetInt("buttons") == 0 && !GlobalControl.globalControl.mobile)
		{
			ButtonLeftController.buttonLeftController.HideButton();
			ButtonLeftController.buttonLeftController.ShowButton();
		}
		if (PlayerPrefs.GetInt("fullscreen") == 0 && PlayerPrefs.GetInt("initialized") != 0)
		{
			fullscreen = false;
		}
		else
		{
			fullscreen = true;
		}
		if (!GlobalControl.globalControl.mobile)
		{
			int num = PlayerPrefs.GetInt("resolutionX");
			int num2 = PlayerPrefs.GetInt("resolutionY");
			if (num > Screen.currentResolution.width)
			{
				num = Screen.currentResolution.width;
				PlayerPrefs.SetInt("resolutionX", num);
			}
			if (num2 > Screen.currentResolution.height)
			{
				num2 = Screen.currentResolution.height;
				PlayerPrefs.SetInt("resolutionY", num2);
			}
			Screen.SetResolution(num, num2, fullscreen);
		}
		if (PlayerPrefs.GetInt("buttons") == 0)
		{
			ButtonLeftController.buttonLeftController.HideButton();
			ButtonRightController.buttonRightController.HideButton();
		}
		else
		{
			ButtonLeftController.buttonLeftController.ShowButton();
			ButtonRightController.buttonRightController.ShowButton();
		}
		ScreenResolutionController.screenResolutionController.updateResolutionNextFrame = true;
	}

	private void ApplyRotation()
	{
		if (PlayerPrefs.GetInt("rotation") == 1)
		{
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
		}
		else
		{
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
		}
		PauseController.pauseController.UpdateRotationIcons();
	}
}
