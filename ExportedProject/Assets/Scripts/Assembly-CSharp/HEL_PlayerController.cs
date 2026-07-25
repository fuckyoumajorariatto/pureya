using UnityEngine;

public class HEL_PlayerController : MonoBehaviour
{
	private SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Sprite[] helicopter;

	public Sprite[] helicopter_s;

	private Sprite[] selectedSprites = new Sprite[2];

	private Sprite[] selectedSprites_s = new Sprite[2];

	private float animationTimer;

	private bool animationFrame0;

	public HEL_Reset reset;

	public Transform renderCameraTR;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public ParticleSystem hitParticles;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private Rigidbody2D rb;

	private Transform tr;

	private Collider2D coll;

	private Vector3 pos;

	private float verticalAcceleration = 3000f;

	private float horizontalAcceleration = 1000f;

	private bool animate;

	private bool firstInput;

	private int skin;

	private float soundTimer;

	public Color[] hitColors;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		tr = base.transform;
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collider2D>();
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
			ManageBounds();
			Animate();
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("HEL");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["HEL"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["HEL"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["HEL"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			selectedSprites[0] = null;
			selectedSprites[1] = null;
			selectedSprites_s[0] = helicopter_s[0];
			selectedSprites_s[1] = helicopter_s[1];
			sr.sprite = null;
			sr_s.sprite = selectedSprites_s[0];
		}
		else
		{
			selectedSprites[0] = helicopter[skin * 2];
			selectedSprites[1] = helicopter[skin * 2 + 1];
			selectedSprites_s[0] = helicopter_s[skin * 2];
			selectedSprites_s[1] = helicopter_s[skin * 2 + 1];
			sr.sprite = selectedSprites[0];
			sr_s.sprite = selectedSprites_s[0];
		}
	}

	private void ManageBounds()
	{
		if (tr.position.x < -5f || tr.position.x > 5f || tr.position.y < -4.75f || tr.position.y > 4.75f)
		{
			ManageCollision();
		}
	}

	public void ResetPlayer()
	{
		hitParticles.Stop();
		tr.position = new Vector3(-3f, 2f, tr.position.z);
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
		rb.rotation = 0f;
		rb.gravityScale = 0.2f;
		sr.color = new Color(255f, 255f, 255f);
		firstInput = false;
		coll.enabled = true;
		hit = false;
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		if (GlobalInput.globalInput.left && GlobalInput.globalInput.right)
		{
			rb.AddForce(new Vector2(0f, verticalAcceleration * Time.deltaTime));
			animate = true;
			if (!firstInput)
			{
				firstInput = true;
				rb.gravityScale = 2f;
			}
		}
		else if (GlobalInput.globalInput.left)
		{
			rb.AddForce(new Vector2((0f - horizontalAcceleration - HEL_Difficulty.HEL_difficulty.speed * 500f) * Time.deltaTime, verticalAcceleration * Time.deltaTime));
			animate = true;
			if (!firstInput)
			{
				firstInput = true;
				rb.gravityScale = 2f;
			}
		}
		else if (GlobalInput.globalInput.right)
		{
			rb.AddForce(new Vector2(horizontalAcceleration * Time.deltaTime, verticalAcceleration * Time.deltaTime));
			animate = true;
			if (!firstInput)
			{
				firstInput = true;
				rb.gravityScale = 2f;
			}
		}
		else if (!GlobalInput.globalInput.left && !GlobalInput.globalInput.right)
		{
			animate = false;
			soundTimer = 1f;
		}
		rb.velocity = Vector2.ClampMagnitude(rb.velocity, 5f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy") && !hit)
		{
			ManageCollision();
		}
	}

	private void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			hit = true;
			hitParticles.Play();
			coll.enabled = false;
			screenShakeRemainingTime = 0.3f;
			animate = false;
			PlayHitSound();
			sr.color = hitColors[skin];
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
		}
	}

	private void Animate()
	{
		if (!animate && firstInput)
		{
			if (animationFrame0)
			{
				sr.sprite = selectedSprites[0];
				sr_s.sprite = selectedSprites_s[0];
				animationFrame0 = false;
				animationTimer = 0f;
				soundTimer = 1f;
			}
			return;
		}
		if (animationTimer > 0.05f)
		{
			if (animationFrame0)
			{
				sr.sprite = selectedSprites[0];
				sr_s.sprite = selectedSprites_s[0];
			}
			else
			{
				sr.sprite = selectedSprites[1];
				sr_s.sprite = selectedSprites_s[1];
			}
			animationFrame0 = !animationFrame0;
			animationTimer = 0f;
			if (soundTimer > 0.05f)
			{
				soundTimer = 0f;
				GlobalAudio.globalAudio.Play(movementSound, 0.4f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
			}
		}
		animationTimer += Time.deltaTime;
		soundTimer += Time.deltaTime;
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
			pos = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
