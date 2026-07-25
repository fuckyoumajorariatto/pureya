using UnityEngine;

public class SET_ProfileController : MonoBehaviour
{
	private int playtime;

	private int minutes;

	private int hours;

	private string playtimeString;

	private char[] charArray;

	public SETController SETcontroller;

	public SET_ProfileSelectionController profileSelectionController;

	public GameObject[] playtimes;

	public SpriteRenderer[] playtimeNumbers;

	public SpriteRenderer[] playtimeNumbers_s;

	public SpriteRenderer[] playtimeNumbers_big;

	public SpriteRenderer[] playtimeNumbers_s_big;

	private int finalProgress;

	public SpriteRenderer[] progressNumbers;

	public SpriteRenderer[] progressNumbers_s;

	public GameObject deleteIcon;

	public SpriteRenderer deleteCounter;

	public GameObject deleteCounterGO;

	public int deleteCount;

	private Color32 white = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private Color32 yellow = new Color32(byte.MaxValue, 237, 0, byte.MaxValue);

	private Color32 red = new Color32(byte.MaxValue, 62, 62, byte.MaxValue);

	public Animator[] arrows;

	public SpriteRenderer[] profileNumber;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Color[] iconColors;

	public bool[] optionsToSkip;

	public SpriteRenderer titleIcon;

	public SpriteRenderer titleIcon_s;

	public SpriteRenderer titleIconOverlay;

	public Sprite titleIconSprite;

	public Color titleIconColor;

	public GameObject profileElements;

	public SET_DeleteSelectionController deleteSelectionController;

	private bool inputActive;

	private int selection;

	public void Initialize()
	{
		GlobalControl.globalControl.LoadData();
		SETcontroller.wheel.SetActive(true);
		SETcontroller.wheelController.ResetNextFrameInput();
		SETcontroller.wheelController.ChangeSelectionAuto(0);
		titleIcon.sprite = titleIconSprite;
		titleIcon_s.sprite = titleIconSprite;
		titleIcon.color = titleIconColor;
		titleIconOverlay.sprite = SETcontroller.profileNumbersBig[GlobalControl.globalControl.profile - 1];
		optionsToSkip = new bool[bigIcons.Length];
		SETcontroller.wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, null, iconColors, optionsToSkip);
		profileElements.SetActive(true);
		UpdateProfileNumber();
		UpdatePlaytime();
		UpdateProgress();
		ResetDeleteIcon();
		SETcontroller.currentMenu = "profile";
		Invoke("ActivateInput", 0.5f);
	}

	private void Update()
	{
		ManageAccept();
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void ManageAccept()
	{
		if (!inputActive || SETcontroller.wheelController.moving)
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			inputActive = false;
			if (SETcontroller.wheelController.selection == 0)
			{
				PlayAccept();
				SETcontroller.ResetNextFrameInput();
				SceneChangeController.sceneChangeController.ManualTransitionInOutAndroidCloud2("title", 0f);
				Invoke("ShowProfileSelection", 1f / 6f);
			}
			else if (SETcontroller.wheelController.selection == 1)
			{
				PlayAccept();
				SETcontroller.ResetNextFrameInput();
				SceneChangeController.sceneChangeController.ManualTransitionInOutAndroidCloud2("title", 0f);
				Invoke("ShowDeleteSelection", 1f / 6f);
			}
			else if (SETcontroller.wheelController.selection == 2)
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

	private void ShowProfileSelection()
	{
		SETcontroller.wheel.SetActive(false);
		profileSelectionController.Initialize();
	}

	private void BackToSettings()
	{
		SETcontroller.ResetNextFrameInput();
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("ShowSettingsMenu", 1f / 6f);
	}

	private void ShowSettingsMenu()
	{
		profileElements.SetActive(false);
		SETcontroller.Initialize();
	}

	private void ShowDeleteSelection()
	{
		deleteSelectionController.Initialize();
	}

	private void UpdateProfileNumber()
	{
		profileNumber[0].sprite = SETcontroller.numberSprites[GlobalControl.globalControl.profile];
		profileNumber[1].sprite = profileNumber[0].sprite;
	}

	public void UpdateProgress()
	{
		finalProgress = GlobalControl.globalControl.CalculateProgress();
		playtimeString = finalProgress + string.Empty;
		charArray = playtimeString.ToCharArray();
		progressNumbers[0].sprite = SETcontroller.numberSprites[0];
		progressNumbers[1].sprite = SETcontroller.numberSprites[0];
		progressNumbers[2].sprite = SETcontroller.numberSprites[0];
		if (finalProgress > 0)
		{
			if (finalProgress < 10)
			{
				progressNumbers[2].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			else if (finalProgress < 100)
			{
				progressNumbers[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				progressNumbers[2].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else
			{
				GlobalControl.globalControl._999 = true;
				progressNumbers[0].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				progressNumbers[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				progressNumbers[2].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[2])];
			}
		}
		progressNumbers_s[0].sprite = progressNumbers[0].sprite;
		progressNumbers_s[1].sprite = progressNumbers[1].sprite;
		progressNumbers_s[2].sprite = progressNumbers[2].sprite;
	}

	private void UpdatePlaytime()
	{
		playtime = GlobalControl.globalControl.playtime;
		if (playtime < 60 && GlobalControl.globalControl.unlockedMinigames.Count > 0)
		{
			playtime = 60;
		}
		if (playtime > 3599940)
		{
			playtime = 3599940;
		}
		minutes = playtime / 60;
		hours = 0;
		while (minutes > 59)
		{
			minutes -= 60;
			hours++;
		}
		if (hours < 100)
		{
			playtimes[0].SetActive(true);
			playtimes[1].SetActive(false);
			playtimeString = minutes + string.Empty;
			charArray = playtimeString.ToCharArray();
			if (minutes > 9)
			{
				playtimeNumbers[2].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				playtimeNumbers[3].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else
			{
				playtimeNumbers[2].sprite = SETcontroller.numberSprites[0];
				playtimeNumbers[3].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			playtimeNumbers_s[2].sprite = playtimeNumbers[2].sprite;
			playtimeNumbers_s[3].sprite = playtimeNumbers[3].sprite;
			playtimeString = hours + string.Empty;
			charArray = playtimeString.ToCharArray();
			if (hours > 9)
			{
				playtimeNumbers[0].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				playtimeNumbers[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else
			{
				playtimeNumbers[0].sprite = SETcontroller.numberSprites[0];
				playtimeNumbers[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			playtimeNumbers_s[0].sprite = playtimeNumbers[0].sprite;
			playtimeNumbers_s[1].sprite = playtimeNumbers[1].sprite;
		}
		else
		{
			playtimes[1].SetActive(true);
			playtimes[0].SetActive(false);
			playtimeString = minutes + string.Empty;
			charArray = playtimeString.ToCharArray();
			if (minutes > 9)
			{
				playtimeNumbers_big[3].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				playtimeNumbers_big[4].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else
			{
				playtimeNumbers_big[3].sprite = SETcontroller.numberSprites[0];
				playtimeNumbers_big[4].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			playtimeNumbers_s_big[3].sprite = playtimeNumbers_big[3].sprite;
			playtimeNumbers_s_big[4].sprite = playtimeNumbers_big[4].sprite;
			playtimeString = hours + string.Empty;
			charArray = playtimeString.ToCharArray();
			playtimeNumbers_big[0].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			playtimeNumbers_big[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			playtimeNumbers_big[2].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[2])];
			playtimeNumbers_s_big[0].sprite = playtimeNumbers_big[0].sprite;
			playtimeNumbers_s_big[1].sprite = playtimeNumbers_big[1].sprite;
			playtimeNumbers_s_big[2].sprite = playtimeNumbers_big[2].sprite;
		}
	}

	public void UpdateProfile()
	{
		GlobalControl.globalControl.LoadData();
		UpdateProfileNumber();
		UpdatePlaytime();
		UpdateProgress();
	}

	public void Delete()
	{
		if (deleteCount == 4)
		{
			deleteCounterGO.SetActive(true);
			deleteCount--;
			deleteIcon.transform.position = new Vector3(0f, -2.6875f, 0f);
			deleteCounterGO.transform.localScale = new Vector2(1f, 1f);
			deleteCounter.sprite = SETcontroller.numberSprites[3];
			deleteCounter.color = white;
		}
		else if (deleteCount == 3)
		{
			deleteCount--;
			deleteIcon.transform.position = new Vector3(2.6875f, -2.6875f, 0f);
			deleteCounterGO.transform.localScale = new Vector2(1.5f, 1.5f);
			deleteCounter.sprite = SETcontroller.numberSprites[2];
			deleteCounter.color = yellow;
		}
		else if (deleteCount == 2)
		{
			deleteCount--;
			deleteIcon.transform.position = new Vector3(0f, -2.6875f, 0f);
			deleteCounterGO.transform.localScale = new Vector2(2f, 2f);
			deleteCounter.sprite = SETcontroller.numberSprites[1];
			deleteCounter.color = red;
		}
		else if (deleteCount == 1)
		{
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0.5f);
			Invoke("ResetSave", 1f / 6f);
		}
	}

	public void ResetDeleteIcon()
	{
		deleteCount = 4;
		deleteIcon.transform.position = new Vector3(2.6875f, -2.6875f, 0f);
		deleteCounterGO.transform.localScale = new Vector2(1f, 1f);
		deleteCounterGO.SetActive(false);
	}

	public void ResetSave()
	{
		Initialize();
		GlobalControl.globalControl.ResetData();
		GlobalControl.globalControl.LoadData();
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
