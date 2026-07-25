using System.Collections.Generic;
using UnityEngine;

public class SPR_PlayerController : MonoBehaviour
{
	public static SPR_PlayerController SPR_playerController;

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

	public GameObject shadow;

	public GameObject SPRScripter;

	public GameObject playerSprite;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public GameObject thread;

	public ParticleSystem hitParticles;

	private ParticleSystem.MainModule main;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public int hits;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public Transform tr;

	private Vector3 pos;

	public Animator anim;

	public Rigidbody2D rb;

	public Collider2D coll;

	public SPR_NailSpawner nailSpawner;

	public SPR_MarbleSpawner marbleSpawner;

	private bool left;

	private bool leftDown;

	private bool right;

	private bool rightDown;

	private bool bothButtons;

	public SpriteRenderer threadSR;

	public Sprite threadSprite;

	private void Start()
	{
		SPR_playerController = this;
		ChangeSkin();
	}

	private void FixedUpdate()
	{
		if (!PauseController.pauseController.paused)
		{
			ApplyInput();
		}
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
			LimitMovement();
			PanAudio();
			ManageHit();
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SPR");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SPR"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SPR"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SPR"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			threadSR.sprite = null;
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			threadSR.sprite = threadSprite;
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites_s[skin][0];
		}
		main = hitParticles.main;
		ParticleSystem.MinMaxGradient minMaxGradient = default(ParticleSystem.MinMaxGradient);
		switch (skin)
		{
		case 0:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("5A3C26FF");
			break;
		case 1:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("8F2020FF");
			break;
		case 2:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("45145DFF");
			break;
		case 3:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("1C1C1CFF");
			break;
		case 4:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("B4AC00FF");
			break;
		case 5:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("1C64C3FF");
			break;
		case 6:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("01E8DBFF");
			break;
		case 7:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("4CFF76FF");
			break;
		case 8:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("997700FF");
			break;
		case 9:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("151515FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("151515FF");
			break;
		default:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("5A3C26FF");
			break;
		}
		main.startColor = new ParticleSystem.MinMaxGradient(minMaxGradient.colorMin, minMaxGradient.colorMax);
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
		if (GlobalInput.globalInput.gameInputActive && !hit)
		{
			if (GlobalInput.globalInput.left)
			{
				left = true;
			}
			if (GlobalInput.globalInput.right)
			{
				right = true;
			}
			if (GlobalInput.globalInput.leftDown)
			{
				leftDown = true;
			}
			if (GlobalInput.globalInput.rightDown)
			{
				rightDown = true;
			}
		}
	}

	private void ApplyInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		ManageInput();
		tr.localScale = new Vector3(1f, 1f, tr.localScale.z);
		if (leftDown || rightDown)
		{
			PlayMoveSound();
		}
		if (left && right)
		{
			bothButtons = true;
			anim.SetBool("moving", false);
			ResetInput();
			return;
		}
		if (left)
		{
			rb.velocity = new Vector2(rb.velocity.x - 100f * Time.deltaTime, 0f);
			if (tr.position.x <= -2.4f)
			{
				rb.velocity = new Vector2(0f, 0f);
				tr.position = new Vector3(-2.5f, tr.position.y, tr.position.z);
			}
		}
		else if (right)
		{
			rb.velocity = new Vector2(rb.velocity.x + 100f * Time.deltaTime, 0f);
			if (tr.position.x >= 2.4f)
			{
				rb.velocity = new Vector2(0f, 0f);
				tr.position = new Vector3(2.5f, tr.position.y, tr.position.z);
			}
		}
		if (leftDown || rightDown)
		{
			bothButtons = false;
			PlayMoveSound();
		}
		else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
		{
			bothButtons = false;
			PlayMoveSound();
		}
		if (left || right)
		{
			receivingInput = true;
		}
		if (receivingInput && !hit)
		{
			anim.SetBool("moving", true);
		}
		else if (!hit)
		{
			anim.SetBool("moving", false);
		}
		ResetInput();
	}

	private void ResetInput()
	{
		left = false;
		right = false;
		leftDown = false;
		rightDown = false;
		receivingInput = false;
	}

	private void LimitMovement()
	{
		if (tr.position.x >= 2.5f)
		{
			tr.position = new Vector3(2.5f, tr.position.y, tr.position.z);
			rb.velocity = new Vector2(0f, 0f);
		}
		else if (tr.position.x <= -2.5f)
		{
			tr.position = new Vector3(-2.5f, tr.position.y, tr.position.z);
			rb.velocity = new Vector2(0f, 0f);
		}
	}

	private void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && tr.position.y > 5.1f)
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
				SPR_Reset.SPR_reset.reset = true;
			}
			hitParticles.transform.position = new Vector3(tr.position.x, hitParticles.transform.position.y, hitParticles.transform.position.z);
			hitParticles.Play();
			screenShakeRemainingTime = 0.1f;
			PlayHitSound();
			playerSprite.SetActive(false);
			thread.SetActive(false);
			coll.enabled = false;
			nailSpawner.enabled = false;
			marbleSpawner.enabled = false;
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
			pos = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), renderCameraTR.position.z);
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
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
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
			sr_s.sprite = skinSprites_s[skin][0];
		}
	}

	public void ChangeToSprite1()
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
}
