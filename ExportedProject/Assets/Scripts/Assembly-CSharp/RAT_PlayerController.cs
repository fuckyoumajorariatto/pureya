using UnityEngine;

public class RAT_PlayerController : MonoBehaviour
{
	private Transform tr;

	private SpriteRenderer sr;

	private Collider2D col;

	public SpriteRenderer sr_s;

	public Sprite[] skins;

	public Sprite[] skins_s;

	public bool hit;

	private int hits;

	private float moveSpeed = 10f;

	private float originalGravitySpeed = 10f;

	private float gravitySpeed = 10f;

	private Vector3 pos;

	public ParticleSystem hitParticles1;

	public Transform hitParticles1TR;

	public ParticleSystem hitParticles2;

	public Transform hitParticles2TR;

	public ParticleSystem hitParticles3;

	public Transform hitParticles3TR;

	public ParticleSystem hitParticles4;

	public Transform hitParticles4TR;

	public Transform renderCameraTR;

	private float screenShakeRemainingTime;

	private bool animate;

	private bool animationFrame0 = true;

	private Sprite[] selectedSprites = new Sprite[2];

	private Sprite selectedSprites_s;

	private float animationTimer;

	private float soundTimer;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public AudioClip landSound;

	private Transform hitTR;

	private Transform hitTR2;

	private RaycastHit2D[] rayHits;

	private bool wallHit;

	private bool previousWallHit;

	private bool firstInput;

	private bool lookingRight;

	private float skinWidth = 1f / 32f;

	public RAT_Reset reset;

	private int skin;

	private void Start()
	{
		tr = base.transform;
		sr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		ChangeSkin();
	}

	private void LateUpdate()
	{
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		if (!PauseController.pauseController.paused)
		{
			ManageGravity();
			ManageInput();
			ManageBounds();
			Animate();
			gravitySpeed = originalGravitySpeed + RAT_Difficulty.RAT_difficulty.speed / 2f;
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("RAT");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["RAT"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["RAT"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["RAT"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			selectedSprites[0] = null;
			selectedSprites[1] = null;
			selectedSprites_s = skins_s[0];
		}
		else
		{
			selectedSprites[0] = skins[skin * 2];
			selectedSprites[1] = skins[skin * 2 + 1];
			selectedSprites_s = skins_s[skin];
		}
		sr.sprite = selectedSprites[0];
		sr_s.sprite = selectedSprites_s;
	}

	private void ManageGravity()
	{
		wallHit = false;
		if (!firstInput)
		{
			tr.Translate(0f, RAT_Difficulty.RAT_difficulty.speed * Time.deltaTime, 0f);
			return;
		}
		tr.Translate(0f, RAT_Difficulty.RAT_difficulty.speed * Time.deltaTime, 0f);
		float y = 0f;
		rayHits = Physics2D.RaycastAll(tr.position + new Vector3(-13f / 32f, -0.1877f, 0f), -tr.up, gravitySpeed * Time.deltaTime + skinWidth);
		for (int i = 0; i < rayHits.Length; i++)
		{
			if (!rayHits[i].collider.isTrigger)
			{
				hitTR = rayHits[i].transform;
				if (hitTR != null && hitTR.CompareTag("wall"))
				{
					wallHit = true;
					y = rayHits[i].transform.position.y + 17f / 32f;
				}
			}
		}
		rayHits = Physics2D.RaycastAll(tr.position + new Vector3(11f / 32f, -0.1877f, 0f), -tr.up, gravitySpeed * Time.deltaTime + skinWidth);
		for (int j = 0; j < rayHits.Length; j++)
		{
			if (!rayHits[j].collider.isTrigger)
			{
				hitTR2 = rayHits[j].transform;
				if (hitTR2 != null && hitTR2.CompareTag("wall"))
				{
					wallHit = true;
					y = rayHits[j].transform.position.y + 17f / 32f;
				}
			}
		}
		if (wallHit)
		{
			tr.position = new Vector3(tr.position.x, y, tr.position.z);
		}
		else
		{
			tr.Translate(0f, (0f - gravitySpeed) * Time.deltaTime, 0f);
		}
		if (!previousWallHit && wallHit)
		{
			PlayLandSound();
		}
		previousWallHit = wallHit;
	}

	private void ManageBounds()
	{
		if (tr.position.y < -5f || tr.position.y > 6f)
		{
			ManageCollision();
			if (tr.position.y > 4.75f)
			{
				firstInput = false;
			}
		}
		if (tr.position.x < -4.6f)
		{
			tr.position = new Vector3(4.6f, tr.position.y, tr.position.z);
		}
		else if (tr.position.x > 4.6f)
		{
			tr.position = new Vector3(-4.6f, tr.position.y, tr.position.z);
		}
	}

	public void ResetPlayer()
	{
		tr.position = new Vector3(0f, -2f, tr.position.z);
		firstInput = false;
		col.enabled = true;
		hit = false;
		lookingRight = false;
		sr.flipX = false;
		sr_s.flipX = false;
		hitParticles1.Stop();
		hitParticles2.Stop();
		hitParticles3.Stop();
		hitParticles4.Stop();
		tr.Translate(0f, RAT_Difficulty.RAT_difficulty.speed * (0f - Time.deltaTime), 0f);
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || hit)
		{
			return;
		}
		if (!firstInput && (GlobalInput.globalInput.left || GlobalInput.globalInput.right))
		{
			firstInput = true;
		}
		if (GlobalInput.globalInput.left && GlobalInput.globalInput.right)
		{
			animate = false;
		}
		else if (GlobalInput.globalInput.left)
		{
			wallHit = false;
			float num = 0f;
			rayHits = Physics2D.RaycastAll(tr.position + new Vector3(-0.374f, 0.0625f, 0f), -tr.right, moveSpeed * Time.deltaTime + skinWidth);
			for (int i = 0; i < rayHits.Length; i++)
			{
				if (!rayHits[i].collider.isTrigger)
				{
					hitTR = rayHits[i].transform;
					if (hitTR != null && hitTR.CompareTag("wall"))
					{
						wallHit = true;
						num = rayHits[i].distance;
					}
				}
			}
			rayHits = Physics2D.RaycastAll(tr.position + new Vector3(-0.374f, -0.1875f, 0f), -tr.right, moveSpeed * Time.deltaTime + skinWidth);
			for (int j = 0; j < rayHits.Length; j++)
			{
				if (!rayHits[j].collider.isTrigger)
				{
					hitTR = rayHits[j].transform;
					if (hitTR != null && hitTR.CompareTag("wall"))
					{
						wallHit = true;
						num = rayHits[j].distance;
					}
				}
			}
			if (!wallHit)
			{
				tr.Translate(moveSpeed * (0f - Time.deltaTime), 0f, 0f);
				animate = true;
			}
			else
			{
				tr.Translate(num * (0f - Time.deltaTime), 0f, 0f);
			}
			if (lookingRight)
			{
				lookingRight = false;
				sr.flipX = false;
				sr_s.flipX = false;
			}
		}
		else if (GlobalInput.globalInput.right)
		{
			wallHit = false;
			float num2 = 0f;
			rayHits = Physics2D.RaycastAll(tr.position + new Vector3(0.3125f, 0.0625f, 0f), tr.right, moveSpeed * Time.deltaTime + skinWidth);
			for (int k = 0; k < rayHits.Length; k++)
			{
				if (!rayHits[k].collider.isTrigger)
				{
					hitTR = rayHits[k].transform;
					if (hitTR != null && hitTR.CompareTag("wall"))
					{
						wallHit = true;
						num2 = rayHits[k].distance;
					}
				}
			}
			rayHits = Physics2D.RaycastAll(tr.position + new Vector3(0.3125f, -0.1875f, 0f), tr.right, moveSpeed * Time.deltaTime + skinWidth);
			for (int l = 0; l < rayHits.Length; l++)
			{
				if (!rayHits[l].collider.isTrigger)
				{
					hitTR = rayHits[l].transform;
					if (hitTR != null && hitTR.CompareTag("wall"))
					{
						wallHit = true;
						num2 = rayHits[l].distance;
					}
				}
			}
			if (!wallHit)
			{
				tr.Translate(moveSpeed * Time.deltaTime, 0f, 0f);
				animate = true;
			}
			else
			{
				tr.Translate(num2 * Time.deltaTime, 0f, 0f);
			}
			if (!lookingRight)
			{
				lookingRight = true;
				sr.flipX = true;
				sr_s.flipX = true;
			}
		}
		else if (!GlobalInput.globalInput.left && !GlobalInput.globalInput.right)
		{
			animate = false;
			soundTimer = 1f;
		}
	}

	private void Animate()
	{
		if (!animate)
		{
			if (animationFrame0)
			{
				sr.sprite = selectedSprites[0];
				animationFrame0 = false;
				animationTimer = 0f;
				soundTimer = 1f;
			}
			return;
		}
		if (animationTimer > 0.2f)
		{
			if (animationFrame0)
			{
				sr.sprite = selectedSprites[0];
			}
			else
			{
				sr.sprite = selectedSprites[1];
			}
			animationFrame0 = !animationFrame0;
			animationTimer = 0f;
			if (soundTimer > 0.1f)
			{
				soundTimer = 0f;
				GlobalAudio.globalAudio.Play(movementSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
			}
		}
		animationTimer += Time.deltaTime;
		soundTimer += Time.deltaTime;
	}

	private void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			hit = true;
			if (tr.position.y < 0f)
			{
				hitParticles1TR.position = new Vector3(tr.position.x, -5f, hitParticles1TR.position.z);
				hitParticles2TR.position = new Vector3(tr.position.x, -5f, hitParticles2TR.position.z);
				hitParticles1.Play();
				hitParticles2.Play();
			}
			else
			{
				hitParticles3TR.position = new Vector3(tr.position.x, 5f, hitParticles3TR.position.z);
				hitParticles4TR.position = new Vector3(tr.position.x, 5f, hitParticles4TR.position.z);
				hitParticles3.Play();
				hitParticles4.Play();
			}
			col.enabled = false;
			screenShakeRemainingTime = 0.3f;
			animate = false;
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
			pos = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PlayLandSound()
	{
		GlobalAudio.globalAudio.Play(landSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
