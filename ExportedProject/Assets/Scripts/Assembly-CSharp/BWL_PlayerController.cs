using UnityEngine;

public class BWL_PlayerController : MonoBehaviour
{
	public BWL_Reset reset;

	private int skin;

	public Sprite[] skinSprites;

	public SpriteRenderer sr1;

	public SpriteRenderer sr2;

	public ParticleSystem hitParticles1;

	public ParticleSystem hitParticles2;

	public Transform renderCameraTR;

	public AudioClip movementSound;

	public AudioClip bounceSound;

	public AudioClip laneSound;

	public AudioClip hitSound;

	private float maxSpeed = 8f;

	private float acceleration = 45f;

	private float currentSpeed;

	private float bumpStrength = 8f;

	public bool hit;

	private bool preHit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	private Vector3 pos;

	public Rigidbody2D rb;

	public Collider2D coll;

	private bool bothButtons;

	private float inputCooldown;

	public Transform ballTexture1TR;

	public Transform ballTexture2TR;

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
			MovePlayer();
			ManageBounds();
		}
	}

	private void ManageInput()
	{
		if (inputCooldown > 0f)
		{
			inputCooldown -= Time.deltaTime;
			return;
		}
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit || preHit)
		{
			bothButtons = true;
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
				if (currentSpeed < 0f - maxSpeed)
				{
					currentSpeed = 0f - maxSpeed;
				}
			}
		}
		else if (GlobalInput.globalInput.right && currentSpeed < maxSpeed)
		{
			currentSpeed += acceleration * Time.deltaTime;
			if (currentSpeed > maxSpeed)
			{
				currentSpeed = maxSpeed;
			}
		}
		if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
		{
			bothButtons = false;
			PlayMoveSound();
		}
		else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
		{
			bothButtons = false;
			PlayMoveSound();
		}
	}

	private void MovePlayer()
	{
		if (hit || preHit)
		{
			tr.Translate(0f, BWL_Difficulty.BWL_difficulty.scroll * Time.deltaTime, 0f);
		}
		else
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
		}
	}

	private void ManageBounds()
	{
		if (tr.position.x < -3.5f)
		{
			if (!preHit)
			{
				preHit = true;
				PlayLaneSound();
			}
			currentSpeed = 0f;
			tr.position = new Vector3(-4f, tr.position.y, tr.position.z);
		}
		else if (tr.position.x > 3.5f)
		{
			if (!preHit)
			{
				preHit = true;
				PlayLaneSound();
			}
			currentSpeed = 0f;
			tr.position = new Vector3(4f, tr.position.y, tr.position.z);
		}
		if (tr.position.y > 4.5f)
		{
			ManageHit();
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("BWL");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["BWL"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["BWL"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["BWL"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr1.sprite = null;
			sr2.sprite = null;
		}
		else
		{
			sr1.sprite = skinSprites[skin];
			sr2.sprite = skinSprites[skin];
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("enemy"))
		{
			ManageBump(collision);
		}
	}

	private void ManageBump(Collider2D collision)
	{
		if (tr.position.x > collision.transform.position.x)
		{
			currentSpeed = bumpStrength;
			collision.GetComponent<BWL_PinController>().Animate(true);
		}
		else
		{
			currentSpeed = 0f - bumpStrength;
			collision.GetComponent<BWL_PinController>().Animate(false);
		}
		inputCooldown = 0.1f;
		PlayBounceSound();
	}

	public void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			hitParticles1.Play();
			hitParticles2.Play();
			screenShakeRemainingTime = 0.3f;
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

	public void DoReset()
	{
		hit = false;
		preHit = false;
		inputCooldown = 0f;
		hitParticles1.Stop();
		hitParticles2.Stop();
		tr.position = new Vector3(0f, 3f, tr.position.z);
		currentSpeed = 0f;
		ballTexture1TR.localPosition = new Vector3(0f, 2.1875f, ballTexture1TR.localPosition.z);
		ballTexture2TR.localPosition = new Vector3(0f, -1.125f, ballTexture1TR.localPosition.z);
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

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(movementSound, 0.7f, Random.Range(0.9f, 1f), tr.position.x / 4f);
		}
	}

	private void PlayBounceSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(bounceSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayLaneSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(laneSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 8f);
		}
	}
}
