using UnityEngine;

public class PPR_PlayerController : MonoBehaviour
{
	public static PPR_PlayerController PPR_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public SpriteRenderer playerSprite;

	public SpriteRenderer playerSpriteShadow;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private float maxSpeed = 2f;

	private float acceleration = 200f;

	private float currentSpeed;

	private float deceleration = 60f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	public Transform playerSpriteTR;

	public Rigidbody2D playerSpriteRB;

	public Transform marblesColliderTR;

	public Rigidbody2D marblesColliderRB;

	public Collider2D marblesCollider;

	public Collider2D tipCollider;

	private ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private bool bothButtons;

	private int skin;

	private void Start()
	{
		PPR_playerController = this;
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
			AlignMarblesCollider();
			ReduceSpeed();
		}
	}

	private void ManageInput()
	{
		receivingInput = false;
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right))
		{
			bothButtons = true;
		}
		else
		{
			if (hit)
			{
				return;
			}
			if (GlobalInput.globalInput.left && tr.position.y >= -3.75f)
			{
				if (currentSpeed > 0f - maxSpeed)
				{
					if (currentSpeed > 0f)
					{
						currentSpeed = 0f;
					}
					currentSpeed -= acceleration * Time.deltaTime;
					if (currentSpeed < 0f - maxSpeed)
					{
						currentSpeed = 0f - maxSpeed;
					}
				}
				TiltPlayer(true);
			}
			else if (GlobalInput.globalInput.right && tr.position.y <= 3.75f)
			{
				if (currentSpeed < maxSpeed)
				{
					if (currentSpeed < 0f)
					{
						currentSpeed = 0f;
					}
					currentSpeed += acceleration * Time.deltaTime;
					if (currentSpeed > maxSpeed)
					{
						currentSpeed = maxSpeed;
					}
				}
				TiltPlayer(false);
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
			if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
			{
				receivingInput = true;
			}
		}
	}

	private void MovePlayer()
	{
		if (hit)
		{
			tr.Translate(5f * Time.deltaTime, 0f, 0f);
			playerSpriteTR.Rotate(Vector3.forward * 30f * Time.deltaTime * 30f);
			return;
		}
		if (playerSpriteTR.rotation.eulerAngles.z <= 46f && playerSpriteTR.rotation.eulerAngles.z < 180f)
		{
			float num = playerSpriteTR.rotation.eulerAngles.z / 3f;
			tr.Translate(0f, num * Time.deltaTime, 0f);
		}
		else if (playerSpriteTR.rotation.eulerAngles.z >= 314f && playerSpriteTR.rotation.eulerAngles.z > 180f)
		{
			float num2 = (playerSpriteTR.rotation.eulerAngles.z - 360f) / 3f;
			tr.Translate(0f, num2 * Time.deltaTime, 0f);
		}
		if (tr.position.y > 3.75f)
		{
			tr.position = new Vector3(tr.position.x, 3.75f, tr.position.z);
		}
		else if (tr.position.y < -3.75f)
		{
			tr.position = new Vector3(tr.position.x, -3.75f, tr.position.z);
		}
		if (tr.position.y == 3.75f && playerSpriteTR.rotation.eulerAngles.z != 0f)
		{
			UntiltPlayer();
		}
		else if (tr.position.y == -3.75f && playerSpriteTR.rotation.eulerAngles.z != 0f)
		{
			UntiltPlayer();
		}
	}

	private void TiltPlayer(bool left)
	{
		if (tr.position.y == 3.75f && playerSpriteTR.rotation.eulerAngles.z == 0f && GlobalInput.globalInput.leftDown)
		{
			playerSpriteTR.Rotate(Vector3.forward * 20f);
			return;
		}
		if (tr.position.y == -3.75f && playerSpriteTR.rotation.eulerAngles.z == 0f && GlobalInput.globalInput.rightDown)
		{
			playerSpriteTR.Rotate(Vector3.forward * -20f);
			return;
		}
		playerSpriteTR.Rotate(Vector3.forward * (0f - currentSpeed) * Time.deltaTime * 40f);
		if (playerSpriteTR.rotation.eulerAngles.z > 45f && playerSpriteTR.rotation.eulerAngles.z < 180f)
		{
			playerSpriteTR.rotation = Quaternion.Euler(playerSpriteTR.rotation.x, playerSpriteTR.rotation.y, 45f);
		}
		else if (playerSpriteTR.rotation.eulerAngles.z < 315f && playerSpriteTR.rotation.eulerAngles.z > 180f)
		{
			playerSpriteTR.rotation = Quaternion.Euler(playerSpriteTR.rotation.x, playerSpriteTR.rotation.y, 315f);
		}
	}

	private void UntiltPlayer()
	{
		currentSpeed = 0f;
		if (playerSpriteTR.rotation.eulerAngles.z > 0f && playerSpriteTR.rotation.eulerAngles.z < 180f)
		{
			playerSpriteTR.Rotate(Vector3.forward * -10f * Time.deltaTime * 40f);
			if (playerSpriteTR.rotation.eulerAngles.z > 180f)
			{
				playerSpriteTR.rotation = Quaternion.Euler(playerSpriteTR.rotation.x, playerSpriteTR.rotation.y, 0f);
			}
		}
		else if (playerSpriteTR.rotation.eulerAngles.z > 0f && playerSpriteTR.rotation.eulerAngles.z > 180f)
		{
			playerSpriteTR.Rotate(Vector3.forward * 10f * Time.deltaTime * 40f);
			if (playerSpriteTR.rotation.eulerAngles.z < 180f)
			{
				playerSpriteTR.rotation = Quaternion.Euler(playerSpriteTR.rotation.x, playerSpriteTR.rotation.y, 0f);
			}
		}
	}

	private void AlignMarblesCollider()
	{
		marblesColliderTR.localPosition = new Vector3(0f, 0f, playerSpriteTR.localPosition.z);
		marblesColliderRB.velocity = new Vector2(0f, 0f);
		marblesColliderRB.angularVelocity = 0f;
		marblesColliderTR.localRotation = Quaternion.Euler(0f, 0f, 0f);
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

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("PPR");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["PPR"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["PPR"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["PPR"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			playerSprite.sprite = null;
			playerSpriteShadow.sprite = sprs_s[0];
		}
		else
		{
			playerSprite.sprite = sprs[skin];
			playerSpriteShadow.sprite = sprs_s[skin];
		}
	}

	public void ManageHit(Collider2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && collision.transform.CompareTag("enemy"))
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				PPR_Reset.PPR_reset.reset = true;
			}
			marblesCollider.enabled = false;
			tipCollider.enabled = false;
			hitParticles.transform.position = collision.transform.position;
			ps = hitParticles.GetComponent<ParticleSystem>();
			main = ps.main;
			main.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
			hitParticles.SetActive(true);
			collision.gameObject.SetActive(false);
			playerSpriteRB.gravityScale = 3f;
			screenShakeRemainingTime = 0.2f;
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
			Vector3 position = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = position;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			Vector3 position2 = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), renderCameraTR.position.z);
			renderCameraTR.position = position2;
		}
		else
		{
			Vector3 position3 = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = position3;
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.9f, 1f), Random.Range(0.9f, 1.1f), 0f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), 0f);
	}
}
