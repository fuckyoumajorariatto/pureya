using System.Collections.Generic;
using UnityEngine;

public class SEA_PlayerController : MonoBehaviour
{
	public static SEA_PlayerController SEA_playerController;

	private Dictionary<int, Sprite[]> skinSprites;

	public Sprite[] sprs0 = new Sprite[2];

	public Sprite[] sprs1 = new Sprite[2];

	public Sprite[] sprs2 = new Sprite[2];

	public Sprite[] sprs3 = new Sprite[2];

	public Sprite[] sprs4 = new Sprite[2];

	public Sprite[] sprs5 = new Sprite[2];

	public Sprite[] sprs6 = new Sprite[2];

	public Sprite[] sprs7 = new Sprite[2];

	public Sprite[] sprs8 = new Sprite[2];

	public Sprite[] sprs_s = new Sprite[2];

	private int skin;

	public Rigidbody2D rb;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip bubbleSound;

	public AudioClip hitSound;

	private float maxSpeed = 10f;

	private float acceleration = 50f;

	private float currentSpeed;

	private float deceleration = 100f;

	private bool receivingInput;

	private float originalMaxFallSpeed;

	private float maxFallSpeed;

	private float reducedFallSpeed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private bool firstBubble;

	private bool touchingBubble;

	private float timer;

	public Transform tr;

	public Transform hitParticlesTR;

	private Vector3 pos;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Collider2D coll;

	private bool bothButtons;

	private float doubleMoveSoundCooldown;

	private void Start()
	{
		SEA_playerController = this;
		ChangeSkin();
		SetDifficultyParas();
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
			CheckBounds();
			PanAudio();
			touchingBubble = false;
			timer += Time.deltaTime;
			if (doubleMoveSoundCooldown > 0f)
			{
				doubleMoveSoundCooldown -= Time.deltaTime;
			}
		}
	}

	public void SetDifficultyParas()
	{
		originalMaxFallSpeed = SEA_Difficulty.SEA_difficulty.speed - SEA_Difficulty.SEA_difficulty.speed / 4f;
		maxFallSpeed = originalMaxFallSpeed;
		reducedFallSpeed = originalMaxFallSpeed / 4f;
		rb.gravityScale = SEA_Difficulty.SEA_difficulty.gravity;
	}

	private void ManageInput()
	{
		receivingInput = false;
		tr.localScale = new Vector3(1f, 1f, tr.localScale.z);
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit || timer < 0.5f)
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
			maxFallSpeed = reducedFallSpeed;
		}
		else if (GlobalInput.globalInput.right)
		{
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime;
			}
			maxFallSpeed = reducedFallSpeed;
		}
		else
		{
			maxFallSpeed = originalMaxFallSpeed;
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			receivingInput = true;
			if (!touchingBubble && rb.velocity.y > 0f)
			{
				rb.velocity = new Vector2(rb.velocity.x, 0f);
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
			if (!(doubleMoveSoundCooldown > 0f))
			{
				PlayMoveSound();
			}
		}
	}

	private void ChangeSkin()
	{
		skinSprites = new Dictionary<int, Sprite[]>();
		skinSprites[0] = sprs0;
		skinSprites[1] = sprs1;
		skinSprites[2] = sprs2;
		skinSprites[3] = sprs3;
		skinSprites[4] = sprs4;
		skinSprites[5] = sprs5;
		skinSprites[6] = sprs6;
		skinSprites[7] = sprs7;
		skinSprites[8] = sprs8;
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SEA");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SEA"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SEA"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SEA"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			sr.sprite = null;
		}
		else
		{
			sr.sprite = skinSprites[0][0];
		}
	}

	private void MovePlayer()
	{
		tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -4.5f)
		{
			tr.position = new Vector2(tr.position.x + 9f, tr.position.y);
		}
		else if (tr.position.x > 4.5f)
		{
			tr.position = new Vector2(tr.position.x - 9f, tr.position.y);
		}
	}

	private void ReduceSpeed()
	{
		if (!receivingInput && currentSpeed != 0f)
		{
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
		if (rb.velocity.y < 0f - maxFallSpeed)
		{
			rb.velocity = new Vector2(rb.velocity.x, 0f - maxFallSpeed);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("prop") && firstBubble)
		{
			PlayBubbleSound();
		}
		else
		{
			firstBubble = true;
		}
	}

	private void CheckBounds()
	{
		if ((tr.position.y < -5.5f || tr.position.y > 5.5f) && !hit)
		{
			hitParticlesTR.position = tr.position;
			hitParticles.SetActive(true);
			ManageHit();
		}
	}

	private void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				SEA_Reset.SEA_reset.reset = true;
			}
			rb.gravityScale = 0f;
			hitParticlesTR.position = tr.position;
			hitParticles.SetActive(true);
			screenShakeRemainingTime = 0.5f;
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

	private void OnCollisionStay2D(Collision2D collision)
	{
		touchingBubble = true;
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
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.9f, 1f), Random.Range(0.75f, 1.25f), tr.position.x / 4f);
		}
	}

	private void PlayBubbleSound()
	{
		if (!hit)
		{
			audioSource = GlobalAudio.globalAudio.Play(bubbleSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ChangeToSprite0()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = sprs_s[0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = sprs_s[0];
		}
	}

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = sprs_s[1];
		}
		else
		{
			sr.sprite = skinSprites[skin][1];
			sr_s.sprite = sprs_s[1];
		}
	}
}
