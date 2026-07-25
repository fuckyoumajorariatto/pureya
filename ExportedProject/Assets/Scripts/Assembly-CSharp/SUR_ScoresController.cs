using UnityEngine;

public class SUR_ScoresController : MonoBehaviour
{
	public Animator[] recordIcons;

	public Animator[] lastScoreIcons;

	public Sprite[] numberSprites;

	public GameObject recordGO;

	public GameObject lastScoreGO;

	public GameObject differenceRecord;

	public GameObject differenceLastScore;

	public SpriteRenderer[] differenceLastScoreArrow;

	public Sprite[] arrows;

	public SpriteRenderer[] recordNumbers;

	public SpriteRenderer[] recordNumbers_s;

	public SpriteRenderer[] lastScoreNumbers;

	public SpriteRenderer[] lastScoreNumbers_s;

	public SpriteRenderer[] differenceRecordNumbers;

	public SpriteRenderer[] differenceRecordNumbers_s;

	public SpriteRenderer[] differenceLastScoreNumbers;

	public SpriteRenderer[] differenceLastScoreNumbers_s;

	public SpriteRenderer[] medal;

	public Sprite medalIcon;

	private bool showMedal;

	public Color32[] arrowColors;

	public AudioClip recordSound;

	public AudioClip scoreSound;

	private string s;

	private char[] charArray;

	private int sprinTrigger;

	private int previousRecord;

	private int previousScore;

	private bool comingFromMinigame;

	public SpriteRenderer[] marblesCounter;

	private void Awake()
	{
		sprinTrigger = Animator.StringToHash("spin");
	}

	public void UpdateScores(string minigame)
	{
		showMedal = false;
		recordGO.SetActive(true);
		lastScoreGO.SetActive(true);
		s = "0";
		for (int i = 0; i < recordNumbers.Length; i++)
		{
			recordNumbers[i].sprite = numberSprites[0];
			recordNumbers_s[i].sprite = recordNumbers[i].sprite;
			lastScoreNumbers[i].sprite = recordNumbers[i].sprite;
			lastScoreNumbers_s[i].sprite = lastScoreNumbers[i].sprite;
			differenceRecordNumbers[i].sprite = recordNumbers[i].sprite;
			differenceRecordNumbers_s[i].sprite = recordNumbers[i].sprite;
			differenceLastScoreNumbers[i].sprite = recordNumbers[i].sprite;
			differenceLastScoreNumbers_s[i].sprite = recordNumbers[i].sprite;
		}
		comingFromMinigame = false;
		if (GlobalControl.globalControl.backFromMinigames)
		{
			comingFromMinigame = true;
			GlobalControl.globalControl.backFromMinigames = false;
		}
		if (GlobalControl.globalControl.alternateMarbles > 999)
		{
			GlobalControl.globalControl.alternateMarbles = 999;
		}
		if (comingFromMinigame)
		{
			if (minigame == "MIX")
			{
				previousRecord = GlobalControl.globalControl.MIXscores[0];
				previousScore = GlobalControl.globalControl.MIXscores[1];
				GlobalControl.globalControl.MIXscores[1] = GlobalControl.globalControl.alternateMarbles;
				if (GlobalControl.globalControl.alternateMarbles > previousRecord)
				{
					GlobalControl.globalControl.MIXscores[0] = GlobalControl.globalControl.alternateMarbles;
				}
			}
			else if (minigame == "MAX")
			{
				previousRecord = GlobalControl.globalControl.MAXscores[0];
				previousScore = GlobalControl.globalControl.MAXscores[1];
				GlobalControl.globalControl.MAXscores[1] = GlobalControl.globalControl.alternateMarbles;
				if (GlobalControl.globalControl.alternateMarbles > previousRecord)
				{
					GlobalControl.globalControl.MAXscores[0] = GlobalControl.globalControl.alternateMarbles;
				}
			}
			else
			{
				previousRecord = GlobalControl.globalControl.minigamesScores[minigame][0];
				previousScore = GlobalControl.globalControl.minigamesScores[minigame][1];
				GlobalControl.globalControl.minigamesScores[minigame][1] = GlobalControl.globalControl.alternateMarbles;
				if (GlobalControl.globalControl.alternateMarbles > previousRecord)
				{
					GlobalControl.globalControl.minigamesScores[minigame][0] = GlobalControl.globalControl.alternateMarbles;
				}
			}
			GlobalControl.globalControl.SaveData();
		}
		Invoke("ResetMarbles", 0.5f);
		if (minigame == "MIX")
		{
			s = GlobalControl.globalControl.MIXscores[0] + string.Empty;
			if (GlobalControl.globalControl.MIXscores[0] > 99)
			{
				showMedal = true;
			}
		}
		else if (minigame == "MAX")
		{
			s = GlobalControl.globalControl.MAXscores[0] + string.Empty;
			if (GlobalControl.globalControl.MAXscores[0] > 99)
			{
				showMedal = true;
			}
		}
		else
		{
			s = GlobalControl.globalControl.minigamesScores[minigame][0] + string.Empty;
			if (GlobalControl.globalControl.minigamesScores[minigame][0] > 99)
			{
				showMedal = true;
			}
		}
		if (showMedal)
		{
			GlobalControl.globalControl.CalculateProgress();
		}
		charArray = s.ToCharArray();
		if (charArray.Length == 1)
		{
			recordNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
		}
		else if (charArray.Length == 2)
		{
			recordNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			recordNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
		}
		else if (charArray.Length == 3)
		{
			recordNumbers[0].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			recordNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
			recordNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[2])];
		}
		for (int j = 0; j < recordNumbers.Length; j++)
		{
			recordNumbers_s[j].sprite = recordNumbers[j].sprite;
		}
		if (comingFromMinigame)
		{
			if ((minigame == "MIX" && GlobalControl.globalControl.MIXscores[0] > previousRecord) || (minigame == "MAX" && GlobalControl.globalControl.MAXscores[0] > previousRecord) || (minigame != "MIX" && minigame != "MAX" && GlobalControl.globalControl.minigamesScores[minigame][0] > previousRecord))
			{
				differenceRecord.SetActive(true);
				if (minigame == "MIX")
				{
					s = GlobalControl.globalControl.MIXscores[0] - previousRecord + string.Empty;
				}
				else if (minigame == "MAX")
				{
					s = GlobalControl.globalControl.MAXscores[0] - previousRecord + string.Empty;
				}
				else
				{
					s = GlobalControl.globalControl.minigamesScores[minigame][0] - previousRecord + string.Empty;
				}
				charArray = s.ToCharArray();
				if (charArray.Length == 1)
				{
					differenceRecordNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
				}
				else if (charArray.Length == 2)
				{
					differenceRecordNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
					differenceRecordNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
				}
				else
				{
					differenceRecordNumbers[0].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
					differenceRecordNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
					differenceRecordNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[2])];
				}
				for (int k = 0; k < differenceRecordNumbers.Length; k++)
				{
					differenceRecordNumbers_s[k].sprite = differenceRecordNumbers[k].sprite;
				}
				Invoke("SpinRecord", 0.5f);
			}
			else
			{
				differenceRecord.SetActive(false);
				if (GlobalControl.globalControl.MIXscores[0] == GlobalControl.globalControl.MIXscores[1])
				{
					Invoke("SpinRecord", 0.5f);
				}
				else if (GlobalControl.globalControl.MAXscores[0] == GlobalControl.globalControl.MAXscores[1])
				{
					Invoke("SpinRecord", 0.5f);
				}
			}
		}
		else
		{
			differenceRecord.SetActive(false);
		}
		if (minigame == "MIX")
		{
			s = GlobalControl.globalControl.MIXscores[1] + string.Empty;
		}
		else if (minigame == "MAX")
		{
			s = GlobalControl.globalControl.MAXscores[1] + string.Empty;
		}
		else
		{
			s = GlobalControl.globalControl.minigamesScores[minigame][1] + string.Empty;
		}
		charArray = s.ToCharArray();
		if (charArray.Length == 1)
		{
			lastScoreNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
		}
		else if (charArray.Length == 2)
		{
			lastScoreNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			lastScoreNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
		}
		else if (charArray.Length == 3)
		{
			lastScoreNumbers[0].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			lastScoreNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
			lastScoreNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[2])];
		}
		for (int l = 0; l < lastScoreNumbers.Length; l++)
		{
			lastScoreNumbers_s[l].sprite = lastScoreNumbers[l].sprite;
		}
		if (comingFromMinigame)
		{
			differenceLastScore.SetActive(true);
			if ((minigame == "MIX" && GlobalControl.globalControl.MIXscores[1] > previousScore) || (minigame == "MAX" && GlobalControl.globalControl.MAXscores[1] > previousScore) || (minigame != "MIX" && minigame != "MAX" && GlobalControl.globalControl.minigamesScores[minigame][1] > previousScore))
			{
				differenceLastScoreArrow[0].sprite = arrows[0];
				differenceLastScoreArrow[1].sprite = arrows[0];
				differenceLastScoreArrow[0].color = arrowColors[0];
				differenceLastScoreNumbers[0].color = arrowColors[0];
				differenceLastScoreNumbers[1].color = arrowColors[0];
				differenceLastScoreNumbers[2].color = arrowColors[0];
				if (minigame == "MIX")
				{
					s = GlobalControl.globalControl.MIXscores[1] - previousScore + string.Empty;
				}
				else if (minigame == "MAX")
				{
					s = GlobalControl.globalControl.MAXscores[1] - previousScore + string.Empty;
				}
				else
				{
					s = GlobalControl.globalControl.minigamesScores[minigame][1] - previousScore + string.Empty;
				}
			}
			else if ((minigame == "MIX" && GlobalControl.globalControl.MIXscores[1] == previousScore) || (minigame == "MAX" && GlobalControl.globalControl.MAXscores[1] == previousScore) || (minigame != "MIX" && minigame != "MAX" && GlobalControl.globalControl.minigamesScores[minigame][1] == previousScore))
			{
				differenceLastScore.SetActive(false);
				s = "0";
			}
			else
			{
				differenceLastScoreArrow[0].sprite = arrows[1];
				differenceLastScoreArrow[1].sprite = arrows[1];
				differenceLastScoreArrow[0].color = arrowColors[1];
				differenceLastScoreNumbers[0].color = arrowColors[1];
				differenceLastScoreNumbers[1].color = arrowColors[1];
				differenceLastScoreNumbers[2].color = arrowColors[1];
				if (minigame == "MIX")
				{
					s = previousScore - GlobalControl.globalControl.MIXscores[1] + string.Empty;
				}
				else if (minigame == "MAX")
				{
					s = previousScore - GlobalControl.globalControl.MAXscores[1] + string.Empty;
				}
				else
				{
					s = previousScore - GlobalControl.globalControl.minigamesScores[minigame][1] + string.Empty;
				}
			}
			charArray = s.ToCharArray();
			if (charArray.Length == 1)
			{
				differenceLastScoreNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			}
			else if (charArray.Length == 2)
			{
				differenceLastScoreNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
				differenceLastScoreNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
			}
			else if (charArray.Length == 3)
			{
				differenceLastScoreNumbers[0].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
				differenceLastScoreNumbers[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
				differenceLastScoreNumbers[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[2])];
			}
			for (int m = 0; m < differenceLastScoreNumbers.Length; m++)
			{
				differenceLastScoreNumbers_s[m].sprite = differenceLastScoreNumbers[m].sprite;
			}
			Invoke("SpinLastScore", 0.25f);
		}
		else
		{
			differenceLastScore.SetActive(false);
		}
		if (showMedal)
		{
			medal[0].sprite = medalIcon;
		}
		else
		{
			medal[0].sprite = null;
		}
		UpdateMarbleCounter();
	}

	private void UpdateMarbleCounter()
	{
		int num = GlobalControl.globalControl.marbles + GlobalControl.globalControl.alternateMarbles;
		if (num > 999)
		{
			num = 999;
		}
		s = num + string.Empty;
		charArray = s.ToCharArray();
		if (num < 10)
		{
			marblesCounter[0].sprite = numberSprites[0];
			marblesCounter[1].sprite = numberSprites[0];
			marblesCounter[2].sprite = numberSprites[num];
		}
		else if (num < 100)
		{
			marblesCounter[0].sprite = numberSprites[0];
			marblesCounter[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			marblesCounter[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
		}
		else
		{
			marblesCounter[0].sprite = numberSprites[(int)char.GetNumericValue(charArray[0])];
			marblesCounter[1].sprite = numberSprites[(int)char.GetNumericValue(charArray[1])];
			marblesCounter[2].sprite = numberSprites[(int)char.GetNumericValue(charArray[2])];
		}
	}

	public void HideScores()
	{
		recordGO.SetActive(false);
		lastScoreGO.SetActive(false);
	}

	private void ResetMarbles()
	{
		GlobalControl.globalControl.marbles += GlobalControl.globalControl.alternateMarbles;
		if (GlobalControl.globalControl.marbles > 999)
		{
			GlobalControl.globalControl.marbles = 999;
		}
		if (GlobalControl.globalControl.goldenMarbles > 999)
		{
			GlobalControl.globalControl.goldenMarbles = 999;
		}
		if (GlobalControl.globalControl.marbles >= 999 && !GlobalControl.globalControl._555)
		{
			GlobalControl.globalControl._555 = true;
			GlobalControl.globalControl.UnlockAchievements();
		}
		GlobalControl.globalControl.alternateMarbles = 0;
	}

	private void SpinRecord()
	{
		recordIcons[0].SetTrigger(sprinTrigger);
		recordIcons[1].SetTrigger(sprinTrigger);
		PlayRecord();
	}

	private void SpinLastScore()
	{
		lastScoreIcons[0].SetTrigger(sprinTrigger);
		lastScoreIcons[1].SetTrigger(sprinTrigger);
		PlayScore();
	}

	private void PlayRecord()
	{
		GlobalAudio.globalAudio.Play(recordSound, 1f, 1f, 0f);
	}

	private void PlayScore()
	{
		GlobalAudio.globalAudio.Play(scoreSound, 1f, 1f, 0f);
	}
}
