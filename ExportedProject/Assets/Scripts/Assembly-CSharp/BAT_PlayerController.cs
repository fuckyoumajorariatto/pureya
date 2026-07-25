using System.Collections.Generic;
using UnityEngine;

public class BAT_PlayerController : MonoBehaviour
{
	private Dictionary<int, Sprite[]> skinSprites;

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

	public GameObject renderCamera;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public GameObject hitParticles;

	private Animator animator;

	public bool firstInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private float scrollSpeed;

	private float timer;

	private Transform tr;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Rigidbody2D rb;

	public Collider2D coll;

	public Color[] hitColors;

	private void Start()
	{
		animator = GetComponent<Animator>();
		ChangeSkin();
		tr = base.transform;
	}

	private void Update()
	{
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		if (!PauseController.pauseController.paused)
		{
			if (timer < 0.5f)
			{
				timer += Time.deltaTime;
			}
			else
			{
				ManageInput();
			}
			TiltPlayer();
			ScrollIfHit();
			LimitVelocity();
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("BAT");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["BAT"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["BAT"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["BAT"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
		}
	}

	private void LimitVelocity()
	{
		if (rb.velocity.y < -10f)
		{
			rb.velocity = new Vector2(rb.velocity.x, -10f);
		}
	}

	private void ScrollIfHit()
	{
		if (hit && !((double)tr.position.y > -3.5))
		{
			scrollSpeed = BAT_Difficulty.BAT_difficulty.speed;
			tr.Translate((0f - scrollSpeed) * Time.deltaTime, 0f, 0f);
		}
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		if (!GlobalInput.globalInput.leftDown || !GlobalInput.globalInput.rightDown)
		{
			if (GlobalInput.globalInput.leftDown)
			{
				if (!firstInput)
				{
					ManageFirstInput();
				}
				Move(true);
				animator.SetTrigger("fly");
			}
			else if (GlobalInput.globalInput.rightDown)
			{
				if (!firstInput)
				{
					ManageFirstInput();
				}
				Move(false);
				animator.SetTrigger("fly");
			}
		}
		if (!firstInput)
		{
			MaintainFlight();
		}
	}

	private void ManageFirstInput()
	{
		firstInput = true;
		rb.gravityScale = 5f;
	}

	private void MaintainFlight()
	{
		rb.velocity = new Vector2(1f, -2f);
	}

	private void Move(bool left)
	{
		if (hit)
		{
			return;
		}
		Vector2 velocity = rb.velocity;
		velocity.y = 12f;
		if (left)
		{
			velocity.x = -4f;
			if (tr.localScale.x == 1f)
			{
				tr.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		else
		{
			velocity.x = 1f;
			if (tr.localScale.x == -1f)
			{
				tr.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		rb.velocity = velocity;
		PlayMoveSound();
	}

	private void TiltPlayer()
	{
		if (!firstInput)
		{
			return;
		}
		float y = rb.velocity.y;
		if (y < 0f)
		{
			if (tr.localScale.x == 1f)
			{
				tr.Rotate(Vector3.forward * y * Time.deltaTime * 12f);
			}
			else
			{
				tr.Rotate(Vector3.back * y * Time.deltaTime * 12f);
			}
		}
		else
		{
			tr.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.5f, 1f), Random.Range(1f, 1.25f), tr.position.x / 4f);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ManageCollision(collision);
	}

	private void ManageCollision(Collision2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && (collision.transform.CompareTag("enemy") || collision.transform.CompareTag("wall")))
		{
			hit = true;
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			if (MinigamesController.minigamesController.Hit())
			{
				BAT_Reset.BAT_reset.reset = true;
			}
			sr.color = hitColors[skin];
			hitParticles.transform.position = tr.position;
			hitParticles.SetActive(true);
			screenShakeRemainingTime = 0.3f;
			coll.enabled = false;
			PlayHit();
			GlobalControl.globalControl.ReduceXp();
		}
	}

	private void PlayHit()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ShakeScreen()
	{
		if (PauseController.pauseController.paused)
		{
			Vector3 position = new Vector3(0f, 0f, renderCamera.transform.position.z);
			renderCamera.transform.position = position;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			Vector3 position2 = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), renderCamera.transform.position.z);
			renderCamera.transform.position = position2;
		}
		else
		{
			Vector3 position3 = new Vector3(0f, 0f, renderCamera.transform.position.z);
			renderCamera.transform.position = position3;
		}
	}

	private void ChangeToSprite0()
	{
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

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites[0][1];
		}
		else
		{
			sr.sprite = skinSprites[skin][1];
			sr_s.sprite = skinSprites[skin][1];
		}
	}
}
