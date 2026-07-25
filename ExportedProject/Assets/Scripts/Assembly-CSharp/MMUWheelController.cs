using UnityEngine;

public class MMUWheelController : MonoBehaviour
{
	public WheelController wheelController;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Sprite[] smallRotationOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	private bool inputActive;

	private bool selectionMade;

	private void Start()
	{
		optionsToSkip = new bool[bigIcons.Length];
		if (GlobalControl.globalControl.mobile)
		{
			optionsToSkip[4] = true;
		}
		else
		{
			optionsToSkip[6] = true;
		}
		if (GlobalControl.globalControl.unlockedMinigames.Count == 0)
		{
			optionsToSkip[1] = true;
			optionsToSkip[2] = true;
			optionsToSkip[3] = true;
		}
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		UpdateRotationIcons();
		if (GlobalControl.globalControl.changeMMUselectionTo != 0)
		{
			wheelController.ChangeSelectionAuto(GlobalControl.globalControl.changeMMUselectionTo);
		}
		GlobalControl.globalControl.changeMMUselectionTo = 0;
		Invoke("ActivateInput", 0.5f);
	}

	private void Update()
	{
		ManageInput();
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void UpdateRotationIcons()
	{
		if (Screen.autorotateToPortrait)
		{
			wheelController.smallIconsOverlays[6] = smallRotationOverlays[1];
			wheelController.bigIconsOverlays[6] = bigRotationOverlays[1];
		}
		else
		{
			wheelController.smallIconsOverlays[6] = smallRotationOverlays[0];
			wheelController.bigIconsOverlays[6] = bigRotationOverlays[0];
		}
		wheelController.UpdateIcons();
	}

	private void ManageInput()
	{
		if (!inputActive || wheelController.moving || wheelController.selectionMade)
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			ManageAccept();
		}
		else if (GlobalInput.globalInput.exitDown)
		{
			if (GlobalControl.globalControl.mobile)
			{
				Exit();
			}
			else if (wheelController.selection == 4)
			{
				wheelController.selectionMade = true;
				PlayAccept();
				Invoke("Exit", 0.5f);
			}
			else
			{
				GlobalAudio.globalAudio.PlayMoveSelection();
				wheelController.ChangeSelectionAuto(4);
			}
		}
	}

	private void ManageAccept()
	{
		wheelController.ResetNextFrameInput();
		if (wheelController.selection == 0 && !selectionMade)
		{
			PlayAccept();
			wheelController.selectionMade = true;
			StartGame();
		}
		else if (wheelController.selection == 1 && !selectionMade)
		{
			if (SceneChangeController.sceneChangeController.transitioning)
			{
				selectionMade = false;
				return;
			}
			selectionMade = true;
			PlayAccept();
			wheelController.selectionMade = true;
			GlobalControl.globalControl.survivalSelection = 0;
			SceneChangeController.sceneChangeController.LoadAndChangeScene("SUR", "title", 0f);
		}
		else if (wheelController.selection == 2 && !selectionMade)
		{
			if (SceneChangeController.sceneChangeController.transitioning)
			{
				selectionMade = false;
				return;
			}
			selectionMade = true;
			PlayAccept();
			wheelController.selectionMade = true;
			StartEndlessMode();
		}
		else if (wheelController.selection == 3 && !selectionMade)
		{
			if (SceneChangeController.sceneChangeController.transitioning)
			{
				selectionMade = false;
				return;
			}
			selectionMade = true;
			PlayAccept();
			wheelController.selectionMade = true;
			SceneChangeController.sceneChangeController.LoadAndChangeScene("COL", "title", 0f);
		}
		else if (wheelController.selection == 4 && !selectionMade)
		{
			if (SceneChangeController.sceneChangeController.transitioning)
			{
				selectionMade = false;
				return;
			}
			selectionMade = true;
			wheelController.selectionMade = true;
			PlayAccept();
			Invoke("Exit", 0.5f);
		}
		else if (wheelController.selection == 5 && !selectionMade)
		{
			if (SceneChangeController.sceneChangeController.transitioning)
			{
				selectionMade = false;
				return;
			}
			selectionMade = true;
			PlayAccept();
			wheelController.selectionMade = true;
			SceneChangeController.sceneChangeController.LoadAndChangeScene("SET", "title", 0f);
		}
		else if (wheelController.selection == 6)
		{
			PauseController.pauseController.ToggleRotation();
			UpdateRotationIcons();
			wheelController.UpdateIcons();
			PlayAccept();
		}
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayMoveSelection()
	{
		GlobalAudio.globalAudio.PlayMoveSelection();
	}

	public void StartGame()
	{
		GlobalControl.globalControl.mode = "normal";
		GlobalAudio.globalAudio.ClearMusic();
		SceneChangeController.sceneChangeController.LoadAndChangeScene("PCK", "title", 0f);
	}

	private void StartEndlessMode()
	{
		GlobalControl.globalControl.mode = "endless";
		GlobalAudio.globalAudio.ClearMusic();
		MinigamesController.minigamesController.StartEndlessMode();
	}

	private void Exit()
	{
		Application.Quit();
	}

	private void PlayCancel()
	{
		GlobalAudio.globalAudio.PlayCancel();
	}
}
