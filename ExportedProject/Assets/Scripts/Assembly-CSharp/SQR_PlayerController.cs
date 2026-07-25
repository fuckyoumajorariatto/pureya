using System.Collections.Generic;
using UnityEngine;

public class SQR_PlayerController : MonoBehaviour
{
	public static SQR_PlayerController SQR_playerController;

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

	public Sprite[] spr_s = new Sprite[2];

	public GameObject shadow;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public Animator animator;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	private float maxSpeed = 8f;

	private float acceleration = 50f;

	private float currentSpeed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	private Vector3 pos;

	public Rigidbody2D rb;

	public Collider2D coll;

	private bool bothButtons;

	public Color[] hitColors;

	private void Start()
	{
		SQR_playerController = this;
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
			PanAudio();
		}
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit)
		{
			bothButtons = true;
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			animator.SetBool("left", true);
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
				if (currentSpeed < 0f - maxSpeed)
				{
					currentSpeed = 0f - maxSpeed;
				}
			}
		}
		else if (GlobalInput.globalInput.right)
		{
			animator.SetBool("left", false);
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime;
				if (currentSpeed > maxSpeed)
				{
					currentSpeed = maxSpeed;
				}
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
		if (hit)
		{
			tr.Translate(5f * Time.deltaTime, 0f, 0f);
			return;
		}
		tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -4.8125f)
		{
			tr.position = new Vector3(4.8125f, tr.position.y, tr.position.z);
		}
		else if (tr.position.x > 4.8125f)
		{
			tr.position = new Vector3(-4.8125f, tr.position.y, tr.position.z);
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
		skinSprites_s.Add(0, spr_s);
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SQR");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SQR"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SQR"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SQR"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites_s[0][0];
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("enemy"))
		{
			ManageHit(collision);
		}
	}

	public void ManageHit(Collider2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && collision.transform.CompareTag("enemy"))
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				SQR_Reset.SQR_reset.reset = true;
			}
			currentSpeed = 0f;
			hitParticles.transform.position = collision.transform.position;
			hitParticles.SetActive(true);
			collision.GetComponent<Rigidbody2D>().gravityScale = 5f;
			rb.gravityScale = 5f;
			screenShakeRemainingTime = 0.3f;
			coll.enabled = false;
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
			pos = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), renderCameraTR.position.z);
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
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.9f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	public void ChangeToSprite0()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites_s[0][0];
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
			sr_s.sprite = skinSprites_s[0][1];
		}
	}
}
