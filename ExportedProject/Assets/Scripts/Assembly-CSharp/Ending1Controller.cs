using UnityEngine;

public class Ending1Controller : MonoBehaviour
{
	public Ending1CreditsController creditsController;

	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public GameObject scene04;

	public GameObject creditsBG;

	public Animator scene01Anim;

	public Animator scene02Anim;

	public Animator scene03Anim;

	public Animator scene04Anim;

	public Animator creditsBGAnim;

	public AudioClip shine;

	public AudioClip turn;

	public AudioClip stretchArms;

	public AudioClip leaveBowl;

	public AudioClip step;

	public AudioClip creak;

	public AudioClip slide;

	public AudioClip turnBowl;

	public AudioClip dropBowl;

	public AudioClip fallingMarbles;

	public AudioClip music;

	public AudioClip creditsMusic;

	private float stepsPan = -0.25f;

	private float stepsVol = 0.4f;

	private AudioSource musicAS;

	private void Start()
	{
		PauseController.pauseController.inCinematic = true;
		PauseController.pauseController.inCinematicSkipped = false;
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		scene04.SetActive(true);
		creditsBG.SetActive(true);
		scene01Anim.Play("ending1_01");
	}

	private void Update()
	{
		if (PauseController.pauseController.skip)
		{
			PauseController.pauseController.skip = false;
			End();
		}
	}

	public void End()
	{
		GlobalControl.globalControl.golden = true;
		GlobalControl.globalControl.goldenIndicator = false;
		GlobalControl.globalControl.storyLevel = 2;
		GlobalControl.globalControl._666 = true;
		GlobalControl.globalControl.UnlockAchievements();
		if (musicAS != null)
		{
			GlobalAudio.globalAudio.FadeMusic(musicAS, 0.5f);
		}
		MinigamesController.minigamesController.firstMusic = true;
		MinigamesController.minigamesController.StartMinigames(false);
		PauseController.pauseController.inCinematic = false;
	}

	public void PlayShine()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(shine, 0.1f, 1f, 0f);
		}
	}

	public void PlayTurn()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(turn, 0.6f, 1f, 0f);
		}
	}

	public void PlayStretchArms()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(stretchArms, 0.8f, 1f, 0f);
		}
	}

	public void PlayLeaveBowl()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(leaveBowl, 0.2f, 1f, 0f);
		}
	}

	public void PlayTurn2()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(turn, 0.8f, 0.75f, 0f);
		}
	}

	public void PlayStep()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(step, stepsVol, 1f, stepsPan);
			stepsPan -= 0.25f;
			stepsVol -= 0.1f;
		}
	}

	public void PlayCreak()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(creak, 0.5f, 1f, 0f);
		}
	}

	public void PlaySlide()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(slide, 0.2f, 1f, 0f);
		}
	}

	public void PlayTurnBowl()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(turnBowl, 0.8f, 1f, 0f);
		}
	}

	public void PlayDropBowl()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(dropBowl, 1f, 1f, 0f);
		}
	}

	public void PlayFallingMarbles()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(fallingMarbles, 1f, 1f, 0f);
		}
	}

	public void ActivateScene02()
	{
		scene02Anim.Play("ending1_02");
		scene01Anim.Play("out");
	}

	public void ActivateScene03()
	{
		scene03Anim.Play("ending1_03");
		scene02Anim.Play("out");
	}

	public void ActivateScene04()
	{
		scene04Anim.Play("ending1_04");
		scene03Anim.Play("out");
	}

	public void ActivateScene04_2()
	{
		scene04Anim.Play("ending1_04_2");
	}

	public void ActivateScene05()
	{
		scene02Anim.Play("ending1_05");
		scene04Anim.Play("out");
		creditsBGAnim.Play("out");
	}

	public void ActivateCreditsBG()
	{
		creditsBGAnim.Play("creditsBG");
	}

	public void ShowTitle()
	{
		creditsController.ShowTitle();
	}

	public void HideTitle()
	{
		creditsController.HideTitle();
	}

	public void PlayCreditsMusic()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			musicAS = GlobalAudio.globalAudio.PlayMusic(creditsMusic, 1f, 1f, 0f);
		}
	}

	public void PlayMusic()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			musicAS = GlobalAudio.globalAudio.PlayMusic(music, 1f, 1f, 0f);
		}
	}

	public void StartCredits()
	{
		creditsController.StartCredits();
	}

	public void EndCredits()
	{
		creditsController.EndCredits();
	}
}
