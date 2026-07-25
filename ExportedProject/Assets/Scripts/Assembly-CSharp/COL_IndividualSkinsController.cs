using System.Collections.Generic;
using UnityEngine;

public class COL_IndividualSkinsController : MonoBehaviour
{
	public COLController controller;

	public COL_SkinsController skinsController;

	public GameObject numberGO;

	public SpriteRenderer numberSR;

	public SpriteRenderer numberSR_s;

	public GameObject medalGO;

	public SpriteRenderer enabledSR;

	public SpriteRenderer enabledSR_s;

	public Sprite[] enabledSprites;

	private Dictionary<string, float> skinsScale;

	public GameObject skinsMenu;

	public GameObject individualSkinsMenu;

	private bool inputActive;

	private bool moving;

	private bool selectionMade;

	private int nextFrameInput;

	private bool leftPressed;

	private float leftPressedTimer;

	private bool rightPressed;

	private float rightPressedTimer;

	public WheelController wheelController;

	private bool[] optionsToSkip;

	private Sprite[] wheelIcons;

	private Sprite[] wheelIcons2;

	private Color[] wheelColors;

	private int[] scales;

	public Sprite backIconSmall;

	public Sprite backIconBig;

	public Sprite collectionIconSmall;

	public Sprite collectionIconBig;

	private int currentSkin;

	private string minigameName;

	private bool backArrowChange;

	public Color[] enabledColors;

	private void Update()
	{
		if (controller.currentMenu == "individualSkins")
		{
			ManageAccept();
			if (currentSkin != wheelController.selection)
			{
				currentSkin = wheelController.selection;
				UpdateInfo();
			}
		}
	}

	public void Initialize()
	{
		selectionMade = false;
		skinsMenu.SetActive(false);
		individualSkinsMenu.SetActive(true);
		wheelController.ResetNextFrameInput();
		SetWheel();
		wheelController.ChangeSelectionAuto(0);
		wheelController.ReplaceSprites(wheelIcons, new Sprite[11], wheelIcons2, new Sprite[11], null, wheelColors, new bool[11], scales);
		currentSkin = wheelController.selection;
		UpdateInfo();
		Invoke("ActivateInput", 0.5f);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void ManageAccept()
	{
		if (!inputActive || wheelController.moving || selectionMade)
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			if (wheelController.selection == 9)
			{
				selectionMade = true;
				controller.ResetNextFrameInput();
				controller.PlayAccept();
				BackToCollection();
			}
			else if (wheelController.selection == 10)
			{
				selectionMade = true;
				controller.ResetNextFrameInput();
				controller.PlayAccept();
				BackToSkins();
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
			BackToSkins();
		}
	}

	private void UpdateInfo()
	{
		if (currentSkin == 9 || currentSkin == 10)
		{
			numberSR.sprite = null;
			numberSR_s.sprite = null;
			medalGO.SetActive(false);
			enabledSR.sprite = null;
			enabledSR_s.sprite = null;
			return;
		}
		numberSR.sprite = controller.numberSprites[skinsController.skinsCollection[minigameName][currentSkin]];
		numberSR_s.sprite = numberSR.sprite;
		if (skinsController.skinsCollection[minigameName][currentSkin] >= 9)
		{
			medalGO.SetActive(true);
		}
		else
		{
			medalGO.SetActive(false);
		}
		if (GlobalControl.globalControl.enabledSkins[minigameName][currentSkin])
		{
			enabledSR.sprite = enabledSprites[1];
			enabledSR.color = enabledColors[1];
		}
		else
		{
			enabledSR.sprite = enabledSprites[0];
			enabledSR.color = enabledColors[0];
		}
		enabledSR_s.sprite = enabledSR.sprite;
	}

	private void ToggleEnabled()
	{
		GlobalControl.globalControl.enabledSkins[minigameName][currentSkin] = !GlobalControl.globalControl.enabledSkins[minigameName][currentSkin];
		UpdateInfo();
	}

	private void BackToSkins()
	{
		controller.currentMenu = "skins";
		inputActive = false;
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("DoBackToSkins", 1f / 6f);
	}

	private void DoBackToSkins()
	{
		GlobalControl.globalControl.SaveData();
		individualSkinsMenu.SetActive(false);
		skinsController.Initialize();
	}

	private void BackToCollection()
	{
		controller.currentMenu = "skins";
		inputActive = false;
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("DoBackToCollection", 1f / 6f);
	}

	private void DoBackToCollection()
	{
		individualSkinsMenu.SetActive(false);
		skinsController.ShowCollectionMenu();
	}

	private void SetWheel()
	{
		wheelIcons = new Sprite[11];
		wheelIcons2 = new Sprite[11];
		wheelColors = new Color[11];
		scales = new int[11];
		minigameName = skinsController.minigames[skinsController.currentMinigame];
		for (int i = 0; i < 9; i++)
		{
			wheelIcons[i] = skinsController.skinsDictionary[minigameName][i];
			wheelIcons2[i] = wheelIcons[i];
			if (skinsController.skinsCollection[minigameName][i] == 0)
			{
				wheelColors[i] = new Color(1f, 1f, 1f, 0f);
			}
			else
			{
				wheelColors[i] = new Color(1f, 1f, 1f, 1f);
			}
			scales[i] = 2;
		}
		wheelIcons[9] = collectionIconSmall;
		wheelIcons2[9] = collectionIconBig;
		wheelColors[9] = controller.titleColor;
		scales[9] = 1;
		wheelIcons[10] = backIconSmall;
		wheelIcons2[10] = backIconBig;
		wheelColors[10] = skinsController.backIconColor;
		scales[10] = 1;
	}
}
