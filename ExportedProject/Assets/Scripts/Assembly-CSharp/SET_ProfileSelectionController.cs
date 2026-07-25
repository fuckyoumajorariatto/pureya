using UnityEngine;

public class SET_ProfileSelectionController : MonoBehaviour
{
	public SETController controller;

	public SET_ProfileController profileController;

	public NumberWheelController numberWheelController;

	public GameObject numberWheel;

	public SpriteRenderer titleIconSR;

	public SpriteRenderer titleIconSR_s;

	public SpriteRenderer titleIconOverlaySR;

	public Color titleIconColor;

	public Sprite profileSprite;

	private bool moving;

	private bool selectionMade;

	private int nextFrameInput;

	private int selection;

	private bool inputActive;

	private int originalProfile;

	private bool leftPressed;

	private float leftPressedTimer;

	private bool rightPressed;

	private float rightPressedTimer;

	private void Update()
	{
		ManageInput();
	}

	public void Initialize()
	{
		selectionMade = false;
		numberWheel.SetActive(true);
		controller.wheelController.ResetNextFrameInput();
		numberWheelController.ChangeToLeftRight();
		titleIconSR.sprite = profileSprite;
		titleIconSR_s.sprite = profileSprite;
		titleIconSR.color = titleIconColor;
		selection = GlobalControl.globalControl.profile;
		originalProfile = selection;
		titleIconOverlaySR.sprite = controller.profileNumbersBig[selection - 1];
		numberWheelController.ChangeNumberTo(selection);
		numberWheelController.TogglePlus(true);
		numberWheelController.ToggleMinus(true);
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
				Accept();
			}
			else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
			{
				selectionMade = true;
				inputActive = false;
				GlobalControl.globalControl.profile = originalProfile;
				GlobalControl.globalControl.LoadData();
				PlayCancel();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowProfileMenu", 1f / 6f);
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

	private void Accept()
	{
		selectionMade = true;
		inputActive = false;
		PlayAccept();
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("ShowProfileMenu", 1f / 6f);
		leftPressed = false;
		rightPressed = false;
		leftPressedTimer = 0f;
		rightPressedTimer = 0f;
		GlobalControl.globalControl.justUnlockedMinigames.Clear();
		MinigamesController.minigamesController.ResetRecentlyUsedMinigames();
		SaveLoadManager.saveLoadManager.UpdateDefaultSaveAndroidCloud(GlobalControl.globalControl.profile);
		SaveLoadManager.saveLoadManager.UpdateDefaultSaveiCloud(GlobalControl.globalControl.profile);
	}

	private void ActivateInput()
	{
		inputActive = true;
	}

	private void Plus()
	{
		moving = true;
		Invoke("FalsifyMoving", 0.2f);
		selection++;
		if (selection > 9)
		{
			selection = 1;
		}
		GlobalControl.globalControl.profile = selection;
		numberWheelController.Plus();
		numberWheelController.ChangeNumberTo(selection);
		profileController.UpdateProfile();
		titleIconOverlaySR.sprite = controller.profileNumbersBig[selection - 1];
		leftPressed = false;
		rightPressed = false;
	}

	private void Minus()
	{
		moving = true;
		Invoke("FalsifyMoving", 0.2f);
		selection--;
		if (selection < 1)
		{
			selection = 9;
		}
		GlobalControl.globalControl.profile = selection;
		numberWheelController.Minus();
		numberWheelController.ChangeNumberTo(selection);
		profileController.UpdateProfile();
		titleIconOverlaySR.sprite = controller.profileNumbersBig[selection - 1];
		leftPressed = false;
		rightPressed = false;
	}

	private void FalsifyMoving()
	{
		moving = false;
	}

	private void ShowProfileMenu()
	{
		numberWheel.SetActive(false);
		profileController.Initialize();
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
