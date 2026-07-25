using System.Collections.Generic;
using UnityEngine;

public class SNW_PlayerController : MonoBehaviour
{
	public static SNW_PlayerController SNW_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Transform playerSpriteTR;

	public SpriteRenderer playerSpriteSR;

	public Collider2D coll;

	public Transform bgParticles1TR;

	public Transform bgParticles2TR;

	public GameObject hitParticles;

	public GameObject trailParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private float maxSpeed = 10f;

	private float acceleration = 80f;

	private float currentSpeed;

	private float deceleration = 60f;

	private bool receivingInput;

	private float scrollSpeed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	private List<GameObject> marbles;

	private List<GameObject> objects;

	private Vector3 pos;

	private bool bothButtons;

	public Color[] hitColors;

	private int skin;

	private void Start()
	{
		SNW_playerController = this;
		SetDifficultyParas();
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
			if (currentSpeed != 0f)
			{
				MovePlayer();
			}
			ReduceSpeed();
			MovePlayerIfHit();
			PanAudio();
		}
	}

	public void SetDifficultyParas()
	{
		scrollSpeed = 7f;
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SNW");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SNW"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SNW"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SNW"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			playerSpriteSR.sprite = null;
		}
		else
		{
			playerSpriteSR.sprite = sprs[skin];
		}
	}

	private void ManageInput()
	{
		receivingInput = false;
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit)
		{
			bothButtons = true;
			UntiltPlayer();
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			if (currentSpeed > 0f - maxSpeed)
			{
				currentSpeed -= acceleration * Time.deltaTime;
			}
			TiltPlayer(true);
		}
		else if (GlobalInput.globalInput.right)
		{
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
			currentSpeed = 0f;
			UntiltPlayer();
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

	private void MovePlayer()
	{
		if (hit)
		{
			tr.Translate(0f, scrollSpeed * Time.deltaTime, 0f);
			return;
		}
		MoveItems();
		MoveParticles();
	}

	private void MovePlayerIfHit()
	{
		if (hit)
		{
			tr.Translate(0f, SNW_Difficulty.SNW_difficulty.scroll_speed * Time.deltaTime, 0f);
		}
	}

	private void MoveParticles()
	{
		bgParticles1TR.Translate(new Vector3((0f - currentSpeed) * 1.1f * Time.deltaTime, 0f, 0f));
		bgParticles2TR.Translate(new Vector3((0f - currentSpeed) * 1.1f * Time.deltaTime, 0f, 0f));
		if (bgParticles1TR.position.x > 12f)
		{
			bgParticles1TR.position = new Vector3(bgParticles1TR.position.x - 24f, bgParticles1TR.position.y, bgParticles1TR.position.z);
		}
		else if (bgParticles1TR.position.x < -12f)
		{
			bgParticles1TR.position = new Vector3(bgParticles1TR.position.x + 24f, bgParticles1TR.position.y, bgParticles1TR.position.z);
		}
		else if (bgParticles2TR.position.x > 12f)
		{
			bgParticles2TR.position = new Vector3(bgParticles2TR.position.x - 24f, bgParticles2TR.position.y, bgParticles2TR.position.z);
		}
		else if (bgParticles2TR.position.x < -12f)
		{
			bgParticles2TR.position = new Vector3(bgParticles2TR.position.x + 24f, bgParticles2TR.position.y, bgParticles2TR.position.z);
		}
	}

	private void MoveItems()
	{
		if (!(SNW_MarbleSpawner.SNW_marbleSpawner != null))
		{
			return;
		}
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marblePooledAmount; i++)
		{
			if (marbles[i].activeInHierarchy)
			{
				marbles[i].transform.Translate(new Vector3((0f - currentSpeed) * Time.deltaTime, 0f, 0f));
			}
		}
		objects = SNW_RockCreator.SNW_rockCreator.rocks;
		for (int j = 0; j < SNW_RockCreator.SNW_rockCreator.pooledAmount; j++)
		{
			if (objects[j].activeInHierarchy)
			{
				objects[j].transform.Translate(new Vector3((0f - currentSpeed) * Time.deltaTime, 0f, 0f));
			}
		}
	}

	private void TiltPlayer(bool left)
	{
		if (left && playerSpriteTR.rotation.z > -0.2f)
		{
			if (playerSpriteTR.rotation.z > 0f)
			{
				UntiltPlayer();
			}
			playerSpriteTR.Rotate(Vector3.forward * currentSpeed * Time.deltaTime * 20f);
		}
		else if (!left && playerSpriteTR.rotation.z < 0.2f)
		{
			if (playerSpriteTR.rotation.z < 0f)
			{
				UntiltPlayer();
			}
			playerSpriteTR.Rotate(Vector3.back * (0f - currentSpeed) * Time.deltaTime * 20f);
		}
	}

	private void UntiltPlayer()
	{
		playerSpriteTR.Rotate(Vector3.forward * 0f);
		if (playerSpriteTR.rotation.z < 0f)
		{
			playerSpriteTR.Rotate(Vector3.forward * (0f - playerSpriteTR.rotation.z) * Time.fixedDeltaTime * 2000f);
		}
		else if (playerSpriteTR.rotation.z > 0f)
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
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				SNW_Reset.SNW_reset.reset = true;
			}
			playerSpriteSR.color = hitColors[skin];
			hitParticles.transform.position = tr.position;
			hitParticles.SetActive(true);
			trailParticles.SetActive(false);
			screenShakeRemainingTime = 0.3f;
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
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		audioSource = GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
