using UnityEngine;
using UnityEngine.SceneManagement;

public class ProposalController : MonoBehaviour
{
	public static ProposalController proposalController;

	private GameObject cinematicNumbers;

	public GameObject number;

	public GameObject number_s;

	public GameObject scene00;

	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public GameObject scene04;

	public GameObject scene05;

	public GameObject scene07;

	public GameObject scene09;

	public GameObject confetti;

	public AudioClip heart;

	public AudioClip reveal;

	public AudioClip wedding;

	public AudioClip slap;

	private bool ended;

	private void Start()
	{
		proposalController = this;
		cinematicNumbers = GameObject.Find("CinematicNumbers");
		scene00.SetActive(true);
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		scene04.SetActive(true);
		scene05.SetActive(true);
		scene07.SetActive(true);
		scene09.SetActive(true);
		ChangeNumber();
		Fade();
		PauseController.pauseController.inCinematic = true;
	}

	private void Update()
	{
		if (PauseController.pauseController.skip)
		{
			PauseController.pauseController.skip = false;
			FinalNumber();
			End();
		}
	}

	private void ChangeNumber()
	{
		number.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.iconSprites[PCK_SlotsController.PCK_slotsController.resultMiddle];
		number_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.iconSprites[PCK_SlotsController.PCK_slotsController.resultMiddle];
	}

	public void End()
	{
		if (!ended)
		{
			ended = true;
			PCK_CinematicsController.PCK_cinematicsController.EndCinematic();
			SceneManager.UnloadSceneAsync("Proposal");
		}
	}

	public void FinalNumber()
	{
		cinematicNumbers.GetComponent<Animator>().SetTrigger("finalNumber");
	}

	public void GirlResult()
	{
		if (PCK_SlotsController.PCK_slotsController.resultLeft == PCK_SlotsController.PCK_slotsController.resultMiddle)
		{
			GirlCry();
		}
		else
		{
			GirlMad();
		}
	}

	public void Fade()
	{
		scene00.GetComponent<Animator>().SetTrigger("action");
	}

	public void ShowGuy()
	{
		scene01.GetComponent<Animator>().SetTrigger("action");
		scene00.GetComponent<Animator>().SetTrigger("out");
	}

	public void ShowGirl()
	{
		scene02.GetComponent<Animator>().SetTrigger("action");
		scene01.GetComponent<Animator>().SetTrigger("out");
	}

	public void Proposal()
	{
		scene03.GetComponent<Animator>().SetTrigger("action");
		scene02.GetComponent<Animator>().SetTrigger("out");
	}

	public void OpeningBox1()
	{
		scene04.GetComponent<Animator>().SetTrigger("action1");
		scene03.GetComponent<Animator>().SetTrigger("out");
	}

	public void GirlReaction()
	{
		scene05.GetComponent<Animator>().SetTrigger("action1");
		scene04.GetComponent<Animator>().SetTrigger("out");
	}

	public void OpeningBox2()
	{
		scene04.GetComponent<Animator>().SetTrigger("action2");
		scene05.GetComponent<Animator>().SetTrigger("out");
	}

	public void GirlCry()
	{
		scene05.GetComponent<Animator>().SetTrigger("action2");
		scene04.GetComponent<Animator>().SetTrigger("out");
	}

	public void GirlMad()
	{
		scene05.GetComponent<Animator>().SetTrigger("action3");
		scene04.GetComponent<Animator>().SetTrigger("out");
	}

	public void Wedding()
	{
		scene07.GetComponent<Animator>().SetTrigger("action");
		scene05.GetComponent<Animator>().SetTrigger("out");
	}

	public void Slap()
	{
		scene09.GetComponent<Animator>().SetTrigger("action");
		scene05.GetComponent<Animator>().SetTrigger("out");
	}

	public void EndFade()
	{
		scene00.GetComponent<Animator>().SetTrigger("endFade");
	}

	public void FinalSceneOut()
	{
		scene07.GetComponent<Animator>().SetTrigger("out");
		scene09.GetComponent<Animator>().SetTrigger("out");
	}

	public void Confetti()
	{
		confetti.SetActive(true);
	}

	public void PlayHeart()
	{
		GlobalAudio.globalAudio.Play(heart, 1f, 1f, 0f);
	}

	public void PlayReveal()
	{
		GlobalAudio.globalAudio.Play(reveal, 1f, 1f, 0f);
	}

	public void PlayWedding()
	{
		GlobalAudio.globalAudio.Play(wedding, 1f, 1f, 0f);
	}

	public void PlaySlap()
	{
		GlobalAudio.globalAudio.Play(slap, 1f, 1f, 0f);
	}
}
