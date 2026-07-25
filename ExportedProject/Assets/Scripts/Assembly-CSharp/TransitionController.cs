using UnityEngine;

public class TransitionController : MonoBehaviour
{
	public AudioClip snap;

	public SpriteRenderer minigameCounterDigit1;

	public SpriteRenderer minigameCounterDigit1_s;

	public Sprite[] sprNumbers;

	public SpriteRenderer[] marbleCounterNumbers;

	public SpriteRenderer[] marbleCounterNumbers_s;

	public Transform[] marbleCounterNumbersTR;

	private string s;

	private char[] charArray;

	private int marbles;

	private bool updatingMarbleCounter;

	public bool transitionClosed;

	private int[] miniNumbers;

	private int n;

	private void Update()
	{
		if (updatingMarbleCounter)
		{
			UpdateMarbleCounter();
		}
	}

	public void PlaySnap()
	{
		GlobalAudio.globalAudio.Play(snap, 1f, 1f, 0f);
	}

	public void EndOut()
	{
		updatingMarbleCounter = false;
		transitionClosed = false;
		SceneChangeController.sceneChangeController.TransitionEnd();
	}

	public void UpdateMinigameCounter()
	{
		n = MinigamesController.minigamesController.remainingMinigames;
		minigameCounterDigit1.sprite = sprNumbers[n];
		minigameCounterDigit1_s.sprite = sprNumbers[n];
	}

	private void TransitionClosed()
	{
		transitionClosed = true;
		SceneChangeController.sceneChangeController.inTransition = false;
	}

	public void StartUpdateMarbleCounter()
	{
		updatingMarbleCounter = true;
		marbleCounterNumbers[0].sprite = null;
		marbleCounterNumbers[1].sprite = null;
		marbleCounterNumbers[2].sprite = null;
	}

	private void UpdateMarbleCounter()
	{
		marbles = GlobalControl.globalControl.alternateMarbles;
		if (marbles > 999)
		{
			marbles = 999;
		}
		s = marbles + string.Empty;
		charArray = s.ToCharArray();
		if (marbles < 10)
		{
			marbleCounterNumbers[0].sprite = sprNumbers[(int)char.GetNumericValue(charArray[0])];
			marbleCounterNumbersTR[0].localPosition = new Vector3(0f, 0f, 0f);
		}
		else if (marbles < 100)
		{
			marbleCounterNumbers[0].sprite = sprNumbers[(int)char.GetNumericValue(charArray[0])];
			marbleCounterNumbers[1].sprite = sprNumbers[(int)char.GetNumericValue(charArray[1])];
			marbleCounterNumbersTR[0].localPosition = new Vector3(-0.875f, 0f, 0f);
			marbleCounterNumbersTR[1].localPosition = new Vector3(0.875f, 0f, 0f);
		}
		else
		{
			marbleCounterNumbers[0].sprite = sprNumbers[(int)char.GetNumericValue(charArray[0])];
			marbleCounterNumbers[1].sprite = sprNumbers[(int)char.GetNumericValue(charArray[1])];
			marbleCounterNumbers[2].sprite = sprNumbers[(int)char.GetNumericValue(charArray[2])];
			marbleCounterNumbersTR[0].localPosition = new Vector3(-1.75f, 0f, 0f);
			marbleCounterNumbersTR[1].localPosition = new Vector3(0f, 0f, 0f);
			marbleCounterNumbersTR[2].localPosition = new Vector3(1.75f, 0f, 0f);
		}
		marbleCounterNumbers_s[0].sprite = marbleCounterNumbers[0].sprite;
		marbleCounterNumbers_s[1].sprite = marbleCounterNumbers[1].sprite;
		marbleCounterNumbers_s[2].sprite = marbleCounterNumbers[2].sprite;
	}
}
