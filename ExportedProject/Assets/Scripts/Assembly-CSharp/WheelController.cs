using UnityEngine;

public class WheelController : MonoBehaviour
{
	public Sprite[] smallIcons;

	public Sprite[] smallIcons_s;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIcons_s;

	public Sprite[] bigIconsOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	public int[] scales;

	public bool[] optionsToSkip;

	public Animator wheelAnimator;

	public AnimationState animState;

	public Transform middleTR;

	public Transform middleTR_s;

	public Transform leftTR;

	public Transform rightTR;

	public RelocateShadow middleRS;

	public RelocateShadow leftRS;

	public RelocateShadow rightRS;

	public SpriteRenderer middleSR;

	public SpriteRenderer middleSR_s;

	public SpriteRenderer middleOverlaySR;

	public SpriteRenderer leftSR;

	public SpriteRenderer leftSR_s;

	public SpriteRenderer leftOverlaySR;

	public SpriteRenderer rightSR;

	public SpriteRenderer rightOverlaySR;

	public SpriteRenderer rightSR_s;

	public SpriteRenderer substituteSR;

	public SpriteRenderer substituteOverlaySR;

	public SpriteRenderer substituteSR_s;

	public int selection;

	private int selectionNext;

	private int selectionPrevious;

	public bool moving;

	public bool movingLeft;

	public bool movingRight;

	public bool selectionMade;

	public int nextFrameInput;

	private float arrowAcceptCooldown;

	private bool leftPressed;

	private float leftPressedTimer;

	private bool rightPressed;

	private float rightPressedTimer;

	public bool movingAnimationFinished;

	private void Update()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			ManageInput();
		}
		if (movingAnimationFinished)
		{
			movingAnimationFinished = false;
		}
	}

	public void ReplaceSprites(Sprite[] smallIcons2, Sprite[] smallIconsOverlays2, Sprite[] bigIcons2, Sprite[] bigIconsOverlays2, Sprite[] bigRotationOverlays2, Color[] iconColors2, bool[] optionsToSkip2)
	{
		smallIcons = smallIcons2;
		smallIcons_s = smallIcons2;
		smallIconsOverlays = smallIconsOverlays2;
		bigIcons = bigIcons2;
		bigIcons_s = bigIcons2;
		bigIconsOverlays = bigIconsOverlays2;
		bigRotationOverlays = bigRotationOverlays2;
		iconColors = iconColors2;
		optionsToSkip = optionsToSkip2;
		scales = null;
		UpdateIcons();
	}

	public void ReplaceSprites(Sprite[] smallIcons2, Sprite[] smallIcons_s2, Sprite[] smallIconsOverlays2, Sprite[] bigIcons2, Sprite[] bigIcons_s2, Sprite[] bigIconsOverlays2, Sprite[] bigRotationOverlays2, Color[] iconColors2, bool[] optionsToSkip2)
	{
		smallIcons = smallIcons2;
		smallIcons_s = smallIcons_s2;
		smallIconsOverlays = smallIconsOverlays2;
		bigIcons = bigIcons2;
		bigIcons_s = bigIcons_s2;
		bigIconsOverlays = bigIconsOverlays2;
		bigRotationOverlays = bigRotationOverlays2;
		iconColors = iconColors2;
		optionsToSkip = optionsToSkip2;
		scales = null;
		UpdateIcons();
	}

	public void ReplaceSprites(Sprite[] smallIcons2, Sprite[] smallIconsOverlays2, Sprite[] bigIcons2, Sprite[] bigIconsOverlays2, Sprite[] bigRotationOverlays2, Color[] iconColors2, bool[] optionsToSkip2, int[] scales2)
	{
		smallIcons = smallIcons2;
		smallIcons_s = smallIcons2;
		smallIconsOverlays = smallIconsOverlays2;
		bigIcons = bigIcons2;
		bigIcons_s = bigIcons2;
		bigIconsOverlays = bigIconsOverlays2;
		bigRotationOverlays = bigRotationOverlays2;
		iconColors = iconColors2;
		optionsToSkip = optionsToSkip2;
		scales = scales2;
		UpdateIcons();
	}

	public void ChangeOrderInLayerTo(int order)
	{
		middleSR.sortingOrder = order;
		middleSR_s.sortingOrder = order;
		middleOverlaySR.sortingOrder = order;
		leftSR.sortingOrder = order;
		leftSR_s.sortingOrder = order;
		leftOverlaySR.sortingOrder = order;
		rightSR.sortingOrder = order;
		rightOverlaySR.sortingOrder = order;
		rightSR_s.sortingOrder = order;
		substituteSR.sortingOrder = order;
		substituteOverlaySR.sortingOrder = order;
		substituteSR_s.sortingOrder = order;
	}

	private void ManageInput()
	{
		if (arrowAcceptCooldown > 0f)
		{
			arrowAcceptCooldown -= Time.unscaledDeltaTime;
		}
		else if (!moving && !selectionMade)
		{
			if (GlobalInput.globalInput.arrowsAcceptDown || (GlobalInput.globalInput.right && GlobalInput.globalInput.left))
			{
				arrowAcceptCooldown = 0.1f;
				leftPressed = false;
				rightPressed = false;
				nextFrameInput = 0;
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
			}
			else if (leftPressed)
			{
				leftPressedTimer += Time.unscaledDeltaTime;
				if (leftPressedTimer >= 0.05f)
				{
					Left();
				}
			}
			else if (rightPressed)
			{
				rightPressedTimer += Time.unscaledDeltaTime;
				if (rightPressedTimer >= 0.05f)
				{
					Right();
				}
			}
			else if (GlobalInput.globalInput.left || GlobalInput.globalInput.touchButtonScreenLeft || nextFrameInput == 1)
			{
				leftPressed = true;
			}
			else if (GlobalInput.globalInput.right || GlobalInput.globalInput.touchButtonScreenRight || nextFrameInput == 2)
			{
				rightPressed = true;
			}
			else if (GlobalInput.globalInput.swipeScreenLeft)
			{
				Right();
			}
			else if (GlobalInput.globalInput.left || GlobalInput.globalInput.swipeScreenRight || nextFrameInput == 2)
			{
				Left();
			}
			else
			{
				leftPressedTimer = 0f;
				rightPressedTimer = 0f;
			}
			nextFrameInput = 0;
		}
		else if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.swipeScreenRight || GlobalInput.globalInput.touchButtonScreenLeft)
		{
			nextFrameInput = 1;
		}
		else if (GlobalInput.globalInput.rightDown || GlobalInput.globalInput.swipeScreenLeft || GlobalInput.globalInput.touchButtonScreenRight)
		{
			nextFrameInput = 2;
		}
	}

	public void ResetNextFrameInput()
	{
		nextFrameInput = 0;
		leftPressed = false;
		rightPressed = false;
	}

	private void Left()
	{
		leftPressed = false;
		rightPressed = false;
		MoveWheel(false);
		selection--;
		if (selection < 0)
		{
			selection = bigIcons.Length - 1;
		}
		while (optionsToSkip[selection])
		{
			selection--;
			if (selection < 0)
			{
				selection = bigIcons.Length - 1;
			}
		}
		PlayMoveSelection();
	}

	private void Right()
	{
		leftPressed = false;
		rightPressed = false;
		MoveWheel(true);
		selection++;
		if (selection > bigIcons.Length - 1)
		{
			selection = 0;
		}
		while (optionsToSkip[selection])
		{
			selection++;
			if (selection > bigIcons.Length - 1)
			{
				selection = 0;
			}
		}
		PlayMoveSelection();
	}

	public void ChangeSelectionAuto(int sel)
	{
		selection = sel;
		UpdateIcons();
	}

	private void MoveWheel(bool next)
	{
		moving = true;
		if (next)
		{
			movingRight = true;
			wheelAnimator.SetTrigger("next");
		}
		else
		{
			movingLeft = true;
			wheelAnimator.SetTrigger("previous");
		}
		UpdateToBigIcons(next);
	}

	public void UpdateIcons()
	{
		if (optionsToSkip.Length == 0)
		{
			return;
		}
		selectionNext = selection + 1;
		if (selectionNext > bigIcons.Length - 1)
		{
			selectionNext = 0;
		}
		while (optionsToSkip[selectionNext])
		{
			selectionNext++;
			if (selectionNext > bigIcons.Length - 1)
			{
				selectionNext = 0;
			}
		}
		selectionPrevious = selection - 1;
		if (selectionPrevious < 0)
		{
			selectionPrevious = bigIcons.Length - 1;
		}
		while (optionsToSkip[selectionPrevious])
		{
			selectionPrevious--;
			if (selectionPrevious < 0)
			{
				selectionPrevious = bigIcons.Length - 1;
			}
		}
		middleSR.sprite = bigIcons[selection];
		middleSR_s.sprite = bigIcons_s[selection];
		middleOverlaySR.sprite = bigIconsOverlays[selection];
		middleSR.color = iconColors[selection];
		rightSR.sprite = smallIcons[selectionNext];
		rightSR_s.sprite = smallIcons_s[selectionNext];
		rightOverlaySR.sprite = smallIconsOverlays[selectionNext];
		rightSR.color = iconColors[selectionNext];
		leftSR.sprite = smallIcons[selectionPrevious];
		leftSR_s.sprite = smallIcons_s[selectionPrevious];
		leftOverlaySR.sprite = smallIconsOverlays[selectionPrevious];
		leftSR.color = iconColors[selectionPrevious];
		if (scales != null)
		{
			middleTR.localScale = new Vector3(scales[selection], scales[selection], 1f);
			rightTR.localScale = new Vector3(1f, 1f, 1f);
			leftTR.localScale = new Vector3(1f, 1f, 1f);
			middleRS.enabled = false;
			middleTR_s.localPosition = new Vector3(0.0625f, -0.0625f, 1f);
			rightRS.enabled = true;
			leftRS.enabled = true;
		}
		moving = false;
		movingRight = false;
		movingLeft = false;
	}

	public void UpdateToBigIcons(bool next)
	{
		selectionNext = selection + 1;
		if (selectionNext > bigIcons.Length - 1)
		{
			selectionNext = 0;
		}
		while (optionsToSkip[selectionNext])
		{
			selectionNext++;
			if (selectionNext > bigIcons.Length - 1)
			{
				selectionNext = 0;
			}
		}
		selectionPrevious = selection - 1;
		if (selectionPrevious < 0)
		{
			selectionPrevious = bigIcons.Length - 1;
		}
		while (optionsToSkip[selectionPrevious])
		{
			selectionPrevious--;
			if (selectionPrevious < 0)
			{
				selectionPrevious = bigIcons.Length - 1;
			}
		}
		middleSR.sprite = bigIcons[selection];
		middleSR_s.sprite = bigIcons_s[selection];
		middleOverlaySR.sprite = bigIconsOverlays[selection];
		middleSR.color = iconColors[selection];
		if (next)
		{
			rightSR.sprite = bigIcons[selectionNext];
			rightSR_s.sprite = bigIcons_s[selectionNext];
			rightOverlaySR.sprite = bigIconsOverlays[selectionNext];
			rightSR.color = iconColors[selectionNext];
			if (scales != null)
			{
				middleTR.localScale = new Vector3(scales[selection], scales[selection], 1f);
				rightTR.localScale = new Vector3(scales[selectionNext], scales[selectionNext], 1f);
				if (scales[selection] == 1)
				{
					middleRS.enabled = true;
				}
				if (scales[selectionNext] != 1)
				{
					rightRS.enabled = false;
				}
			}
			selectionNext++;
			if (selectionNext > bigIcons.Length - 1)
			{
				selectionNext -= bigIcons.Length;
			}
			while (optionsToSkip[selectionNext])
			{
				selectionNext++;
				if (selectionNext > bigIcons.Length - 1)
				{
					selectionNext -= bigIcons.Length;
				}
			}
			substituteSR.sprite = smallIcons[selectionNext];
			substituteSR_s.sprite = smallIcons_s[selectionNext];
			substituteOverlaySR.sprite = smallIconsOverlays[selectionNext];
			substituteSR.color = iconColors[selectionNext];
			return;
		}
		leftSR.sprite = bigIcons[selectionPrevious];
		leftSR_s.sprite = bigIcons_s[selectionPrevious];
		leftOverlaySR.sprite = bigIconsOverlays[selectionPrevious];
		leftSR.color = iconColors[selectionPrevious];
		if (scales != null)
		{
			middleTR.localScale = new Vector3(scales[selection], scales[selection], 1f);
			leftTR.localScale = new Vector3(scales[selectionPrevious], scales[selectionPrevious], 1f);
			if (scales[selection] == 1)
			{
				middleRS.enabled = true;
			}
			if (scales[selectionNext] != 1)
			{
				leftRS.enabled = false;
			}
		}
		selectionPrevious--;
		if (selectionPrevious < 0)
		{
			selectionPrevious += bigIcons.Length;
		}
		while (optionsToSkip[selectionPrevious])
		{
			selectionPrevious--;
			if (selectionPrevious < 0)
			{
				selectionPrevious += bigIcons.Length;
			}
		}
		substituteSR.sprite = smallIcons[selectionPrevious];
		substituteSR_s.sprite = smallIcons_s[selectionPrevious];
		substituteOverlaySR.sprite = smallIconsOverlays[selectionPrevious];
		substituteSR.color = iconColors[selectionPrevious];
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayMoveSelection()
	{
		GlobalAudio.globalAudio.PlayMoveSelection();
	}

	private void MovingAnimationFinished()
	{
		movingAnimationFinished = true;
	}
}
