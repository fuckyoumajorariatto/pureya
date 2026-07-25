using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SET_CreditsController : MonoBehaviour
{
	public SETController SETcontroller;

	public NumberWheelController numberWheelController;

	public GameObject numberWheel;

	public Transform numberWheelTR;

	public GameObject creditsElements;

	public SpriteRenderer titleIconSR;

	public SpriteRenderer titleIconSR_s;

	public SpriteRenderer titleIconOverlaySR;

	private bool moving;

	private bool selectionMade;

	private int nextFrameInput;

	private bool inputActive;

	public int currentPage;

	private int lariatPage = 5;

	public Text creditsText;

	public SpriteRenderer[] numbers;

	public SpriteRenderer[] numbers_s;

	public GameObject title;

	public SpriteRenderer[] icons;

	public SpriteRenderer[] icons_s;

	public Sprite[] iconSprites;

	public Sprite[] iconSprites_s;

	public List<string> credits;

	private List<string[]> creditsIcons;

	private List<string> nameList;

	private string s;

	private char[] charArray;

	private Color32 white = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private Color32 black = new Color32(0, 0, 0, byte.MaxValue);

	private Color32 red = new Color32(byte.MaxValue, 85, 90, byte.MaxValue);

	private bool leftPressed;

	private float leftPressedTimer;

	private bool rightPressed;

	private float rightPressedTimer;

	private int lariatCounter;

	private bool activateSecretIfAccept;

	public AudioClip secretClip;

	private void Update()
	{
		ManageInput();
	}

	public void Initialize()
	{
		Invoke("ActivateInput", 0.25f);
		selectionMade = false;
		InitializeCredits();
		InitializeIcons();
		InitializePageNumbers();
		UpdatePage();
		titleIconSR.sprite = null;
		titleIconSR_s.sprite = null;
		titleIconOverlaySR.sprite = null;
		creditsElements.SetActive(true);
		SETcontroller.wheel.SetActive(false);
		SETcontroller.wheelController.ResetNextFrameInput();
		numberWheel.SetActive(true);
		numberWheelController.ChangeToLeftRight();
		numberWheelController.HideNumber();
		numberWheelController.TogglePlus(true);
		numberWheelController.ToggleMinus(true);
		numberWheelTR.position = new Vector3(numberWheelTR.position.x, -2f, numberWheelTR.position.z);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void ManageInput()
	{
		if (!inputActive)
		{
			return;
		}
		if (!moving && !selectionMade)
		{
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
			{
				inputActive = false;
				PlayAccept();
				if (currentPage == lariatPage)
				{
					if (activateSecretIfAccept)
					{
						Invoke("UnlockAll", 1f / 6f);
					}
					else
					{
						lariatCounter++;
					}
				}
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowSettingsMenu", 1f / 6f);
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
				selectionMade = true;
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				inputActive = false;
				PlayCancel();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowSettingsMenu", 1f / 6f);
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
				selectionMade = true;
			}
			else if ((leftPressed && rightPressed) || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || (GlobalInput.globalInput.touchButtonScreenLeft && GlobalInput.globalInput.touchButtonScreenRight))
			{
				leftPressed = false;
				rightPressed = false;
			}
			else if (leftPressed)
			{
				leftPressedTimer += Time.unscaledDeltaTime;
				if (leftPressedTimer >= 0.1f)
				{
					moving = true;
					Invoke("FalsifyMoving", 0.2f);
					PreviousPage();
					leftPressed = false;
					rightPressed = false;
				}
			}
			else if (rightPressed)
			{
				rightPressedTimer += Time.unscaledDeltaTime;
				if (rightPressedTimer >= 0.1f)
				{
					moving = true;
					Invoke("FalsifyMoving", 0.2f);
					NextPage();
					leftPressed = false;
					rightPressed = false;
				}
			}
			else if (GlobalInput.globalInput.left || nextFrameInput == 1)
			{
				leftPressed = true;
			}
			else if (GlobalInput.globalInput.right || nextFrameInput == 2)
			{
				rightPressed = true;
			}
			else if (GlobalInput.globalInput.swipeScreenLeft || GlobalInput.globalInput.touchButtonScreenRight)
			{
				moving = true;
				Invoke("FalsifyMoving", 0.2f);
				NextPage();
				leftPressed = false;
				rightPressed = false;
			}
			else if (GlobalInput.globalInput.swipeScreenRight || GlobalInput.globalInput.touchButtonScreenLeft)
			{
				moving = true;
				Invoke("FalsifyMoving", 0.2f);
				PreviousPage();
				leftPressed = false;
				rightPressed = false;
			}
			else
			{
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
			}
			nextFrameInput = 0;
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
			{
				leftPressed = false;
				rightPressed = false;
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				leftPressed = false;
				rightPressed = false;
			}
		}
		else if ((GlobalInput.globalInput.rightDown && !GlobalInput.globalInput.left) || GlobalInput.globalInput.touchButtonScreenRight)
		{
			nextFrameInput = 2;
		}
		else if ((GlobalInput.globalInput.leftDown && !GlobalInput.globalInput.right) || GlobalInput.globalInput.touchButtonScreenLeft)
		{
			nextFrameInput = 1;
		}
	}

	private void FalsifyMoving()
	{
		moving = false;
	}

	private void ShowSettingsMenu()
	{
		inputActive = false;
		SETcontroller.ResetNextFrameInput();
		numberWheelTR.position = new Vector3(numberWheelTR.position.x, -0.5f, numberWheelTR.position.z);
		numberWheel.SetActive(false);
		creditsElements.SetActive(false);
		activateSecretIfAccept = false;
		SETcontroller.Initialize();
	}

	private void UnlockAll()
	{
		GlobalControl.globalControl.UnlockAllCheat();
		PlaySecretClip();
	}

	public void InitializeCredits()
	{
		credits = new List<string>();
		creditsIcons = new List<string[]>();
		credits.Add("pureya\n(v" + GlobalControl.globalControl.version + ")");
		creditsIcons.Add(new string[3] { "title", "title", "title" });
		credits.Add("Alva Majo");
		creditsIcons.Add(new string[3] { "gameplay", "code", "art" });
		credits.Add("Alejandro Maciá");
		creditsIcons.Add(new string[3] { "null", "music", "null" });
		credits.Add("Charditronic");
		creditsIcons.Add(new string[3] { "null", "art", "null" });
		credits.Add("Lariat");
		lariatPage = credits.Count;
		creditsIcons.Add(new string[3] { "majorariatto", "heart", "majorariatto" });
		credits.Add("Kony Pyon");
		creditsIcons.Add(new string[3] { "majorariatto", "heart", "majorariatto" });
		credits.Add("C. Navarro");
		creditsIcons.Add(new string[3] { "majorariatto", "heart", "majorariatto" });
		credits.Add("Joe Firth\nIván Gregorio\nDEValen");
		creditsIcons.Add(new string[3] { "null", "heart", "null" });
		credits.Add("Unity Technologies");
		creditsIcons.Add(new string[3] { "null", "unity", "null" });
		credits.Add("sonniss.com\nsoundopolis.net\nSound Ex Machina");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("Dustyroom\nObsydianX\nKenney Vleugels");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("Imphenzia\nSoundBits\nGameDevMarket");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("BLACK LODGE GAMES, LLC");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("Double Trouble Audio");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("Mike Koenig\nPsychoBird\n(soundible.com)");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("Daniel Simion\n(soundible.com)");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		credits.Add("THOMAS REX BEVERLY");
		creditsIcons.Add(new string[3] { "null", "sound", "null" });
		nameList = new List<string>();
		nameList.Add("7h3_lark");
		nameList.Add("ani_music");
		nameList.Add("betterchinese");
		nameList.Add("contramundum");
		nameList.Add("davidworksonline");
		nameList.Add("driftworks");
		nameList.Add("Erokia");
		nameList.Add("EverHeat");
		nameList.Add("Hssmusic");
		nameList.Add("Hybrid_V");
		nameList.Add("InspectorJ");
		nameList.Add("martypinso");
		nameList.Add("musita182");
		nameList.Add("plingativator");
		nameList.Add("Ramjac");
		nameList.Add("Rvgerxini");
		nameList.Add("sl130594");
		nameList.Add("SoundCollectah");
		nameList.Add("SpliceSound");
		nameList.Add("themfish");
		nameList.Add("TurboFool");
		nameList.Add("twinpix");
		nameList.Add("vacuumfan7072");
		nameList.Add("WiesenWiesel");
		nameList.Add("Yarmonics");
		nameList.Add("ZeSoundResearchInc.");
		int num = 0;
		string text = string.Empty;
		for (int i = 0; i < nameList.Count; i++)
		{
			switch (num)
			{
			case 0:
				if (i < nameList.Count - 1 && nameList[i].Length < 16)
				{
					text += nameList[i];
					num++;
					break;
				}
				text += nameList[i].Substring(0, 8);
				text += "-\n";
				text += nameList[i].Substring(8, nameList[i].Length - 9);
				text += "\n(freesound.org)";
				credits.Add(text);
				creditsIcons.Add(new string[3] { "null", "sound", "null" });
				num = 0;
				text = string.Empty;
				break;
			case 1:
				if (nameList[i].Length < 16)
				{
					text = text + "\n" + nameList[i];
					text += "\n(freesound.org)";
					credits.Add(text);
					creditsIcons.Add(new string[3] { "null", "sound", "null" });
					num = 0;
					text = string.Empty;
					break;
				}
				text += "\n(freesound.org)";
				credits.Add(text);
				creditsIcons.Add(new string[3] { "null", "sound", "null" });
				text = string.Empty;
				text += nameList[i].Substring(0, 8);
				text += "-\n";
				text += nameList[i].Substring(8, nameList[i].Length - 9);
				text += "\n(freesound.org)";
				credits.Add(text);
				creditsIcons.Add(new string[3] { "null", "sound", "null" });
				num = 0;
				text = string.Empty;
				break;
			}
		}
		credits.Add("Majorariatto 2021");
		creditsIcons.Add(new string[3] { "null", "majorariatto", "null" });
	}

	public void InitializeIcons()
	{
		if (title != null)
		{
			title.SetActive(true);
		}
		for (int i = 0; i < 3; i++)
		{
			icons[i].sprite = null;
			icons_s[i].sprite = null;
		}
	}

	private void ManageIcons(string[] iconsString)
	{
		if (iconsString[0] == "title")
		{
			InitializeIcons();
			return;
		}
		if (title != null)
		{
			title.SetActive(false);
		}
		for (int i = 0; i < 3; i++)
		{
			if (iconsString[i] == "null")
			{
				icons[i].sprite = null;
				icons_s[i].sprite = null;
			}
			else if (iconsString[i] == "majorariatto")
			{
				icons[i].sprite = iconSprites[0];
				icons[i].color = white;
				icons_s[i].sprite = iconSprites_s[0];
			}
			else if (iconsString[i] == "gameplay")
			{
				icons[i].sprite = iconSprites[1];
				icons[i].color = white;
				icons_s[i].sprite = iconSprites_s[1];
			}
			else if (iconsString[i] == "code")
			{
				icons[i].sprite = iconSprites[2];
				icons[i].color = white;
				icons_s[i].sprite = iconSprites_s[2];
			}
			else if (iconsString[i] == "art")
			{
				icons[i].sprite = iconSprites[3];
				icons[i].color = white;
				icons_s[i].sprite = iconSprites_s[3];
			}
			else if (iconsString[i] == "heart")
			{
				icons[i].sprite = iconSprites[4];
				icons[i].color = red;
				icons_s[i].sprite = iconSprites_s[4];
			}
			else if (iconsString[i] == "unity")
			{
				icons[i].sprite = iconSprites[5];
				icons[i].color = white;
				icons_s[i].sprite = iconSprites_s[5];
			}
			else if (iconsString[i] == "music")
			{
				icons[i].sprite = iconSprites[6];
				icons[i].color = black;
				icons_s[i].sprite = iconSprites_s[6];
			}
			else if (iconsString[i] == "sound")
			{
				icons[i].sprite = iconSprites[7];
				icons[i].color = black;
				icons_s[i].sprite = iconSprites_s[7];
			}
		}
	}

	public void InitializePageNumbers()
	{
		currentPage = 1;
		if (!(numbers[0] == null))
		{
			for (int i = 0; i < numbers.Length; i++)
			{
				numbers[i].sprite = SETcontroller.numberSprites[0];
				numbers_s[i].sprite = numbers[i].sprite;
			}
			numbers[1].sprite = SETcontroller.numberSprites[1];
			s = credits.Count + string.Empty;
			charArray = s.ToCharArray();
			if (charArray.Length == 1)
			{
				numbers[2].sprite = SETcontroller.numberSprites[0];
				numbers[3].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			else if (charArray.Length == 2)
			{
				numbers[2].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				numbers[3].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			for (int j = 0; j < numbers.Length; j++)
			{
				numbers_s[j].sprite = numbers[j].sprite;
			}
		}
	}

	public void NextPage()
	{
		currentPage++;
		if (currentPage > credits.Count)
		{
			currentPage = 1;
		}
		UpdatePage();
		activateSecretIfAccept = false;
		ShowSecretUnlock();
		if (numberWheelController != null)
		{
			numberWheelController.Plus();
		}
	}

	private void ShowSecretUnlock()
	{
		if (currentPage == lariatPage && lariatCounter == 3)
		{
			lariatCounter = 0;
			activateSecretIfAccept = true;
			icons[0].sprite = iconSprites[8];
			icons[0].color = new Color32(byte.MaxValue, 85, 85, byte.MaxValue);
			icons_s[0].sprite = iconSprites_s[8];
			icons[1].sprite = iconSprites[9];
			icons[1].color = white;
			icons_s[1].sprite = iconSprites_s[9];
			icons[2].sprite = iconSprites[10];
			icons[2].color = new Color32(byte.MaxValue, 85, 85, byte.MaxValue);
			icons_s[2].sprite = iconSprites_s[10];
		}
	}

	public void PreviousPage()
	{
		currentPage--;
		if (currentPage < 1)
		{
			currentPage = credits.Count;
		}
		UpdatePage();
		activateSecretIfAccept = false;
		numberWheelController.Minus();
	}

	public void UpdatePage()
	{
		creditsText.text = credits[currentPage - 1];
		s = currentPage + string.Empty;
		charArray = s.ToCharArray();
		ManageIcons(creditsIcons[currentPage - 1]);
		if (!(numbers[0] == null))
		{
			if (currentPage > 9)
			{
				numbers[0].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				numbers[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else
			{
				numbers[0].sprite = SETcontroller.numberSprites[0];
				numbers[1].sprite = SETcontroller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			numbers_s[0].sprite = numbers[0].sprite;
			numbers_s[1].sprite = numbers[1].sprite;
		}
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayCancel()
	{
		GlobalAudio.globalAudio.PlayCancel();
	}

	private void PlaySecretClip()
	{
		GlobalAudio.globalAudio.Play(secretClip, 0.9f, 1f, 0f);
	}
}
