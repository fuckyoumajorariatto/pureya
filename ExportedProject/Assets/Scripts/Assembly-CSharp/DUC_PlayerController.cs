using UnityEngine;

public class DUC_PlayerController : MonoBehaviour
{
	public Transform tr;

	public Rigidbody2D rb;

	private float gravityCorrectionTimer;

	private int gravityChangesCounter;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public Collider2D coll;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Transform renderCameraTR;

	public bool hit;

	private float initialTimer;

	public AudioClip movementSound;

	public AudioClip movementAirSound;

	public AudioClip splashSound;

	public AudioClip hitSound;

	private AudioSource audioSource;

	private float screenShakeRemainingTime;

	public int hits;

	private Vector3 pos;

	public ParticleSystem hitParticles;

	public Transform hitParticlesTR;

	private float jumpCooldown;

	private bool jumpActive = true;

	public ParticleSystem ps;

	private ParticleSystem.MainModule main;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main2;

	private float splashMaxSpeed;

	public DUC_Reset DUC_reset;

	private bool bothButtons;

	private int skin;

	private void Start()
	{
		main = ps.main;
		main2 = ps2.main;
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
			initialTimer += Time.deltaTime;
			if (jumpCooldown > 0f)
			{
				jumpCooldown -= Time.deltaTime;
			}
			ManageGravity();
			ManageInput();
			ManegeBounds();
		}
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit || initialTimer < 0.5f)
		{
			bothButtons = true;
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			rb.gravityScale = 4f;
			PlayMoveSound();
		}
		else if (GlobalInput.globalInput.right && tr.position.y < 1f && jumpActive)
		{
			if (tr.position.y > -0.5f)
			{
				if (GlobalInput.globalInput.rightDown)
				{
					rb.velocity = Vector3.zero;
					rb.AddForce(tr.up * 800f);
					jumpActive = false;
					PlayMoveAirSound();
				}
			}
			else if (tr.position.y < 0.4375f)
			{
				rb.gravityScale = -4f;
				PlayMoveSound();
			}
		}
		else if (GlobalInput.globalInput.rightDown && tr.position.y >= 1f && jumpActive)
		{
			rb.velocity = Vector3.zero;
			rb.AddForce(tr.up * 800f);
			jumpActive = false;
			PlayMoveAirSound();
		}
		if (tr.position.y < -3.5f)
		{
			tr.position = new Vector2(tr.position.x, -3.5f);
			rb.velocity = Vector3.zero;
		}
		else if (tr.position.y > 4f)
		{
			tr.position = new Vector2(tr.position.x, 4f);
			rb.velocity = Vector3.zero;
		}
		if (tr.position.y > 1.5f)
		{
			jumpActive = false;
		}
		else if (tr.position.y < 0.4375f)
		{
			jumpActive = true;
		}
	}

	private void ManageGravity()
	{
		if (hit)
		{
			return;
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			gravityCorrectionTimer = 0f;
			gravityChangesCounter = 0;
		}
		gravityCorrectionTimer += Time.deltaTime;
		if (gravityCorrectionTimer > 0.5f)
		{
			if (gravityChangesCounter > 4)
			{
				tr.position = new Vector2(tr.position.x, 0.4375f);
				rb.velocity = Vector3.zero;
				rb.gravityScale = 0f;
			}
			gravityChangesCounter = 0;
			gravityCorrectionTimer = 0f;
		}
		if (tr.position.y > 0.4375f)
		{
			if (rb.gravityScale < 0f)
			{
				gravityChangesCounter++;
			}
			rb.gravityScale = 4f;
		}
		else if (tr.position.y < 0.4375f)
		{
			if (rb.gravityScale > 0f)
			{
				gravityChangesCounter++;
			}
			rb.gravityScale = -2f;
		}
		if (tr.position.x < -3.25f && !hit)
		{
			tr.Translate(1.5f * Time.deltaTime, 0f, 0f);
			if (tr.position.x > -3.25f)
			{
				tr.position = new Vector2(-3.25f, tr.position.y);
			}
		}
		else if (tr.position.x > -3.25f && !hit)
		{
			tr.Translate(-1f * Time.deltaTime, 0f, 0f);
			if (tr.position.x < -3.25f)
			{
				tr.position = new Vector2(-3.25f, tr.position.y);
			}
		}
	}

	private void ManegeBounds()
	{
		if (tr.position.y < -3.5f)
		{
			tr.position = new Vector2(tr.position.x, -3.5f);
			rb.velocity = Vector3.zero;
		}
		else if (tr.position.y > 4f)
		{
			tr.position = new Vector2(tr.position.x, 4f);
			rb.velocity = Vector3.zero;
		}
		if (tr.position.x < -4.875f)
		{
			ManageHit();
		}
	}

	private void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				DUC_reset.reset = true;
			}
			screenShakeRemainingTime = 0.5f;
			PlayHitSound();
			rb.velocity = Vector3.zero;
			coll.enabled = false;
			hitParticlesTR.position = new Vector2(hitParticlesTR.position.x, tr.position.y);
			hitParticles.Play();
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
			jumpActive = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("prop"))
		{
			splashMaxSpeed = rb.velocity.magnitude * 0.75f;
			if (splashMaxSpeed > 8f)
			{
				splashMaxSpeed = 8f;
			}
			main.startSpeed = new ParticleSystem.MinMaxCurve(1f, splashMaxSpeed);
			ps.Play();
			if (splashMaxSpeed > 2f)
			{
				PlaySplashSound();
			}
		}
		else if (collision.CompareTag("wall"))
		{
			splashMaxSpeed = rb.velocity.magnitude * 0.4f;
			if (splashMaxSpeed > 4f)
			{
				splashMaxSpeed = 5f;
			}
			if (splashMaxSpeed < 2f)
			{
				splashMaxSpeed = 2f;
			}
			main2.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, splashMaxSpeed);
			ps2.Play();
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("DUC");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["DUC"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["DUC"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["DUC"] = -1;
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
			pos = new Vector3(Random.Range(-0.25f, 0.4f), Random.Range(-0.25f, 0.25f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
			{
				bothButtons = false;
				GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.6f, 0.8f), Random.Range(0.8f, 1.2f), 0f);
			}
			else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
			{
				bothButtons = false;
				GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.6f, 0.8f), Random.Range(0.8f, 1.2f), 0f);
			}
		}
	}

	private void PlayMoveAirSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(movementAirSound, Random.Range(0.6f, 0.8f), Random.Range(0.8f, 1.2f), 0f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), 0f);
	}

	private void PlaySplashSound()
	{
		GlobalAudio.globalAudio.Play(splashSound, 0.5f, Random.Range(0.8f, 1.3f), 0f);
	}
}
