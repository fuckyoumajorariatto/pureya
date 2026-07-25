using UnityEngine;

public class CRA_PlayerController : MonoBehaviour
{
	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	public Transform tr;

	private Collider2D[] colls;

	public Rigidbody2D rb;

	public SpriteRenderer sr_l;

	public SpriteRenderer sr_r;

	public SpriteRenderer sr_l_s;

	public SpriteRenderer sr_r_s;

	public SpriteRenderer sr_l2;

	public SpriteRenderer sr_r2;

	public SpriteRenderer sr_l_s2;

	public SpriteRenderer sr_r_s2;

	public Transform renderCameraTR;

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

	public CRA_Reset reset;

	private float lateralForce = 600f;

	private float verticalForce = 300f;

	public Animator anim;

	public CRA_BulletSpawner bulletSpawner;

	private GameObject bullet;

	public Transform hitParticlesTR;

	public ParticleSystem ps;

	public Transform touchParticlesTR;

	public ParticleSystem touchParticlesPS;

	private float touchedSpeed;

	private float cooldown;

	private void Start()
	{
		colls = GetComponents<Collider2D>();
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
			cooldown -= Time.deltaTime;
			ManageInput();
			ManegeBounds();
		}
	}

	private void ManageInput()
	{
		if (!PauseController.pauseController.paused && GlobalInput.globalInput.gameInputActive && !hit && !(initialTimer < 0.5f) && !(cooldown > 0f))
		{
			if (GlobalInput.globalInput.leftDown && GlobalInput.globalInput.rightDown)
			{
				rb.AddForce(tr.up * verticalForce);
				anim.SetTrigger("shootR");
				Invoke("ShootRight", 0.1f);
				anim.SetTrigger("shootL");
				Invoke("ShootLeft", 0.1f);
				PlayMoveSound();
				cooldown = 0.1f;
			}
			else if (GlobalInput.globalInput.leftDown)
			{
				rb.AddForce(-tr.right * lateralForce);
				rb.AddForce(tr.up * verticalForce);
				anim.SetTrigger("shootR");
				Invoke("ShootRight", 0.1f);
				anim.SetTrigger("shootL");
				Invoke("ShootLeft", 0.1f);
				PlayMoveSound();
				cooldown = 0.1f;
			}
			else if (GlobalInput.globalInput.rightDown)
			{
				rb.AddForce(tr.right * lateralForce);
				rb.AddForce(tr.up * verticalForce);
				anim.SetTrigger("shootL");
				Invoke("ShootLeft", 0.1f);
				anim.SetTrigger("shootR");
				Invoke("ShootRight", 0.1f);
				PlayMoveSound();
				cooldown = 0.1f;
			}
		}
	}

	private void ShootLeft()
	{
		bullet = bulletSpawner.GetObj();
		if (bullet != null)
		{
			bullet.transform.position = new Vector2(tr.position.x - 0.563f, tr.position.y - 19f / 32f);
			bullet.GetComponent<CRA_BulletController>().Initialize();
			bullet.SetActive(true);
		}
		bullet = bulletSpawner.GetObj();
		if (bullet != null)
		{
			bullet.transform.position = new Vector2(tr.position.x - 0.563f + 8f, tr.position.y - 19f / 32f);
			bullet.GetComponent<CRA_BulletController>().Initialize();
			bullet.SetActive(true);
		}
	}

	private void ShootRight()
	{
		bullet = bulletSpawner.GetObj();
		if (bullet != null)
		{
			bullet.transform.position = new Vector2(tr.position.x + 0.563f, tr.position.y - 19f / 32f);
			bullet.GetComponent<CRA_BulletController>().Initialize();
			bullet.SetActive(true);
		}
		bullet = bulletSpawner.GetObj();
		if (bullet != null)
		{
			bullet.transform.position = new Vector2(tr.position.x + 0.563f - 8f, tr.position.y - 19f / 32f);
			bullet.GetComponent<CRA_BulletController>().Initialize();
			bullet.SetActive(true);
		}
	}

	private void ManegeBounds()
	{
		if (hit)
		{
			return;
		}
		if (tr.position.y > 3.3125f)
		{
			tr.Translate(0f, -1.25f * (tr.position.y - 2f) * Time.deltaTime, 0f);
			if (tr.position.y < 3.25f)
			{
				tr.position = new Vector2(tr.position.y, 3.25f);
			}
		}
		float num = 4f;
		float num2 = -4f;
		if (skin == 9)
		{
			num = 4.0625f;
			num2 = -3.9375f;
		}
		if (tr.position.x > num)
		{
			tr.Translate(-8f, 0f, 0f);
		}
		else if (tr.position.x < num2)
		{
			tr.Translate(8f, 0f, 0f);
		}
		else if (tr.position.y > 4.5f)
		{
			Transform obj = tr;
			Vector3 position = new Vector3(tr.position.x, 4.5f, 0f);
			hitParticlesTR.position = position;
			obj.position = position;
			ManageHit();
		}
	}

	public void ManageHit()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			ps.Play();
			screenShakeRemainingTime = 0.5f;
			PlayHitSound();
			tr.position = new Vector3(7f, 7f, 0f);
			rb.velocity = Vector3.zero;
			for (int i = 0; i < colls.Length; i++)
			{
				colls[i].enabled = false;
			}
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
		}
	}

	public void DoReset()
	{
		for (int i = 0; i < colls.Length; i++)
		{
			colls[i].enabled = true;
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("CRA");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["CRA"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["CRA"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["CRA"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			sr_l.sprite = null;
			sr_l_s.sprite = sprs_s[0];
			sr_r.sprite = null;
			sr_r_s.sprite = sprs_s[0];
		}
		else
		{
			sr_l.sprite = sprs[skin * 2];
			sr_l_s.sprite = sprs_s[skin * 2];
			sr_r.sprite = sprs[skin * 2];
			sr_r_s.sprite = sprs_s[skin * 2];
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
			pos = new Vector3(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), renderCameraTR.position.z);
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
		if (!SceneChangeController.sceneChangeController.transitioning && (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown))
		{
			GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.6f, 0.8f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ChangeToSpriteL0()
	{
		if (skin == 9)
		{
			sr_l.sprite = null;
			sr_l_s.sprite = sprs_s[0];
			sr_l2.sprite = null;
			sr_l_s2.sprite = sprs_s[0];
		}
		else
		{
			sr_l.sprite = sprs[skin * 2];
			sr_l_s.sprite = sprs_s[skin * 2];
			sr_l2.sprite = sprs[skin * 2];
			sr_l_s2.sprite = sprs_s[skin * 2];
		}
	}

	private void ChangeToSpriteL1()
	{
		if (skin == 9)
		{
			sr_l.sprite = null;
			sr_l_s.sprite = sprs_s[1];
			sr_l2.sprite = null;
			sr_l_s2.sprite = sprs_s[1];
		}
		else
		{
			sr_l.sprite = sprs[skin * 2 + 1];
			sr_l_s.sprite = sprs_s[skin * 2 + 1];
			sr_l2.sprite = sprs[skin * 2 + 1];
			sr_l_s2.sprite = sprs_s[skin * 2 + 1];
		}
	}

	private void ChangeToSpriteR0()
	{
		if (skin == 9)
		{
			sr_r.sprite = null;
			sr_r_s.sprite = sprs_s[0];
			sr_r2.sprite = null;
			sr_r_s2.sprite = sprs_s[0];
		}
		else
		{
			sr_r.sprite = sprs[skin * 2];
			sr_r_s.sprite = sprs_s[skin * 2];
			sr_r2.sprite = sprs[skin * 2];
			sr_r_s2.sprite = sprs_s[skin * 2];
		}
	}

	private void ChangeToSpriteR1()
	{
		if (skin == 9)
		{
			sr_r.sprite = null;
			sr_r_s.sprite = sprs_s[1];
			sr_r2.sprite = null;
			sr_r_s2.sprite = sprs_s[1];
		}
		else
		{
			sr_r.sprite = sprs[skin * 2 + 1];
			sr_r_s.sprite = sprs_s[skin * 2 + 1];
			sr_r2.sprite = sprs[skin * 2 + 1];
			sr_r_s2.sprite = sprs[skin * 2 + 1];
		}
	}
}
