using UnityEngine;

public class PLA_PlayerController : MonoBehaviour
{
	private Transform tr;

	public Sprite[] skinSprites;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Collider2D coll;

	public ParticleSystem hitParticles1;

	public ParticleSystem hitParticles2;

	private ParticleSystem.MainModule main1;

	private ParticleSystem.MainModule main2;

	public bool hit;

	private int hits;

	private int skin;

	public Transform renderCameraTR;

	public PLA_Reset reset;

	private float movementSoundTimer;

	public AudioClip moveSound;

	public AudioClip hitSound;

	private AudioSource movementAS;

	private float screenShakeRemainingTime;

	private bool moveSoundPlayed;

	private Vector3 pos;

	private float inertia;

	private float maxInertia = 3f;

	private float acceleration = 10f;

	private float deceleration;

	private void Start()
	{
		tr = base.transform;
		ChangeSkin();
	}

	private void Update()
	{
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		if (!PauseController.pauseController.paused)
		{
			Move();
		}
	}

	private void FixedUpdate()
	{
		if (PauseController.pauseController.paused)
		{
			return;
		}
		tr.eulerAngles += Vector3.forward * inertia;
		if (inertia > 0f)
		{
			inertia -= Time.deltaTime * deceleration;
			if (inertia < 0f)
			{
				inertia = 0f;
			}
		}
		else if (inertia < 0f)
		{
			inertia += Time.deltaTime * deceleration;
			if (inertia > 0f)
			{
				inertia = 0f;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy") && !hit)
		{
			ManageCollision();
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("PLA");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["PLA"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["PLA"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["PLA"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites[0];
		}
		else
		{
			sr.sprite = skinSprites[skin];
			sr_s.sprite = skinSprites[skin];
		}
		main1 = hitParticles1.main;
		main2 = hitParticles2.main;
		ParticleSystem.MinMaxGradient minMaxGradient = default(ParticleSystem.MinMaxGradient);
		switch (skin)
		{
		case 0:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("F9EEC6FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("A78206FF");
			break;
		case 1:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFC77DFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("EA8D11FF");
			break;
		case 2:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("E6D88AFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("BA7A23FF");
			break;
		case 3:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("C3C3C3FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("707070FF");
			break;
		case 4:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFF0BAFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFC700FF");
			break;
		case 5:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFF0BAFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFC700FF");
			break;
		case 6:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("8B0072FF");
			break;
		case 7:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("09A9E3FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("009208FF");
			break;
		case 8:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFEDB2FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("997700FF");
			break;
		case 9:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("000000FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("000000FF");
			break;
		default:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("8E8B00FF");
			break;
		}
		main1.startColor = new ParticleSystem.MinMaxGradient(minMaxGradient.colorMin, minMaxGradient.colorMax);
		main2.startColor = new ParticleSystem.MinMaxGradient(minMaxGradient.colorMin, minMaxGradient.colorMax);
	}

	public void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			hit = true;
			hitParticles1.Play();
			hitParticles2.Play();
			sr.enabled = false;
			sr_s.enabled = false;
			coll.enabled = false;
			screenShakeRemainingTime = 0.5f;
			PlayHitSound();
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
		}
	}

	public void ResetPlayer()
	{
		sr.enabled = true;
		sr_s.enabled = true;
		coll.enabled = true;
		hitParticles1.Stop();
		hitParticles2.Stop();
		hitParticles1.Clear();
		hitParticles2.Clear();
		inertia = 0f;
		tr.eulerAngles = new Vector3(0f, 0f, -45f);
		moveSoundPlayed = false;
		hit = false;
	}

	private void ShakeScreen()
	{
		if (PauseController.pauseController.paused)
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			pos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void Move()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		if (GlobalInput.globalInput.left && inertia < maxInertia)
		{
			inertia += Time.deltaTime * acceleration;
		}
		if (GlobalInput.globalInput.right && inertia > maxInertia * -1f)
		{
			inertia -= Time.deltaTime * acceleration;
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			if (movementSoundTimer > 0.05f && !moveSoundPlayed)
			{
				moveSoundPlayed = true;
				GlobalAudio.globalAudio.Play(moveSound, 0.5f, Random.Range(0.85f, 1.15f), tr.position.x / 4f);
				movementSoundTimer = -2f;
			}
			movementSoundTimer += Time.deltaTime;
		}
		else
		{
			moveSoundPlayed = false;
			movementSoundTimer = 0f;
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
