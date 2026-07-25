using UnityEngine;

public class ButtonRightController : MonoBehaviour
{
	public static ButtonRightController buttonRightController;

	public GameObject sidesGO;

	public GameObject arrowsGO;

	public SpriteRenderer arrowSR;

	public bool isEnabled;

	public Animator anim;

	private int enabledA;

	private int pressedA;

	private int currentArrowSpriteIndex;

	private bool buttonEnabled = true;

	public Sprite[] arrowSprites;

	private void Start()
	{
		if (buttonRightController == null)
		{
			buttonRightController = this;
		}
		enabledA = Animator.StringToHash("enabled");
		pressedA = Animator.StringToHash("pressed");
	}

	private void Update()
	{
		ManageAnimation();
	}

	private void ManageAnimation()
	{
		bool flag = true;
		if (PCK_InputController.PCK_inputController != null)
		{
			flag = PCK_InputController.PCK_inputController.inputActive;
		}
		if (!PauseController.pauseController.paused && (PauseController.pauseController.inCinematic || SceneChangeController.sceneChangeController.currentScene == "splash"))
		{
			isEnabled = false;
			anim.SetBool(enabledA, false);
			return;
		}
		if (!flag)
		{
			if (!PauseController.pauseController.paused)
			{
				isEnabled = false;
				anim.SetBool(enabledA, false);
				return;
			}
			isEnabled = true;
			anim.SetBool(enabledA, true);
		}
		else
		{
			isEnabled = true;
			anim.SetBool(enabledA, true);
		}
		if (GlobalInput.globalInput.right && buttonEnabled)
		{
			anim.SetBool(pressedA, true);
		}
		else
		{
			anim.SetBool(pressedA, false);
		}
	}

	public void HideButton()
	{
		sidesGO.SetActive(false);
		arrowsGO.SetActive(false);
		buttonEnabled = false;
	}

	public void ShowButton()
	{
		sidesGO.SetActive(true);
		arrowsGO.SetActive(true);
		buttonEnabled = true;
	}

	private void ChangeArrowToEnabled()
	{
		arrowSR.sprite = arrowSprites[0];
		currentArrowSpriteIndex = 0;
	}

	private void ChangeArrowToPressed()
	{
		arrowSR.sprite = arrowSprites[1];
		currentArrowSpriteIndex = 1;
	}

	private void ChangeArrowToDisabled()
	{
		arrowSR.sprite = arrowSprites[2];
		currentArrowSpriteIndex = 2;
	}

	public void UpdateArrowSprite()
	{
		arrowSR.sprite = arrowSprites[currentArrowSpriteIndex];
	}
}
