using UnityEngine;

public class PCK_SlotsProgressController : MonoBehaviour
{
	public Transform leftTR;

	public Transform rightTR;

	public SpriteRenderer leftSR;

	public SpriteRenderer rightSR;

	public Color lowerColor;

	public Color higherColor;

	public Color higherColor2;

	private float currentProgress;

	private float progressTimer;

	private float higherColorChangeTimer;

	private bool higherColorChangeTimerRising = true;

	public AudioClip progressUp;

	public AudioClip reached;

	private float slotsProgressAdjusted;

	private bool reachPlayed;

	private void Start()
	{
		Initialize();
	}

	private void Update()
	{
		UpdateProgress();
	}

	public void Initialize()
	{
		currentProgress = GlobalControl.globalControl.slotsProgress;
	}

	public void ResetProgress()
	{
		GlobalControl.globalControl.slotsProgress = 0f;
		UpdateSlotsProgressAdjusted();
		higherColorChangeTimerRising = true;
		higherColorChangeTimer = 0f;
		reachPlayed = false;
	}

	public void AddProgress(bool golden)
	{
		float num = 0.3f;
		num = ((GlobalControl.globalControl.jackpots >= 10) ? 0.3f : 0.4f);
		if (golden)
		{
			num = 0.4f;
		}
		if (GlobalControl.globalControl.slotsProgress + num < 0.4f)
		{
			num *= 1.1f;
		}
		else if (!(GlobalControl.globalControl.slotsProgress + num < 0.6f))
		{
			num *= 0.9f;
		}
		if (GlobalControl.globalControl.slotsProgress == 1f)
		{
			PCK_SlotsController.PCK_slotsController.AddToQueue();
			if (golden)
			{
				PCK_SlotsController.PCK_slotsController.AddToQueue();
			}
			UpdateSlotsProgressAdjusted();
			return;
		}
		GlobalControl.globalControl.slotsProgress += num;
		if (GlobalControl.globalControl.slotsProgress >= 0.9f)
		{
			GlobalControl.globalControl.slotsProgress = 1f;
		}
		UpdateSlotsProgressAdjusted();
		PlayProgressUp();
	}

	public void UpdateSlotsProgressAdjusted()
	{
		slotsProgressAdjusted = GlobalControl.globalControl.slotsProgress * 3.28125f;
		if (slotsProgressAdjusted % 0.0625f != 0f)
		{
			float num;
			for (num = slotsProgressAdjusted; num >= 0.0625f; num -= 0.0625f)
			{
			}
			if (num > 0f)
			{
				num = 0.0625f - num;
			}
			slotsProgressAdjusted += num;
		}
	}

	private void UpdateProgress()
	{
		currentProgress = Mathf.SmoothDamp(currentProgress, slotsProgressAdjusted, ref progressTimer, 0.2f);
		if (currentProgress >= 1f && GlobalControl.globalControl.slotsProgress == 1f && !reachPlayed)
		{
			reachPlayed = true;
			PlayReached();
		}
		leftTR.localPosition = new Vector3(0f, -3.3125f + currentProgress, -0.1f);
		rightTR.localPosition = new Vector3(0f, -3.3125f + currentProgress, -0.1f);
		if ((double)currentProgress < 0.99)
		{
			leftSR.color = Color.Lerp(lowerColor, higherColor, currentProgress);
			rightSR.color = Color.Lerp(lowerColor, higherColor, currentProgress);
			return;
		}
		if (higherColorChangeTimerRising)
		{
			higherColorChangeTimer += Time.deltaTime;
			if (higherColorChangeTimer >= 1f)
			{
				higherColorChangeTimer = 1f;
				higherColorChangeTimerRising = false;
			}
		}
		else
		{
			higherColorChangeTimer -= Time.deltaTime;
			if (higherColorChangeTimer <= 0f)
			{
				higherColorChangeTimer = 0f;
				higherColorChangeTimerRising = true;
			}
		}
		leftSR.color = Color.Lerp(higherColor, higherColor2, higherColorChangeTimer);
		rightSR.color = Color.Lerp(higherColor, higherColor2, higherColorChangeTimer);
	}

	private void PlayProgressUp()
	{
		GlobalAudio.globalAudio.Play(progressUp, 0.3f, 0.75f + GlobalControl.globalControl.slotsProgress / 2f, 0f);
	}

	private void PlayReached()
	{
		GlobalAudio.globalAudio.Play(reached, 0.4f, 1f, 0f);
	}
}
