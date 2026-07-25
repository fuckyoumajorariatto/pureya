using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseballController : MonoBehaviour
{
	private GameObject cinematicNumbers;

	public GameObject scene00;

	public GameObject scene01;

	public GameObject scene04;

	public GameObject scene05;

	public GameObject scene06;

	public GameObject numberBad1;

	public GameObject numberBad1_s;

	public GameObject numberGood1;

	public GameObject numberGood1_s;

	public AudioClip ambience;

	public AudioClip whoosh1;

	public AudioClip whoosh2;

	public AudioClip ballSquish;

	public AudioClip ballThrow;

	public AudioClip ballAscending;

	public AudioClip burning;

	public AudioClip space;

	public AudioClip explosion;

	public AudioClip ballFalling;

	public AudioClip swing;

	public AudioClip ballHit;

	public AudioClip shine;

	private int result1;

	private int result2;

	private bool ended;

	private bool skipped;

	private void Start()
	{
		cinematicNumbers = GameObject.Find("CinematicNumbers");
		scene00.SetActive(true);
		scene01.SetActive(true);
		scene04.SetActive(true);
		scene05.SetActive(true);
		scene06.SetActive(true);
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
		numberGood1.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		numberGood1_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		numberBad1.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
		numberBad1_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
	}

	public void End()
	{
		if (!ended)
		{
			ended = true;
			PCK_CinematicsController.PCK_cinematicsController.EndCinematic();
			SceneManager.UnloadSceneAsync("Baseball");
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

	public void ShowScene04()
	{
		scene01.GetComponent<Animator>().Play("out");
		scene04.GetComponent<Animator>().Play("action");
	}

	public void ShowScene05()
	{
		scene04.GetComponent<Animator>().Play("out");
		scene05.GetComponent<Animator>().Play("action1");
	}

	public void ShowScene06()
	{
		scene05.GetComponent<Animator>().Play("out");
		scene06.GetComponent<Animator>().Play("action1");
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
		scene06.GetComponent<Animator>().Play("action2");
	}

	public void DuelLose()
	{
		scene05.GetComponent<Animator>().Play("action2");
		scene06.GetComponent<Animator>().Play("out");
	}

	public void FinalSceneOut()
	{
		scene05.GetComponent<Animator>().Play("out");
		scene06.GetComponent<Animator>().Play("out");
	}

	public void EndFade()
	{
		scene00.GetComponent<Animator>().SetTrigger("endFade");
	}

	public void PlayAmbience()
	{
		GlobalAudio.globalAudio.Play(ambience, 0.8f, 1f, 0f);
	}

	public void PlayWhoosh1()
	{
		GlobalAudio.globalAudio.Play(whoosh1, 1f, 1f, 0f);
	}

	public void PlayWhoosh2()
	{
		GlobalAudio.globalAudio.Play(whoosh2, 1f, 1f, 0f);
	}

	public void PlayBallSquish()
	{
		GlobalAudio.globalAudio.Play(ballSquish, 1f, 1f, 0f);
	}

	public void PlayBallThrow()
	{
		GlobalAudio.globalAudio.Play(ballThrow, 1f, 1f, 0f);
	}

	public void PlayBallAscending()
	{
		GlobalAudio.globalAudio.Play(ballAscending, 1f, 1f, 0f);
	}

	public void PlayBurning()
	{
		GlobalAudio.globalAudio.Play(burning, 1f, 1f, 0f);
	}

	public void PlaySpace()
	{
		GlobalAudio.globalAudio.Play(space, 1f, 1f, 0f);
	}

	public void PlayExplosion()
	{
		GlobalAudio.globalAudio.Play(explosion, 1f, 1f, 0f);
	}

	public void PlayBallFalling()
	{
		GlobalAudio.globalAudio.Play(ballFalling, 1f, 1f, 0f);
	}

	public void PlaySwing()
	{
		GlobalAudio.globalAudio.Play(swing, 1f, 1f, 0f);
	}

	public void PlayBallHit()
	{
		GlobalAudio.globalAudio.Play(ballHit, 1f, 1f, 0f);
	}

	public void PlayShine()
	{
		GlobalAudio.globalAudio.Play(shine, 1f, 1f, 0f);
	}
}
