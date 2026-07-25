using System.Collections.Generic;
using UnityEngine;

public class SET_ScreenResolutionController : MonoBehaviour
{
	public SETController controller;

	public SET_ScreenController screenController;

	public NumberWheelController numberWheelController;

	public GameObject numberWheel;

	public SpriteRenderer titleIconSR;

	public SpriteRenderer titleIconSR_s;

	public SpriteRenderer titleIconOverlaySR;

	public Color titleIconColor;

	public Sprite titleSprite;

	private bool moving;

	private bool selectionMade;

	private int nextFrameInput;

	private bool inputActive;

	private int originalSelection;

	public GameObject resolutionElements;

	public SpriteRenderer[] xNumbers;

	public SpriteRenderer[] xNumbers_s;

	public SpriteRenderer[] yNumbers;

	public SpriteRenderer[] yNumbers_s;

	private string s;

	private char[] charArray;

	public Sprite dot;

	public Resolution[] resolutions;

	public Resolution[] resolutionsTemp;

	public int resolutionSelection;

	private bool currentFullscreen;

	private Vector2 currentResolutionNumbers;

	private Vector2 currentResolution;

	private List<Resolution> pixelPerfectResolutions;

	private List<int> pixelPerfectNumbers;

	private Resolution newPixelPerfectResolution;

	private bool leftPressed;

	private float leftPressedTimer;

	private bool rightPressed;

	private float rightPressedTimer;

	private List<Resolution> resolutions_unrepeated;

	private Resolution firstResolution;

	private int monitorWidth;

	private int monitorHeight;

	private int pixelPerfectResolution;

	private int errorMargin;

	private int indexMaster;

	private int indexPerfect;

	private Resolution newRes;

	private int properRefreshRate;

	private Resolution newResolution;

	private float aspectRatio;

	private float aspectRatio2;

	private void Update()
	{
		ManageInput();
		ManageWindowResize();
	}

	public void Initialize()
	{
		selectionMade = false;
		numberWheel.SetActive(true);
		controller.wheelController.ResetNextFrameInput();
		numberWheelController.HideNumber();
		numberWheelController.ChangeToMinusPlus();
		titleIconSR.sprite = titleSprite;
		titleIconSR_s.sprite = titleSprite;
		titleIconSR.color = titleIconColor;
		titleIconOverlaySR.sprite = null;
		resolutionElements.SetActive(true);
		InitializeResolutions();
		UpdateInitialResolution();
		numberWheelController.TogglePlus(resolutionSelection != resolutions.Length - 1);
		numberWheelController.ToggleMinus(resolutionSelection != 0);
		Invoke("ActivateInput", 0.25f);
	}

	private void ManageInput()
	{
		if (!inputActive || controller.wheelController.moving)
		{
			return;
		}
		if (!moving && !selectionMade)
		{
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
			{
				selectionMade = true;
				inputActive = false;
				controller.ResetNextFrameInput();
				PlayAccept();
				Apply();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowScreenMenu", 1f / 6f);
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				selectionMade = true;
				inputActive = false;
				controller.ResetNextFrameInput();
				PlayCancel();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowScreenMenu", 1f / 6f);
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
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
					Minus();
				}
			}
			else if (rightPressed)
			{
				rightPressedTimer += Time.unscaledDeltaTime;
				if (rightPressedTimer >= 0.1f)
				{
					Plus();
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
				Plus();
			}
			else if (GlobalInput.globalInput.swipeScreenRight || GlobalInput.globalInput.touchButtonScreenLeft)
			{
				Minus();
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

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void Plus()
	{
		if (resolutionSelection < resolutions.Length - 1)
		{
			moving = true;
			Invoke("FalsifyMoving", 0.2f);
			RaiseResolution();
			numberWheelController.Plus();
		}
		numberWheelController.TogglePlus(resolutionSelection != resolutions.Length - 1);
		numberWheelController.ToggleMinus(resolutionSelection != 0);
		leftPressed = false;
		rightPressed = false;
	}

	private void Minus()
	{
		if (resolutionSelection > 0)
		{
			moving = true;
			Invoke("FalsifyMoving", 0.2f);
			LowerResolution();
			numberWheelController.Minus();
		}
		numberWheelController.ToggleMinus(resolutionSelection != 0);
		numberWheelController.TogglePlus(resolutionSelection != resolutions.Length - 1);
		leftPressed = false;
		rightPressed = false;
	}

	private void FalsifyMoving()
	{
		moving = false;
	}

	private void ShowScreenMenu()
	{
		inputActive = false;
		resolutionElements.SetActive(false);
		numberWheel.SetActive(false);
		screenController.Initialize();
	}

	public void LowerResolution()
	{
		resolutionSelection--;
		UpdateResolutionNumbersTo(resolutions[resolutionSelection].width, resolutions[resolutionSelection].height);
	}

	public void RaiseResolution()
	{
		resolutionSelection++;
		UpdateResolutionNumbersTo(resolutions[resolutionSelection].width, resolutions[resolutionSelection].height);
	}

	public void Apply()
	{
		Screen.SetResolution(resolutions[resolutionSelection].width, resolutions[resolutionSelection].height, Screen.fullScreen);
		PlayerPrefs.SetInt("resolutionX", resolutions[resolutionSelection].width);
		PlayerPrefs.SetInt("resolutionY", resolutions[resolutionSelection].height);
	}

	private void UpdateInitialResolution()
	{
		UpdateResolutionNumbersTo(Screen.width, Screen.height);
	}

	private void UpdateResolutionNumbersTo(int x, int y)
	{
		if (!(xNumbers[0] == null))
		{
			currentResolutionNumbers.x = x;
			currentResolutionNumbers.y = y;
			for (int i = 0; i < xNumbers.Length; i++)
			{
				xNumbers[i].sprite = null;
				yNumbers[i].sprite = null;
			}
			s = x + string.Empty;
			charArray = s.ToCharArray();
			if (charArray.Length == 1)
			{
				xNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			else if (charArray.Length == 2)
			{
				xNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				xNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else if (charArray.Length == 3)
			{
				xNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				xNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				xNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
			}
			else if (charArray.Length == 4)
			{
				xNumbers[1].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				xNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				xNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
				xNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[3])];
			}
			else if (charArray.Length == 5)
			{
				xNumbers[0].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				xNumbers[1].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				xNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
				xNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[3])];
				xNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[4])];
			}
			else
			{
				xNumbers[0].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				xNumbers[1].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				xNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
				xNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[3])];
				xNumbers[4].sprite = dot;
			}
			s = y + string.Empty;
			charArray = s.ToCharArray();
			if (charArray.Length == 1)
			{
				yNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			else if (charArray.Length == 2)
			{
				yNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				yNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else if (charArray.Length == 3)
			{
				yNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				yNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				yNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
			}
			else if (charArray.Length == 4)
			{
				yNumbers[1].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				yNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				yNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
				yNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[3])];
			}
			else if (charArray.Length == 5)
			{
				yNumbers[0].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				yNumbers[1].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				yNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
				yNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[3])];
				yNumbers[4].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[4])];
			}
			else
			{
				yNumbers[0].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[0])];
				yNumbers[1].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[1])];
				yNumbers[2].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[2])];
				yNumbers[3].sprite = controller.numberSprites[(int)char.GetNumericValue(charArray[3])];
				yNumbers[4].sprite = dot;
			}
			for (int j = 0; j < xNumbers.Length; j++)
			{
				xNumbers_s[j].sprite = xNumbers[j].sprite;
				yNumbers_s[j].sprite = yNumbers[j].sprite;
			}
		}
	}

	private void InitializeResolutions()
	{
		resolutions = Screen.resolutions;
		if (resolutions.Length == 0)
		{
			resolutions = new Resolution[1] { default(Resolution) };
			resolutions[0].width = Screen.width;
			resolutions[0].height = Screen.height;
		}
		properRefreshRate = Screen.currentResolution.refreshRate;
		if (properRefreshRate == 0)
		{
			for (int i = 0; i < resolutions.Length; i++)
			{
				if (resolutions[i].refreshRate != 0)
				{
					properRefreshRate = resolutions[i].refreshRate;
					break;
				}
			}
		}
		resolutions_unrepeated = new List<Resolution>();
		firstResolution = resolutions[0];
		if (properRefreshRate != 0)
		{
			firstResolution.refreshRate = properRefreshRate;
		}
		resolutions_unrepeated.Add(firstResolution);
		if (properRefreshRate != 0)
		{
			for (int j = 1; j < resolutions.Length; j++)
			{
				if (resolutions[j].width != resolutions[j - 1].width || resolutions[j].height != resolutions[j - 1].height)
				{
					newRes = resolutions[j];
					newRes.refreshRate = properRefreshRate;
					resolutions_unrepeated.Add(newRes);
				}
			}
			resolutions = new Resolution[resolutions_unrepeated.Count];
			for (int k = 0; k < resolutions.Length; k++)
			{
				resolutions[k] = resolutions_unrepeated[k];
			}
			if (Screen.fullScreen)
			{
				resolutions_unrepeated = new List<Resolution>();
				aspectRatio = (float)resolutions[resolutions.Length - 1].width / (float)resolutions[resolutions.Length - 1].height;
				for (int l = 0; l < resolutions.Length; l++)
				{
					aspectRatio2 = (float)resolutions[l].width / (float)resolutions[l].height;
					if (aspectRatio2 == aspectRatio)
					{
						resolutions_unrepeated.Add(resolutions[l]);
					}
				}
				resolutions = new Resolution[resolutions_unrepeated.Count];
				for (int m = 0; m < resolutions.Length; m++)
				{
					resolutions[m] = resolutions_unrepeated[m];
				}
			}
			pixelPerfectResolutions = new List<Resolution>();
			pixelPerfectNumbers = new List<int>();
			monitorWidth = resolutions[resolutions.Length - 1].width;
			monitorHeight = resolutions[resolutions.Length - 1].height;
			pixelPerfectResolution = 256;
			newPixelPerfectResolution = default(Resolution);
			if (properRefreshRate != 0)
			{
				newPixelPerfectResolution.refreshRate = properRefreshRate;
			}
			while (pixelPerfectResolution <= monitorHeight)
			{
				pixelPerfectNumbers.Add(pixelPerfectResolution);
				pixelPerfectResolution *= 2;
			}
			if (Screen.fullScreen)
			{
				aspectRatio = (float)resolutions[resolutions.Length - 1].width / (float)resolutions[resolutions.Length - 1].height;
				for (int n = 0; n < pixelPerfectNumbers.Count; n++)
				{
					newPixelPerfectResolution.height = pixelPerfectNumbers[n];
					newPixelPerfectResolution.width = Mathf.RoundToInt((float)newPixelPerfectResolution.height * aspectRatio);
					pixelPerfectResolutions.Add(newPixelPerfectResolution);
				}
			}
			else
			{
				for (int num = 0; num < pixelPerfectNumbers.Count; num++)
				{
					newPixelPerfectResolution.width = pixelPerfectNumbers[num];
					newPixelPerfectResolution.height = pixelPerfectNumbers[num];
					pixelPerfectResolutions.Add(newPixelPerfectResolution);
				}
			}
			for (int num2 = 0; num2 < resolutions.Length; num2++)
			{
				for (int num3 = 0; num3 < pixelPerfectResolutions.Count; num3++)
				{
					if (resolutions[num2].width == pixelPerfectResolutions[num3].width && resolutions[num2].height == pixelPerfectResolutions[num3].height)
					{
						pixelPerfectResolutions.Remove(pixelPerfectResolutions[num3]);
					}
				}
			}
			errorMargin = 0;
			if (pixelPerfectResolutions.Count > 0)
			{
				resolutionsTemp = new Resolution[resolutions.Length + pixelPerfectResolutions.Count];
				indexMaster = 0;
				indexPerfect = 0;
				for (int num4 = 0; num4 < resolutionsTemp.Length; num4++)
				{
					if (indexMaster < resolutions.Length && indexPerfect < pixelPerfectResolutions.Count)
					{
						if (resolutions[indexMaster].width < pixelPerfectResolutions[indexPerfect].width)
						{
							resolutionsTemp[num4] = resolutions[indexMaster];
							indexMaster++;
							continue;
						}
						if (resolutions[indexMaster].width > pixelPerfectResolutions[indexPerfect].width)
						{
							resolutionsTemp[num4] = pixelPerfectResolutions[indexPerfect];
							indexPerfect++;
							continue;
						}
						if (resolutions[indexMaster].height < pixelPerfectResolutions[indexPerfect].height)
						{
							resolutionsTemp[num4] = resolutions[indexMaster];
							indexMaster++;
							continue;
						}
						if (resolutions[indexMaster].height > pixelPerfectResolutions[indexPerfect].height)
						{
							resolutionsTemp[num4] = pixelPerfectResolutions[indexPerfect];
							indexPerfect++;
							continue;
						}
						Debug.Log("ERROR: Duplicated resolutions after removing duplicates.");
						resolutionsTemp[num4] = resolutions[indexMaster];
						indexMaster++;
						indexPerfect++;
					}
					else if (indexMaster < resolutions.Length && indexPerfect >= pixelPerfectResolutions.Count)
					{
						resolutionsTemp[num4] = resolutions[indexMaster];
						indexMaster++;
					}
					else if (indexMaster >= resolutions.Length && indexPerfect < pixelPerfectResolutions.Count)
					{
						resolutionsTemp[num4] = pixelPerfectResolutions[indexPerfect];
						indexPerfect++;
					}
					else
					{
						errorMargin++;
					}
				}
			}
			if (errorMargin > 0)
			{
				resolutions = resolutionsTemp;
				resolutionsTemp = new Resolution[resolutions.Length - errorMargin];
				for (int num5 = 0; num5 < resolutionsTemp.Length; num5++)
				{
					resolutionsTemp[num5] = resolutions[num5];
				}
			}
			resolutions = resolutionsTemp;
		}
		resolutionSelection = -1;
		for (int num6 = 0; num6 < resolutions.Length; num6++)
		{
			if (resolutions[num6].width == Screen.width && resolutions[num6].height == Screen.height)
			{
				resolutionSelection = num6;
			}
		}
		if (resolutionSelection != -1)
		{
			return;
		}
		int num7 = 0;
		bool flag = false;
		for (int num8 = 0; num8 < resolutions.Length; num8++)
		{
			if (resolutions[num8].width > Screen.width || (resolutions[num8].width == Screen.width && resolutions[num8].height > Screen.height))
			{
				num7 = num8;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			num7 = resolutions.Length;
		}
		resolutionsTemp = new Resolution[resolutions.Length + 1];
		for (int num9 = 0; num9 < resolutionsTemp.Length; num9++)
		{
			if (num9 < num7)
			{
				resolutionsTemp[num9] = resolutions[num9];
			}
			else if (num9 == num7)
			{
				newResolution = default(Resolution);
				newResolution.width = Screen.width;
				newResolution.height = Screen.height;
				if (properRefreshRate != 0)
				{
					newResolution.refreshRate = properRefreshRate;
				}
				resolutionsTemp[num9] = newResolution;
				resolutionSelection = num9;
			}
			else
			{
				resolutionsTemp[num9] = resolutions[num9 - 1];
			}
		}
		resolutions = resolutionsTemp;
	}

	private void ManageWindowResize()
	{
		if (currentResolution.x != (float)Screen.width || currentResolution.y != (float)Screen.height || currentFullscreen != Screen.fullScreen)
		{
			currentResolution = new Vector2(Screen.width, Screen.height);
			currentFullscreen = Screen.fullScreen;
			InitializeResolutions();
			UpdateResolutionNumbersTo(resolutions[resolutionSelection].width, resolutions[resolutionSelection].height);
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
}
