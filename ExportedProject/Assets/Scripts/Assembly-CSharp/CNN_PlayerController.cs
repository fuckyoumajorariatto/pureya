using System.Collections.Generic;
using UnityEngine;

public class CNN_PlayerController : MonoBehaviour
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

	public Sprite[] spr_s = new Sprite[2];

	public ParticleSystem hitParticles;

	private ParticleSystem.MainModule main;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private Animator animator;

	public Transform bigShipTR;

	private float maxSpeed = 5.5f;

	private float acceleration = 250f;

	private float currentSpeed;

	private float deceleration = 150f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	public SpriteRenderer SR;

	public SpriteRenderer SR_s;

	public Transform hitParticlesTR;

	public Collider2D coll;

	private Vector3 pos;

	private bool bothButtons;

	private void Start()
	{
		animator = GetComponent<Animator>();
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
			if (currentSpeed != 0f && !hit)
			{
				MovePlayer();
				MoveBigShip();
			}
			ReduceSpeed();
			if (hit)
			{
				tr.Translate(0f, -5f * Time.deltaTime, 0f);
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("CNN");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["CNN"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["CNN"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["CNN"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			SR.sprite = null;
			SR_s.sprite = spr_s[0];
		}
		else
		{
			SR.sprite = skinSprites[skin][0];
			SR_s.sprite = spr_s[0];
		}
		main = hitParticles.main;
		ParticleSystem.MinMaxGradient minMaxGradient = default(ParticleSystem.MinMaxGradient);
		switch (skin)
		{
		case 0:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("DDA731FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("894C24FF");
			break;
		case 1:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FF9F9FFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("C92323FF");
			break;
		case 2:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("DDA731FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("894C24FF");
			break;
		case 3:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFCF2EFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("363317FF");
			break;
		case 4:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("B6B6B6FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("363317FF");
			break;
		case 5:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("363317FF");
			break;
		case 6:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("747474FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("181818FF");
			break;
		case 7:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FDF10AFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("081186FF");
			break;
		case 8:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FFF3CCFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("C59A0CFF");
			break;
		case 9:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("1E1E1EFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("1E1E1EFF");
			break;
		default:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("DDA731FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("894C24FF");
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
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		receivingInput = false;
		if (GlobalInput.globalInput.left && GlobalInput.globalInput.right)
		{
			bothButtons = true;
			animator.SetBool("forward", false);
			animator.SetBool("backwards", false);
		}
		else if (GlobalInput.globalInput.left && tr.position.x > -3f)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime * 0.9f;
			}
			animator.SetBool("forward", true);
		}
		else if (GlobalInput.globalInput.right && tr.position.x < 3f)
		{
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime * 1.5f;
			}
			animator.SetBool("backwards", true);
		}
		else if (GlobalInput.globalInput.left)
		{
			receivingInput = true;
			animator.SetBool("forward", true);
		}
		else if (GlobalInput.globalInput.right)
		{
			receivingInput = true;
			animator.SetBool("backwards", true);
		}
		else
		{
			receivingInput = false;
			animator.SetBool("forward", false);
			animator.SetBool("backwards", false);
		}
	}

	private void MovePlayer()
	{
		if (!hit)
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
			if (tr.position.x < -3f)
			{
				tr.position = new Vector3(-3f, tr.position.y, tr.position.z);
			}
			else if (tr.position.x > 3f)
			{
				tr.position = new Vector3(3f, tr.position.y, tr.position.z);
			}
			PlayMoveSound();
		}
	}

	private void MoveBigShip()
	{
		bigShipTR.Translate(currentSpeed / 10f * Time.deltaTime, 0f, 0f);
		if (bigShipTR.position.x < -0.523f)
		{
			bigShipTR.position = new Vector3(-0.523f, bigShipTR.position.y, bigShipTR.position.z);
		}
		else if (bigShipTR.position.x > 0.275f)
		{
			bigShipTR.position = new Vector3(0.275f, bigShipTR.position.y, bigShipTR.position.z);
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble") && !hit)
		{
			ManageCollision();
		}
	}

	private void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				CNN_Reset.CNN_reset.reset = true;
			}
			hitParticlesTR = hitParticles.transform;
			hitParticlesTR.position = new Vector3(tr.position.x, hitParticlesTR.position.y, tr.position.z);
			hitParticles.Play();
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
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			pos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), renderCameraTR.position.z);
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
			if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
			{
				bothButtons = false;
				audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
			}
			else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
			{
				bothButtons = false;
				audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
			}
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ChangeToSprite0()
	{
		if (skin == 9)
		{
			SR.sprite = null;
			SR_s.sprite = spr_s[0];
		}
		else
		{
			SR.sprite = skinSprites[skin][0];
			SR_s.sprite = spr_s[0];
		}
	}

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			SR.sprite = null;
			SR_s.sprite = spr_s[1];
		}
		else
		{
			SR.sprite = skinSprites[skin][1];
			SR_s.sprite = spr_s[1];
		}
	}
}
