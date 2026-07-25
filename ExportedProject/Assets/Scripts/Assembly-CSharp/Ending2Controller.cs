using UnityEngine;

public class Ending2Controller : MonoBehaviour
{
	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public GameObject marbleScreen;

	public Animator scene01Anim;

	public Animator scene02Anim;

	public Animator scene03Anim;

	public Animator marbleScreenAnim;

	private float wingPan = -0.9f;

	public AudioClip music;

	public AudioClip shine;

	public AudioClip turn;

	public AudioClip ambience;

	public AudioClip bird_wing;

	public AudioClip window;

	public AudioClip girl_movement1;

	public AudioClip girl_movement2;

	public AudioClip marbles_throw;

	public AudioClip marble_fly;

	private AudioSource musicAS;

	private void Start()
	{
		PauseController.pauseController.inCinematic = true;
		PauseController.pauseController.inCinematicSkipped = false;
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		marbleScreen.SetActive(true);
		scene01Anim.Play("action");
	}

	private void Update()
	{
		if (PauseController.pauseController.skip)
		{
			PauseController.pauseController.skip = false;
			End();
		}
	}

	public void ActivateScene02()
	{
		scene02Anim.Play("ending2_02");
		scene01Anim.Play("out");
	}

	public void ActivateScene03()
	{
		scene03Anim.Play("ending2_03");
		marbleScreenAnim.Play("action");
		scene02Anim.Play("out");
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

	public void PlayAmbience()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(ambience, 1f, 1f, 0f);
		}
	}

	public void PlayBirdWing()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			wingPan += 0.3f;
			GlobalAudio.globalAudio.Play(bird_wing, 0.2f, Random.Range(0.9f, 1.1f), wingPan);
		}
	}

	public void PlayWindow()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(window, 1f, 1f, 0f);
		}
	}

	public void PlayGirlMovement1()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(girl_movement1, 0.5f, 1f, 0f);
		}
	}

	public void PlayGirlMovement2()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(girl_movement2, 0.7f, 1f, 0f);
		}
	}

	public void PlayMarblesThrow()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(marbles_throw, 1f, 1f, 0f);
		}
	}

	public void PlayMarbleFly()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(marble_fly, 1f, 1f, 0f);
		}
	}

	public void PlayMusic()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			musicAS = GlobalAudio.globalAudio.PlayMusic(music, 1f, 1f, 0f);
		}
	}

	public void End()
	{
		if (musicAS != null)
		{
			GlobalAudio.globalAudio.FadeMusic(musicAS, 0.5f);
		}
		GlobalControl.globalControl.storyLevel = 3;
		GlobalControl.globalControl.SaveData();
		Invoke("End2", 1.5f);
		SceneChangeController.sceneChangeController.LoadAndChangeScene("MMU", "title", 2f);
	}

	private void End2()
	{
		GlobalAudio.globalAudio.PlayMainMenuMusic();
		GlobalControl.globalControl.firstPCKMusicPlayed = false;
		GlobalInput.globalInput.gameInputActive = true;
		PauseController.pauseController.inCinematic = false;
	}
}
