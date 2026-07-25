using UnityEngine;

public class SNK_PlayerController : MonoBehaviour
{
	public SNK_Spawner spawner;

	public Transform tr;

	public Transform[] trs;

	public Collider2D coll;

	private int direction;

	private int[] directions;

	private Vector3 movementDirection;

	private Vector3 partMovementDirection;

	private float initialTimer;

	public bool hit;

	public int hits;

	private int skin;

	private float screenShakeRemainingTime;

	private Vector3 pos;

	public Sprite[] skin_1;

	public Sprite[] skin_2;

	public Sprite[] skin_3;

	public Sprite[] skin_4;

	public Sprite[] skin_5;

	public Sprite[] skin_6;

	public Sprite[] skin_7;

	public Sprite[] skin_8;

	public Sprite[] skin_9;

	public Sprite[] skin_1_s;

	public Sprite[] skin_2_s;

	public Sprite[] skin_3_s;

	public Sprite[] skin_4_s;

	public Sprite[] skin_5_s;

	public Sprite[] skin_6_s;

	public Sprite[] skin_7_s;

	public Sprite[] skin_8_s;

	public Sprite[] skin_9_s;

	public SpriteRenderer[] sr;

	public SpriteRenderer[] sr_s;

	public Transform renderCameraTR;

	public AudioClip movementSound;

	public AudioClip hitSound;

	public ParticleSystem hitParticles;

	public SNK_Reset SNK_reset;

	public ParticleSystem moveParticles;

	public Color[] hitColors;

	private bool directionChanged;

	private void Start()
	{
		ChangeSkin();
		movementDirection = tr.up;
		partMovementDirection = tr.up;
		directions = new int[trs.Length];
		DoReset();
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
			Move();
			for (int i = 0; i < trs.Length; i++)
			{
				MovePart(i);
				directionChanged = false;
			}
		}
	}

	private void ManageInput()
	{
		if (GlobalInput.globalInput.gameInputActive && (!GlobalInput.globalInput.left || !GlobalInput.globalInput.right) && !hit && !(initialTimer < 0.5f))
		{
			if (GlobalInput.globalInput.leftDown)
			{
				tr.Rotate(0f, 0f, 90f);
				ChangeDirection(true);
				PlayMoveSound();
			}
			else if (GlobalInput.globalInput.rightDown)
			{
				tr.Rotate(0f, 0f, -90f);
				ChangeDirection(false);
				PlayMoveSound();
			}
		}
	}

	private void Move()
	{
		if (!hit)
		{
			float num = SNK_Difficulty.SNK_difficulty.playerSpeed;
			switch (direction)
			{
			case 0:
				num *= 1.5f;
				break;
			case 2:
				num /= 1.5f;
				break;
			}
			tr.Translate(movementDirection * num * Time.deltaTime);
		}
		tr.position = new Vector3(tr.position.x, tr.position.y - SNK_Difficulty.SNK_difficulty.scroll * Time.deltaTime, tr.position.z);
	}

	private void ManegeBounds()
	{
		if (tr.position.x > 4.5f)
		{
			Hit();
		}
		else if (tr.position.x < -4.5f)
		{
			Hit();
		}
		if (tr.position.y > 4.5f)
		{
			spawner.CreateFaceCactus();
			Hit();
		}
		else if (tr.position.y < -4.5f)
		{
			Hit();
		}
	}

	public void ManageHit(Collider2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
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
				SNK_reset.reset = true;
			}
			screenShakeRemainingTime = 0.3f;
			PlayHitSound();
			coll.enabled = false;
			hitParticles.Play();
			moveParticles.Stop();
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
			for (int i = 0; i < sr.Length; i++)
			{
				sr[i].color = hitColors[skin];
			}
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("SNK");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["SNK"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["SNK"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			GlobalControl.globalControl.lastUnlockedSkins["SNK"] = -1;
			skin = num;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		for (int i = 0; i < sr.Length; i++)
		{
			if (skin == 0)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_1[i];
					sr_s[i].sprite = skin_1_s[i];
					break;
				case 9:
					sr[i].sprite = skin_1[3];
					sr_s[i].sprite = skin_1_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_1[1];
					sr_s[i].sprite = skin_1_s[1];
					break;
				default:
					sr[i].sprite = skin_1[2];
					sr_s[i].sprite = skin_1_s[2];
					break;
				}
			}
			else if (skin == 1)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_2[i];
					sr_s[i].sprite = skin_2_s[i];
					break;
				case 9:
					sr[i].sprite = skin_2[3];
					sr_s[i].sprite = skin_2_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_2[1];
					sr_s[i].sprite = skin_2_s[1];
					break;
				default:
					sr[i].sprite = skin_2[2];
					sr_s[i].sprite = skin_2_s[2];
					break;
				}
			}
			else if (skin == 2)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_3[i];
					sr_s[i].sprite = skin_3_s[i];
					break;
				case 9:
					sr[i].sprite = skin_3[3];
					sr_s[i].sprite = skin_3_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_3[1];
					sr_s[i].sprite = skin_3_s[1];
					break;
				default:
					sr[i].sprite = skin_3[2];
					sr_s[i].sprite = skin_3_s[2];
					break;
				}
			}
			else if (skin == 3)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_4[i];
					sr_s[i].sprite = skin_4_s[i];
					break;
				case 9:
					sr[i].sprite = skin_4[3];
					sr_s[i].sprite = skin_4_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_4[1];
					sr_s[i].sprite = skin_4_s[1];
					break;
				default:
					sr[i].sprite = skin_4[2];
					sr_s[i].sprite = skin_4_s[2];
					break;
				}
			}
			else if (skin == 4)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_5[i];
					sr_s[i].sprite = skin_5_s[i];
					break;
				case 9:
					sr[i].sprite = skin_5[3];
					sr_s[i].sprite = skin_5_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_5[1];
					sr_s[i].sprite = skin_5_s[1];
					break;
				default:
					sr[i].sprite = skin_5[2];
					sr_s[i].sprite = skin_5_s[2];
					break;
				}
			}
			else if (skin == 5)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_6[i];
					sr_s[i].sprite = skin_6_s[i];
					break;
				case 9:
					sr[i].sprite = skin_6[3];
					sr_s[i].sprite = skin_6_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_6[1];
					sr_s[i].sprite = skin_6_s[1];
					break;
				default:
					sr[i].sprite = skin_6[2];
					sr_s[i].sprite = skin_6_s[2];
					break;
				}
			}
			else if (skin == 6)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_7[i];
					sr_s[i].sprite = skin_7_s[i];
					break;
				case 9:
					sr[i].sprite = skin_7[3];
					sr_s[i].sprite = skin_7_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_7[1];
					sr_s[i].sprite = skin_7_s[1];
					break;
				default:
					sr[i].sprite = skin_7[2];
					sr_s[i].sprite = skin_7_s[2];
					break;
				}
			}
			else if (skin == 7)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_8[i];
					sr_s[i].sprite = skin_8_s[i];
					break;
				case 9:
					sr[i].sprite = skin_8[3];
					sr_s[i].sprite = skin_8_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_8[1];
					sr_s[i].sprite = skin_8_s[1];
					break;
				default:
					sr[i].sprite = skin_8[2];
					sr_s[i].sprite = skin_1_s[2];
					break;
				}
			}
			else if (skin == 8)
			{
				switch (i)
				{
				case 0:
					sr[i].sprite = skin_9[i];
					sr_s[i].sprite = skin_9_s[i];
					break;
				case 9:
					sr[i].sprite = skin_9[3];
					sr_s[i].sprite = skin_9_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr[i].sprite = skin_9[1];
					sr_s[i].sprite = skin_9_s[1];
					break;
				default:
					sr[i].sprite = skin_9[2];
					sr_s[i].sprite = skin_9_s[2];
					break;
				}
			}
			else if (skin == 9)
			{
				sr[i].sprite = null;
				switch (i)
				{
				case 0:
					sr_s[i].sprite = skin_1_s[i];
					break;
				case 9:
					sr_s[i].sprite = skin_1_s[3];
					break;
				case 1:
				case 3:
				case 5:
				case 7:
					sr_s[i].sprite = skin_1_s[1];
					break;
				default:
					sr_s[i].sprite = skin_1_s[2];
					break;
				}
			}
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
			pos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), renderCameraTR.position.z);
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
			GlobalAudio.globalAudio.Play(movementSound, 0.6f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 0.9f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void ChangeDirection(bool left)
	{
		if (left)
		{
			direction--;
			if (direction < 0)
			{
				direction = 3;
			}
		}
		else
		{
			direction++;
			if (direction > 3)
			{
				direction = 0;
			}
		}
		switch (direction)
		{
		case 0:
			movementDirection = tr.up;
			break;
		case 1:
			movementDirection = -tr.right;
			break;
		case 2:
			movementDirection = -tr.up;
			break;
		case 3:
			movementDirection = tr.right;
			break;
		}
		directionChanged = true;
	}

	private void MovePart(int index)
	{
		float num = 0.125f;
		float num2 = 0.125f;
		Transform transform = null;
		Transform transform2 = null;
		if (index == 0)
		{
			transform = tr;
			transform2 = trs[index];
			num = 0.25f;
			num2 = 0.25f;
		}
		else
		{
			transform = trs[index - 1];
			transform2 = trs[index];
			if (index == trs.Length - 1)
			{
				num2 = 0.25f;
			}
		}
		int num3 = directions[index];
		float num4 = SNK_Difficulty.SNK_difficulty.playerSpeed;
		switch (direction)
		{
		case 0:
			num4 *= 1.5f;
			break;
		case 2:
			num4 /= 1.5f;
			break;
		}
		if (!hit)
		{
			transform2.Translate(partMovementDirection * num4 * Time.deltaTime);
		}
		transform2.position = new Vector3(transform2.position.x, transform2.position.y - SNK_Difficulty.SNK_difficulty.scroll * Time.deltaTime, transform2.position.z);
		if (directions[index] == 0)
		{
			if (transform.position.x >= transform2.position.x + num)
			{
				num3 = 1;
				transform2.Rotate(0f, 0f, -90f);
			}
			else if (transform.position.x <= transform2.position.x - num)
			{
				num3 = 3;
				transform2.Rotate(0f, 0f, 90f);
			}
			else if (transform.position.y <= transform2.position.y - num)
			{
				num3 = 2;
				transform2.Rotate(0f, 0f, -180f);
			}
		}
		else if (directions[index] == 1)
		{
			if (transform.position.y >= transform2.position.y + num)
			{
				num3 = 0;
				transform2.Rotate(0f, 0f, 90f);
			}
			else if (transform.position.y <= transform2.position.y - num)
			{
				num3 = 2;
				transform2.Rotate(0f, 0f, -90f);
			}
			else if (transform.position.x <= transform2.position.x - num)
			{
				num3 = 3;
				transform2.Rotate(0f, 0f, -180f);
			}
		}
		else if (directions[index] == 2)
		{
			if (transform.position.x >= transform2.position.x + num)
			{
				num3 = 1;
				transform2.Rotate(0f, 0f, 90f);
			}
			else if (transform.position.x <= transform2.position.x - num)
			{
				num3 = 3;
				transform2.Rotate(0f, 0f, -90f);
			}
			else if (transform.position.y >= transform2.position.y + num)
			{
				num3 = 0;
				transform2.Rotate(0f, 0f, -180f);
			}
		}
		else if (directions[index] == 3)
		{
			if (transform.position.y >= transform2.position.y + num)
			{
				num3 = 0;
				transform2.Rotate(0f, 0f, -90f);
			}
			else if (transform.position.y <= transform2.position.y - num)
			{
				num3 = 2;
				transform2.Rotate(0f, 0f, 90f);
			}
			else if (transform.position.x >= transform2.position.x + num)
			{
				num3 = 1;
				transform2.Rotate(0f, 0f, -180f);
			}
		}
		if (num3 != directions[index] || directionChanged)
		{
			directions[index] = num3;
			if (index == 9)
			{
				Debug.Break();
			}
			switch (directions[index])
			{
			case 0:
				transform2.position = new Vector3(transform.position.x, transform.position.y - num2, transform2.position.z);
				break;
			case 1:
				transform2.position = new Vector3(transform.position.x - num2, transform.position.y, transform2.position.z);
				break;
			case 2:
				transform2.position = new Vector3(transform.position.x, transform.position.y + num2, transform2.position.z);
				break;
			case 3:
				transform2.position = new Vector3(transform.position.x + num2, transform.position.y, transform2.position.z);
				break;
			}
		}
		switch (directions[index])
		{
		case 0:
			partMovementDirection = transform2.up;
			break;
		case 1:
			partMovementDirection = -transform2.right;
			break;
		case 2:
			partMovementDirection = -transform2.up;
			break;
		case 3:
			partMovementDirection = transform2.right;
			break;
		}
	}

	public void DoReset()
	{
		tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		tr.localPosition = new Vector3(0f, 0f, tr.localPosition.z);
		hit = false;
		coll.enabled = true;
		hitParticles.Stop();
		moveParticles.Play();
		movementDirection = tr.up;
		partMovementDirection = tr.up;
		direction = 0;
		directions = new int[trs.Length];
		for (int i = 0; i < trs.Length; i++)
		{
			trs[i].rotation = new Quaternion(0f, 0f, 0f, 0f);
		}
		trs[0].localPosition = new Vector3(0f, -0.25f, trs[0].localPosition.z);
		trs[1].localPosition = new Vector3(0f, -0.375f, trs[1].localPosition.z);
		trs[2].localPosition = new Vector3(0f, -0.5f, trs[2].localPosition.z);
		trs[3].localPosition = new Vector3(0f, -0.625f, trs[3].localPosition.z);
		trs[4].localPosition = new Vector3(0f, -0.75f, trs[4].localPosition.z);
		trs[5].localPosition = new Vector3(0f, -0.875f, trs[5].localPosition.z);
		trs[6].localPosition = new Vector3(0f, -1f, trs[6].localPosition.z);
		trs[7].localPosition = new Vector3(0f, -1.125f, trs[7].localPosition.z);
		trs[8].localPosition = new Vector3(0f, -1.375f, trs[8].localPosition.z);
		for (int j = 0; j < sr.Length; j++)
		{
			sr[j].color = Color.white;
		}
	}
}
