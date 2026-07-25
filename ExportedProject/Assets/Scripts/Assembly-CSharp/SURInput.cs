using UnityEngine;

public class SURInput : MonoBehaviour
{
	public WheelController wheelController;

	public SUR_WheelController SUR_wheelController;

	private bool selectionMade;

	private bool continuousPressBool;

	private float continuousPressTimer;

	private int intSelection;

	private bool inputActive;

	private void Start()
	{
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

	private void ManageInput()
	{
		if (!selectionMade && inputActive && !wheelController.moving)
		{
			if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
			{
				ManageAccept();
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				wheelController.selectionMade = true;
				PlayCancel();
				BackToMMU();
			}
		}
	}

	private void ManageAccept()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			selectionMade = false;
			return;
		}
		selectionMade = true;
		if (wheelController.selection < SUR_wheelController.minigames.Count)
		{
			StartGame();
			return;
		}
		PlayAccept();
		BackToMMU();
	}

	private void StartGame()
	{
		PlayAccept();
		GlobalControl.globalControl.survivalSelection = wheelController.selection;
		GlobalAudio.globalAudio.ClearMusic();
		if (SUR_wheelController.minigames[wheelController.selection] == "MIX")
		{
			MinigamesController.minigamesController.mix = true;
			MinigamesController.minigamesController.max = false;
			MinigamesController.minigamesController.StartSurvivalMix();
		}
		else if (SUR_wheelController.minigames[wheelController.selection] == "MAX")
		{
			MinigamesController.minigamesController.mix = true;
			MinigamesController.minigamesController.max = true;
			MinigamesController.minigamesController.StartSurvivalMix();
		}
		else
		{
			MinigamesController.minigamesController.mix = false;
			MinigamesController.minigamesController.max = false;
			MinigamesController.minigamesController.StartSurvival(SUR_wheelController.minigames[wheelController.selection]);
		}
	}

	private void BackToMMU()
	{
		wheelController.ResetNextFrameInput();
		GlobalControl.globalControl.changeMMUselectionTo = 1;
		SceneChangeController.sceneChangeController.LoadAndChangeScene("MMU", "title", 0f);
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayMoveSelection()
	{
		GlobalAudio.globalAudio.PlayMoveSelection();
	}

	private void PlayInvalid()
	{
		GlobalAudio.globalAudio.PlayInvalid();
	}

	private void PlayCancel()
	{
		GlobalAudio.globalAudio.PlayCancel();
	}
}
