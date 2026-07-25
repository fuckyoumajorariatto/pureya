using UnityEngine;

public class WGN_PlayerController : MonoBehaviour
{
	public Sprite[] sprs;

	public Sprite[] sprs_s;

	public ParticleSystem hitParticles;

	public Transform renderCameraTR;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public WGN_Reset reset;

	private int maxSpeed = 12;

	private int acceleration = 70;

	private float currentSpeed;

	private float deceleration = 70f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	public Rigidbody2D rb;

	public Collider2D coll;

	private Vector3 pos;

	private float hitSpeed;

	public ParticleSystem wheelParticlesL;

	public ParticleSystem wheelParticlesR;

	private bool bothButtons;

	private float doubleMoveSoundCooldown;

	public Color[] hitColors;

	private int skin;

	private void Start()
	{
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
			ManageInput();
			if (currentSpeed != 0f)
			{
				MovePlayer();
			}
			ReduceSpeed();
			if (hit)
			{
				tr.Translate(0f, (0f - hitSpeed) * Time.deltaTime, 0f);
			}
			if (doubleMoveSoundCooldown > 0f)
			{
				doubleMoveSoundCooldown -= Time.deltaTime;
			}
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("WGN");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["WGN"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["WGN"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["WGN"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = sprs_s[0];
		}
		else
		{
			sr.sprite = sprs[skin];
			sr_s.sprite = sprs_s[skin];
		}
	}

	private void ManageInput()
	{
		receivingInput = false;
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
		if (GlobalInput.globalInput.left && tr.position.x > -4f)
		{
			if (currentSpeed > (float)(-maxSpeed))
			{
				currentSpeed -= (float)acceleration * Time.deltaTime;
				wheelParticlesR.Play();
				wheelParticlesL.Stop();
			}
		}
		else if (GlobalInput.globalInput.right && tr.position.x < 4f)
		{
			if (currentSpeed < (float)maxSpeed)
			{
				currentSpeed += (float)acceleration * Time.deltaTime;
				wheelParticlesL.Play();
				wheelParticlesR.Stop();
			}
		}
		else
		{
			tr.localScale = new Vector3(1f, 1f, tr.localScale.z);
			wheelParticlesL.Stop();
			wheelParticlesR.Stop();
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

	private void MovePlayer()
	{
		if (!hit)
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
			if (tr.position.x < -4f)
			{
				tr.position = new Vector2(-4f, tr.position.y);
			}
			else if (tr.position.x > 4f)
			{
				tr.position = new Vector2(4f, tr.position.y);
			}
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

	private void SquishPlayer()
	{
		tr.localScale = new Vector3(0.9f, 1f, 1f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble") && !hit)
		{
			ManageCollision(collision);
		}
	}

	private void ManageCollision(Collision2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			sr.color = hitColors[skin];
			hitSpeed = WGN_Difficulty.WGN_difficulty.rockSpeed;
			hitParticles.Play();
			screenShakeRemainingTime = 0.4f;
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

	public void DoReset()
	{
		sr.color = Color.white;
		tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		tr.position = new Vector3(0f, -2.75f, tr.position.z);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		coll.enabled = true;
		hit = false;
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(movementSound, 0.5f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
