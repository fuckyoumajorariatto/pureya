using System.Collections.Generic;
using UnityEngine;

public class COL_Minigames_Controller : MonoBehaviour
{
	public WheelController wheelController;

	public SUR_WheelController SUR_wheelController;

	public COLController controller;

	public COL_IndividualSkinsController individualSkinsController;

	public GameObject minigamesMenu;

	public GameObject colMenu;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Sprite[] smallRotationOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public List<string> minigames;

	public Color white;

	public SpriteRenderer enabledSR;

	public SpriteRenderer enabledSR_s;

	private bool inputActive;

	private int currentMinigame;

	private void Start()
	{
		PreInitialize();
	}

	private void Update()
	{
		if (controller.currentMenu == "minigames")
		{
			ManageAccept();
			if (currentMinigame != wheelController.selection)
			{
				currentMinigame = wheelController.selection;
				UpdateInfo();
			}
		}
	}

	public void Initialize()
	{
		colMenu.SetActive(false);
		minigamesMenu.SetActive(true);
		wheelController.ResetNextFrameInput();
		currentMinigame = 0;
		UpdateInfo();
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void PreInitialize()
	{
		optionsToSkip = new bool[GlobalControl.globalControl.minigamesList.Count + 1];
		smallIcons = new Sprite[optionsToSkip.Length];
		smallIconsOverlays = new Sprite[smallIcons.Length];
		bigIcons = new Sprite[smallIcons.Length];
		bigIconsOverlays = new Sprite[smallIcons.Length];
		iconColors = new Color[smallIcons.Length];
		minigames = new List<string>();
		for (int i = 0; i < GlobalControl.globalControl.minigamesList.Count; i++)
		{
			minigames.Add(GlobalControl.globalControl.minigamesList[i]);
			if (!GlobalControl.globalControl.unlockedMinigames.Contains(GlobalControl.globalControl.minigamesList[i]))
			{
				optionsToSkip[i] = true;
			}
			smallIcons[i] = SUR_wheelController.circleSmall_s;
			smallIconsOverlays[i] = SUR_wheelController.minigamesIconsDictionary[GlobalControl.globalControl.minigamesList[i]][1];
			bigIcons[i] = SUR_wheelController.circleBig_s;
			bigIconsOverlays[i] = SUR_wheelController.minigamesIconsDictionary[GlobalControl.globalControl.minigamesList[i]][0];
			iconColors[i] = white;
		}
		smallIcons[smallIcons.Length - 1] = SUR_wheelController.backIconSmall;
		bigIcons[smallIcons.Length - 1] = SUR_wheelController.backIconBig;
		iconColors[smallIcons.Length - 1] = SUR_wheelController.backIconColor;
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
	}

	private void ManageAccept()
	{
		if (!inputActive || wheelController.moving || controller.currentMenu != "minigames")
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
		controller.ResetNextFrameInput();
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("ShowCollectionMenu", 1f / 6f);
	}

	private void ShowCollectionMenu()
	{
		GlobalControl.globalControl.SaveData();
		wheelController.selection = 0;
		minigamesMenu.SetActive(false);
		colMenu.SetActive(true);
		controller.Initialize();
	}

	private void ToggleEnabled()
	{
		GlobalControl.globalControl.enabledMinigames[minigames[wheelController.selection]] = !GlobalControl.globalControl.enabledMinigames[minigames[wheelController.selection]];
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
		if (GlobalControl.globalControl.enabledMinigames[minigames[wheelController.selection]])
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
}
