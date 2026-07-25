using System.Collections.Generic;
using UnityEngine;

public class PNG_PlayerController : MonoBehaviour
{
	public static PNG_PlayerController PNG_playerController;

	private Dictionary<int, Sprite[]> skinSprites;

	private Dictionary<int, Sprite[]> skinSprites_s;

	private int skin;

	public Sprite[] spr0 = new Sprite[3];

	public Sprite[] spr1 = new Sprite[3];

	public Sprite[] spr2 = new Sprite[3];

	public Sprite[] spr3 = new Sprite[3];

	public Sprite[] spr4 = new Sprite[3];

	public Sprite[] spr5 = new Sprite[3];

	public Sprite[] spr6 = new Sprite[3];

	public Sprite[] spr7 = new Sprite[3];

	public Sprite[] spr8 = new Sprite[3];

	public Sprite[] spr0_s = new Sprite[3];

	public Sprite[] spr1_s = new Sprite[3];

	public Sprite[] spr2_s = new Sprite[3];

	public Sprite[] spr3_s = new Sprite[3];

	public Sprite[] spr4_s = new Sprite[3];

	public Sprite[] spr5_s = new Sprite[3];

	public Sprite[] spr6_s = new Sprite[3];

	public Sprite[] spr7_s = new Sprite[3];

	public Sprite[] spr8_s = new Sprite[3];

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public AudioClip jumpSound;

	public Rigidbody2D rb;

	private float maxSpeed = 6f;

	private float acceleration = 40f;

	private float currentSpeed;

	private float deceleration = 150f;

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

	private float moveSoundCooldown;

	private float noGroundedTimer;

	public Transform tr;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Animator anim;

	public Collider2D coll;

	private Vector3 pos;

	private RaycastHit2D hitRC;

	private Transform hitTR;

	private bool pauseFirstFrameCheck;

	private bool forcedRightDown;

	private float jumpInputPool;

	private void Start()
	{
		PNG_playerController = this;
		ChangeSkin();
	}

	private void FixedUpdate()
	{
		if (rb.velocity.magnitude > 20.4f && rb.velocity.y > 0f)
		{
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 0f, 20.4f));
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
		ManageInput();
		if (currentSpeed != 0f && !hit)
		{
			MovePlayer();
		}
		else
		{
			moveSoundCooldown = 0f;
		}
		ReduceSpeed();
		if (hit)
		{
			tr.Translate(0f, -5f * Time.deltaTime, 0f);
		}
		CheckIfWalled();
		CheckIfDead();
		groundedCooldownTimer += Time.deltaTime;
		if (PauseController.pauseController.paused)
		{
			if (!pauseFirstFrameCheck)
			{
				pauseFirstFrameCheck = true;
				if (!GlobalInput.globalInput.right)
				{
					forcedRightDown = true;
				}
			}
		}
		else
		{
			pauseFirstFrameCheck = false;
		}
	}

	private void ChangeSkin()
	{
		skinSprites = new Dictionary<int, Sprite[]>();
		skinSprites.Add(0, spr0);
		skinSprites.Add(1, spr1);
		skinSprites.Add(2, spr2);
		skinSprites.Add(3, spr3);
		skinSprites.Add(4, spr4);
		skinSprites.Add(5, spr5);
		skinSprites.Add(6, spr6);
		skinSprites.Add(7, spr7);
		skinSprites.Add(8, spr8);
		skinSprites_s = new Dictionary<int, Sprite[]>();
		skinSprites_s.Add(0, spr0_s);
		skinSprites_s.Add(1, spr1_s);
		skinSprites_s.Add(2, spr2_s);
		skinSprites_s.Add(3, spr3_s);
		skinSprites_s.Add(4, spr4_s);
		skinSprites_s.Add(5, spr5_s);
		skinSprites_s.Add(6, spr6_s);
		skinSprites_s.Add(7, spr7_s);
		skinSprites_s.Add(8, spr8_s);
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("PNG");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["PNG"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["PNG"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["PNG"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites[0][0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites[skin][0];
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
		if (!GlobalInput.globalInput.gameInputActive || SceneChangeController.sceneChangeController.transitioning || hit)
		{
			return;
		}
		if (GlobalInput.globalInput.right && GlobalInput.globalInput.left)
		{
			currentSpeed = PNG_Difficulty.PNG_difficulty.speed * Time.deltaTime;
			if ((GlobalInput.globalInput.rightDown || (forcedRightDown && GlobalInput.globalInput.right)) && grounded)
			{
				Jump();
			}
			return;
		}
		if (GlobalInput.globalInput.right)
		{
			if (currentSpeed < 0f)
			{
				currentSpeed = 0f;
			}
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime;
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
		if (GlobalInput.globalInput.left)
		{
			forcedRightDown = false;
			if (currentSpeed > 0f)
			{
				currentSpeed = 0f;
			}
			if (grounded)
			{
				if (currentSpeed > 0f - PNG_Difficulty.PNG_difficulty.backwardsMaxSpeed)
				{
					currentSpeed -= acceleration * 2f * Time.deltaTime;
				}
			}
			else if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * 2f * Time.deltaTime;
			}
		}
		if ((GlobalInput.globalInput.rightDown || (forcedRightDown && GlobalInput.globalInput.right)) && grounded)
		{
			forcedRightDown = false;
			Jump();
		}
		if (GlobalInput.globalInput.left)
		{
			forcedRightDown = false;
			receivingInput = true;
		}
		else if (GlobalInput.globalInput.right)
		{
			receivingInput = true;
		}
		else
		{
			forcedRightDown = false;
			receivingInput = false;
		}
	}

	private void Jump()
	{
		groundedCooldownTimer = 0f;
		grounded = false;
		rb.velocity = new Vector2(rb.velocity.x, 0f);
		rb.AddForce(new Vector2(0f, 1300f));
		anim.SetBool("air", true);
		PlayJump();
	}

	private void MovePlayer()
	{
		if (tr.position.x <= -4f && currentSpeed < 0f)
		{
			currentSpeed = 0f;
		}
		else if (tr.position.x >= 4f && currentSpeed > 0f)
		{
			currentSpeed = 0f;
		}
		rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
		if (grounded)
		{
			PlayMoveSound();
		}
	}

	private void CheckIfDead()
	{
		if (tr.position.x < -4.5f || tr.position.y < -4.5f)
		{
			ManageHit();
		}
	}

	private bool CheckIfGrounded()
	{
		if (rb.velocity.y < 0f)
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
		pos.x -= 0.25f;
		hitRC = Physics2D.Raycast(pos, Vector2.down, 0.75f, layerMask);
		if (hitRC.collider != null)
		{
			hitTR = hitRC.transform;
			if (hitTR.position.y <= tr.position.y - 0.9f && groundedCooldownTimer > 0.1f)
			{
				framesToToggleGrounded = 5;
				grounded = true;
				anim.SetBool("air", false);
				return true;
			}
		}
		pos = tr.position;
		pos.x += 0.25f;
		hitRC = Physics2D.Raycast(pos, Vector2.down, 0.75f, layerMask);
		if (hitRC.collider != null)
		{
			hitTR = hitRC.transform;
			if (hitTR.position.y <= tr.position.y - 0.9f && groundedCooldownTimer > 0.1f)
			{
				framesToToggleGrounded = 5;
				grounded = true;
				anim.SetBool("air", false);
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
		return false;
	}

	private void CheckIfWalled()
	{
		if (!CheckIfGrounded())
		{
			hitRC = Physics2D.Raycast(new Vector2(tr.position.x, tr.position.y - 0.6875f), Vector2.right, 0.375f, layerMask);
			if (hitRC.collider != null && (hitRC.collider.transform.CompareTag("wall") || hitRC.collider.transform.CompareTag("prop")) && rb.velocity.y <= 0f)
			{
				rb.velocity = new Vector2(-1f, rb.velocity.y);
			}
		}
	}

	private void ReduceSpeed()
	{
		if (verticalSpeed != 0f)
		{
			if (verticalSpeed > 0f)
			{
				verticalSpeed -= Time.deltaTime * deceleration;
				if (verticalSpeed < 0f)
				{
					verticalSpeed = 0f;
				}
			}
			else if (verticalSpeed < 0f)
			{
				verticalSpeed += Time.deltaTime * deceleration;
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
			currentSpeed -= Time.deltaTime * verticalDeceleration;
			if (currentSpeed < 0f)
			{
				currentSpeed = 0f;
			}
		}
		else if (currentSpeed < 0f)
		{
			currentSpeed += Time.deltaTime * verticalDeceleration;
			if (currentSpeed > 0f)
			{
				currentSpeed = 0f;
			}
		}
	}

	private void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				PNG_Reset.PNG_reset.reset = true;
			}
			PlayHitSound();
			coll.enabled = false;
			screenShakeRemainingTime = 0.5f;
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
			pos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.15f, 0.25f), renderCameraTR.position.z);
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
		if (SceneChangeController.sceneChangeController.transitioning || PauseController.pauseController.paused)
		{
			return;
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			if (moveSoundCooldown <= 0f)
			{
				audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
				moveSoundCooldown = 0.25f + Time.deltaTime;
			}
			moveSoundCooldown -= Time.deltaTime;
		}
		else
		{
			moveSoundCooldown = 0f;
		}
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
			GlobalAudio.globalAudio.Play(jumpSound, 1f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
		}
	}

	private void ChangeToSprite0()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites_s[skin][0];
		}
	}

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][1];
		}
		else
		{
			sr.sprite = skinSprites[skin][1];
			sr_s.sprite = skinSprites_s[skin][1];
		}
	}

	private void ChangeToSprite2()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][2];
		}
		else
		{
			sr.sprite = skinSprites[skin][2];
			sr_s.sprite = skinSprites_s[skin][2];
		}
	}
}
