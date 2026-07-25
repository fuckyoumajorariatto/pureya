using UnityEngine;

public class COLController : MonoBehaviour
{
	public Sprite[] numberSprites;

	public WheelController wheelController;

	public GameObject wheel;

	public Sprite[] smallIcons;

	public Sprite[] smallIcons_s;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIcons_s;

	public Sprite[] bigIconsOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public SpriteRenderer titleIconSR;

	public SpriteRenderer titleIconSR_s;

	public SpriteRenderer titleIconOverlaySR;

	public Sprite collectionIcon;

	public Color titleColor;

	public string currentMenu = "collection";

	public COL_SkinsController skinsController;

	public COL_Minigames_Controller minigamesController;

	public COL_TracksController tracksController;

	private bool inputActive;

	private bool callResetNextFrameInput;

	private void Start()
	{
		optionsToSkip = new bool[bigIcons.Length];
		if (GlobalControl.globalControl.profile == 0)
		{
			GlobalControl.globalControl.profile = 1;
		}
		if (GlobalControl.globalControl.HowManyUnlockedTracks() == 0)
		{
			optionsToSkip[2] = true;
		}
		wheelController.ReplaceSprites(smallIcons, smallIcons_s, smallIconsOverlays, bigIcons, bigIcons_s, bigIconsOverlays, null, iconColors, optionsToSkip);
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
		wheelController.ReplaceSprites(smallIcons, smallIcons_s, smallIconsOverlays, bigIcons, bigIcons_s, bigIconsOverlays, null, iconColors, optionsToSkip);
		titleIconSR.sprite = collectionIcon;
		titleIconSR_s.sprite = collectionIcon;
		titleIconSR.color = titleColor;
		titleIconOverlaySR.sprite = null;
		if (currentMenu == "skins")
		{
			wheelController.ChangeSelectionAuto(0);
		}
		else if (currentMenu == "minigames")
		{
			wheelController.ChangeSelectionAuto(1);
		}
		else if (currentMenu == "tracks")
		{
			wheelController.ChangeSelectionAuto(2);
		}
		currentMenu = "collection";
	}

	private void Update()
	{
		if (currentMenu == "collection" && inputActive)
		{
			ManageInput();
		}
		if (callResetNextFrameInput)
		{
			ResetNextFrameInput();
		}
	}

	private void ManageInput()
	{
		if (!wheelController.selectionMade && !wheelController.moving)
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
			currentMenu = "skins";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowSkinsMenu", 1f / 6f);
		}
		else if (wheelController.selection == 1)
		{
			currentMenu = "minigames";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowMinigamesMenu", 1f / 6f);
		}
		else if (wheelController.selection == 2)
		{
			currentMenu = "tracks";
			PlayAccept();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowTracksMenu", 1f / 6f);
		}
		else if (wheelController.selection == 3 && !SceneChangeController.sceneChangeController.transitioning)
		{
			PlayAccept();
			BackToMMU();
		}
	}

	private void BackToMMU()
	{
		inputActive = false;
		currentMenu = string.Empty;
		GlobalControl.globalControl.changeMMUselectionTo = 3;
		SceneChangeController.sceneChangeController.LoadAndChangeScene("MMU", "title", 0f);
	}

	public void ResetNextFrameInput()
	{
		callResetNextFrameInput = false;
		wheelController.ResetNextFrameInput();
	}

	private void ShowSkinsMenu()
	{
		inputActive = false;
		skinsController.Initialize();
	}

	private void ShowMinigamesMenu()
	{
		inputActive = false;
		minigamesController.Initialize();
	}

	private void ShowTracksMenu()
	{
		inputActive = false;
		tracksController.Initialize();
	}

	public void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	public void PlayCancel()
	{
		GlobalAudio.globalAudio.PlayCancel();
	}
}
