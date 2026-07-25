using UnityEngine;
using UnityEngine.SceneManagement;

public class HorseboysController : MonoBehaviour
{
	public static HorseboysController horseboysController;

	private GameObject cinematicNumbers;

	public GameObject scene00;

	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public GameObject scene04;

	public GameObject scene05;

	public GameObject scene06;

	public GameObject numberGood;

	public GameObject numberGood_s;

	public GameObject numberBad;

	public GameObject numberBad_s;

	public GameObject numberGood2;

	public GameObject numberGood2_s;

	public GameObject numberBad2;

	public GameObject numberBad2_s;

	public AudioClip desertAmbience;

	public AudioClip shoot;

	public AudioClip snort1;

	public AudioClip snort2;

	public AudioClip whinny;

	public AudioClip wind;

	public AudioClip drop;

	private int result1;

	private int result2;

	private bool ended;

	private bool skipped;

	private void Start()
	{
		horseboysController = this;
		cinematicNumbers = GameObject.Find("CinematicNumbers");
		scene00.SetActive(true);
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		scene04.SetActive(true);
		scene05.SetActive(true);
		scene06.SetActive(true);
		SetNumbers();
		ShowLegs();
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
		numberGood.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		numberGood_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		numberBad.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
		numberBad_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
		numberGood2.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		numberGood2_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result1];
		numberBad2.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
		numberBad2_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[result2];
	}

	public void End()
	{
		if (!ended)
		{
			ended = true;
			PCK_CinematicsController.PCK_cinematicsController.EndCinematic();
			SceneManager.UnloadSceneAsync("Horseboys");
		}
	}

	public void ShowCinematicNumbers()
	{
		cinematicNumbers.GetComponent<Animator>().SetTrigger("show");
	}

	public void FinalNumber()
	{
		cinematicNumbers.GetComponent<Animator>().SetTrigger("finalNumber");
	}

	public void DuelResult()
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

	public void ShowLegs()
	{
		scene01.GetComponent<Animator>().SetTrigger("action");
		scene00.GetComponent<Animator>().SetTrigger("out");
	}

	public void ShowHorse1()
	{
		scene02.GetComponent<Animator>().SetTrigger("action");
		scene01.GetComponent<Animator>().SetTrigger("out");
	}

	public void ShowHorse2()
	{
		scene03.GetComponent<Animator>().SetTrigger("action");
		scene02.GetComponent<Animator>().SetTrigger("out");
	}

	public void ShowDuel()
	{
		scene04.GetComponent<Animator>().SetTrigger("action");
		scene03.GetComponent<Animator>().SetTrigger("out");
	}

	public void DuelWin()
	{
		scene04.GetComponent<Animator>().SetTrigger("win");
	}

	public void DuelLose()
	{
		scene04.GetComponent<Animator>().SetTrigger("lose");
	}

	public void ShowWinner()
	{
		scene05.GetComponent<Animator>().SetTrigger("action");
		scene04.GetComponent<Animator>().SetTrigger("out");
	}

	public void ShowLoser()
	{
		scene06.GetComponent<Animator>().SetTrigger("action");
		scene04.GetComponent<Animator>().SetTrigger("out");
	}

	public void EndFade()
	{
		scene00.GetComponent<Animator>().SetTrigger("endFade");
	}

	public void FinalSceneOut()
	{
		scene05.GetComponent<Animator>().SetTrigger("out");
		scene06.GetComponent<Animator>().SetTrigger("out");
	}

	public void PlayDesertAmbience()
	{
		GlobalAudio.globalAudio.Play(desertAmbience, 1f, 1f, 0f);
	}

	public void PlayShoot()
	{
		GlobalAudio.globalAudio.Play(shoot, 1f, 1f, 0f);
	}

	public void PlaySnort1()
	{
		GlobalAudio.globalAudio.Play(snort1, 1f, 1f, 0f);
	}

	public void PlaySnort2()
	{
		GlobalAudio.globalAudio.Play(snort2, 1f, 1f, 0f);
	}

	public void PlayWhinny()
	{
		GlobalAudio.globalAudio.Play(whinny, 1f, 1f, 0f);
	}

	public void PlayWind()
	{
		GlobalAudio.globalAudio.Play(wind, 0.7f, 1f, 0f);
	}

	public void PlayDropLeft()
	{
		GlobalAudio.globalAudio.Play(drop, 0.6f, 1f, -0.5f);
	}

	public void PlayDropRight()
	{
		GlobalAudio.globalAudio.Play(drop, 0.6f, 1f, 0.5f);
	}
}
