using UnityEngine;

public class PauseButtonsInputController : MonoBehaviour
{
	public static PauseButtonsInputController pauseButtonsInputController;

	private GameObject[] buttons;

	public GameObject b1;

	public GameObject b2;

	public GameObject b3;

	public GameObject b4;

	public GameObject b5;

	public GameObject b6;

	public GameObject b7;

	public GameObject b8;

	public GameObject b9;

	private bool[] bControl;

	public bool[] bTouch;

	private bool[] bControlResetOnRelease;

	private bool preSwipe;

	public bool swipe;

	private void Start()
	{
		pauseButtonsInputController = this;
		buttons = new GameObject[9] { b1, b2, b3, b4, b5, b6, b7, b8, b9 };
		bControl = new bool[9];
		bTouch = new bool[9];
		bControlResetOnRelease = new bool[9];
	}

	private void Update()
	{
		ResetTouch();
		ManageInput();
	}

	private void ResetTouch()
	{
		for (int i = 0; i < bTouch.Length; i++)
		{
			bTouch[i] = false;
		}
	}

	private void ManageInput()
	{
		if (Input.touchCount > 0)
		{
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
				{
					ManageTouch(Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), new Vector2(0f, 0f)));
				}
			}
		}
		else if (!bTouch[0] && !bTouch[1] && !bTouch[2] && !bTouch[3] && !bTouch[4] && !bTouch[5] && !bTouch[6] && !bTouch[7] && !bTouch[8])
		{
			ManageCursorTouch(Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0f, 0f)));
		}
	}

	private void ManageCursorTouch(RaycastHit2D hit)
	{
		if (hit.transform == null)
		{
			return;
		}
		if (Input.GetMouseButton(0))
		{
			ManageTouch(hit);
			return;
		}
		preSwipe = false;
		swipe = false;
		for (int i = 0; i < buttons.Length; i++)
		{
			if (bControlResetOnRelease[i])
			{
				bControl[i] = false;
				bControlResetOnRelease[i] = false;
			}
		}
	}

	private void ManageTouch(RaycastHit2D hit)
	{
		if (hit.transform == null)
		{
			return;
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			if (hit.transform.transform.gameObject == buttons[i] && !bControl[i] && !bControlResetOnRelease[i])
			{
				if (!preSwipe)
				{
					preSwipe = true;
					bTouch[i] = true;
				}
				else
				{
					swipe = true;
				}
				if (swipe && !bControl[i])
				{
					bControlResetOnRelease[i] = true;
				}
				break;
			}
			if (hit.transform.transform.gameObject != buttons[i] && !swipe)
			{
				bControl[i] = false;
			}
		}
	}

	public void ResetControls()
	{
		for (int i = 0; i < bControl.Length; i++)
		{
			bControl[i] = false;
		}
	}
}
