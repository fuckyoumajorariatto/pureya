using UnityEngine;

public class SWO_PlayerController : MonoBehaviour
{
	public static SWO_PlayerController SWO_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public Transform playerSpriteTR;

	public SpriteRenderer playerSpriteSR;

	public SpriteRenderer playerSpriteShadow;

	public Transform renderCameraTR;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public Transform tr;

	public Collider2D coll;

	private float maxSpeed = 12f;

	private float acceleration = 180f;

	private float currentSpeed;

	private float deceleration = 60f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private float initialTimer;

	private bool bothButtons;

	private int skin;

	private void Start()
	{
		SWO_playerController = this;
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
			ReduceSpeed();
			initialTimer += Time.deltaTime;
		}
	}

	private void ManageInput()
	{
		receivingInput = false;
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit || initialTimer < 0.5f)
		{
			bothButtons = true;
			return;
		}
		if (GlobalInput.globalInput.left && base.transform.position.y >= -3.75f)
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
		else if (GlobalInput.globalInput.right && base.transform.position.y <= 3.75f)
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

	private void TiltPlayer(bool left)
	{
		if (tr.position.y == 3.75f && tr.rotation.eulerAngles.z == 0f && left)
		{
			tr.Rotate(Vector3.forward * 20f);
			return;
		}
		if (tr.position.y == -3.75f && tr.rotation.eulerAngles.z == 0f && !left)
		{
			tr.Rotate(Vector3.forward * -20f);
			return;
		}
		tr.Rotate(Vector3.forward * (0f - currentSpeed) * Time.deltaTime * 40f);
		if (tr.rotation.eulerAngles.z > 90f && tr.rotation.eulerAngles.z < 180f)
		{
			tr.rotation = Quaternion.Euler(tr.rotation.x, tr.rotation.y, 90f);
		}
		else if (tr.rotation.eulerAngles.z < 270f && tr.rotation.eulerAngles.z > 180f)
		{
			tr.rotation = Quaternion.Euler(tr.rotation.x, tr.rotation.y, 270f);
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
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SWO");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SWO"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SWO"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SWO"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			playerSpriteSR.sprite = null;
			playerSpriteShadow.sprite = sprs_s[0];
		}
		else
		{
			playerSpriteSR.sprite = sprs[skin];
			playerSpriteShadow.sprite = sprs_s[skin];
		}
	}

	public void ManageHit(Collider2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && collision.transform.CompareTag("enemy"))
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				SWO_Reset.SWO_reset.reset = true;
			}
			screenShakeRemainingTime = 0.3f;
			PlayHitSound();
			coll.enabled = false;
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
			Vector3 position = new Vector3(0f, 0f, renderCameraTR.transform.position.z);
			renderCameraTR.transform.position = position;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			Vector3 position2 = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), renderCameraTR.transform.position.z);
			renderCameraTR.transform.position = position2;
		}
		else
		{
			Vector3 position3 = new Vector3(0f, 0f, renderCameraTR.transform.position.z);
			renderCameraTR.transform.position = position3;
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
