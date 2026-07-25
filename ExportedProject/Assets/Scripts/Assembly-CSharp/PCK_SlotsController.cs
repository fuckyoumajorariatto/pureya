using System.Collections.Generic;
using UnityEngine;

public class PCK_SlotsController : MonoBehaviour
{
	public static PCK_SlotsController PCK_slotsController;

	public Animator animator;

	private SpriteRenderer[,] boxes;

	public GameObject box00;

	public GameObject box01;

	public GameObject box10;

	public GameObject box11;

	public GameObject box20;

	public GameObject box21;

	private int normalRoll;

	private int fastRoll;

	private int reachRoll;

	private int jackpotRoll;

	private int fightRoll;

	private int winFightBool;

	public Sprite[] numberSprites;

	public Sprite[] iconSprites;

	public GameObject confetti;

	public AudioClip confetti_sound;

	public bool rolling;

	public bool inJackpot;

	public AudioClip rouletteSound;

	public AudioClip slotStopSound;

	public AudioClip reachSound;

	public AudioClip jackpotSound;

	public int resultLeft = 1;

	public int resultMiddle = 1;

	public int resultRight = 1;

	public string resultKind;

	public int rollQueue;

	public float rollCoolDown;

	private bool cinematicShown;

	private List<int> possibleResults;

	private int maxResult;

	private float randomN;

	private float beginnerLuck;

	private List<int> possibilities;

	private float repeatedChance;

	public PCK_SlotsProgressController slotsProgress;

	private float rollLoopRemainingTime;

	private bool finishingRotation;

	public int marblesSkipped;

	public int badFightNumber;

	private bool forceCinematic;

	private int progress;

	private void Start()
	{
		PCK_slotsController = this;
		InitializeBoxes();
		InitializeTriggersToInt();
	}

	private void Update()
	{
		if (!rolling)
		{
			if (rollCoolDown > 0f)
			{
				rollCoolDown -= Time.deltaTime;
			}
			ManageQueue();
		}
		else
		{
			rollLoopRemainingTime -= Time.deltaTime;
			if (rollLoopRemainingTime <= 0f && !finishingRotation)
			{
				FinishRotation();
			}
		}
		Cheats();
	}

	private void Cheats()
	{
		if (!GlobalControl.globalControl.testmode)
		{
			return;
		}
		if (Input.GetKeyDown("0"))
		{
			resultLeft = 1;
			resultRight = 2;
			resultMiddle = 3;
			rolling = true;
			resultKind = "nothing";
			ApplyCheat();
		}
		else if (Input.GetKeyDown("1"))
		{
			resultLeft = 1;
			resultRight = resultLeft;
			resultMiddle = resultLeft;
			rolling = true;
			resultKind = "jackpot";
			ApplyCheat();
		}
		else if (Input.GetKeyDown("2"))
		{
			Debug.Log(Time.timeScale);
			if (Time.timeScale == 1f)
			{
				Time.timeScale = 4f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
		else if (Input.GetKeyDown("3"))
		{
			rolling = true;
			finishingRotation = true;
			resultLeft = 9;
			resultRight = 9;
			resultMiddle = 8;
			resultKind = "reach";
			forceCinematic = true;
			SetCinematics();
			ApplyCheat();
		}
		else if (Input.GetKeyDown("4"))
		{
			finishingRotation = true;
			rolling = true;
			resultLeft = 9;
			resultRight = resultLeft;
			resultMiddle = resultLeft;
			resultKind = "jackpot";
			ApplyCheat();
		}
		else if (Input.GetKeyDown("9"))
		{
			AddToQueue();
		}
	}

	private void ApplyCheat()
	{
		if (resultKind == "nothing")
		{
			if (rollQueue > 0)
			{
				animator.SetTrigger(fastRoll);
			}
			else
			{
				animator.SetTrigger(normalRoll);
			}
		}
		else if (resultKind == "reach" || resultKind == "jackpot")
		{
			animator.SetTrigger(reachRoll);
		}
		else if (resultKind == "cinematic" || resultKind == "cinematicJackpot")
		{
			animator.SetTrigger(reachRoll);
		}
	}

	public bool AreSlotsWorking()
	{
		if (!rolling && rollQueue == 0 && rollCoolDown <= 0f)
		{
			return false;
		}
		return true;
	}

	private void ManageQueue()
	{
		if (!rolling && rollQueue > 0 && rollCoolDown <= 0f && !PCK_Controller.PCK_controller.managingJackpot)
		{
			rollQueue--;
			PCK_HoleController.PCK_holeController.StartStarParticles();
			rollLoopRemainingTime = 2f;
			StartRotation();
		}
	}

	public void AddToQueue()
	{
		if (rolling || rollQueue > 0 || inJackpot)
		{
			PCK_HoleStarsController.PCK_holeStarsController.ShowStar();
			if (GlobalControl.globalControl.rank < 75)
			{
				if (rollLoopRemainingTime < 1f)
				{
					rollLoopRemainingTime = 1f;
				}
			}
			else if (GlobalControl.globalControl.rank >= 99)
			{
				rollLoopRemainingTime = 0f;
			}
		}
		else
		{
			rollQueue++;
		}
	}

	public void StartRotation()
	{
		rolling = true;
		finishingRotation = false;
		animator.SetTrigger("rollLoop");
	}

	public void FinishRotation()
	{
		finishingRotation = true;
		rolling = true;
		SetNextResult();
		if (resultKind == "nothing")
		{
			if (rollQueue > 0)
			{
				animator.SetTrigger(fastRoll);
			}
			else
			{
				animator.SetTrigger(normalRoll);
			}
		}
		else if (resultKind == "reach" || resultKind == "jackpot")
		{
			animator.SetTrigger(reachRoll);
		}
		else if (resultKind == "cinematic" || resultKind == "cinematicJackpot")
		{
			animator.SetTrigger(reachRoll);
		}
	}

	private void SetNextResult()
	{
		beginnerLuck = 0f;
		if (GlobalControl.globalControl.jackpots == 0)
		{
			beginnerLuck = 0.5f;
		}
		else if (GlobalControl.globalControl.jackpots < 5)
		{
			beginnerLuck = 0.25f;
		}
		else if (GlobalControl.globalControl.jackpots < 10)
		{
			beginnerLuck = 0.1f;
		}
		progress = GlobalControl.globalControl.CalculateProgress();
		if (progress >= 75 && progress < 100)
		{
			beginnerLuck = 0.3f;
		}
		if (GlobalControl.globalControl.goldenIndicator)
		{
			beginnerLuck += 0.2f;
		}
		resultLeft = GetRandomSlotNumber();
		resultRight = GetRandomSlotNumber();
		resultMiddle = GetRandomSlotNumber();
		if (resultLeft != resultRight || resultLeft != resultMiddle)
		{
			marblesSkipped = 0;
			float num = Random.Range(0f, 1f);
			if (num < 0.3f - (float)GlobalControl.globalControl.rank / 450f)
			{
				resultLeft = 1;
			}
			else if (num < 0.5f - (float)GlobalControl.globalControl.rank / 300f)
			{
				resultLeft = 2;
			}
			else if (num < 0.65f - (float)GlobalControl.globalControl.rank / 260f)
			{
				resultLeft = 3;
			}
			else if (num < 0.75f - (float)GlobalControl.globalControl.rank / 300f)
			{
				resultLeft = 4;
			}
			else if (num < 0.83f - (float)GlobalControl.globalControl.rank / 350f)
			{
				resultLeft = 5;
			}
			else if (num < 0.9f - (float)GlobalControl.globalControl.rank / 430f)
			{
				resultLeft = 6;
			}
			else if (num < 0.96f - (float)GlobalControl.globalControl.rank / 540f)
			{
				resultLeft = 7;
			}
			else if (num < 0.99f - (float)GlobalControl.globalControl.rank / 990f)
			{
				resultLeft = 8;
			}
			else
			{
				resultLeft = 9;
			}
			if (progress >= 75 && progress < 100 && Random.Range(0f, 1f) < 0.5f)
			{
				resultLeft += Random.Range(1, 6);
				if (resultLeft > 9)
				{
					resultLeft = 9;
				}
			}
			if (GlobalControl.globalControl.rank == 99 && resultLeft < 3)
			{
				resultLeft = 3;
			}
			else if (GlobalControl.globalControl.rank > 50 && resultLeft < 2)
			{
				resultLeft = 2;
			}
			float num2 = 0.6f - (float)resultLeft / 18f + (float)GlobalControl.globalControl.rank / 111f + beginnerLuck;
			if (num2 > 0.99f && resultLeft > 3)
			{
				num2 = 0.99f;
			}
			if (resultLeft == 1 && num2 < 0.8f)
			{
				num2 = 0.8f;
			}
			else if (resultLeft == 2 && num2 < 0.6f)
			{
				num2 = 0.6f;
			}
			else if (resultLeft == 3 && num2 < 0.5f)
			{
				num2 = 0.5f;
			}
			float num3 = Random.Range(0f, 1f);
			if (num3 < num2)
			{
				resultRight = resultLeft;
				resultMiddle = resultLeft;
			}
			else
			{
				num3 = Random.Range(0f, 1f);
				if (num3 < num2 + (float)(resultLeft / 12) || GlobalControl.globalControl.rank > 30)
				{
					resultRight = resultLeft;
					resultMiddle = Random.Range(1, 10);
					if (resultMiddle == resultLeft)
					{
						if (resultLeft == 1)
						{
							resultMiddle++;
						}
						else
						{
							resultMiddle--;
						}
					}
				}
				else
				{
					resultRight = Random.Range(1, 10);
					if (resultRight == resultLeft)
					{
						if (resultLeft == 1)
						{
							resultRight++;
						}
						else
						{
							resultRight--;
						}
					}
					resultMiddle = Random.Range(1, 10);
				}
			}
		}
		if (GlobalControl.globalControl.storyLevel == 0)
		{
			resultLeft = GetRandomSlotNumber();
			resultRight = GetRandomSlotNumber();
			while (resultRight == resultLeft)
			{
				resultRight++;
				if (resultRight > 9)
				{
					resultRight = 1;
				}
			}
			resultMiddle = GetRandomSlotNumber();
		}
		if (resultLeft == resultRight && resultLeft == resultMiddle)
		{
			resultKind = "jackpot";
		}
		else if (resultLeft == resultRight)
		{
			resultKind = "reach";
		}
		else
		{
			resultKind = "nothing";
		}
		if (GlobalControl.globalControl.jackpots > 10)
		{
			SetCinematics();
		}
	}

	private int GetWeightedNumber(float multiplyingFactor)
	{
		possibilities = new List<int>();
		repeatedChance = 1f;
		if (multiplyingFactor >= 1f)
		{
			for (int num = 9; num > 0; num--)
			{
				for (int i = 0; i < Mathf.FloorToInt(repeatedChance); i++)
				{
					possibilities.Add(num);
				}
				repeatedChance = repeatedChance * multiplyingFactor + 1f;
			}
		}
		else
		{
			for (int j = 1; j <= 9; j++)
			{
				for (int k = 0; k < Mathf.FloorToInt(repeatedChance); k++)
				{
					possibilities.Add(j);
				}
				repeatedChance = repeatedChance / multiplyingFactor + 1f;
			}
		}
		return possibilities[Random.Range(0, possibilities.Count)];
	}

	private void SetCinematics()
	{
		if (forceCinematic)
		{
			resultKind = "cinematic";
			cinematicShown = true;
			forceCinematic = false;
		}
		else
		{
			if (cinematicShown || resultKind == "nothing")
			{
				return;
			}
			if (resultKind == "jackpot")
			{
				float num = 0.9f;
				if (progress > 75)
				{
					num = 0.3f;
				}
				if (resultLeft > 7 && Random.Range(0f, 1f) < num)
				{
					resultMiddle -= Random.Range(1, 4);
					if (resultMiddle < 1)
					{
						resultMiddle = 1;
					}
					resultKind = "cinematicJackpot";
					cinematicShown = true;
				}
			}
			if (resultKind == "reach" && resultLeft > 3 && (GlobalControl.globalControl.rank > 20 || resultLeft > 5) && Random.Range(0f, 1f) < (float)resultLeft / 25f + (float)GlobalControl.globalControl.rank / 200f)
			{
				resultKind = "cinematic";
				cinematicShown = true;
			}
		}
	}

	private void InitializeBoxes()
	{
		boxes = new SpriteRenderer[3, 2];
		boxes[0, 0] = box00.GetComponent<SpriteRenderer>();
		boxes[0, 1] = box01.GetComponent<SpriteRenderer>();
		boxes[1, 0] = box10.GetComponent<SpriteRenderer>();
		boxes[1, 1] = box11.GetComponent<SpriteRenderer>();
		boxes[2, 0] = box20.GetComponent<SpriteRenderer>();
		boxes[2, 1] = box21.GetComponent<SpriteRenderer>();
	}

	private void InitializeTriggersToInt()
	{
		fastRoll = Animator.StringToHash("fastRoll");
		normalRoll = Animator.StringToHash("normalRoll");
		reachRoll = Animator.StringToHash("reachRoll");
		jackpotRoll = Animator.StringToHash("jackpot");
		fightRoll = Animator.StringToHash("fight");
		winFightBool = Animator.StringToHash("win");
	}

	private void ManageJackpot()
	{
		if (GlobalControl.globalControl.jackpots == 0)
		{
			PCK_UnlocksController.PCK_unlocksController.firstJackpot = true;
			GlobalControl.globalControl._111 = true;
			GlobalControl.globalControl.UnlockAchievements();
		}
		GlobalControl.globalControl.jackpots += resultLeft;
		PCK_InputController.PCK_inputController.CancelShoots();
		confetti.SetActive(true);
		GlobalAudio.globalAudio.Play(confetti_sound, 1f, 1f, 0f);
		animator.SetTrigger(jackpotRoll);
		Invoke("DisableConfetti", 3f);
		PlayJackpotSound();
		PCK_Controller.PCK_controller.ManageJackpot(resultLeft);
		rolling = false;
		inJackpot = true;
	}

	public void ManageHole()
	{
		if (rollQueue == 0)
		{
			GlobalControl.globalControl.rank = 0;
			PCK_RankController.PCK_rankController.UpdateRankCorner();
			rolling = true;
			PCK_HoleController.PCK_holeController.StopStarParticles();
			PCK_HoleController.PCK_holeController.ChangePosition();
			Invoke("FalsifyRolling", 0.25f);
		}
	}

	private void FalsifyRolling()
	{
		rolling = false;
	}

	private void DisableConfetti()
	{
		confetti.SetActive(false);
	}

	private void PlayJackpotSound()
	{
		GlobalAudio.globalAudio.Play(jackpotSound, 1f, 1f, 0f);
	}

	private void PlayReachSound()
	{
		GlobalAudio.globalAudio.Play(reachSound, 1f, 1f, 0f);
	}

	private void PlayRoulette()
	{
		GlobalAudio.globalAudio.Play(rouletteSound, Random.Range(0.75f, 1f), 1f, 0f);
	}

	private void PlaySlotStop()
	{
		GlobalAudio.globalAudio.Play(slotStopSound, Random.Range(0.75f, 1f), Random.Range(0.85f, 1.15f), 0f);
	}

	private void ChangeBoxNumbersDuringRoll3()
	{
		boxes[0, 1].sprite = boxes[0, 0].sprite;
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[2, 1].sprite = boxes[2, 0].sprite;
		boxes[0, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		boxes[1, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		boxes[2, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		PlayRoulette();
	}

	private int GetInferiorOrAlternateNumber(int n)
	{
		int num = n;
		num--;
		if (num < 0)
		{
			num = numberSprites.Length - 1;
		}
		return num;
	}

	private void SetFinalNumber3()
	{
		boxes[0, 1].sprite = boxes[0, 0].sprite;
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[2, 1].sprite = boxes[2, 0].sprite;
		boxes[0, 0].sprite = numberSprites[resultLeft - 1];
		boxes[1, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		boxes[2, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		PlayRoulette();
	}

	private void ChangeBoxNumbersDuringRoll2()
	{
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[2, 1].sprite = boxes[2, 0].sprite;
		boxes[1, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		boxes[2, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		PlayRoulette();
	}

	private void SetFinalNumber2()
	{
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[2, 1].sprite = boxes[2, 0].sprite;
		boxes[1, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		boxes[2, 0].sprite = numberSprites[resultRight - 1];
		PlayRoulette();
	}

	private void ChangeBoxNumbersDuringRoll1()
	{
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[1, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
		PlayRoulette();
	}

	private void SetFinalNumber1()
	{
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[1, 0].sprite = numberSprites[resultMiddle - 1];
		PlayRoulette();
	}

	private void ChangeBoxNumbersEndFight()
	{
		boxes[1, 1].sprite = boxes[1, 0].sprite;
		boxes[1, 0].sprite = numberSprites[GetRandomSlotNumber() - 1];
	}

	private void SetFinalNumberFight()
	{
		boxes[1, 0].sprite = numberSprites[resultLeft - 1];
	}

	public void RollFinished()
	{
		if (resultKind == "nothing")
		{
			rollCoolDown = 0.5f;
			ManageHole();
			slotsProgress.ResetProgress();
		}
		if (resultKind == "cinematic")
		{
			rolling = true;
			SetFinalNumberFight();
			badFightNumber = resultMiddle;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				resultMiddle = resultLeft;
				resultKind = "jackpot";
			}
			else
			{
				resultKind = "nothing";
			}
			animator.SetTrigger("preCinematicFight");
		}
		else if (resultKind == "cinematicJackpot")
		{
			rolling = true;
			SetFinalNumberFight();
			badFightNumber = resultMiddle;
			if (Random.Range(0f, 1f) < 0.9f)
			{
				resultMiddle = resultLeft;
				resultKind = "jackpot";
			}
			else
			{
				resultKind = "nothing";
			}
			animator.SetTrigger("preCinematicFight");
		}
		else if (resultKind == "reach")
		{
			if (Random.Range(0f, 1f) < 0.1f + (float)GlobalControl.globalControl.rank / 170f)
			{
				rolling = true;
				SetFinalNumberFight();
				if (Random.Range(0f, 1f) < 0.5f + (float)GlobalControl.globalControl.rank / 250f)
				{
					resultMiddle = resultLeft;
					resultKind = "jackpot";
					animator.SetTrigger(fightRoll);
					animator.SetBool(winFightBool, true);
				}
				else
				{
					resultKind = "nothing";
					animator.SetTrigger(fightRoll);
					animator.SetBool(winFightBool, false);
				}
			}
			else
			{
				rollCoolDown = 0.5f;
				ManageHole();
				slotsProgress.ResetProgress();
			}
		}
		else if (resultKind == "jackpot")
		{
			rollCoolDown = 2f;
			ManageJackpot();
		}
	}

	public int GetRandomSlotNumber()
	{
		return Random.Range(1, 10);
	}
}
