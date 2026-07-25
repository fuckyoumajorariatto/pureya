using System.Collections.Generic;
using UnityEngine;

public class MNK_PlayerController : MonoBehaviour
{
	public static MNK_PlayerController MNK_playerController;

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

	public Sprite[] spr_1_s;

	public Sprite[] spr_2_s;

	public GameObject hitParticles;

	public Transform hitParticlesTR;

	public Transform renderCamera;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip movementSound2;

	public AudioClip hitSound;

	private bool armsStretched = true;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public float inputCooldown;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	private float distance;

	public Transform tr;

	public Collider2D coll;

	public Color[] hitColors;

	private void Start()
	{
		MNK_playerController = this;
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
			if (inputCooldown > 0f)
			{
				inputCooldown -= Time.deltaTime;
			}
			if (!hit)
			{
				DieIfOut();
			}
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("MNK");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["MNK"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["MNK"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["MNK"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = spr_1_s[0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = spr_1_s[skin];
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
		if (GlobalInput.globalInput.gameInputActive && (!GlobalInput.globalInput.left || !GlobalInput.globalInput.right))
		{
			if (GlobalInput.globalInput.leftDown || (GlobalInput.globalInput.left && inputCooldown <= 0f))
			{
				MovePlayer(true);
			}
			else if (GlobalInput.globalInput.rightDown || (GlobalInput.globalInput.right && inputCooldown <= 0f))
			{
				MovePlayer(false);
			}
		}
	}

	private void MovePlayer(bool left)
	{
		if (!hit && !(inputCooldown > 0f))
		{
			inputCooldown = 0.11f;
			distance = 0.625f;
			if (left)
			{
				distance *= -1f;
			}
			tr.Translate(distance, 0f, 0f);
			if (armsStretched)
			{
				ChangeToSprite1();
				armsStretched = false;
			}
			else
			{
				ChangeToSprite0();
				armsStretched = true;
			}
			PlayMoveSound();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble") && !hit)
		{
			ManageCollision();
		}
	}

	private void DieIfOut()
	{
		if (tr.position.x > 5f || tr.position.x < -5f)
		{
			ManageCollision();
		}
	}

	private void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				MNK_Reset.MNK_reset.reset = true;
			}
			sr.color = hitColors[skin];
			hitParticlesTR.position = new Vector3(tr.position.x, hitParticlesTR.position.y, tr.position.z);
			hitParticles.SetActive(true);
			screenShakeRemainingTime = 0.4f;
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
			Vector3 position = new Vector3(0f, 0f, renderCamera.position.z);
			renderCamera.position = position;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			Vector3 position2 = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), renderCamera.position.z);
			renderCamera.position = position2;
		}
		else
		{
			Vector3 position3 = new Vector3(0f, 0f, renderCamera.position.z);
			renderCamera.position = position3;
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			if (!armsStretched)
			{
				audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
			}
			else
			{
				audioSource = GlobalAudio.globalAudio.Play(movementSound2, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
			}
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	public void ResetSprite()
	{
		armsStretched = true;
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = spr_1_s[0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = spr_1_s[skin];
		}
	}

	public void ChangeToSprite0()
	{
		if (!armsStretched)
		{
			if (skin == 9)
			{
				sr.sprite = null;
				sr_s.sprite = spr_1_s[0];
			}
			else
			{
				sr.sprite = skinSprites[skin][0];
				sr_s.sprite = spr_1_s[skin];
			}
		}
	}

	private void ChangeToSprite1()
	{
		if (armsStretched)
		{
			if (skin == 9)
			{
				sr.sprite = null;
				sr_s.sprite = spr_2_s[0];
			}
			else
			{
				sr.sprite = skinSprites[skin][1];
				sr_s.sprite = spr_2_s[skin];
			}
		}
	}
}
