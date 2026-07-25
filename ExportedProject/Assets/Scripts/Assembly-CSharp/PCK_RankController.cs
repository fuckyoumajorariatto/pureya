using UnityEngine;

public class PCK_RankController : MonoBehaviour
{
	public static PCK_RankController PCK_rankController;

	public Sprite[] numbers;

	public SpriteRenderer[] marblesCounterNumbers;

	public SpriteRenderer[] marblesCounterNumbers_S;

	public SpriteRenderer[] cornerStarsN;

	public AudioClip bounce;

	public AudioClip starClip;

	public AudioClip transition;

	private string nString;

	private float marblesPitch;

	private float rankPitch;

	public int cornerStarsNumber;

	private void Start()
	{
		PCK_rankController = this;
		Initialize();
	}

	private void Initialize()
	{
		UpdateRankCorner();
		marblesPitch = 1f;
		rankPitch = 1f;
	}

	public void ShowRank()
	{
		UpdateMarblesCounter();
		Invoke("ActivateInput", 0.5f);
		Invoke("End", 1f);
	}

	private void ActivateInput()
	{
		PCK_InputController.PCK_inputController.inputActive = true;
	}

	private void End()
	{
		PCK_Controller.PCK_controller.marbleRefilling = false;
	}

	private void ChangeMarblesNumberTo(int n)
	{
		if (n < 10)
		{
			marblesCounterNumbers[1].sprite = numbers[n];
			marblesCounterNumbers_S[1].sprite = numbers[n];
		}
		else
		{
			nString = n + string.Empty;
			marblesCounterNumbers[0].sprite = numbers[int.Parse(nString[0].ToString())];
			marblesCounterNumbers_S[0].sprite = numbers[int.Parse(nString[0].ToString())];
			marblesCounterNumbers[1].sprite = numbers[int.Parse(nString[1].ToString())];
			marblesCounterNumbers_S[1].sprite = numbers[int.Parse(nString[1].ToString())];
		}
		PlayBounce();
		marblesPitch += 0.015f;
	}

	private void UpdateMarblesCounter()
	{
		PCK_CounterController.PCK_counterController.ShowNumber(GlobalControl.globalControl.marbles);
	}

	public void RaiseRankCorner()
	{
		GlobalControl.globalControl.rank += 10;
		cornerStarsNumber = GlobalControl.globalControl.rank;
		if (cornerStarsNumber > 9)
		{
			nString = cornerStarsNumber + string.Empty;
			cornerStarsN[0].sprite = numbers[int.Parse(nString[0].ToString())];
			cornerStarsN[1].sprite = numbers[int.Parse(nString[1].ToString())];
		}
		else
		{
			cornerStarsN[1].sprite = numbers[cornerStarsNumber];
		}
	}

	public void RaiseRankCornerWithSound()
	{
		GlobalControl.globalControl.rank += 10;
		cornerStarsNumber = GlobalControl.globalControl.rank;
		if (cornerStarsNumber > 99)
		{
			cornerStarsNumber = 99;
		}
		if (cornerStarsNumber > 9)
		{
			nString = cornerStarsNumber + string.Empty;
			cornerStarsN[0].sprite = numbers[int.Parse(nString[0].ToString())];
			cornerStarsN[1].sprite = numbers[int.Parse(nString[1].ToString())];
		}
		else
		{
			cornerStarsN[1].sprite = numbers[cornerStarsNumber];
		}
		rankPitch = 1f;
		PlayStar();
	}

	public void UpdateRankCorner()
	{
		cornerStarsNumber = GlobalControl.globalControl.rank;
		if (cornerStarsNumber > 99)
		{
			cornerStarsNumber = 99;
		}
		cornerStarsN[0].sprite = numbers[0];
		if (cornerStarsNumber > 9)
		{
			nString = cornerStarsNumber + string.Empty;
			cornerStarsN[0].sprite = numbers[int.Parse(nString[0].ToString())];
			cornerStarsN[1].sprite = numbers[int.Parse(nString[1].ToString())];
		}
		else
		{
			cornerStarsN[1].sprite = numbers[cornerStarsNumber];
		}
	}

	private void PlayBounce()
	{
		GlobalAudio.globalAudio.Play(bounce, 1f, marblesPitch, 0f);
	}

	public void PlayTransition()
	{
		GlobalAudio.globalAudio.Play(transition, 0.5f, Random.Range(1.75f, 2f), 0f);
	}

	public void PlayStar()
	{
		if (!PauseController.pauseController.inCinematic)
		{
			GlobalAudio.globalAudio.Play(starClip, 1f, rankPitch, 0f);
		}
		rankPitch += 0.125f;
	}
}
