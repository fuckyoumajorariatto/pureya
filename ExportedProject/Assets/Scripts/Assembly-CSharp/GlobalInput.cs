using UnityEngine;

public class GlobalInput : MonoBehaviour
{
	public static GlobalInput globalInput;

	public bool up;

	public bool down;

	public bool left;

	public bool right;

	public bool upDown;

	public bool downDown;

	public bool leftDown;

	public bool rightDown;

	public bool accept;

	public bool acceptDown;

	public bool cancel;

	public bool cancelDown;

	public bool exit;

	public bool exitDown;

	public bool start;

	public bool startDown;

	public bool arrowsAcceptDown;

	public bool touchButtonLeft;

	public bool touchButtonRight;

	public bool touchButtonScreen;

	public bool touchButtonScreenMiddle;

	public bool touchButtonScreenLeft;

	public bool touchButtonScreenRight;

	public bool swipeScreenLeft;

	public bool swipeScreenRight;

	private bool[] leftQueue = new bool[5];

	private bool[] rightQueue = new bool[5];

	private bool[] leftScreenQueue = new bool[5];

	private bool[] rightScreenQueue = new bool[5];

	public bool stepped;

	private float timePressed;

	public bool gameInputActive = true;

	private int frameCount;

	private int Stick1Strikes;

	private bool Stick1Malfunctioning;

	private int Stick2Strikes;

	private bool Stick2Malfunctioning;

	private int DpadStrikes;

	private bool DpadMalfunctioning;

	private int triggersStrikes;

	private bool triggersMalfunctioning;

	private int bumpersStrikes;

	private bool bumpersMalfunctioning;

	private float autoInputTimer = -3f;

	private string continuedAutoPress = string.Empty;

	private void Start()
	{
		globalInput = this;
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < leftQueue.Length - 1; i++)
		{
			leftQueue[i] = leftQueue[i + 1];
			rightQueue[i] = rightQueue[i + 1];
		}
		leftQueue[leftQueue.Length - 1] = left;
		rightQueue[rightQueue.Length - 1] = right;
		for (int j = 0; j < leftScreenQueue.Length - 1; j++)
		{
			leftScreenQueue[j] = leftScreenQueue[j + 1];
			rightScreenQueue[j] = rightScreenQueue[j + 1];
		}
		leftScreenQueue[leftScreenQueue.Length - 1] = touchButtonScreenLeft;
		rightScreenQueue[rightScreenQueue.Length - 1] = touchButtonScreenRight;
	}

	private void Update()
	{
		if (frameCount < 11)
		{
			AxisCheck();
			return;
		}
		if (Input.GetAxisRaw("DownArrow") == 1f || Input.GetAxisRaw("DownWASD") == 1f || (Input.GetAxisRaw("VerticalStick1") == 1f && !Stick1Malfunctioning) || (Input.GetAxisRaw("VerticalStick2") == 1f && !Stick2Malfunctioning) || (Input.GetAxisRaw("VerticalDpad") == 1f && !DpadMalfunctioning))
		{
			if (!down)
			{
				downDown = true;
			}
			else
			{
				downDown = false;
			}
			down = true;
		}
		else
		{
			down = false;
			downDown = false;
		}
		if (Input.GetAxisRaw("UpArrow") == 1f || Input.GetAxisRaw("UpWASD") == 1f || (Input.GetAxisRaw("VerticalStick1") == -1f && !Stick1Malfunctioning) || (Input.GetAxisRaw("VerticalStick2") == -1f && !Stick2Malfunctioning) || (Input.GetAxisRaw("VerticalDpad") == -1f && !DpadMalfunctioning))
		{
			if (!up)
			{
				upDown = true;
			}
			else
			{
				upDown = false;
			}
			up = true;
		}
		else
		{
			up = false;
			upDown = false;
		}
		if (touchButtonRight || Input.GetAxisRaw("RightArrow") == 1f || Input.GetAxisRaw("RightWASD") == 1f || (Input.GetAxisRaw("HorizontalStick1") == 1f && !Stick1Malfunctioning) || (Input.GetAxisRaw("HorizontalStick2") == 1f && !Stick2Malfunctioning) || (Input.GetAxisRaw("HorizontalDpad") == 1f && !DpadMalfunctioning) || (Input.GetAxisRaw("RightTrigger") == 1f && !triggersMalfunctioning) || (Input.GetAxisRaw("RightBumper") == 1f && !bumpersMalfunctioning))
		{
			if (!right)
			{
				rightDown = true;
			}
			else
			{
				rightDown = false;
			}
			right = true;
		}
		else
		{
			right = false;
			rightDown = false;
		}
		if (touchButtonLeft || Input.GetAxisRaw("LeftArrow") == 1f || Input.GetAxisRaw("LeftWASD") == 1f || (Input.GetAxisRaw("HorizontalStick1") == -1f && !Stick1Malfunctioning) || (Input.GetAxisRaw("HorizontalStick2") == -1f && !Stick2Malfunctioning) || (Input.GetAxisRaw("HorizontalDpad") == -1f && !DpadMalfunctioning) || (Input.GetAxisRaw("LeftTrigger") == 1f && !triggersMalfunctioning) || (Input.GetAxisRaw("LeftBumper") == 1f && !bumpersMalfunctioning))
		{
			if (!left)
			{
				leftDown = true;
			}
			else
			{
				leftDown = false;
			}
			left = true;
		}
		else
		{
			left = false;
			leftDown = false;
		}
		if (Input.GetAxisRaw("Accept") == 1f || Input.GetAxisRaw("Enter") == 1f)
		{
			if (!accept)
			{
				acceptDown = true;
			}
			else
			{
				acceptDown = false;
			}
			accept = true;
		}
		else
		{
			accept = false;
			acceptDown = false;
		}
		if (Input.GetAxisRaw("Cancel") == 1f || Input.GetAxisRaw("Back") == 1f)
		{
			if (!cancel)
			{
				cancelDown = true;
			}
			else
			{
				cancelDown = false;
			}
			cancel = true;
		}
		else
		{
			cancel = false;
			cancelDown = false;
		}
		if (Input.GetAxisRaw("Esc") == 1f)
		{
			if (!exit)
			{
				exitDown = true;
			}
			else
			{
				exitDown = false;
			}
			exit = true;
		}
		else
		{
			exit = false;
			exitDown = false;
		}
		if (Input.GetAxisRaw("Start") == 1f)
		{
			if (!start)
			{
				startDown = true;
			}
			else
			{
				startDown = false;
			}
			start = true;
		}
		else
		{
			start = false;
			startDown = false;
		}
		if (up || down || left || right)
		{
			if (!stepped && timePressed == 0f)
			{
				stepped = true;
			}
			else if (timePressed >= 0.5f)
			{
				stepped = true;
				timePressed = 0f;
			}
			else
			{
				stepped = false;
			}
			timePressed += Time.unscaledDeltaTime;
		}
		else
		{
			timePressed = 0f;
			stepped = false;
		}
		if ((leftDown && rightDown) || (touchButtonScreenLeft && touchButtonScreenRight))
		{
			arrowsAcceptDown = true;
		}
		else if (leftDown && right)
		{
			arrowsAcceptDown = false;
			for (int i = 0; i < rightQueue.Length; i++)
			{
				if (!rightQueue[i])
				{
					arrowsAcceptDown = true;
					break;
				}
			}
		}
		else if (left && rightDown)
		{
			arrowsAcceptDown = false;
			for (int j = 0; j < leftQueue.Length; j++)
			{
				if (!leftQueue[j])
				{
					arrowsAcceptDown = true;
					break;
				}
			}
		}
		else if (touchButtonScreenLeft)
		{
			arrowsAcceptDown = false;
			for (int k = 0; k < rightScreenQueue.Length; k++)
			{
				if (rightScreenQueue[k])
				{
					arrowsAcceptDown = true;
					break;
				}
			}
		}
		else if (touchButtonScreenRight)
		{
			arrowsAcceptDown = false;
			for (int l = 0; l < leftScreenQueue.Length; l++)
			{
				if (leftScreenQueue[l])
				{
					arrowsAcceptDown = true;
					break;
				}
			}
		}
		else
		{
			arrowsAcceptDown = false;
		}
	}

	private void AxisCheck()
	{
		frameCount++;
		if (Input.GetAxis("VerticalStick1") == 1f || Input.GetAxis("VerticalStick1") == -1f || Input.GetAxis("HorizontalStick1") == -1f || Input.GetAxis("HorizontalStick1") == 1f)
		{
			Stick1Strikes++;
		}
		else
		{
			Stick1Strikes = 0;
		}
		if (Input.GetAxis("VerticalStick2") == 1f || Input.GetAxis("VerticalStick2") == -1f || Input.GetAxis("HorizontalStick2") == -1f || Input.GetAxis("HorizontalStick2") == 1f)
		{
			Stick2Strikes++;
		}
		else
		{
			Stick2Strikes = 0;
		}
		if (Input.GetAxis("VerticalDpad") == 1f || Input.GetAxis("VerticalDpad") == -1f || Input.GetAxis("HorizontalDpad") == -1f || Input.GetAxis("HorizontalDpad") == 1f)
		{
			DpadStrikes++;
		}
		else
		{
			DpadStrikes = 0;
		}
		if (Input.GetAxisRaw("LeftTrigger") == 1f || Input.GetAxisRaw("RightTrigger") == 1f)
		{
			triggersStrikes++;
		}
		else
		{
			triggersStrikes = 0;
		}
		if (Input.GetAxisRaw("LeftBumper") == 1f || Input.GetAxisRaw("RightBumper") == 1f)
		{
			bumpersStrikes++;
		}
		else
		{
			bumpersStrikes = 0;
		}
		if (frameCount == 10)
		{
			if (Stick1Strikes > 4)
			{
				Stick1Malfunctioning = true;
				Debug.Log("Stick1 malfunctioning. Strikes: " + Stick1Strikes);
			}
			if (Stick2Strikes > 4)
			{
				Stick2Malfunctioning = true;
				Debug.Log("Stick2 malfunctioning. Strikes: " + Stick2Strikes);
			}
			if (DpadStrikes > 4)
			{
				DpadMalfunctioning = true;
				Debug.Log("Dpad malfunctioning. Strikes: " + DpadStrikes);
			}
			if (triggersStrikes > 4)
			{
				triggersMalfunctioning = true;
				Debug.Log("Triggers malfunctioning. Strikes: " + triggersMalfunctioning);
			}
			if (bumpersStrikes > 4)
			{
				bumpersMalfunctioning = true;
				Debug.Log("Bumpers malfunctioning. Strikes: " + bumpersStrikes);
			}
		}
	}

	private void AutoInput()
	{
		if (autoInputTimer > 1f)
		{
			autoInputTimer = 0f;
			if (Random.Range(0f, 1f) > 0.5f)
			{
				leftDown = true;
				left = true;
				if (Random.Range(0f, 1f) > 0.5f)
				{
					continuedAutoPress = "left";
				}
				else
				{
					continuedAutoPress = string.Empty;
				}
			}
			else
			{
				rightDown = true;
				right = true;
				if (Random.Range(0f, 1f) > 0.5f)
				{
					continuedAutoPress = "right";
				}
				else
				{
					continuedAutoPress = string.Empty;
				}
			}
		}
		else if (continuedAutoPress == "left")
		{
			left = true;
		}
		else if (continuedAutoPress == "right")
		{
			right = true;
		}
		autoInputTimer += Time.unscaledDeltaTime;
	}
}
