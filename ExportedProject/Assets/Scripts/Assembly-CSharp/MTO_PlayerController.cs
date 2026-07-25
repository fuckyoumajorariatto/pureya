using UnityEngine;

public class MTO_PlayerController : MonoBehaviour
{
	public Sprite[] sprs = new Sprite[9];

	public Transform bgParticles1;

	public Transform bgParticles2;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private float maxSpeed = 6f;

	private float acceleration = 50f;

	private float currentSpeed;

	private float deceleration = 60f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private Transform tr;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public MTO_BulletSpawner MTO_bulletSpawner;

	private Vector3 pos;

	public Collider2D coll;

	private bool bothButtons;

	private float doubleMoveSoundCooldown;

	public Color[] hitColors;

	private int skin;

	private void Start()
	{
		ChangeSkin();
		tr = base.transform;
	}

	private void Update()
	{
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		if (!PauseController.pauseController.paused)
		{
			ManageInput();
			if (currentSpeed != 0f)
			{
				MovePlayer();
			}
			ReduceSpeed();
			MoveStars();
			PanAudio();
			if (doubleMoveSoundCooldown > 0f)
			{
				doubleMoveSoundCooldown -= Time.deltaTime;
			}
		}
	}

	private void ManageInput()
	{
		receivingInput = false;
		tr.localScale = new Vector3(1f, 1f, tr.localScale.z);
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit)
		{
			bothButtons = true;
			if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
			{
				doubleMoveSoundCooldown = 0.25f;
				PlayMoveSound();
			}
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
			}
			SquishPlayer();
		}
		else if (GlobalInput.globalInput.right)
		{
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime;
			}
			SquishPlayer();
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			receivingInput = true;
		}
		if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
		{
			bothButtons = false;
			PlayMoveSound();
		}
		else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
		{
			bothButtons = false;
			if (!(doubleMoveSoundCooldown > 0f))
			{
				PlayMoveSound();
			}
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("MTO");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["MTO"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["MTO"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["MTO"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = sprs[0];
		}
		else
		{
			sr.sprite = sprs[skin];
			sr_s.sprite = sprs[skin];
		}
	}

	private void MovePlayer()
	{
		tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -4.4375f)
		{
			tr.position = new Vector2(tr.position.x + 8.875f, tr.position.y);
		}
		else if (tr.position.x > 4.4375f)
		{
			tr.position = new Vector2(tr.position.x - 8.875f, tr.position.y);
		}
	}

	private void ReduceSpeed()
	{
		if (receivingInput || currentSpeed == 0f)
		{
			return;
		}
		if (currentSpeed > 0f)
		{
			currentSpeed -= Time.deltaTime * deceleration;
			if (currentSpeed < 0f)
			{
				currentSpeed = 0f;
			}
		}
		else if (currentSpeed < 0f)
		{
			currentSpeed += Time.deltaTime * deceleration;
			if (currentSpeed > 0f)
			{
				currentSpeed = 0f;
			}
		}
	}

	private void MoveStars()
	{
		if (bgParticles1.position.x + 0.05f < tr.position.x * -0.15f)
		{
			bgParticles1.Translate(-3f * Time.deltaTime, 0f, 0f);
		}
		else if (bgParticles1.position.x - 0.05f > tr.position.x * -0.15f)
		{
			bgParticles1.Translate(3f * Time.deltaTime, 0f, 0f);
		}
		if (bgParticles2.position.x + 0.05f < tr.position.x * -0.1f)
		{
			bgParticles2.Translate(-2f * Time.deltaTime, 0f, 0f);
		}
		else if (bgParticles2.position.x - 0.05f > tr.position.x * -0.1f)
		{
			bgParticles2.Translate(2f * Time.deltaTime, 0f, 0f);
		}
	}

	private void SquishPlayer()
	{
		tr.localScale = new Vector3(0.8f, 1f, tr.localScale.z);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble"))
		{
			ManageHit(collision);
		}
	}

	private void ManageHit(Collision2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				MTO_Reset.MTO_reset.reset = true;
			}
			sr.color = hitColors[skin];
			MTO_bulletSpawner.enabled = false;
			hitParticles.transform.position = tr.position;
			hitParticles.SetActive(true);
			screenShakeRemainingTime = 0.25f;
			coll.enabled = false;
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
			pos = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PanAudio()
	{
		if (audioSource != null)
		{
			if (audioSource.isPlaying && (audioSource.clip == movementSound || audioSource.clip == hitSound))
			{
				audioSource.panStereo = tr.position.x / 4f;
			}
			else
			{
				audioSource = null;
			}
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.8f, 0.9f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
