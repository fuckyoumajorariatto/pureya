using System;
using UnityEngine;

public class FTL_PlayerController : MonoBehaviour
{
	public static FTL_PlayerController FTL_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_truck = new Sprite[9];

	public Sprite[] sprs_truck_s = new Sprite[9];

	public SpriteRenderer[] laddersSR;

	public SpriteRenderer playerSR;

	public SpriteRenderer truck;

	public SpriteRenderer truck_s;

	public GameObject[] ladders;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	public Transform tr;

	public Collider2D coll;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private float speed = 2f;

	private float acceleration = 50f;

	private float maxSpeed = 13f;

	private float initialY = -1.9375f;

	private float laddersDistance = 0.625f;

	private int ladderIndex;

	private int ladderIndexCalc;

	private float translateY;

	private float translateX;

	private Vector3 pos;

	private bool bothButtons;

	private int skin;

	private void Start()
	{
		FTL_playerController = this;
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
			PanAudio();
		}
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit)
		{
			bothButtons = true;
		}
		else
		{
			if (hit)
			{
				return;
			}
			if (GlobalInput.globalInput.left && tr.position.y <= 3.5625f)
			{
				MovePlayer(true);
				if (speed < maxSpeed)
				{
					speed += acceleration * Time.deltaTime;
				}
			}
			else if (GlobalInput.globalInput.right && tr.position.y >= -1.875f)
			{
				MovePlayer(false);
				if (speed < maxSpeed)
				{
					speed += acceleration * Time.deltaTime;
				}
			}
			else
			{
				speed = 1f;
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

	private void MovePlayer(bool left)
	{
		if (hit)
		{
			return;
		}
		float f = (float)Math.PI * 53f / 360f;
		if (left)
		{
			translateY = (0f - speed) * Time.deltaTime;
			translateX = Mathf.Tan(f) * translateY;
			tr.Translate(translateX, translateY, 0f);
		}
		else
		{
			translateY = speed * Time.deltaTime;
			translateX = Mathf.Tan(f) * translateY;
			tr.Translate(translateX, translateY, 0f);
		}
		if (tr.position.y > 3.5625f)
		{
			tr.position = new Vector3(-0.2264624f, 3.5625f, tr.position.z);
		}
		else if (tr.position.y < -1.875f)
		{
			tr.position = new Vector3(-2.9375f, -1.875f, tr.position.z);
		}
		ladderIndexCalc = 0;
		for (float num = initialY + laddersDistance; num < initialY + laddersDistance * (float)ladders.Length; num += laddersDistance)
		{
			if (tr.position.y >= num)
			{
				ladderIndexCalc++;
			}
		}
		if (ladderIndexCalc != ladderIndex)
		{
			if (ladderIndexCalc > ladderIndex)
			{
				for (int i = ladderIndex; i <= ladderIndexCalc; i++)
				{
					ladders[i].SetActive(true);
				}
			}
			else
			{
				for (int num2 = ladderIndex; num2 > ladderIndexCalc; num2--)
				{
					ladders[num2].SetActive(false);
				}
			}
		}
		ladderIndex = ladderIndexCalc;
	}

	public void ResetLadders()
	{
		for (int i = 1; i < ladders.Length; i++)
		{
			ladders[i].SetActive(false);
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("FTL");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["FTL"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["FTL"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["FTL"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = UnityEngine.Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			playerSR.sprite = null;
			truck.sprite = null;
			truck_s.sprite = sprs_truck_s[0];
			for (int i = 0; i < laddersSR.Length; i++)
			{
				laddersSR[i].sprite = null;
			}
		}
		else
		{
			playerSR.sprite = sprs[skin];
			truck.sprite = sprs_truck[skin];
			truck_s.sprite = sprs_truck_s[skin];
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ManageHit(collision);
	}

	public void ManageHit(Collider2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && collision.transform.CompareTag("enemy"))
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				FTL_Reset.FTL_reset.reset = true;
			}
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
			pos = new Vector3(UnityEngine.Random.Range(-0.15f, 0.15f), UnityEngine.Random.Range(-0.15f, 0.15f), renderCameraTR.position.z);
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
			audioSource = GlobalAudio.globalAudio.Play(movementSound, UnityEngine.Random.Range(0.9f, 1f), UnityEngine.Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, UnityEngine.Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
