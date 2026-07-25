using UnityEngine;
using UnityEngine.SceneManagement;

public class RacecarsController : MonoBehaviour
{
	private GameObject cinematicNumbers;

	public GameObject scene00;

	public GameObject scene01;

	public GameObject scene02;

	public GameObject scene03;

	public Sprite[] numberSprites;

	public GameObject numberGood;

	public GameObject numberGood_s;

	public GameObject numberBad;

	public GameObject numberBad_s;

	public AudioClip engine1;

	public AudioClip engine2;

	public AudioClip drift;

	public AudioClip crash;

	public AudioClip fire;

	public AudioClip carTouch;

	public AudioClip engineRevs;

	public AudioClip claps;

	public AudioClip shoot;

	private float enginePitch;

	private int result1;

	private int result2;

	private bool ended;

	private bool skipped;

	public ParticleSystem car1PS1;

	public ParticleSystem car1PS2;

	public ParticleSystem car2PS1;

	public ParticleSystem car2PS2;

	private AudioSource as1;

	private AudioSource as2;

	private float soundsVolume;

	private bool riseEngine;

	private float riseEngineTimer;

	private bool risePitch;

	private float pitchLimit;

	private bool lowerEngine;

	private void Start()
	{
		cinematicNumbers = GameObject.Find("CinematicNumbers");
		scene00.SetActive(true);
		scene01.SetActive(true);
		scene02.SetActive(true);
		scene03.SetActive(true);
		SetNumbers();
		ShowScene01();
		PauseController.pauseController.inCinematic = true;
		PauseController.pauseController.inCinematicSkipped = false;
	}

	private void Update()
	{
		if (ended)
		{
			return;
		}
		if (PauseController.pauseController.skip && !skipped)
		{
			skipped = true;
			PauseController.pauseController.skip = false;
			EndFade();
		}
		if (riseEngine)
		{
			riseEngineTimer += Time.deltaTime / 2f;
			if (riseEngineTimer > 1f)
			{
				riseEngine = false;
				riseEngineTimer = 0f;
			}
			else
			{
				as1.volume = Mathf.Lerp(0f, soundsVolume * 0.5f, riseEngineTimer);
				as2.volume = Mathf.Lerp(0f, soundsVolume * 0.5f, riseEngineTimer);
			}
		}
		if (risePitch)
		{
			as1.pitch += Time.deltaTime / 10f;
			as2.pitch += Time.deltaTime / 10f;
			if (as1.pitch > pitchLimit)
			{
				as1.pitch -= 0.1f;
				as2.pitch -= 0.1f;
				pitchLimit += 0.2f;
			}
		}
		if (lowerEngine)
		{
			riseEngineTimer += Time.deltaTime;
			if (riseEngineTimer > 1f)
			{
				lowerEngine = false;
				riseEngineTimer = 0f;
				as1.volume = 0f;
				as2.volume = 0f;
			}
			else
			{
				as1.volume = Mathf.Lerp(soundsVolume * 0.5f, 0f, riseEngineTimer);
				as2.volume = Mathf.Lerp(soundsVolume * 0.5f, 0f, riseEngineTimer);
			}
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
	}

	public void End()
	{
		if (!ended)
		{
			ended = true;
			as1.Stop();
			as2.Stop();
			as1.loop = false;
			as2.loop = false;
			as1.volume = 0f;
			as2.volume = 0f;
			PCK_CinematicsController.PCK_cinematicsController.EndCinematic();
			SceneManager.UnloadSceneAsync("Racecars");
		}
	}

	public void FinalNumber()
	{
		cinematicNumbers.GetComponent<Animator>().SetTrigger("finalNumber");
	}

	public void ShowScene01()
	{
		if (PCK_SlotsController.PCK_slotsController.resultLeft == PCK_SlotsController.PCK_slotsController.resultMiddle)
		{
			scene01.GetComponent<Animator>().Play("action2");
		}
		else
		{
			scene01.GetComponent<Animator>().Play("action");
		}
	}

	public void DuelWin()
	{
		scene03.GetComponent<Animator>().Play("action");
		scene01.GetComponent<Animator>().Play("out");
	}

	public void DuelLose()
	{
		scene02.GetComponent<Animator>().Play("action");
		scene01.GetComponent<Animator>().Play("out");
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

	public void EnableParticles1()
	{
		car1PS1.Play();
		car1PS2.Play();
	}

	public void EnableParticles2()
	{
		car2PS1.Play();
		car2PS2.Play();
	}

	public void DisableParticles1()
	{
		car1PS1.Stop();
		car1PS2.Stop();
	}

	public void DisableParticles2()
	{
		car2PS1.Stop();
		car2PS2.Stop();
	}

	public void PlayEngine1()
	{
		soundsVolume = GlobalAudio.globalAudio.GetSoundsVolume();
		as1 = GlobalAudio.globalAudio.Play(engine1, 0.6f, 1f, -0.5f);
		as1.loop = true;
		as1.volume = 0f;
		pitchLimit = 1.2f;
	}

	public void PlayEngine2()
	{
		as2 = GlobalAudio.globalAudio.Play(engine1, 0.6f, 1f, 0.5f);
		as2.loop = true;
		as2.volume = 0f;
		riseEngine = true;
		risePitch = true;
	}

	public void LowerEngine()
	{
		lowerEngine = true;
	}

	public void PlayDriftL()
	{
		GlobalAudio.globalAudio.Play(drift, 1f, Random.Range(0.8f, 0.95f), -0.5f);
	}

	public void PlayDriftR()
	{
		GlobalAudio.globalAudio.Play(drift, 1f, Random.Range(1.05f, 1.2f), 0.5f);
	}

	public void PlayCrash()
	{
		GlobalAudio.globalAudio.Play(crash, 1f, 1f, 0f);
	}

	public void PlayFire()
	{
		GlobalAudio.globalAudio.Play(fire, 1f, 1f, 0f);
	}

	public void PlayCarTouch()
	{
		GlobalAudio.globalAudio.Play(carTouch, 1f, 1f, 0f);
	}

	public void PlayEngineRevs()
	{
		GlobalAudio.globalAudio.Play(engineRevs, 1f, 1f, 0f);
	}

	public void PlayClaps()
	{
		GlobalAudio.globalAudio.Play(claps, 1f, 1f, 0f);
	}

	public void PlayShoot()
	{
		GlobalAudio.globalAudio.Play(shoot, 1f, 1f, 0f);
	}
}
