using UnityEngine;

public class BAL_PlayerController : MonoBehaviour
{
	public static BAL_PlayerController BAL_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite sprs_s;

	public GameObject shadow;

	public ParticleSystem hitParticles;

	private ParticleSystem.MainModule main;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public AudioClip bounce;

	private Animator animator;

	public Transform tr;

	public Collider2D coll;

	public SpriteRenderer SR;

	public SpriteRenderer SR_s;

	private Vector3 pos;

	private int skin;

	private float maxSpeed = 6f;

	private float acceleration = 250f;

	private float currentSpeed;

	private float deceleration = 150f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public bool firstBounce = true;

	private bool bothButtons;

	private float doubleMoveSoundCooldown;

	private void Start()
	{
		BAL_playerController = this;
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
			}
			ReduceSpeed();
			if (hit)
			{
				base.transform.Translate(0f, -5f * Time.deltaTime, 0f);
			}
			if (doubleMoveSoundCooldown > 0f)
			{
				doubleMoveSoundCooldown -= Time.deltaTime;
			}
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("BAL");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["BAL"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["BAL"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["BAL"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			SR.sprite = null;
		}
		else
		{
			SR.sprite = sprs[skin];
		}
		main = hitParticles.main;
		ParticleSystem.MinMaxGradient minMaxGradient = default(ParticleSystem.MinMaxGradient);
		switch (skin)
		{
		case 0:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FF78A7FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 1:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FF78A7FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 2:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("ff3030ff");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("25c800ff");
			break;
		case 3:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("bdfb01FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 4:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("ff6666ff");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 5:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("fe8306ff");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 6:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("ffffffff");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 7:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("0196f4ff");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 8:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("00000000");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
			break;
		case 9:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("3D3D3DFF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("3D3D3DFF");
			break;
		default:
			minMaxGradient.colorMin = GlobalControl.globalControl.HexToColor("FF78A7FF");
			minMaxGradient.colorMax = GlobalControl.globalControl.HexToColor("FFFFFFFF");
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
		if (GlobalInput.globalInput.left && GlobalInput.globalInput.right)
		{
			bothButtons = true;
			if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
			{
				doubleMoveSoundCooldown = 0.25f;
				PlayMoveSound();
			}
			return;
		}
		receivingInput = false;
		if (GlobalInput.globalInput.left && tr.position.x > -4.3125f)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime * 0.9f;
			}
		}
		else if (GlobalInput.globalInput.right && tr.position.x < 4.3125f)
		{
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime * 1.5f;
			}
		}
		else if (GlobalInput.globalInput.left)
		{
			receivingInput = true;
		}
		else if (GlobalInput.globalInput.right)
		{
			receivingInput = true;
		}
		else
		{
			receivingInput = false;
		}
		if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
		{
			bothButtons = false;
			PlayMoveSound();
		}
		if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
		{
			bothButtons = false;
			if (!(doubleMoveSoundCooldown > 0f))
			{
				PlayMoveSound();
			}
		}
	}

	private void MovePlayer()
	{
		if (!hit)
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
			if (tr.position.x < -4.3125f)
			{
				tr.position = new Vector3(-4.3125f, tr.position.y, tr.position.z);
			}
			else if (tr.position.x > 4.3125f)
			{
				tr.position = new Vector3(4.3125f, tr.position.y, tr.position.z);
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble") && !collision.transform.CompareTag("wall") && !hit)
		{
			ManageCollision();
		}
		else if (collision.transform.CompareTag("wall") && !hit)
		{
			ManageBounce();
		}
	}

	private void ManageBounce()
	{
		animator.SetTrigger("bounce");
		PlayBounce();
	}

	private void PlayBounce()
	{
		audioSource = GlobalAudio.globalAudio.Play(bounce, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (MinigamesController.minigamesController.Hit())
			{
				BAL_Reset.BAL_reset.reset = true;
			}
			hit = true;
			hitParticles.transform.position = tr.position;
			hitParticles.Play();
			SR.enabled = false;
			SR_s.enabled = false;
			coll.enabled = false;
			screenShakeRemainingTime = 0.1f;
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
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
