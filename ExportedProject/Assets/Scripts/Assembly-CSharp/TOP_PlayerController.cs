using UnityEngine;

public class TOP_PlayerController : MonoBehaviour
{
	private int skin;

	public Sprite[] spr = new Sprite[9];

	public Sprite[] spr_s = new Sprite[9];

	private Color32 shadowColor = new Color32(69, 69, 69, byte.MaxValue);

	public TOP_Reset reset;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public AudioClip jumpSound;

	public AudioClip contactSound;

	public AudioClip ballContactSound;

	public AudioClip ballHitSound;

	public Rigidbody2D rb;

	public ParticleSystem bumpPS;

	public Transform bumpTR;

	public ParticleSystem ballPopPS;

	public Transform ballPopTR;

	public ParticleSystem outPS;

	public Transform outTR;

	public ParticleSystem ballHitPS;

	public Transform ballHitTR;

	private float maxSpeed = 6f;

	private float acceleration = 50f;

	private float currentSpeed;

	private float deceleration = 10f;

	private float verticalSpeed;

	private float verticalDeceleration = 20f;

	private bool grounded = true;

	private int framesToToggleGrounded = 3;

	private float groundedCooldownTimer;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public LayerMask layerMask;

	public LayerMask enemyLayerMask;

	private float noGroundedTimer;

	public Transform tr;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Animator anim;

	public Collider2D coll;

	public Collider2D coll2;

	private Vector3 pos;

	private RaycastHit2D hitRC;

	private Transform hitTR;

	private bool pauseFirstFrameCheck;

	private bool forcedLeftDown;

	private float jumpInputPool;

	private bool rebound;

	private GameObject marble;

	private MarbleController marbleController;

	private float initialCooldown;

	private ContactPoint2D[] contacts = new ContactPoint2D[1];

	private bool canPlayFloorContact = true;

	private float blockedInput;

	private float bumpInputBlock = 0.15f;

	public SpriteMask mask;

	public Color[] rotationEffectColors;

	public ParticleSystem rotationParticles;

	private ParticleSystem.MainModule main;

	private void Start()
	{
		ChangeSkin();
		DoReset();
	}

	private void FixedUpdate()
	{
		if (rb.velocity.magnitude > 20.4f && rb.velocity.y > 0f)
		{
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 0f, 20.4f));
		}
		if (rebound && grounded && !hit)
		{
			Rebound();
		}
	}

	private void Update()
	{
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		if (PauseController.pauseController.paused)
		{
			return;
		}
		if (initialCooldown > 0f)
		{
			initialCooldown -= Time.deltaTime;
		}
		if (hit)
		{
			tr.position = new Vector3(tr.position.x + Time.deltaTime * TOP_Difficulty.TOP_difficulty.scroll, tr.position.y, tr.position.z);
			return;
		}
		if (blockedInput > 0f)
		{
			blockedInput -= Time.deltaTime;
		}
		else
		{
			ManageInput();
		}
		if (currentSpeed != 0f && !hit)
		{
			MovePlayer();
		}
		ReduceSpeed();
		CheckIfGrounded();
		CheckIfDead();
		groundedCooldownTimer += Time.deltaTime;
		if (PauseController.pauseController.paused)
		{
			if (!pauseFirstFrameCheck)
			{
				pauseFirstFrameCheck = true;
				if (!GlobalInput.globalInput.left)
				{
					forcedLeftDown = true;
				}
			}
		}
		else
		{
			pauseFirstFrameCheck = false;
		}
	}

	private void LateUpdate()
	{
		if (!hit)
		{
			rb.angularVelocity = 0f;
			tr.rotation = Quaternion.identity;
		}
		if (currentSpeed == 0f && grounded)
		{
			rb.velocity = new Vector2(0f, rb.velocity.y);
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("TOP");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["TOP"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["TOP"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["TOP"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = spr_s[0];
			mask.sprite = spr_s[0];
			main = rotationParticles.main;
			main.startColor = rotationEffectColors[skin];
		}
		else
		{
			sr.sprite = spr[skin];
			sr_s.sprite = spr_s[skin];
			mask.sprite = spr_s[skin];
			main = rotationParticles.main;
			main.startColor = rotationEffectColors[skin];
		}
	}

	private void PanAudio()
	{
		if (audioSource != null)
		{
			if (!audioSource.isPlaying)
			{
				audioSource = null;
			}
			else
			{
				GlobalAudio.globalAudio.UpdatePan(audioSource, tr.position.x / 4f);
			}
		}
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || SceneChangeController.sceneChangeController.transitioning || hit || initialCooldown > 0f)
		{
			return;
		}
		if (GlobalInput.globalInput.right && GlobalInput.globalInput.left && grounded)
		{
			if ((GlobalInput.globalInput.leftDown || (forcedLeftDown && GlobalInput.globalInput.left)) && grounded)
			{
				Jump();
			}
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
			}
			if (GlobalInput.globalInput.leftDown)
			{
				PlayMoveSound();
			}
			if (!grounded)
			{
				jumpInputPool += Time.deltaTime;
			}
			else if (grounded && jumpInputPool > 0f && jumpInputPool < 0.05f)
			{
				Jump();
			}
		}
		else
		{
			jumpInputPool = 0f;
		}
		if (GlobalInput.globalInput.right)
		{
			forcedLeftDown = false;
			if (currentSpeed < maxSpeed)
			{
				if (grounded)
				{
					currentSpeed += acceleration * Time.deltaTime * 1.25f;
				}
				else
				{
					currentSpeed += acceleration * Time.deltaTime * 1.5f;
				}
			}
			if (GlobalInput.globalInput.rightDown)
			{
				PlayMoveSound();
			}
		}
		if ((GlobalInput.globalInput.leftDown || (forcedLeftDown && GlobalInput.globalInput.left)) && grounded)
		{
			forcedLeftDown = false;
			Jump();
		}
		if (GlobalInput.globalInput.right)
		{
			forcedLeftDown = false;
			receivingInput = true;
		}
		else if (GlobalInput.globalInput.left)
		{
			receivingInput = true;
		}
		else
		{
			forcedLeftDown = false;
			receivingInput = false;
			tr.Translate(Time.deltaTime * TOP_Difficulty.TOP_difficulty.scroll, 0f, 0f);
		}
	}

	private void Jump()
	{
		groundedCooldownTimer = 0f;
		grounded = false;
		rebound = true;
		if (currentSpeed > 0f)
		{
			currentSpeed = 0f;
		}
		rb.velocity = Vector2.zero;
		rb.AddForce(new Vector2(0f, 1200f));
		PlayJump();
	}

	private void Rebound()
	{
		rebound = false;
		rb.velocity = new Vector2(rb.velocity.x, 0f);
		rb.AddForce(new Vector2(0f, 300f));
	}

	private void BallRebound()
	{
		rebound = false;
		rb.velocity = new Vector2(rb.velocity.x, 0f);
		rb.AddForce(new Vector2(0f, 800f));
	}

	private void MovePlayer()
	{
		ApplyCurrentSpeed();
	}

	private void ApplyCurrentSpeed()
	{
		rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
	}

	private void CheckIfDead()
	{
		if ((tr.position.x < -5f || tr.position.x > 5f || tr.position.y < -4f) && !hit)
		{
			if (tr.position.x < -5f)
			{
				outTR.localScale = new Vector3(-1f, 1f, 1f);
				outTR.position = new Vector3(tr.position.x, tr.position.y, outTR.position.z);
				outPS.Play();
			}
			else if (tr.position.x > 5f)
			{
				outTR.localScale = new Vector3(1f, 1f, 1f);
				outTR.position = new Vector3(tr.position.x, tr.position.y, outTR.position.z);
				outPS.Play();
			}
			ManageHit(false);
		}
	}

	private bool CheckIfGrounded()
	{
		if (rb.velocity.y < -1f)
		{
			noGroundedTimer += Time.deltaTime;
		}
		else
		{
			noGroundedTimer = 0f;
		}
		if (noGroundedTimer > 0.5f)
		{
			grounded = false;
			return false;
		}
		pos = tr.position;
		pos.x -= 1f / 32f;
		pos.y += 1f / 32f;
		hitRC = Physics2D.Raycast(pos, Vector2.down, 0.0625f, layerMask);
		if (hitRC.collider != null)
		{
			hitTR = hitRC.transform;
			if (hitTR.position.y <= tr.position.y - 0.125f && groundedCooldownTimer > 0.1f)
			{
				framesToToggleGrounded = 5;
				grounded = true;
				if (hit)
				{
					return true;
				}
				rb.velocity = new Vector2(rb.velocity.x, 0f);
				if (tr.position.y < hitTR.position.y + 13f / 32f && tr.position.y > hitTR.position.y + 0.125f && tr.position.x < hitTR.position.x + 1.15625f && tr.position.x > hitTR.position.x - 1.15625f)
				{
					tr.position = new Vector3(tr.position.x, hitTR.position.y + 0.375f, tr.position.z);
				}
				if (canPlayFloorContact)
				{
					canPlayFloorContact = false;
					PlayContact();
				}
				return true;
			}
		}
		pos = tr.position;
		pos.x += 1f / 32f;
		pos.y += 1f / 32f;
		hitRC = Physics2D.Raycast(pos, Vector2.down, 0.0625f, layerMask);
		if (hitRC.collider != null)
		{
			hitTR = hitRC.transform;
			if (hitTR.position.y <= tr.position.y - 0.125f && groundedCooldownTimer > 0.1f)
			{
				framesToToggleGrounded = 5;
				grounded = true;
				if (hit)
				{
					return true;
				}
				rb.velocity = new Vector2(rb.velocity.x, 0f);
				if (tr.position.y < hitTR.position.y + 13f / 32f && tr.position.y > hitTR.position.y + 0.125f && tr.position.x < hitTR.position.x + 1.15625f && tr.position.x > hitTR.position.x - 1.15625f)
				{
					tr.position = new Vector3(tr.position.x, hitTR.position.y + 0.375f, tr.position.z);
				}
				if (canPlayFloorContact)
				{
					canPlayFloorContact = false;
					PlayContact();
				}
				return true;
			}
		}
		if (framesToToggleGrounded == 0)
		{
			grounded = false;
		}
		else
		{
			framesToToggleGrounded--;
		}
		canPlayFloorContact = true;
		return false;
	}

	private void ReduceSpeed()
	{
		if (verticalSpeed != 0f)
		{
			if (verticalSpeed > 0f)
			{
				verticalSpeed -= Time.deltaTime * verticalDeceleration;
				if (verticalSpeed < 0f)
				{
					verticalSpeed = 0f;
				}
			}
			else if (verticalSpeed < 0f)
			{
				verticalSpeed += Time.deltaTime * verticalDeceleration;
				if (verticalSpeed > 0f)
				{
					verticalSpeed = 0f;
				}
			}
		}
		if (receivingInput || currentSpeed == 0f)
		{
			return;
		}
		if (currentSpeed > 0f)
		{
			if (currentSpeed > maxSpeed)
			{
				currentSpeed -= Time.deltaTime * deceleration * 2f;
			}
			else
			{
				currentSpeed -= Time.deltaTime * deceleration;
			}
			if (currentSpeed < 0f)
			{
				currentSpeed = 0f;
			}
		}
		else if (currentSpeed < 0f)
		{
			if (currentSpeed < 0f - maxSpeed)
			{
				currentSpeed += Time.deltaTime * deceleration * 2f;
			}
			else
			{
				currentSpeed += Time.deltaTime * deceleration;
			}
			if (currentSpeed > 0f)
			{
				currentSpeed = 0f;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (hit)
		{
			return;
		}
		if (collision.transform.CompareTag("enemy"))
		{
			if (collision.transform.position.y > tr.position.y + 0.3f)
			{
				if (collision.transform.position.x > tr.position.x)
				{
					ManageHit(false);
					bumpTR.position = new Vector3((collision.transform.position.x + tr.position.x) / 2f, (collision.transform.position.y + tr.position.y) / 2f, ballHitTR.position.z);
					bumpPS.Play();
					ballHitPS.Play();
					rotationParticles.Stop();
				}
				else
				{
					ManageHit(true);
					bumpTR.position = new Vector3((collision.transform.position.x + tr.position.x) / 2f, (collision.transform.position.y + tr.position.y) / 2f, ballHitTR.position.z);
					bumpPS.Play();
					ballHitPS.Play();
					rotationParticles.Stop();
				}
				blockedInput += bumpInputBlock;
				PlayBallContact();
			}
			else
			{
				BallRebound();
				collision.gameObject.GetComponentInChildren<TOP_BallController>().AnimatedDisable();
				CreateMarble(collision.transform);
				ballPopTR.position = new Vector3(tr.position.x, tr.position.y + 0.0625f, ballPopTR.position.z);
				ballPopPS.Play();
				PlayBallHit();
			}
		}
		else if (collision.transform.CompareTag("wall"))
		{
			if (collision.transform.position.x < tr.position.x - 1.25f && collision.transform.position.y > tr.position.y - 0.3125f && rb.velocity.x < TOP_Difficulty.TOP_difficulty.scroll)
			{
				currentSpeed = 5f;
				rb.velocity = new Vector2(5f, 0f);
				rb.AddForce(new Vector2(0f, 500f));
				collision.GetContacts(contacts);
				bumpTR.position = new Vector3(contacts[0].point.x, contacts[0].point.y, bumpTR.position.z);
				blockedInput += bumpInputBlock;
				bumpPS.Play();
				PlayContact();
				rotationParticles.Stop();
			}
			else if (collision.transform.position.x > tr.position.x + 1.25f && collision.transform.position.y > tr.position.y - 0.3125f && currentSpeed > 0f)
			{
				currentSpeed = -5f;
				rb.velocity = new Vector2(-5f, 0f);
				rb.AddForce(new Vector2(0f, 500f));
				collision.GetContacts(contacts);
				bumpTR.position = new Vector3(contacts[0].point.x, contacts[0].point.y, bumpTR.position.z);
				blockedInput += bumpInputBlock;
				bumpPS.Play();
				PlayContact();
				rotationParticles.Stop();
			}
		}
	}

	private void CreateMarble(Transform ballTR)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			marble = MarbleCreator.marbleCreator.GetMarble();
			SetMarbleConstants(marble);
			marble.transform.position = ballTR.position;
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = TOP_Difficulty.TOP_difficulty.scroll;
		marbleController.direction = MarbleController.Direction.right;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_right;
		marbleController.maxOut = 7f;
		marble.SetActive(true);
	}

	private void ManageHit(bool left)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			anim.SetTrigger("fallen");
			if (left)
			{
				rb.AddTorque(-100f);
			}
			else
			{
				rb.AddTorque(100f);
			}
			PlayHitSound();
			coll.enabled = false;
			coll2.enabled = true;
			screenShakeRemainingTime = 0.4f;
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
			currentSpeed = 0f;
			rb.velocity = Vector2.zero;
		}
	}

	public void DoReset()
	{
		anim.SetTrigger("reset");
		anim.ResetTrigger("fallLeft");
		anim.ResetTrigger("fallRight");
		rb.angularVelocity = 0f;
		rb.rotation = 0f;
		coll.enabled = true;
		coll2.enabled = false;
		tr.position = new Vector3(4f, 0f, tr.position.z);
		rb.velocity = new Vector2(-13f, 0f);
		currentSpeed = -13f;
		hit = false;
		initialCooldown = 0.1f;
		blockedInput = 0f;
		rotationParticles.Play();
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

	private void PlayMoveSound()
	{
		GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.2f, 0.3f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void PlayHitSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayJump()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(jumpSound, 0.5f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
		}
	}

	private void PlayContact()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(contactSound, 0.8f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
		}
	}

	private void PlayBallContact()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(ballContactSound, 0.8f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
		}
	}

	private void PlayBallHit()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(ballHitSound, 0.8f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
		}
	}
}
