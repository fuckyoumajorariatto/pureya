using UnityEngine;

public class BEE_PlayerController : MonoBehaviour
{
	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public Transform tr;

	public Transform referenceTR;

	public Collider2D coll;

	public Rigidbody2D rb;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public Transform renderCameraTR;

	private float speed = 200f;

	private float scrollSpeed;

	public bool hit;

	private float initialTimer;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private AudioSource audioSource;

	private float screenShakeRemainingTime;

	public int hits;

	private int skin;

	private Vector3 pos;

	private Transform plantTR;

	public BEE_Reset BEE_reset;

	public GameObject hitParticles;

	private bool bothButtons;

	private void Start()
	{
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
			initialTimer += Time.deltaTime;
			ManageInput();
			ManegeBounds();
			if ((GlobalInput.globalInput.left || GlobalInput.globalInput.right) && (!GlobalInput.globalInput.left || !GlobalInput.globalInput.right))
			{
				tr.Translate(0f, (0f - scrollSpeed) * Time.deltaTime, 0f, Space.World);
			}
			else
			{
				tr.Translate(0f, (0f - scrollSpeed) / 2f * Time.deltaTime, 0f, Space.World);
			}
		}
	}

	private void FixedUpdate()
	{
		if (hit && plantTR != null)
		{
			tr.position = new Vector3(plantTR.position.x, plantTR.position.y, tr.position.z);
		}
		else
		{
			rb.velocity = tr.up * speed * Time.deltaTime;
		}
	}

	public void UpdateDifficulty()
	{
		scrollSpeed = BEE_Difficulty.BEE_difficulty.speed;
		speed = 200f;
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit || initialTimer < 0.5f)
		{
			bothButtons = true;
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			tr.Rotate(Vector3.forward * 5f);
		}
		else if (GlobalInput.globalInput.right)
		{
			tr.Rotate(Vector3.forward * -5f);
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

	private void ManegeBounds()
	{
		if (tr.position.x > 4.5625f)
		{
			tr.position = new Vector3(-4.5625f, tr.position.y, tr.position.z);
		}
		else if (tr.position.x < -4.5625f)
		{
			tr.position = new Vector3(4.5625f, tr.position.y, tr.position.z);
		}
		if (tr.position.y > 4.5625f)
		{
			plantTR = tr;
			Hit();
		}
		else if (tr.position.y < -4.5625f)
		{
			plantTR = tr;
			Hit();
		}
	}

	public void ManageHit(Collider2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
			plantTR = collision.transform;
			Hit();
		}
	}

	private void Hit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				BEE_reset.reset = true;
			}
			screenShakeRemainingTime = 0.3f;
			PlayHitSound();
			rb.velocity = Vector3.zero;
			coll.enabled = false;
			hitParticles.SetActive(true);
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ManageHit(collision);
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("BEE");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["BEE"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["BEE"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["BEE"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		sr.sprite = sprs[skin];
		sr_s.GetComponent<SpriteRenderer>().sprite = sprs_s[skin];
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
			pos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), renderCameraTR.position.z);
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
			GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.6f, 0.8f), Random.Range(0.8f, 1.2f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ChangeToSprite0()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = sprs_s[0];
		}
		else
		{
			sr.sprite = sprs[skin * 2];
			sr_s.sprite = sprs_s[skin * 2];
		}
	}

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = sprs_s[1];
		}
		else
		{
			sr.sprite = sprs[1 + skin * 2];
			sr_s.sprite = sprs_s[1 + skin * 2];
		}
	}
}
