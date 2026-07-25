using System.Collections.Generic;
using UnityEngine;

public class FRO_PlayerController : MonoBehaviour
{
	public static FRO_PlayerController FRO_playerController;

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

	public AudioClip hitSound;

	public AudioClip jumpSound;

	public AudioClip landSound;

	public AudioClip invalidSound;

	public Transform splashTR;

	public Animator splashAnim;

	public SpriteRenderer SR;

	public SpriteRenderer SR_s;

	private float scrollSpeed;

	private float hitScrollSpeed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private bool jumping;

	private Transform targetLilypad;

	private Vector2 previousPlayerPos;

	private float jumpTimer;

	public Transform tr;

	public Collider2D coll;

	private Vector3 pos;

	private float shakeFactor;

	private float timer;

	private void Start()
	{
		FRO_playerController = this;
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
			MovePlayer();
			if (hit)
			{
				tr.Translate(0f, -5f * Time.deltaTime, 0f);
			}
			timer += Time.deltaTime;
		}
	}

	public void SetDifficultyParas()
	{
		scrollSpeed = FRO_Difficulty.FRO_difficulty.speed;
		hitScrollSpeed = FRO_Difficulty.FRO_difficulty.speed * 2f;
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("FRO");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["FRO"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["FRO"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["FRO"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
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
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit || timer < 0.5f)
		{
			return;
		}
		if (GlobalInput.globalInput.leftDown && !jumping)
		{
			if (tr.position.x != -3.125f)
			{
				Jump(true);
				return;
			}
			shakeFactor = 0.1f;
			screenShakeRemainingTime = 0.1f;
			InvalidMovement();
		}
		else if (GlobalInput.globalInput.rightDown && !jumping)
		{
			if (tr.position.x != 3.125f)
			{
				Jump(false);
				return;
			}
			shakeFactor = 0.1f;
			screenShakeRemainingTime = 0.1f;
			InvalidMovement();
		}
	}

	private void Jump(bool left)
	{
		if (!hit)
		{
			targetLilypad = FRO_LilypadSpawner.FRO_lilypadSpawner.GetTargetLilypad(tr, left);
			if (targetLilypad != null)
			{
				previousPlayerPos = tr.position;
				jumpTimer = 0f;
				jumping = true;
				ChangeToSprite1();
				PlayJump();
			}
			else
			{
				shakeFactor = 0.1f;
				screenShakeRemainingTime = 0.1f;
				InvalidMovement();
			}
		}
	}

	private void MovePlayer()
	{
		if (hit)
		{
			jumping = false;
			splashTR.Translate(0f, (0f - hitScrollSpeed) * Time.deltaTime, 0f);
		}
		else if (!jumping)
		{
			tr.Translate(0f, (0f - scrollSpeed) * Time.deltaTime, 0f);
		}
		else
		{
			float x = Mathf.Lerp(previousPlayerPos.x, targetLilypad.position.x, jumpTimer);
			float y = Mathf.Lerp(previousPlayerPos.y, targetLilypad.position.y, jumpTimer);
			Vector3 position = new Vector3(x, y, tr.position.z);
			tr.position = position;
			if (jumpTimer >= 1f)
			{
				jumping = false;
				ChangeToSprite0();
				PlayLandSound();
			}
			else
			{
				jumpTimer += Time.deltaTime * 5f;
			}
		}
		if (!hit && (tr.position.y > 5f || tr.position.y < -6f))
		{
			tr.Translate(0f, 1f, 0f);
			ManageHit();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!hit)
		{
			ManageHit();
		}
	}

	private void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				FRO_Reset.FRO_reset.reset = true;
			}
			SR.enabled = false;
			SR_s.enabled = false;
			splashTR.position = tr.position;
			splashAnim.SetTrigger("splash");
			shakeFactor = 0.25f;
			screenShakeRemainingTime = 0.25f;
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
			pos = new Vector3(Random.Range(shakeFactor * -1f, shakeFactor), Random.Range(shakeFactor * -1f, shakeFactor), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PlayLandSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(landSound, 1f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
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
			GlobalAudio.globalAudio.Play(jumpSound, 1f, Random.Range(0.85f, 1.15f), tr.position.x / 4f);
		}
	}

	private void InvalidMovement()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(invalidSound, 1f, 1f, 0f);
		}
	}

	public void ChangeToSprite0()
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
