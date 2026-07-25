using UnityEngine;

public class SET_SoundsVolumeController : MonoBehaviour
{
	public SETController controller;

	public SET_VolumeController volumeController;

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

	private int selection;

	private int limit = 100;

	private bool inputActive;

	private int originalSelection;

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
		numberWheelController.ChangeToMinusPlus();
		titleIconSR.sprite = titleSprite;
		titleIconSR_s.sprite = titleSprite;
		titleIconSR.color = titleIconColor;
		selection = PlayerPrefs.GetInt("soundsVolume");
		originalSelection = selection;
		titleIconOverlaySR.sprite = null;
		numberWheelController.ChangeNumberTo(selection);
		numberWheelController.TogglePlus(selection != limit);
		numberWheelController.ToggleMinus(selection != 0);
		Invoke("ActivateInput", 0.25f);
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
				selectionMade = true;
				inputActive = false;
				controller.ResetNextFrameInput();
				PlayAccept();
				PlayerPrefs.SetInt("soundsVolume", selection);
				GlobalAudio.globalAudio.UpdateSoundsVolumeTo(selection);
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowVolumeMenu", 1f / 6f);
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
				GlobalAudio.globalAudio.UpdateSoundsVolumeTo(originalSelection);
				PlayCancel();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowVolumeMenu", 1f / 6f);
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
		if (selection < limit)
		{
			moving = true;
			Invoke("FalsifyMoving", 0.2f);
			selection += 5;
			GlobalAudio.globalAudio.UpdateSoundsVolumeTo(selection);
			numberWheelController.Plus();
			numberWheelController.ChangeNumberTo(selection);
		}
		numberWheelController.TogglePlus(selection != limit);
		numberWheelController.ToggleMinus(selection != 0);
		leftPressed = false;
		rightPressed = false;
	}

	private void Minus()
	{
		if (selection > 0)
		{
			moving = true;
			Invoke("FalsifyMoving", 0.2f);
			selection -= 5;
			GlobalAudio.globalAudio.UpdateSoundsVolumeTo(selection);
			numberWheelController.Minus();
			numberWheelController.ChangeNumberTo(selection);
		}
		numberWheelController.ToggleMinus(selection != 0);
		numberWheelController.TogglePlus(selection != limit);
		leftPressed = false;
		rightPressed = false;
	}

	private void FalsifyMoving()
	{
		moving = false;
	}

	private void ShowVolumeMenu()
	{
		inputActive = false;
		numberWheel.SetActive(false);
		volumeController.Initialize(true);
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
