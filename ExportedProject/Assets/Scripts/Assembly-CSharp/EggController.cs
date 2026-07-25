using UnityEngine;
using UnityEngine.SceneManagement;

public class EggController : MonoBehaviour
{
	private GameObject cinematicNumbers;

	public GameObject scene00;

	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public GameObject scene04;

	public GameObject number1;

	public GameObject number1_s;

	public GameObject number2;

	public GameObject number2_s;

	public AudioClip wind;

	public AudioClip step;

	public AudioClip eggMovement;

	public AudioClip eggPush;

	public AudioClip wingsFlap;

	public AudioClip headMovement;

	public AudioClip headMovement2;

	public AudioClip penguinVoice;

	public AudioClip numberSound;

	public AudioClip finalNumberSound;

	private int result1;

	private int result2;

	private bool ended;

	private bool skipped;

	private void Start()
	{
		cinematicNumbers = GameObject.Find("CinematicNumbers");
		scene00.SetActive(true);
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		scene04.SetActive(true);
		SetNumbers();
		ShowScene01();
		PauseController.pauseController.inCinematic = true;
		PauseController.pauseController.inCinematicSkipped = false;
	}

	private void Update()
	{
		if (!ended && PauseController.pauseController.skip && !skipped)
		{
			skipped = true;
			PauseController.pauseController.skip = false;
			EndFade();
		}
	}

	private void SetNumbers()
	{
		result1 = PCK_SlotsController.PCK_slotsController.resultLeft - 1;
		result2 = PCK_SlotsController.PCK_slotsController.badFightNumber - 1;
		if (PCK_SlotsController.PCK_slotsController.resultLeft == PCK_SlotsController.PCK_slotsController.resultMiddle)
		{
			number1.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
			number1_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
			number2.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
			number2_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		}
		else
		{
			number1.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
			number1_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
			number2.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
			number2_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
		}
	}

	public void End()
	{
		if (!ended)
		{
			ended = true;
			PCK_CinematicsController.PCK_cinematicsController.EndCinematic();
			SceneManager.UnloadSceneAsync("Egg");
		}
	}

	public void FinalNumber()
	{
		cinematicNumbers.GetComponent<Animator>().SetTrigger("finalNumber");
	}

	public void ShowScene01()
	{
		scene01.GetComponent<Animator>().Play("action");
	}

	public void ShowScene02()
	{
		scene01.GetComponent<Animator>().Play("out");
		scene02.GetComponent<Animator>().Play("action");
	}

	public void ShowFinalScene()
	{
		if (PCK_SlotsController.PCK_slotsController.resultLeft == PCK_SlotsController.PCK_slotsController.resultMiddle)
		{
			DuelWin();
		}
		else
		{
			DuelLose();
		}
	}

	public void DuelWin()
	{
		scene04.GetComponent<Animator>().Play("action");
		scene02.GetComponent<Animator>().Play("out");
	}

	public void DuelLose()
	{
		scene03.GetComponent<Animator>().Play("action");
		scene02.GetComponent<Animator>().Play("out");
	}

	public void FinalSceneOut()
	{
		scene02.GetComponent<Animator>().Play("out");
		scene03.GetComponent<Animator>().Play("out");
	}

	public void EndFade()
	{
		scene00.GetComponent<Animator>().SetTrigger("endFade");
	}

	public void PlayWind()
	{
		GlobalAudio.globalAudio.Play(wind, 1f, 1f, 0f);
	}

	public void PlayStep()
	{
		GlobalAudio.globalAudio.Play(step, Random.Range(0.6f, 0.8f), Random.Range(0.9f, 1.1f), 0f);
	}

	public void PlayEggMovement()
	{
		GlobalAudio.globalAudio.Play(eggMovement, 1f, Random.Range(0.8f, 1.1f), 0f);
	}

	public void PlayEggPush()
	{
		GlobalAudio.globalAudio.Play(eggPush, 1f, Random.Range(0.8f, 1.1f), 0f);
	}

	public void PlayWings()
	{
		GlobalAudio.globalAudio.Play(wingsFlap, 1f, 1f, 0f);
	}

	public void PlayHeadMovement()
	{
		GlobalAudio.globalAudio.Play(headMovement, 1f, 1f, 0f);
	}

	public void PlayHeadMovement2()
	{
		GlobalAudio.globalAudio.Play(headMovement2, 1f, 1f, 0f);
	}

	public void PlayPenguinVoice()
	{
		GlobalAudio.globalAudio.Play(penguinVoice, 1f, 1f, 0f);
	}

	public void PlayNumberSound()
	{
		GlobalAudio.globalAudio.Play(numberSound, 1f, 1f, 0f);
	}

	public void PlayFinalNumberSound()
	{
		GlobalAudio.globalAudio.Play(finalNumberSound, 0.6f, 1f, 0f);
	}
}
