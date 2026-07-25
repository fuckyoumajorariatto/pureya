using UnityEngine;

public class ButtonsInputController : MonoBehaviour
{
	public static ButtonsInputController buttonsInputController;

	public Transform buttonLeft;

	public Transform buttonRight;

	public Transform buttonScreen;

	private float swipeDistanceX = 1f;

	private float swipeDistanceY = 0.5f;

	private float swipeDistanceXtouch = 0.2f;

	private float swipeDistanceYtouch = 0.2f;

	private Transform hitTR;

	private Touch touch;

	private int touchIndex;

	private Transform[] touchOriginalTarget = new Transform[4];

	private Vector2[] touchPosition = new Vector2[4];

	private Vector2[,] touchPositions = new Vector2[4, 30];

	private bool[] resetTouchPositions = new bool[4];

	private bool[] touchSwipe = new bool[4];

	private int[] touchOriginalSwipePointIndex = new int[4];

	private bool[] hasBeenTouchSwipe = new bool[4];

	private Vector2 cursorPosition;

	private Vector2[] cursorPositions = new Vector2[30];

	private bool resetCursorPositions;

	private Transform cursorOriginalTarget;

	private bool cursorSwipe;

	private bool hasBeenSwipe;

	private int cursorOriginalSwipePointIndex;

	public bool inheritedTouchControl;

	private bool cursorSwipeLeftDone;

	private bool cursorSwipeRightDone;

	public int squareAspectRatio;

	private void Start()
	{
		buttonsInputController = this;
	}

	private void Update()
	{
		GlobalInput.globalInput.touchButtonLeft = false;
		GlobalInput.globalInput.touchButtonRight = false;
		GlobalInput.globalInput.touchButtonScreen = false;
		GlobalInput.globalInput.touchButtonScreenLeft = false;
		GlobalInput.globalInput.touchButtonScreenMiddle = false;
		GlobalInput.globalInput.touchButtonScreenRight = false;
		GlobalInput.globalInput.swipeScreenLeft = false;
		GlobalInput.globalInput.swipeScreenRight = false;
		ManageTouch();
		if (Input.touchCount <= 0)
		{
			ManageMouse();
		}
	}

	private void FixedUpdate()
	{
		if (Input.touchCount > 0)
		{
			touchIndex = 0;
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.fingerId >= 4)
				{
					continue;
				}
				touchIndex = touch.fingerId;
				if (resetTouchPositions[touchIndex])
				{
					resetTouchPositions[touchIndex] = false;
					ResetTouchPositions(touchIndex);
				}
				else
				{
					for (int j = 0; j < touchPositions.GetLength(1) - 1; j++)
					{
						touchPositions[touchIndex, j + 1] = touchPositions[touchIndex, j];
					}
				}
				touchPositions[touchIndex, 0] = touchPosition[touchIndex];
				touchIndex++;
			}
		}
		if (!Input.GetMouseButton(0))
		{
			return;
		}
		if (resetCursorPositions)
		{
			resetCursorPositions = false;
			ResetCursorPositions();
		}
		else
		{
			for (int k = 0; k < cursorPositions.Length - 1; k++)
			{
				cursorPositions[k + 1] = cursorPositions[k];
			}
		}
		cursorPositions[0] = cursorPosition;
	}

	private void ManageButtonScreenArea(Vector2 point)
	{
		if ((buttonScreen.position.x - point.x) * (8f / buttonScreen.localScale.x) > 2f)
		{
			GlobalInput.globalInput.touchButtonScreenLeft = true;
		}
		else if ((buttonScreen.position.x - point.x) * (8f / buttonScreen.localScale.x) < -2f)
		{
			GlobalInput.globalInput.touchButtonScreenRight = true;
		}
		else
		{
			GlobalInput.globalInput.touchButtonScreenMiddle = true;
		}
	}

	private void ManageButtonScreenAreaSquareAspect(Vector2 point)
	{
		if ((buttonScreen.position.x - point.x) * (8f / buttonScreen.localScale.x) > 2f)
		{
			GlobalInput.globalInput.touchButtonLeft = true;
		}
		else if ((buttonScreen.position.x - point.x) * (8f / buttonScreen.localScale.x) < -2f)
		{
			GlobalInput.globalInput.touchButtonRight = true;
		}
	}

	private void ManageButtonScreenAreaSquareAspectEnd(Vector2 point)
	{
		if ((buttonScreen.position.x - point.x) * (8f / buttonScreen.localScale.x) > 2f)
		{
			GlobalInput.globalInput.touchButtonLeft = true;
			return;
		}
		if ((buttonScreen.position.x - point.x) * (8f / buttonScreen.localScale.x) < -2f)
		{
			GlobalInput.globalInput.touchButtonRight = true;
			return;
		}
		GlobalInput.globalInput.touchButtonScreenMiddle = true;
		GlobalInput.globalInput.touchButtonScreen = true;
	}

	private void ManageTouch()
	{
		if (Input.touchCount <= 0)
		{
			return;
		}
		for (int i = 0; i < touchPositions.GetLength(0); i++)
		{
			if (Input.touches.Length <= i)
			{
				continue;
			}
			touch = Input.touches[i];
			if (touch.fingerId >= 4)
			{
				continue;
			}
			touchIndex = touch.fingerId;
			touchPosition[touchIndex] = Camera.main.ScreenToWorldPoint(touch.position);
			hitTR = Physics2D.Raycast(touchPosition[touchIndex], new Vector2(0f, 0f)).transform;
			if (touch.phase == TouchPhase.Began)
			{
				ResetTouchPositions(touchIndex);
				touchOriginalTarget[touchIndex] = hitTR;
			}
			if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
			{
				if (hitTR == buttonLeft && (touchOriginalTarget[touchIndex] == buttonLeft || touchOriginalTarget[touchIndex] == buttonRight))
				{
					GlobalInput.globalInput.touchButtonLeft = true;
				}
				else if (hitTR == buttonRight && (touchOriginalTarget[touchIndex] == buttonLeft || touchOriginalTarget[touchIndex] == buttonRight))
				{
					GlobalInput.globalInput.touchButtonRight = true;
				}
				else if (hitTR == buttonScreen && touchOriginalTarget[touchIndex] == buttonScreen)
				{
					touchSwipe[touchIndex] = false;
					for (int j = 0; j < touchPositions.GetLength(1); j++)
					{
						if (Vector2.Distance(touchPositions[touchIndex, j], touchPosition[touchIndex]) > swipeDistanceYtouch)
						{
							touchOriginalSwipePointIndex[touchIndex] = j;
							touchSwipe[touchIndex] = true;
							hasBeenTouchSwipe[touchIndex] = true;
						}
					}
					if (touchSwipe[touchIndex])
					{
						if (!(touchPositions[touchIndex, touchOriginalSwipePointIndex[touchIndex]].y < touchPosition[touchIndex].y - swipeDistanceYtouch) && !(touchPositions[touchIndex, touchOriginalSwipePointIndex[touchIndex]].y > touchPosition[touchIndex].y + swipeDistanceYtouch))
						{
							if (touchPositions[touchIndex, touchOriginalSwipePointIndex[touchIndex]].x < touchPosition[touchIndex].x - swipeDistanceXtouch && !cursorSwipeLeftDone)
							{
								cursorSwipeLeftDone = true;
								cursorSwipeRightDone = false;
								resetTouchPositions[touchIndex] = true;
								GlobalInput.globalInput.swipeScreenRight = true;
							}
							else if (touchPositions[touchIndex, touchOriginalSwipePointIndex[touchIndex]].x > touchPosition[touchIndex].x + swipeDistanceXtouch && !cursorSwipeRightDone)
							{
								cursorSwipeRightDone = true;
								cursorSwipeLeftDone = false;
								resetTouchPositions[touchIndex] = true;
								GlobalInput.globalInput.swipeScreenLeft = true;
							}
						}
					}
					else if (squareAspectRatio > 0)
					{
						ManageButtonScreenAreaSquareAspect(touchPosition[touchIndex]);
					}
				}
			}
			if (touch.phase != TouchPhase.Ended)
			{
				continue;
			}
			if (hitTR == buttonScreen && touchOriginalTarget[touchIndex] == buttonScreen && !hasBeenTouchSwipe[touchIndex])
			{
				if (squareAspectRatio == 0 || PauseController.pauseController.paused)
				{
					GlobalInput.globalInput.touchButtonScreen = true;
					ManageButtonScreenArea(touchPosition[touchIndex]);
				}
				else if (squareAspectRatio > 0)
				{
					ManageButtonScreenAreaSquareAspectEnd(touchPosition[touchIndex]);
				}
			}
			hasBeenTouchSwipe[touchIndex] = false;
			cursorSwipeLeftDone = false;
			cursorSwipeRightDone = false;
		}
	}

	private void ResetTouchPositions(int index)
	{
		touchPosition[index] = Camera.main.ScreenToWorldPoint(touch.position);
		for (int i = 0; i < touchPositions.GetLength(1); i++)
		{
			touchPositions[index, i] = touchPosition[index];
		}
	}

	private void ManageMouse()
	{
		if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
		{
			return;
		}
		cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		hitTR = Physics2D.Raycast(cursorPosition, new Vector2(0f, 0f)).transform;
		if (hitTR == null)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			cursorOriginalTarget = hitTR;
			ResetCursorPositions();
		}
		if (Input.GetMouseButton(0))
		{
			if (hitTR == buttonLeft && (cursorOriginalTarget == buttonLeft || cursorOriginalTarget == buttonRight))
			{
				GlobalInput.globalInput.touchButtonLeft = true;
			}
			else if (hitTR == buttonRight && (cursorOriginalTarget == buttonLeft || cursorOriginalTarget == buttonRight))
			{
				GlobalInput.globalInput.touchButtonRight = true;
			}
			else if (hitTR == buttonScreen && cursorOriginalTarget == buttonScreen)
			{
				cursorSwipe = false;
				for (int i = 0; i < cursorPositions.Length; i++)
				{
					if (Vector2.Distance(cursorPositions[i], cursorPosition) > swipeDistanceY)
					{
						cursorOriginalSwipePointIndex = i;
						cursorSwipe = true;
						hasBeenSwipe = true;
					}
				}
				if (cursorSwipe && !(cursorPositions[cursorOriginalSwipePointIndex].y < cursorPosition.y - swipeDistanceY) && !(cursorPositions[cursorOriginalSwipePointIndex].y > cursorPosition.y + swipeDistanceY))
				{
					if (cursorPositions[cursorOriginalSwipePointIndex].x < cursorPosition.x - swipeDistanceX && !cursorSwipeLeftDone)
					{
						cursorSwipeLeftDone = true;
						cursorSwipeRightDone = false;
						resetCursorPositions = true;
						GlobalInput.globalInput.swipeScreenRight = true;
					}
					else if (cursorPositions[cursorOriginalSwipePointIndex].x > cursorPosition.x + swipeDistanceX && !cursorSwipeRightDone)
					{
						cursorSwipeRightDone = true;
						cursorSwipeLeftDone = false;
						resetCursorPositions = true;
						GlobalInput.globalInput.swipeScreenLeft = true;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (hitTR == buttonScreen && cursorOriginalTarget == buttonScreen && !hasBeenSwipe)
			{
				GlobalInput.globalInput.touchButtonScreen = true;
				ManageButtonScreenArea(cursorPosition);
			}
			hasBeenSwipe = false;
			cursorSwipeLeftDone = false;
			cursorSwipeRightDone = false;
		}
	}

	private void ResetCursorPositions()
	{
		cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		for (int i = 0; i < cursorPositions.Length; i++)
		{
			cursorPositions[i] = cursorPosition;
		}
	}
}
