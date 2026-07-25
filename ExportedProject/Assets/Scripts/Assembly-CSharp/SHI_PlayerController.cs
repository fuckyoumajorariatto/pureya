using UnityEngine;

public class SHI_PlayerController : MonoBehaviour
{
	private Transform tr;

	public Rigidbody2D rb;

	public Sprite[] hullSprites;

	public Sprite[] sailSprites;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public SpriteRenderer sailSR;

	public SpriteRenderer sailSR_s;

	public Collider2D coll;

	public Transform shipStick;

	public ParticleSystem hitParticles;

	public ParticleSystem foamParticles;

	public bool hit;

	private int hits;

	private int skin;

	public SHI_Reset reset;

	public Transform renderCameraTR;

	private Vector3 pos;

	public SHI_Difficulty difficulty;

	private Vector2 forceToAdd;

	private float turnSpeed = 70f;

	private bool boostL;

	private bool boostR;

	private float changeToAngularVelocity;

	private float movementSoundTimer;

	private float turnQuantity;

	public AudioClip turnSound;

	public AudioClip hitSound;

	private AudioSource movementAS;

	private float screenShakeRemainingTime;

	private bool turnSoundPlayed;

	private void Start()
	{
		tr = base.transform;
		rb = GetComponent<Rigidbody2D>();
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
			Accelerate();
			TurnSail();
			TurnShip();
			ManageBounds();
			tr.position = new Vector3(tr.position.x, tr.position.y - difficulty.speed * Time.deltaTime, tr.position.z);
		}
	}

	private void FixedUpdate()
	{
		if (!PauseController.pauseController.paused)
		{
			rb.angularVelocity += changeToAngularVelocity;
			changeToAngularVelocity = 0f;
			rb.AddForce(forceToAdd);
			forceToAdd = Vector2.zero;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, SHI_Difficulty.SHI_difficulty.maxSpeed);
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SHI");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SHI"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SHI"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SHI"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sailSR.sprite = null;
		}
		else
		{
			sr.sprite = hullSprites[skin];
			sailSR.sprite = sailSprites[skin];
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
			sr.enabled = false;
			sr_s.enabled = false;
			sailSR.enabled = false;
			sailSR_s.enabled = false;
			coll.enabled = false;
			foamParticles.Stop();
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

	private void ManageBounds()
	{
		if (tr.position.x < -4.625f || tr.position.x > 4.625f || tr.position.y < -4.625f || tr.position.y > 4.625f)
		{
			ManageCollision();
		}
	}

	public void ResetPlayer()
	{
		sr.enabled = true;
		sr_s.enabled = true;
		sailSR.enabled = true;
		sailSR_s.enabled = true;
		coll.enabled = true;
		foamParticles.Play();
		hitParticles.Stop();
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
		forceToAdd = Vector2.zero;
		changeToAngularVelocity = 0f;
		shipStick.localRotation = Quaternion.identity;
		tr.position = new Vector3(0f, -3f, tr.position.z);
		tr.rotation = Quaternion.identity;
		turnSoundPlayed = false;
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
			pos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void Accelerate()
	{
		if (GlobalInput.globalInput.gameInputActive && !hit && !GlobalInput.globalInput.left && !GlobalInput.globalInput.right)
		{
			forceToAdd += (Vector2)tr.up * Time.deltaTime * difficulty.acceleration;
		}
	}

	private void TurnSail()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		boostL = false;
		boostR = false;
		if (GlobalInput.globalInput.left)
		{
			shipStick.Rotate(Vector3.forward * Time.deltaTime * turnSpeed);
			if (shipStick.localEulerAngles.z > 20f && shipStick.localEulerAngles.z < 300f)
			{
				shipStick.localEulerAngles = new Vector3(0f, 0f, 20f);
			}
			boostL = true;
		}
		if (GlobalInput.globalInput.right)
		{
			shipStick.Rotate(Vector3.forward * Time.deltaTime * (0f - turnSpeed));
			if (shipStick.localEulerAngles.z < 340f && shipStick.localEulerAngles.z > 300f)
			{
				shipStick.localEulerAngles = new Vector3(0f, 0f, 340f);
			}
			boostR = true;
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			if (movementSoundTimer > 0.1f && !turnSoundPlayed)
			{
				turnSoundPlayed = true;
				GlobalAudio.globalAudio.Play(turnSound, 0.55f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
				movementSoundTimer = -2f;
			}
			movementSoundTimer += Time.deltaTime;
		}
		else
		{
			turnSoundPlayed = false;
			movementSoundTimer = 0f;
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right || (!(shipStick.localEulerAngles.z < 3f) && !(shipStick.localEulerAngles.z > 357f)))
		{
			return;
		}
		if (shipStick.localEulerAngles.z < 5f)
		{
			shipStick.localEulerAngles = new Vector3(0f, 0f, shipStick.localEulerAngles.z - Time.deltaTime * 3f);
			if (shipStick.localEulerAngles.z > 300f)
			{
				shipStick.localEulerAngles = Vector3.zero;
			}
		}
		else if (shipStick.localEulerAngles.z > 355f)
		{
			shipStick.localEulerAngles = new Vector3(0f, 0f, shipStick.localEulerAngles.z + Time.deltaTime * 3f);
			if (shipStick.localEulerAngles.z < 10f)
			{
				shipStick.localEulerAngles = Vector3.zero;
			}
		}
	}

	private void TurnShip()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		turnQuantity = shipStick.localEulerAngles.z;
		if (turnQuantity > 300f)
		{
			turnQuantity -= 360f;
		}
		turnQuantity *= 6f;
		if (turnQuantity >= 120f && boostL)
		{
			turnQuantity += 40f;
		}
		else if (turnQuantity <= -120f && boostR)
		{
			turnQuantity -= 40f;
		}
		if (rb.angularVelocity >= 0f && turnQuantity > 0f)
		{
			if (turnQuantity > rb.angularVelocity)
			{
				changeToAngularVelocity = Mathf.Clamp(turnSpeed / 6f, 0f, 160f - rb.angularVelocity);
			}
			else if (turnQuantity < rb.angularVelocity)
			{
				changeToAngularVelocity = Mathf.Clamp(turnSpeed / 6f, 0f, rb.angularVelocity - turnQuantity) * -1f;
			}
			else
			{
				changeToAngularVelocity = 0f;
			}
		}
		else if (rb.angularVelocity <= 0f && turnQuantity < 0f)
		{
			if (turnQuantity < rb.angularVelocity)
			{
				changeToAngularVelocity = Mathf.Clamp(turnSpeed / 6f, 0f, 160f + rb.angularVelocity) * -1f;
			}
			else if (turnQuantity > rb.angularVelocity)
			{
				changeToAngularVelocity = Mathf.Clamp(turnSpeed / 6f, 0f, turnQuantity - rb.angularVelocity);
			}
			else
			{
				changeToAngularVelocity = 0f;
			}
		}
		else if (rb.angularVelocity > 0f && turnQuantity < 0f)
		{
			changeToAngularVelocity = Mathf.Clamp(turnSpeed / 6f, 0f, 160f + rb.angularVelocity) * -1f;
		}
		else if (rb.angularVelocity < 0f && turnQuantity > 0f)
		{
			changeToAngularVelocity = Mathf.Clamp(turnSpeed / 6f, 0f, 160f - rb.angularVelocity);
		}
		else
		{
			changeToAngularVelocity = 0f;
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
