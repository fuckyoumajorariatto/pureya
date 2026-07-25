using UnityEngine;

public class IntroController : MonoBehaviour
{
	public static IntroController introController;

	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public GameObject scene04;

	public GameObject scene05;

	public GameObject scene06;

	public GameObject scene07;

	public GameObject scene08;

	public AudioClip touch;

	public AudioClip head_turn;

	public AudioClip stretch_arm;

	public AudioClip knock_bowl;

	public AudioClip falling_marbles;

	public AudioClip spaceship_grab;

	public AudioClip fanfare;

	public AudioClip music1;

	public AudioClip music2;

	public AudioClip shine;

	private bool ended;

	private AudioSource musicAS;

	private void Start()
	{
		introController = this;
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		scene04.SetActive(true);
		scene05.SetActive(true);
		scene06.SetActive(true);
		scene07.SetActive(true);
		scene08.SetActive(true);
		PauseController.pauseController.inCinematic = true;
		PauseController.pauseController.inCinematicSkipped = false;
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
		if (!ended)
		{
			ended = true;
			GlobalControl.globalControl.UnlockInitialMinigames();
			GlobalControl.globalControl.storyLevel = 1;
			if (musicAS != null)
			{
				GlobalAudio.globalAudio.FadeMusic(musicAS, 0.5f);
			}
			MinigamesController.minigamesController.firstMusic = true;
			MinigamesController.minigamesController.StartMinigames(false);
			PauseController.pauseController.inCinematic = false;
		}
	}

	public void Turnhead()
	{
		scene02.GetComponent<Animator>().SetTrigger("action");
		scene01.GetComponent<Animator>().SetTrigger("out");
	}

	public void Shelve()
	{
		scene03.GetComponent<Animator>().SetTrigger("action1");
		scene02.GetComponent<Animator>().SetTrigger("out");
	}

	public void Reach()
	{
		scene04.GetComponent<Animator>().SetTrigger("action");
		scene03.GetComponent<Animator>().SetTrigger("out");
	}

	public void BallsRolling()
	{
		scene03.GetComponent<Animator>().SetTrigger("action2");
		scene04.GetComponent<Animator>().SetTrigger("out");
	}

	public void Squid()
	{
		scene05.GetComponent<Animator>().SetTrigger("action");
		scene03.GetComponent<Animator>().SetTrigger("out");
	}

	public void Ship()
	{
		scene06.GetComponent<Animator>().SetTrigger("action");
		scene05.GetComponent<Animator>().SetTrigger("out");
	}

	public void Spaceship()
	{
		scene07.GetComponent<Animator>().SetTrigger("action");
		scene06.GetComponent<Animator>().SetTrigger("out");
	}

	public void Hold()
	{
		scene08.GetComponent<Animator>().SetTrigger("action");
		scene07.GetComponent<Animator>().SetTrigger("out");
	}

	public void PlayTouch1()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(touch, 1f, 1f, 0f);
		}
	}

	public void PlayTouch2()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(touch, 1f, 1.1f, 0f);
		}
	}

	public void PlayHeadTurn()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(head_turn, 1f, 1f, 0f);
		}
	}

	public void PlayStretchArm()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(stretch_arm, 1f, 1f, 0f);
		}
	}

	public void PlayKnockBowl()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(knock_bowl, 1f, 1f, 0f);
		}
	}

	public void PlayFallingMarbles()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(falling_marbles, 1f, 1f, 0f);
		}
	}

	public void PlaySpaceshipGrab()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(spaceship_grab, 1f, 1f, 0f);
		}
	}

	public void PlayFanfare()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			musicAS = GlobalAudio.globalAudio.PlayMusic(fanfare, 0.8f, 1f, 0f);
		}
	}

	public void PlayMusic1()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			musicAS = GlobalAudio.globalAudio.PlayMusic(music1, 0.7f, 1f, 0f);
		}
	}

	public void PlayMusic2()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			musicAS = GlobalAudio.globalAudio.PlayMusic(music2, 1f, 1f, 0f);
		}
	}

	public void PlayShine()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(shine, 0.1f, 1f, 0f);
		}
	}
}
