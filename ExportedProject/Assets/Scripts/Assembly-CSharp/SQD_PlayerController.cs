using System.Collections.Generic;
using UnityEngine;

public class SQD_PlayerController : MonoBehaviour
{
	private Dictionary<int, Sprite[]> skinSprites;

	private Dictionary<int, Sprite[]> skinSprites_s;

	private int skin;

	public Sprite[] spr0 = new Sprite[8];

	public Sprite[] spr1 = new Sprite[8];

	public Sprite[] spr2 = new Sprite[8];

	public Sprite[] spr3 = new Sprite[8];

	public Sprite[] spr4 = new Sprite[8];

	public Sprite[] spr5 = new Sprite[8];

	public Sprite[] spr6 = new Sprite[8];

	public Sprite[] spr7 = new Sprite[8];

	public Sprite[] spr8 = new Sprite[8];

	public Sprite[] spr0_s = new Sprite[8];

	public Sprite[] spr1_s = new Sprite[8];

	public Sprite[] spr2_s = new Sprite[8];

	public Sprite[] spr3_s = new Sprite[8];

	public Sprite[] spr4_s = new Sprite[8];

	public Sprite[] spr5_s = new Sprite[8];

	public Sprite[] spr6_s = new Sprite[8];

	public Sprite[] spr7_s = new Sprite[8];

	public Sprite[] spr8_s = new Sprite[8];

	public Transform renderCameraTR;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public Animator animator;

	public GameObject bubbles;

	public GameObject bubblesHit;

	public Transform bgParticles_1TR;

	public Transform bgParticles_2TR;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private float yWhenHit;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	private Vector3 pos;

	private float previousY;

	private float nextY;

	private Vector2 vel;

	public Rigidbody2D rb;

	public Transform tr;

	public Collider2D coll;

	public Color[] hitColors;

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
			ManageInput();
			RelocateInsideBounds();
			LimitVelocity();
			MoveBG();
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
		skinSprites_s = new Dictionary<int, Sprite[]>();
		skinSprites_s.Add(0, spr0_s);
		skinSprites_s.Add(1, spr1_s);
		skinSprites_s.Add(2, spr2_s);
		skinSprites_s.Add(3, spr3_s);
		skinSprites_s.Add(4, spr4_s);
		skinSprites_s.Add(5, spr5_s);
		skinSprites_s.Add(6, spr6_s);
		skinSprites_s.Add(7, spr7_s);
		skinSprites_s.Add(8, spr8_s);
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SQD");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SQD"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SQD"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["SQD"] = -1;
		}
		else if (skin != 9)
		{
			skin = unlockedAndEnabledSkins[Random.Range(0, unlockedAndEnabledSkins.Length)];
		}
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites_s[skin][0];
		}
	}

	private void MoveBG()
	{
		pos = new Vector3(0f, bgParticles_1TR.position.y, bgParticles_1TR.position.z);
		pos.x = 5f - tr.position.x / 8f;
		previousY = bgParticles_1TR.position.y;
		nextY = tr.position.y / -8f;
		if (nextY > previousY + 0.1f)
		{
			nextY = previousY + (nextY - previousY) / 10f;
		}
		else if (nextY < previousY - 0.1f)
		{
			nextY = previousY - (previousY - nextY) / 10f;
		}
		pos.y = nextY;
		bgParticles_1TR.position = pos;
		pos = new Vector3(0f, bgParticles_2TR.position.y, bgParticles_2TR.position.z);
		pos.x = 5f - tr.position.x / 16f;
		previousY = bgParticles_2TR.position.y;
		nextY = tr.position.y / -16f;
		if (nextY > previousY + 0.1f)
		{
			nextY = previousY + (nextY - previousY) / 10f;
		}
		else if (nextY < previousY - 0.1f)
		{
			nextY = previousY - (previousY - nextY) / 10f;
		}
		pos.y = nextY;
		bgParticles_2TR.position = pos;
	}

	private void ManageInput()
	{
		if (GlobalInput.globalInput.gameInputActive && !hit && (!GlobalInput.globalInput.leftDown || !GlobalInput.globalInput.rightDown))
		{
			if (GlobalInput.globalInput.leftDown)
			{
				Move(true);
			}
			else if (GlobalInput.globalInput.rightDown)
			{
				Move(false);
			}
			else if (!GlobalInput.globalInput.left && !GlobalInput.globalInput.right)
			{
				animator.SetTrigger("release");
				animator.ResetTrigger("swim");
			}
		}
	}

	private void Move(bool left)
	{
		if (!hit)
		{
			vel = rb.velocity;
			vel.y = 12f;
			if (left)
			{
				vel.x = -4f;
			}
			else
			{
				vel.x = 2f;
			}
			rb.velocity = vel;
			PlayMoveSound();
			animator.ResetTrigger("release");
			animator.SetTrigger("swim");
			if (GlobalInput.globalInput.gameInputActive && GlobalInput.globalInput.left && GlobalInput.globalInput.right)
			{
				animator.SetTrigger("release");
			}
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.5f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void LimitVelocity()
	{
		vel = rb.velocity;
		if (vel.y < -5f)
		{
			rb.velocity = new Vector2(vel.x, -5f);
		}
	}

	private void RelocateInsideBounds()
	{
		if (!hit)
		{
			pos = tr.position;
			if (tr.position.y > 4.8f)
			{
				pos.y = -4.8f;
			}
			else if (tr.position.y < -4.8f)
			{
				pos.y = 4.8f;
			}
			if (tr.position.x > 4f)
			{
				pos.x = 4f;
				rb.velocity = new Vector2(0f, rb.velocity.y);
			}
			else if (tr.position.x < -4f)
			{
				pos.x = -4f;
				rb.velocity = new Vector2(0f, rb.velocity.y);
			}
			tr.position = pos;
		}
		else
		{
			rb.velocity = new Vector2(0f - SQD_Difficulty.SQD_difficulty.urchin_speed, 0f);
			tr.position = new Vector2(tr.position.x, yWhenHit);
			tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ManageCollision(collision);
	}

	private void ManageCollision(Collision2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning && !collision.transform.CompareTag("marble"))
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				SQD_Reset.SQD_reset.reset = true;
			}
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			sr.color = hitColors[skin];
			yWhenHit = tr.position.y;
			animator.SetBool("dead", true);
			screenShakeRemainingTime = 0.5f;
			bubbles.SetActive(false);
			bubblesHit.SetActive(true);
			coll.enabled = false;
			PlayHit();
			GlobalControl.globalControl.ReduceXp();
		}
	}

	private void PlayHit()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
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
			pos = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void ChangeToSprite0()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][0];
		}
		else
		{
			sr.sprite = skinSprites[skin][0];
			sr_s.sprite = skinSprites_s[skin][0];
		}
	}

	private void ChangeToSprite1()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][1];
		}
		else
		{
			sr.sprite = skinSprites[skin][1];
			sr_s.sprite = skinSprites_s[skin][1];
		}
	}

	private void ChangeToSprite2()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][2];
		}
		else
		{
			sr.sprite = skinSprites[skin][2];
			sr_s.sprite = skinSprites_s[skin][2];
		}
	}

	private void ChangeToSprite3()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][3];
		}
		else
		{
			sr.sprite = skinSprites[skin][3];
			sr_s.sprite = skinSprites_s[skin][3];
		}
	}

	private void ChangeToSprite4()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][4];
		}
		else
		{
			sr.sprite = skinSprites[skin][4];
			sr_s.sprite = skinSprites_s[skin][4];
		}
	}

	private void ChangeToSprite5()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][5];
		}
		else
		{
			sr.sprite = skinSprites[skin][5];
			sr_s.sprite = skinSprites_s[skin][5];
		}
	}

	private void ChangeToSprite6()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][6];
		}
		else
		{
			sr.sprite = skinSprites[skin][6];
			sr_s.sprite = skinSprites_s[skin][6];
		}
	}

	private void ChangeToSprite7()
	{
		if (skin == 9)
		{
			sr.sprite = null;
			sr_s.sprite = skinSprites_s[0][7];
		}
		else
		{
			sr.sprite = skinSprites[skin][7];
			sr_s.sprite = skinSprites_s[skin][7];
		}
	}
}
