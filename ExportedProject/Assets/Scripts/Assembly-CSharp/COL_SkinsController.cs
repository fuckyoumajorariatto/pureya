using System.Collections.Generic;
using UnityEngine;

public class COL_SkinsController : MonoBehaviour
{
	public PCK_UnlockAssets PCK_unlockAssets;

	public Dictionary<string, Sprite[]> skinsDictionary;

	public Dictionary<string, int[]> skinsCollection;

	public List<string> minigames;

	public int currentMinigame;

	public GameObject[] skinsGO;

	public Transform[] skinsTR;

	public SpriteRenderer[] skinsSR;

	public SpriteRenderer[] skinsSR_s;

	public SpriteRenderer[] skinsNumbers;

	public GameObject[] skinsNumbersGO;

	public GameObject[] medalsGO;

	public SpriteRenderer[] enabledIconsSR;

	public Color[] enabledColors;

	private Dictionary<string, float> skinsScale;

	public Sprite backIconBig;

	public Color backIconColor;

	public COLController controller;

	public COL_IndividualSkinsController individualSkins;

	public GameObject collectionMenu;

	public GameObject skinsMenu;

	public GameObject menuBackground;

	public GameObject skinsBackground;

	private bool inputActive;

	private bool moving;

	private bool selectionMade;

	private int nextFrameInput;

	private bool leftPressed;

	private float leftPressedTimer;

	private bool rightPressed;

	private float rightPressedTimer;

	public GameObject backIcon;

	private void Start()
	{
		InitializeMinigamesList();
		InitializePageNumbers();
		skinsDictionary = PCK_unlockAssets.GetSkinsDictionary();
		skinsScale = PCK_unlockAssets.GetSkinsScale();
		skinsCollection = GlobalControl.globalControl.skinsDictionary;
		UpdateSkins();
	}

	public void Initialize()
	{
		UpdateSkins();
		selectionMade = false;
		collectionMenu.SetActive(false);
		skinsMenu.SetActive(true);
		menuBackground.SetActive(false);
		skinsBackground.SetActive(true);
		controller.wheelController.ResetNextFrameInput();
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void Update()
	{
		ManageInput();
	}

	private void ManageInput()
	{
		if (!inputActive || controller.wheelController.moving)
		{
			return;
		}
		if (!moving && !selectionMade)
		{
			if (GlobalInput.globalInput.arrowsAcceptDown)
			{
				selectionMade = true;
				controller.PlayAccept();
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
				if (currentMinigame == minigames.Count)
				{
					BackToCollection();
				}
				else
				{
					ShowIndividualSkins();
				}
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
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle)
			{
				selectionMade = true;
				controller.PlayAccept();
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
				if (currentMinigame == minigames.Count)
				{
					BackToCollection();
				}
				else
				{
					ShowIndividualSkins();
				}
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				selectionMade = true;
				controller.PlayCancel();
				leftPressed = false;
				rightPressed = false;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
				BackToCollection();
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

	public void Plus()
	{
		moving = true;
		Invoke("FalsifyMoving", 0.2f);
		currentMinigame++;
		if (currentMinigame > minigames.Count)
		{
			currentMinigame = 0;
		}
		UpdateSkins();
		leftPressed = false;
		rightPressed = false;
		GlobalAudio.globalAudio.PlayMoveSelection();
	}

	public void Minus()
	{
		moving = true;
		Invoke("FalsifyMoving", 0.2f);
		currentMinigame--;
		if (currentMinigame < 0)
		{
			currentMinigame = minigames.Count;
		}
		UpdateSkins();
		leftPressed = false;
		rightPressed = false;
		GlobalAudio.globalAudio.PlayMoveSelection();
	}

	private void FalsifyMoving()
	{
		moving = false;
	}

	private void BackToCollection()
	{
		inputActive = false;
		controller.ResetNextFrameInput();
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("ShowCollectionMenu", 1f / 6f);
	}

	public void ShowCollectionMenu()
	{
		currentMinigame = 0;
		skinsMenu.SetActive(false);
		collectionMenu.SetActive(true);
		menuBackground.SetActive(true);
		skinsBackground.SetActive(false);
		controller.Initialize();
	}

	private void ShowIndividualSkins()
	{
		controller.currentMenu = "individualSkins";
		inputActive = false;
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("DoShowIndividualSkins", 1f / 6f);
	}

	private void DoShowIndividualSkins()
	{
		individualSkins.Initialize();
	}

	private void InitializePageNumbers()
	{
		currentMinigame = 0;
	}

	private void UpdateSkins()
	{
		if (currentMinigame == minigames.Count)
		{
			backIcon.SetActive(true);
			for (int i = 0; i < 9; i++)
			{
				skinsGO[i].SetActive(false);
				skinsNumbers[i].sprite = null;
				skinsSR_s[i].sprite = null;
				skinsSR[i].sprite = null;
				medalsGO[i].SetActive(false);
			}
			return;
		}
		if (backIcon.activeSelf)
		{
			backIcon.SetActive(false);
		}
		for (int j = 0; j < 4; j++)
		{
			skinsGO[j].SetActive(true);
			skinsTR[j].localScale = new Vector3(skinsScale[minigames[currentMinigame]], skinsScale[minigames[currentMinigame]], 1f);
			skinsNumbers[j].sprite = controller.numberSprites[skinsCollection[minigames[currentMinigame]][j]];
			skinsSR_s[j].sprite = skinsDictionary[minigames[currentMinigame]][j];
			if (skinsCollection[minigames[currentMinigame]][j] > 0)
			{
				skinsSR[j].sprite = skinsSR_s[j].sprite;
				if (skinsCollection[minigames[currentMinigame]][j] >= 9)
				{
					skinsNumbers[j].sprite = null;
					medalsGO[j].SetActive(true);
				}
				else
				{
					medalsGO[j].SetActive(false);
				}
			}
			else if (skinsCollection[minigames[currentMinigame]][j] == 0)
			{
				skinsSR[j].sprite = null;
				medalsGO[j].SetActive(false);
			}
			if (GlobalControl.globalControl.enabledSkins[minigames[currentMinigame]][j])
			{
				enabledIconsSR[j].color = enabledColors[1];
			}
			else
			{
				enabledIconsSR[j].color = enabledColors[0];
			}
		}
		skinsTR[4].localScale = new Vector3(skinsScale[minigames[currentMinigame]], skinsScale[minigames[currentMinigame]], 1f);
		skinsGO[4].SetActive(true);
		skinsNumbers[4].sprite = controller.numberSprites[skinsCollection[minigames[currentMinigame]][8]];
		skinsSR_s[4].sprite = skinsDictionary[minigames[currentMinigame]][8];
		if (skinsCollection[minigames[currentMinigame]][8] > 0)
		{
			skinsSR[4].sprite = skinsSR_s[4].sprite;
			if (skinsCollection[minigames[currentMinigame]][4] >= 9)
			{
				skinsNumbers[4].sprite = null;
				medalsGO[4].SetActive(true);
			}
			else
			{
				medalsGO[4].SetActive(false);
			}
		}
		else if (skinsCollection[minigames[currentMinigame]][8] == 0)
		{
			skinsSR[4].sprite = null;
			medalsGO[4].SetActive(false);
		}
		if (GlobalControl.globalControl.enabledSkins[minigames[currentMinigame]][8])
		{
			enabledIconsSR[4].color = enabledColors[1];
		}
		else
		{
			enabledIconsSR[4].color = enabledColors[0];
		}
		for (int k = 4; k < 8; k++)
		{
			skinsGO[k + 1].SetActive(true);
			skinsTR[k + 1].localScale = new Vector3(skinsScale[minigames[currentMinigame]], skinsScale[minigames[currentMinigame]], 1f);
			skinsNumbers[k + 1].sprite = controller.numberSprites[skinsCollection[minigames[currentMinigame]][k]];
			skinsSR_s[k + 1].sprite = skinsDictionary[minigames[currentMinigame]][k];
			if (skinsCollection[minigames[currentMinigame]][k] > 0)
			{
				skinsSR[k + 1].sprite = skinsSR_s[k + 1].sprite;
				if (skinsCollection[minigames[currentMinigame]][k] >= 9)
				{
					skinsNumbers[k + 1].sprite = null;
					medalsGO[k + 1].SetActive(true);
				}
				else
				{
					medalsGO[k + 1].SetActive(false);
				}
			}
			else if (skinsCollection[minigames[currentMinigame]][k] == 0)
			{
				skinsSR[k + 1].sprite = null;
				medalsGO[k + 1].SetActive(false);
			}
			if (GlobalControl.globalControl.enabledSkins[minigames[currentMinigame]][k])
			{
				enabledIconsSR[k + 1].color = enabledColors[1];
			}
			else
			{
				enabledIconsSR[k + 1].color = enabledColors[0];
			}
		}
	}

	private void InitializeMinigamesList()
	{
		minigames = new List<string>();
		for (int i = 0; i < GlobalControl.globalControl.minigamesList.Count; i++)
		{
			for (int j = 0; j < GlobalControl.globalControl.unlockedMinigames.Count; j++)
			{
				if (GlobalControl.globalControl.minigamesList[i] == GlobalControl.globalControl.unlockedMinigames[j])
				{
					minigames.Add(GlobalControl.globalControl.minigamesList[i]);
					break;
				}
			}
		}
	}
}
