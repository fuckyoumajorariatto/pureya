using UnityEngine;

public class CBL_PlayerController : MonoBehaviour
{
	public static CBL_PlayerController CBL_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public SpriteRenderer shadow;

	public GameObject playerSprite;

	public Transform playerSpriteTR;

	public SpriteRenderer playerSpriteSR;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private float maxSpeed = 13f;

	private float acceleration = 50f;

	public float currentSpeed;

	private float deceleration = 80f;

	private bool receivingInput;

	private float scrollSpeed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Collider2D coll1;

	public PolygonCollider2D coll2;

	public Transform tr;

	private Vector3 pos;

	public GameObject playerSpritePrefab;

	private Quaternion playerSpriteRotation;

	private int callResetColliders;

	private int[] skins;

	private int lastUnlockedSkin;

	private int skin;

	private bool bothButtons;

	public Color[] hitColors;

	private void Start()
	{
		CBL_playerController = this;
		SetDifficultyParas();
		ChangeSkin();
	}

	private void Update()
	{
		ManageInput();
		MovePlayer();
		ReduceSpeed();
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		PanAudio();
		if (callResetColliders > 0)
		{
			callResetColliders--;
			if (callResetColliders == 0)
			{
				ResetColliders();
			}
		}
	}

	public void SetDifficultyParas()
	{
		scrollSpeed = CBL_Difficulty.CBL_difficulty.scrollSpeed;
	}

	private void ChangeSkin()
	{
		skins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("CBL");
		if (skins.Length == 0)
		{
			skin = 9;
		}
		lastUnlockedSkin = GlobalControl.globalControl.lastUnlockedSkins["CBL"];
		if (lastUnlockedSkin != -1 && !GlobalControl.globalControl.enabledSkins["CBL"][lastUnlockedSkin])
		{
			lastUnlockedSkin = -1;
		}
		if (lastUnlockedSkin != -1)
		{
			skin = lastUnlockedSkin;
			GlobalControl.globalControl.lastUnlockedSkins["CBL"] = -1;
		}
		else if (skin != 9)
		{
			int num = Random.Range(0, skins.Length);
			skin = skins[num];
		}
		if (skin == 9)
		{
			playerSpriteSR.sprite = null;
			shadow.sprite = sprs_s[0];
		}
		else
		{
			playerSpriteSR.sprite = sprs[skin];
			shadow.sprite = sprs_s[skin];
		}
	}

	private void ApplySkin()
	{
		if (skin == 9)
		{
			playerSpriteSR.sprite = null;
			shadow.sprite = sprs_s[0];
		}
		else
		{
			playerSpriteSR.sprite = sprs[skin];
			shadow.sprite = sprs_s[skin];
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
			UntiltPlayer();
			return;
		}
		if (GlobalInput.globalInput.left && tr.position.x > -4f)
		{
			if (currentSpeed > 0f)
			{
				currentSpeed = 0f;
			}
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
			}
			TiltPlayer(true);
		}
		else if (GlobalInput.globalInput.right && tr.position.x < 4f)
		{
			if (currentSpeed < 0f)
			{
				currentSpeed = 0f;
			}
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration * Time.deltaTime;
			}
			TiltPlayer(false);
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			receivingInput = true;
		}
		else
		{
			UntiltPlayer();
		}
	}

	private void MovePlayer()
	{
		if (!GlobalInput.globalInput.gameInputActive)
		{
			return;
		}
		if (hit)
		{
			tr.Translate(0f, (0f - scrollSpeed) * Time.deltaTime, 0f);
			return;
		}
		if (tr.position.x < -3.1f || tr.position.x + currentSpeed * Time.deltaTime < -3.1f)
		{
			tr.position = new Vector3(-3.1f, tr.position.y, tr.position.z);
		}
		else if (tr.position.x > 3.1f || tr.position.x + currentSpeed * Time.deltaTime > 3.1f)
		{
			tr.position = new Vector3(3.1f, tr.position.y, tr.position.z);
		}
		else
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
		}
		PlayMoveSound();
	}

	private void TiltPlayer(bool left)
	{
		if (left && playerSpriteTR.rotation.z < 0.1f)
		{
			if (playerSpriteTR.rotation.z < 0f)
			{
				UntiltPlayer();
			}
			playerSpriteTR.Rotate(Vector3.forward * (0f - currentSpeed) * Time.deltaTime * 10f);
		}
		else if (!left && playerSpriteTR.rotation.z > -0.1f)
		{
			if (playerSpriteTR.rotation.z > 0f)
			{
				UntiltPlayer();
			}
			playerSpriteTR.Rotate(Vector3.back * currentSpeed * Time.deltaTime * 10f);
		}
	}

	public void UntiltPlayer()
	{
		playerSpriteTR.Rotate(Vector3.forward * 0f);
		if (playerSpriteTR.rotation.z < 0f)
		{
			playerSpriteTR.Rotate(Vector3.forward * (0f - playerSpriteTR.rotation.z) * Time.fixedDeltaTime * 2000f);
		}
		else if (playerSprite.transform.rotation.z > 0f)
		{
			playerSpriteTR.Rotate(Vector3.back * playerSpriteTR.rotation.z * Time.fixedDeltaTime * 2000f);
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

	public void ManageHit()
	{
		if (hit || !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			callResetColliders = 0;
			if (MinigamesController.minigamesController.Hit())
			{
				CBL_Reset.CBL_reset.reset = true;
			}
			playerSpriteSR.color = hitColors[skin];
			hitParticles.transform.position = tr.position;
			hitParticles.SetActive(true);
			screenShakeRemainingTime = 0.2f;
			coll1.enabled = false;
			coll2.enabled = false;
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
			Vector3 position2 = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), renderCameraTR.position.z);
			renderCameraTR.position = position2;
		}
		else
		{
			Vector3 position3 = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = position3;
		}
	}

	private void PanAudio()
	{
		if (audioSource != null)
		{
			if (audioSource.isPlaying && (audioSource.clip == movementSound || audioSource.clip == hitSound))
			{
				audioSource.panStereo = base.transform.position.x / 4f;
			}
			else
			{
				audioSource = null;
			}
		}
	}

	public void CallResetColliders()
	{
		callResetColliders = 3;
	}

	private void ResetColliders()
	{
		playerSpriteRotation = playerSpriteTR.rotation;
		Object.Destroy(playerSprite);
		playerSprite = Object.Instantiate(playerSpritePrefab, tr.position, playerSpriteRotation);
		playerSprite.transform.parent = tr;
		coll1 = playerSprite.GetComponent<CapsuleCollider2D>();
		coll2 = playerSprite.GetComponent<PolygonCollider2D>();
		playerSpriteTR = playerSprite.transform;
		playerSpriteSR = playerSprite.GetComponent<SpriteRenderer>();
		shadow = playerSprite.GetComponent<CBL_PlayerColliderController>().shadow;
		ApplySkin();
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
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
