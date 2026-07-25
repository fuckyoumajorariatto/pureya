using UnityEngine;

public class SKI_PlayerController : MonoBehaviour
{
	private Transform tr;

	public Transform spriteTR;

	public SpriteRenderer sr;

	public Collider2D col;

	public SpriteRenderer sr_s;

	public Sprite[] skins1;

	public Sprite[] skins2;

	public Sprite[] skins1_s;

	public Sprite[] skins2_s;

	public bool hit;

	private int hits;

	private float maxSpeed = 9f;

	private float currentSpeed;

	private float acceleration = 0.45f;

	private Vector3 pos;

	private int skin;

	public Animator anim;

	public ParticleSystem hitParticles1;

	public ParticleSystem movementParticles;

	private ParticleSystem.EmissionModule emission;

	private float rateOverTime;

	public Transform renderCameraTR;

	private float screenShakeRemainingTime;

	private float initialCooldown;

	private float soundTimer;

	private bool canPlayMovement = true;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private Transform hitTR;

	private bool lookingRight = true;

	public SKI_Reset reset;

	private float rotation = 1000f;

	public Vector2 collisionUp;

	public Color[] hitColors;

	private void Start()
	{
		tr = base.transform;
		emission = movementParticles.emission;
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
			if (hit)
			{
				Spin();
			}
			soundTimer += Time.deltaTime;
			if (soundTimer > 0.05f)
			{
				canPlayMovement = true;
			}
			if (initialCooldown < 0.2f)
			{
				initialCooldown += Time.deltaTime;
			}
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SKI");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SKI"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SKI"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SKI"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		ChangeToSprite1();
	}

	private void Spin()
	{
		tr.position = new Vector3(tr.position.x, tr.position.y + collisionUp.y * Time.deltaTime * (rotation / 200f), tr.position.z);
		if (lookingRight)
		{
			tr.Rotate(0f, 0f, (0f - Time.deltaTime) * rotation);
		}
		else
		{
			tr.Rotate(0f, 0f, Time.deltaTime * rotation);
		}
		if (rotation > 0f)
		{
			rotation -= Time.deltaTime * 510f;
			if (rotation < 0f)
			{
				rotation = 0f;
			}
		}
	}

	private void ManageBounds()
	{
		if (tr.position.x < -5f)
		{
			collisionUp = Vector3.zero;
			ManageCollision();
		}
		else if (tr.position.x > 5f)
		{
			collisionUp = Vector3.zero;
			ManageCollision();
		}
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit || initialCooldown < 0.2f)
		{
			return;
		}
		if (!GlobalInput.globalInput.left || !GlobalInput.globalInput.right)
		{
			if (GlobalInput.globalInput.left)
			{
				currentSpeed -= acceleration;
				if (lookingRight)
				{
					lookingRight = false;
					anim.SetBool("right", false);
				}
				rateOverTime = 100f;
				PlayMovementSound();
			}
			else if (GlobalInput.globalInput.right)
			{
				currentSpeed += acceleration;
				if (!lookingRight)
				{
					lookingRight = true;
					anim.SetBool("right", true);
				}
				rateOverTime = 100f;
				PlayMovementSound();
			}
			else if (!GlobalInput.globalInput.left && !GlobalInput.globalInput.right)
			{
				rateOverTime -= Time.deltaTime * 200f;
				soundTimer = 1f;
				if (currentSpeed > 0f)
				{
					currentSpeed -= acceleration * 0.2f;
					if (currentSpeed < 0f)
					{
						currentSpeed = 0f;
					}
				}
				else if (currentSpeed < 0f)
				{
					currentSpeed += acceleration * 0.2f;
					if (currentSpeed > 0f)
					{
						currentSpeed = 0f;
					}
				}
			}
		}
		if (rateOverTime < 0f)
		{
			rateOverTime = 0f;
		}
		else if (rateOverTime > 100f)
		{
			rateOverTime = 100f;
		}
		emission.rateOverTime = rateOverTime;
		if (currentSpeed > maxSpeed)
		{
			currentSpeed = maxSpeed;
		}
		else if (currentSpeed < 0f - maxSpeed)
		{
			currentSpeed = 0f - maxSpeed;
		}
		tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
		tr.position = new Vector3(tr.position.x - Time.deltaTime * SKI_Difficulty.SKI_difficulty.speed, 0f, tr.position.z);
	}

	public void DoReset()
	{
		tr.position = new Vector3(1.5f, 0f, tr.position.z);
		tr.rotation = Quaternion.identity;
		if (!lookingRight)
		{
			lookingRight = true;
			anim.Rebind();
		}
		rateOverTime = 0f;
		currentSpeed = 0f;
		col.enabled = true;
		hit = false;
		sr.color = Color.white;
		hitParticles1.Stop();
		rotation = 1000f;
		ChangeToSprite1();
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
			sr.color = hitColors[skin];
			col.enabled = false;
			screenShakeRemainingTime = 0.3f;
			hitParticles1.Play();
			PlayHitSound();
			Spin();
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
			pos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PlayMovementSound()
	{
		if (canPlayMovement)
		{
			soundTimer = 0f;
			canPlayMovement = false;
			GlobalAudio.globalAudio.Play(movementSound, 0.4f, Random.Range(0.95f, 1.05f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	public void ChangeToSprite1()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skins1_s[0];
		}
		else
		{
			sr.sprite = skins1[skin];
			sr_s.sprite = skins1_s[skin];
		}
	}

	public void ChangeToSprite2()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skins2_s[0];
		}
		else
		{
			sr.sprite = skins2[skin];
			sr_s.sprite = skins2_s[skin];
		}
	}
}
