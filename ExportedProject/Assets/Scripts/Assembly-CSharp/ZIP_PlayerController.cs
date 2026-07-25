using UnityEngine;

public class ZIP_PlayerController : MonoBehaviour
{
	public static ZIP_PlayerController ZIP_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public Transform lineTR;

	public Transform hookTR;

	public Transform hookJointTR;

	public Rigidbody2D hookJointRB;

	public GameObject playerSprite;

	public GameObject playerSpriteShadow;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private float maxSpeed = 0.5f;

	private float acceleration = 1.2f;

	private float currentSpeed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private Vector3 pos;

	public Transform tr;

	public Collider2D coll;

	public Rigidbody2D rb;

	private bool bothButtons;

	public Color[] hitColors;

	private int skin;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public SpriteRenderer hookSR;

	public Sprite hookSprite;

	private float strightenSpeed = 8f;

	private void Start()
	{
		ZIP_playerController = this;
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
			RelocateHook();
			ManageInput();
			LimitVelocity();
			PanAudio();
			RelocateHook();
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
			if (!(lineTR.eulerAngles.z > 340f) && !(lineTR.eulerAngles.z < 3f))
			{
				return;
			}
			if (currentSpeed < maxSpeed)
			{
				if (currentSpeed < 0f)
				{
					currentSpeed = 0f;
				}
				currentSpeed += acceleration * Time.deltaTime;
			}
			TiltLine();
			return;
		}
		if (GlobalInput.globalInput.right)
		{
			if (!(lineTR.eulerAngles.z > 357f) && !(lineTR.eulerAngles.z < 20f))
			{
				return;
			}
			if (currentSpeed > maxSpeed * -1f)
			{
				if (currentSpeed > 0f)
				{
					currentSpeed = 0f;
				}
				currentSpeed -= acceleration * Time.deltaTime;
			}
			TiltLine();
			return;
		}
		currentSpeed = 0f;
		if (lineTR.eulerAngles.z < 340f && lineTR.eulerAngles.z > 0f)
		{
			if (lineTR.eulerAngles.z < Time.deltaTime * strightenSpeed)
			{
				lineTR.Rotate(0f, 0f, 0f - lineTR.eulerAngles.z);
			}
			else
			{
				lineTR.Rotate(0f, 0f, Time.deltaTime * (0f - strightenSpeed));
			}
		}
		else if (lineTR.eulerAngles.z > 340f && lineTR.eulerAngles.z > 0f)
		{
			if (lineTR.eulerAngles.z > 360f - Time.deltaTime * strightenSpeed)
			{
				lineTR.Rotate(0f, 0f, 360f - lineTR.eulerAngles.z);
			}
			else
			{
				lineTR.Rotate(0f, 0f, Time.deltaTime * strightenSpeed);
			}
		}
	}

	private void TiltLine()
	{
		if (!hit)
		{
			lineTR.Rotate(0f, 0f, currentSpeed);
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

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("ZIP");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["ZIP"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["ZIP"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["ZIP"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			hookSR.sprite = null;
			sr.sprite = null;
			sr_s.sprite = sprs_s[0];
		}
		else
		{
			hookSR.sprite = hookSprite;
			sr.sprite = sprs[skin];
			sr_s.sprite = sprs_s[skin];
		}
	}

	private void RelocateHook()
	{
		if (!hit)
		{
			hookTR.position = new Vector3(hookJointTR.position.x, hookJointTR.position.y - 0.125f, hookTR.position.z);
		}
		else
		{
			hookTR.Translate(0f, -3f * Time.deltaTime, 0f);
		}
	}

	private void LimitVelocity()
	{
		if (hookJointRB.velocity.x > 5f)
		{
			hookJointRB.velocity = new Vector2(5f, hookJointRB.velocity.y);
		}
		else if (hookJointRB.velocity.x < -5f)
		{
			hookJointRB.velocity = new Vector2(-5f, hookJointRB.velocity.y);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ManageHit(collision);
	}

	public void ManageHit(Collider2D collision)
	{
		if (!SceneChangeController.sceneChangeController.transitioning && collision.transform.CompareTag("enemy") && !hit)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				ZIP_Reset.ZIP_reset.reset = true;
			}
			sr.color = hitColors[skin];
			hitParticles.transform.position = collision.transform.position;
			hitParticles.SetActive(true);
			screenShakeRemainingTime = 0.1f;
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
			audioSource = GlobalAudio.globalAudio.Play(movementSound, 0.6f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.8f, 1.1f), tr.position.x / 4f);
	}
}
