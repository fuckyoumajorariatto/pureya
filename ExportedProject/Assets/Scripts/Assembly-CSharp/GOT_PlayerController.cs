using System.Collections.Generic;
using UnityEngine;

public class GOT_PlayerController : MonoBehaviour
{
	public static GOT_PlayerController GOT_playerController;

	private Dictionary<int, Sprite[]> skinSprites;

	private Dictionary<int, Sprite[]> skinSprites_s;

	private int skin;

	public Sprite[] spr0 = new Sprite[2];

	public Sprite[] spr1 = new Sprite[2];

	public Sprite[] spr2 = new Sprite[2];

	public Sprite[] spr3 = new Sprite[2];

	public Sprite[] spr4 = new Sprite[2];

	public Sprite[] spr5 = new Sprite[2];

	public Sprite[] spr6 = new Sprite[2];

	public Sprite[] spr7 = new Sprite[2];

	public Sprite[] spr8 = new Sprite[2];

	public Sprite[] spr0_s = new Sprite[2];

	public Sprite[] spr1_s = new Sprite[2];

	public Sprite[] spr2_s = new Sprite[2];

	public Sprite[] spr3_s = new Sprite[2];

	public Sprite[] spr4_s = new Sprite[2];

	public Sprite[] spr5_s = new Sprite[2];

	public Sprite[] spr6_s = new Sprite[2];

	public Sprite[] spr7_s = new Sprite[2];

	public Sprite[] spr8_s = new Sprite[2];

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public AudioClip jumpSound;

	private float maxSpeed = 8f;

	private float acceleration = 250f;

	private float currentSpeed;

	private float deceleration = 150f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private bool lookingRight = true;

	public Transform tr;

	public SpriteRenderer SR;

	public SpriteRenderer SR_s;

	public Rigidbody2D rb;

	public Animator anim;

	public Collider2D coll;

	private float speed = 16f;

	private Vector3 pos;

	private int maintainVelocity;

	private RaycastHit2D hitRC;

	private Transform hitTR;

	public LayerMask layerMask;

	public Transform raycastOrigin;

	private bool bothButtons;

	public float initialCooldown;

	private void Start()
	{
		GOT_playerController = this;
		ChangeSkin();
		InitialJump();
	}

	private void FixedUpdate()
	{
		if (!PauseController.pauseController.paused)
		{
			CheckJump();
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
		if (maintainVelocity > 0)
		{
			if (rb.velocity.y < 0.1f)
			{
				tr.Translate(0f, 0.0625f, 0f);
				rb.velocity = new Vector2(0f, speed);
				maintainVelocity = 3;
			}
			maintainVelocity--;
		}
		ManageInput();
		if (currentSpeed != 0f && !hit)
		{
			MovePlayer();
		}
		ReduceSpeed();
		if (hit)
		{
			tr.Translate(0f, -5f * Time.deltaTime, 0f);
		}
		ManageFall();
	}

	private void InitialJump()
	{
		rb.velocity = new Vector2(0f, speed);
		anim.SetTrigger("jump");
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("GOT");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["GOT"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["GOT"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["GOT"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			SR.sprite = null;
			SR_s.sprite = skinSprites[0][0];
		}
		else
		{
			SR.sprite = skinSprites[skin][0];
			SR_s.sprite = skinSprites[skin][0];
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
		if (initialCooldown < 0.1f)
		{
			initialCooldown += Time.deltaTime;
			return;
		}
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit)
		{
			bothButtons = true;
			return;
		}
		if (GlobalInput.globalInput.left && lookingRight)
		{
			lookingRight = false;
			anim.SetBool("right", false);
		}
		else if (GlobalInput.globalInput.right && !lookingRight)
		{
			lookingRight = true;
			anim.SetBool("right", true);
		}
		if (GlobalInput.globalInput.left)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
			}
			receivingInput = true;
		}
		else if (GlobalInput.globalInput.right)
		{
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime;
			}
			receivingInput = true;
		}
		else
		{
			receivingInput = false;
		}
	}

	public void UpdateDifficulty()
	{
		rb.gravityScale = GOT_Difficulty.GOT_difficulty.gravity;
	}

	private void MovePlayer()
	{
		if (!hit)
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
			if (tr.position.x > 4.9f)
			{
				tr.position = new Vector3(-4.9f, tr.position.y, tr.position.z);
			}
			else if (tr.position.x < -4.9f)
			{
				tr.position = new Vector3(4.9f, tr.position.y, tr.position.z);
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

	private void CheckJump()
	{
		if (!(rb.velocity.y < 0f))
		{
			return;
		}
		hitRC = Physics2D.Raycast(raycastOrigin.position, Vector2.down, 0.1f, layerMask);
		if (hitRC.collider != null)
		{
			hitTR = hitRC.transform;
			if (hitTR.CompareTag("wall") && tr.position.y <= 4.625f)
			{
				Jump(hitRC.collider.gameObject);
			}
		}
	}

	private void Jump(GameObject go)
	{
		PlayJump();
		rb.velocity = new Vector2(0f, speed);
		maintainVelocity = 3;
		go.GetComponent<Animator>().SetTrigger("action");
		anim.SetTrigger("jump");
	}

	private void ManageFall()
	{
		if (!hit && !(tr.position.y > -8f) && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				GOT_Reset.GOT_reset.reset = true;
			}
			coll.enabled = false;
			PlayHitSound();
			currentSpeed = 0f;
			GOT_RockSpawner.GOT_rockSpawner.CrumbleAllRocks();
			screenShakeRemainingTime = 0.5f;
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

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
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
			SR.sprite = null;
			SR_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			SR.sprite = skinSprites[skin][0];
			SR_s.sprite = skinSprites_s[skin][0];
		}
	}

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			SR.sprite = null;
			SR_s.sprite = skinSprites_s[0][1];
		}
		else
		{
			SR.sprite = skinSprites[skin][1];
			SR_s.sprite = skinSprites_s[skin][1];
		}
	}
}
