using UnityEngine;

public class FLA_PlayerController : MonoBehaviour
{
	public static FLA_PlayerController FLA_playerController;

	public Sprite[] sprs = new Sprite[9];

	public Sprite[] sprs_s = new Sprite[9];

	private Color32 spr0_left_c = new Color32(164, 71, 72, byte.MaxValue);

	private Color32 spr0_right_c = new Color32(byte.MaxValue, 69, 74, byte.MaxValue);

	private Color32 spr1_left_c = new Color32(30, 30, 30, byte.MaxValue);

	private Color32 spr1_right_c = new Color32(50, 50, 50, byte.MaxValue);

	private Color32 spr2_left_c = new Color32(30, 30, 30, byte.MaxValue);

	private Color32 spr2_right_c = new Color32(50, 50, 50, byte.MaxValue);

	private Color32 spr3_left_c = new Color32(30, 30, 30, byte.MaxValue);

	private Color32 spr3_right_c = new Color32(50, 50, 50, byte.MaxValue);

	private Color32 spr4_left_c = new Color32(95, 95, 95, byte.MaxValue);

	private Color32 spr4_right_c = new Color32(120, 120, 120, byte.MaxValue);

	private Color32 spr5_left_c = new Color32(164, 71, 72, byte.MaxValue);

	private Color32 spr5_right_c = new Color32(byte.MaxValue, 69, 74, byte.MaxValue);

	private Color32 spr6_left_c = new Color32(133, 1, 1, byte.MaxValue);

	private Color32 spr6_right_c = new Color32(211, 1, 1, byte.MaxValue);

	private Color32 spr7_left_c = new Color32(30, 30, 30, byte.MaxValue);

	private Color32 spr7_right_c = new Color32(50, 50, 50, byte.MaxValue);

	private Color32 spr8_left_c = new Color32(153, 119, 0, byte.MaxValue);

	private Color32 spr8_right_c = new Color32(byte.MaxValue, 207, 46, byte.MaxValue);

	private int skin;

	public SpriteRenderer upper_leg_rightSR;

	public Transform knee_right;

	public SpriteRenderer leg_rightSR;

	public Transform leg_rightTR;

	public Transform foot_right_jointTR;

	public SpriteRenderer foot_rightSR;

	public Transform foot_rightTR;

	public Collider2D foot_right_collider;

	public Rigidbody2D foot_rightRB;

	public SpriteRenderer upper_leg_leftSR;

	public Transform knee_left;

	public SpriteRenderer leg_leftSR;

	public Transform leg_leftTR;

	public Transform foot_left_jointTR;

	public SpriteRenderer foot_leftSR;

	public Transform foot_leftTR;

	public Collider2D foot_left_collider;

	public Rigidbody2D foot_leftRB;

	public SpriteRenderer shadowSR;

	public GameObject hitParticles;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip hitSound;

	public AudioClip stepSound;

	public AudioClip stretchSound;

	public SpriteRenderer playerSR;

	public Transform playerTR;

	private float speed;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	private bool left;

	private bool moving;

	public bool stepping;

	private bool advancing;

	private bool resetting;

	private float destinyX;

	private Vector3 flamingoV;

	private Vector3 destinyV;

	private Vector3 leg2V;

	private Vector3 legScaleV;

	private float stepTimer;

	private Vector3 resetV;

	private float resetTimer;

	private bool outOfScreenHit;

	private Vector3 footJointTargetPos;

	private float hopTimer;

	public bool hopping;

	private bool hopUp;

	private void Start()
	{
		FLA_playerController = this;
		ChangeSkin();
		resetV = leg_leftTR.eulerAngles;
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
			if (hopping)
			{
				Hop();
			}
			HitIfOut();
		}
	}

	public void SetDifficultyParas()
	{
		speed = FLA_Difficulty.FLA_difficulty.scroll_speed;
	}

	public void ResetPose()
	{
		leg_leftTR.localScale = new Vector3(1f, 37f, 1f);
		leg_leftTR.eulerAngles = new Vector3(0f, 0f, 0f);
		leg_rightTR.localScale = new Vector3(1f, 10f, 1f);
		leg_rightTR.eulerAngles = new Vector3(0f, 0f, -90f);
		left = false;
		moving = false;
		advancing = false;
		resetting = false;
		stepping = false;
		resetTimer = 0f;
		stepTimer = 0f;
	}

	private void ManageInput()
	{
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right && !moving) || hit)
		{
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			if (moving)
			{
				StretchLeg();
			}
			else if (!advancing && !resetting)
			{
				moving = true;
			}
		}
		else if (GlobalInput.globalInput.right && !moving && !stepping && !advancing)
		{
			hopping = true;
		}
		else if (moving && !stepping)
		{
			stepping = true;
			if (left)
			{
				destinyX = foot_left_jointTR.position.x;
			}
			else
			{
				destinyX = foot_right_jointTR.position.x;
			}
		}
	}

	private void Hop()
	{
		playerTR.Translate(4f * Time.deltaTime, 0f, 0f);
		float y = Mathf.Lerp(3.03125f, 3.1f, hopTimer);
		playerTR.position = new Vector3(playerTR.position.x, y, playerTR.position.z);
		float y2 = Mathf.Lerp(37f, 35f, hopTimer);
		if (left)
		{
			leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, y2, leg_rightTR.localScale.z);
		}
		else
		{
			leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, y2, leg_leftTR.localScale.z);
		}
		if (hopUp)
		{
			hopTimer += Time.deltaTime * 20f;
			if (hopTimer >= 1f)
			{
				hopTimer = 1f;
				hopUp = false;
			}
		}
		else
		{
			hopTimer -= Time.deltaTime * 20f;
			if (hopTimer <= 0f)
			{
				PlayHopSound();
				hopTimer = 0f;
				hopUp = true;
				if (!GlobalInput.globalInput.right || hit)
				{
					hopping = false;
				}
			}
		}
		MatchLegsAndKnees();
	}

	private void HitIfOut()
	{
		if (!moving && !advancing && !hit && playerTR.position.x > 6.5f)
		{
			outOfScreenHit = true;
			ManageHit(null);
		}
	}

	private void StretchLeg()
	{
		PlayStretchSound();
		if (left)
		{
			if (foot_leftTR.position.x > -4f)
			{
				leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y + 150f * Time.deltaTime, leg_leftTR.localScale.z);
				return;
			}
			stepping = true;
			destinyX = foot_left_jointTR.position.x;
		}
		else if (foot_rightTR.position.x > -4f)
		{
			leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y + 150f * Time.deltaTime, leg_rightTR.localScale.z);
		}
		else
		{
			stepping = true;
			destinyX = foot_right_jointTR.position.x;
		}
	}

	private void LowerFoot()
	{
		if (left)
		{
			leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y + 150f * Time.deltaTime, leg_leftTR.localScale.z);
			while (foot_left_jointTR.position.x < destinyX)
			{
				leg_leftTR.eulerAngles = new Vector3(0f, 0f, leg_leftTR.eulerAngles.z + 0.01f);
			}
			while (foot_left_jointTR.position.y < -2.9375f)
			{
				leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y - 0.01f, leg_leftTR.localScale.z);
			}
		}
		else
		{
			leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y + 150f * Time.deltaTime, leg_rightTR.localScale.z);
			while (foot_right_jointTR.position.x < destinyX)
			{
				leg_rightTR.eulerAngles = new Vector3(0f, 0f, leg_rightTR.eulerAngles.z + 0.01f);
			}
			while (foot_right_jointTR.position.y < -2.9375f)
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y - 0.01f, leg_rightTR.localScale.z);
			}
		}
	}

	private void TransformStretchedLeg()
	{
		int num = 0;
		if (left)
		{
			leg_leftTR.eulerAngles = Vector3.Lerp(leg2V, new Vector3(0f, 0f, 360f), stepTimer);
			leg_leftTR.localScale = Vector3.Lerp(legScaleV, new Vector3(1f, 37f, 1f), stepTimer);
			stepTimer += 5f * Time.deltaTime;
			footJointTargetPos = new Vector3(foot_leftTR.position.x - 0.0625f, foot_leftTR.position.y, foot_leftTR.position.z);
			while (Vector2.Distance(foot_left_jointTR.position, footJointTargetPos) > 0.1f && num < 500)
			{
				if (foot_left_jointTR.position.x > footJointTargetPos.x)
				{
					leg_leftTR.eulerAngles = new Vector3(leg_leftTR.eulerAngles.x, leg_leftTR.eulerAngles.y, leg_leftTR.eulerAngles.z - 0.1f);
				}
				else if (foot_left_jointTR.position.x < footJointTargetPos.x)
				{
					leg_leftTR.eulerAngles = new Vector3(leg_leftTR.eulerAngles.x, leg_leftTR.eulerAngles.y, leg_leftTR.eulerAngles.z + 0.1f);
				}
				if (foot_left_jointTR.position.y < footJointTargetPos.y)
				{
					leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y - 0.1f, leg_leftTR.localScale.z);
				}
				else if (foot_left_jointTR.position.y > footJointTargetPos.y)
				{
					leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y + 0.1f, leg_leftTR.localScale.z);
				}
				num++;
			}
			num = 0;
			while (Vector2.Distance(foot_left_jointTR.position, footJointTargetPos) > 0.01f && num < 500)
			{
				if (foot_left_jointTR.position.x > footJointTargetPos.x)
				{
					leg_leftTR.eulerAngles = new Vector3(leg_leftTR.eulerAngles.x, leg_leftTR.eulerAngles.y, leg_leftTR.eulerAngles.z - 0.01f);
				}
				else if (foot_left_jointTR.position.x < footJointTargetPos.x)
				{
					leg_leftTR.eulerAngles = new Vector3(leg_leftTR.eulerAngles.x, leg_leftTR.eulerAngles.y, leg_leftTR.eulerAngles.z + 0.01f);
				}
				if (foot_left_jointTR.position.y < footJointTargetPos.y)
				{
					leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y - 0.01f, leg_leftTR.localScale.z);
				}
				else if (foot_left_jointTR.position.y > footJointTargetPos.y)
				{
					leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y + 0.01f, leg_leftTR.localScale.z);
				}
				num++;
			}
			if (playerTR.position.x <= destinyV.x)
			{
				leg_leftTR.eulerAngles = Vector3.Lerp(resetV, new Vector3(0f, 0f, 0f), resetTimer);
				leg_leftTR.localScale = new Vector3(1f, 37f, 1f);
				resetV = leg_leftTR.eulerAngles;
				advancing = false;
				left = !left;
				stepTimer = 0f;
			}
			return;
		}
		leg_rightTR.eulerAngles = Vector3.Lerp(leg2V, new Vector3(0f, 0f, 360f), stepTimer);
		leg_rightTR.localScale = Vector3.Lerp(legScaleV, new Vector3(1f, 37f, 1f), stepTimer);
		stepTimer += 5f * Time.deltaTime;
		footJointTargetPos = new Vector3(foot_rightTR.position.x - 0.0625f, foot_rightTR.position.y, foot_rightTR.position.z);
		while (Vector2.Distance(foot_right_jointTR.position, footJointTargetPos) > 0.1f && num < 500)
		{
			if (foot_right_jointTR.position.x > footJointTargetPos.x)
			{
				leg_rightTR.eulerAngles = new Vector3(leg_rightTR.eulerAngles.x, leg_rightTR.eulerAngles.y, leg_rightTR.eulerAngles.z - 0.1f);
			}
			else if (foot_right_jointTR.position.x < footJointTargetPos.x)
			{
				leg_rightTR.eulerAngles = new Vector3(leg_rightTR.eulerAngles.x, leg_rightTR.eulerAngles.y, leg_rightTR.eulerAngles.z + 0.1f);
			}
			if (foot_right_jointTR.position.y < footJointTargetPos.y)
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y - 0.1f, leg_rightTR.localScale.z);
			}
			else if (foot_right_jointTR.position.y > footJointTargetPos.y)
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y + 0.1f, leg_rightTR.localScale.z);
			}
			num++;
		}
		num = 0;
		while (Vector2.Distance(foot_right_jointTR.position, footJointTargetPos) > 0.01f && num < 500)
		{
			if (foot_right_jointTR.position.x > footJointTargetPos.x)
			{
				leg_rightTR.eulerAngles = new Vector3(leg_rightTR.eulerAngles.x, leg_rightTR.eulerAngles.y, leg_rightTR.eulerAngles.z - 0.01f);
			}
			else if (foot_right_jointTR.position.x < footJointTargetPos.x)
			{
				leg_rightTR.eulerAngles = new Vector3(leg_rightTR.eulerAngles.x, leg_rightTR.eulerAngles.y, leg_rightTR.eulerAngles.z + 0.01f);
			}
			if (foot_right_jointTR.position.y < footJointTargetPos.y)
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y - 0.01f, leg_rightTR.localScale.z);
			}
			else if (foot_right_jointTR.position.y > footJointTargetPos.y)
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y + 0.01f, leg_rightTR.localScale.z);
			}
			num++;
		}
		if (playerTR.position.x <= destinyV.x)
		{
			leg_rightTR.eulerAngles = Vector3.Lerp(resetV, new Vector3(0f, 0f, 0f), resetTimer);
			leg_rightTR.localScale = new Vector3(1f, 37f, 1f);
			resetV = leg_rightTR.eulerAngles;
			advancing = false;
			left = !left;
			stepTimer = 0f;
		}
	}

	private void ResetLeg()
	{
		if (!left && resetTimer < 1f)
		{
			resetTimer += 10f * Time.deltaTime;
			leg_leftTR.eulerAngles = Vector3.Lerp(resetV, new Vector3(0f, 0f, -90f), resetTimer);
			if (leg_leftTR.localScale.y > 10f)
			{
				leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, leg_leftTR.localScale.y - 500f * Time.deltaTime, 1f);
				if (leg_leftTR.localScale.y < 10f)
				{
					leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, 10f, 1f);
				}
			}
		}
		else if (resetTimer < 1f)
		{
			resetTimer += 10f * Time.deltaTime;
			leg_rightTR.eulerAngles = Vector3.Lerp(resetV, new Vector3(0f, 0f, -90f), resetTimer);
			if (leg_rightTR.localScale.y > 10f)
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, leg_rightTR.localScale.y - 500f * Time.deltaTime, 1f);
				if (leg_rightTR.localScale.y < 10f)
				{
					leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, 10f, 1f);
				}
			}
		}
		else
		{
			resetting = false;
			resetTimer = 0f;
		}
	}

	private void MovePlayer()
	{
		playerTR.Translate(speed * Time.deltaTime, 0f, 0f);
		flamingoV = new Vector3(flamingoV.x + speed * Time.deltaTime, flamingoV.y, flamingoV.z);
		destinyX += speed * Time.deltaTime;
		if (hit)
		{
			MatchLegsAndKnees();
		}
		else if (stepping)
		{
			if (left)
			{
				if (foot_left_jointTR.position.y > -2.93f)
				{
					LowerFoot();
				}
				else
				{
					PlayStepSound();
					moving = false;
					stepping = false;
					advancing = true;
					resetting = true;
					flamingoV = playerTR.position;
					destinyV = new Vector3(destinyX - 1.34375f, flamingoV.y, flamingoV.z);
					leg2V = leg_leftTR.eulerAngles;
					legScaleV = leg_leftTR.localScale;
				}
			}
			else if (foot_right_jointTR.position.y > -2.93f)
			{
				LowerFoot();
			}
			else
			{
				PlayStepSound();
				moving = false;
				stepping = false;
				advancing = true;
				resetting = true;
				flamingoV = playerTR.position;
				destinyV = new Vector3(destinyX - 29f / 32f, flamingoV.y, flamingoV.z);
				leg2V = leg_rightTR.eulerAngles;
				legScaleV = leg_rightTR.localScale;
			}
			MatchLegsAndKnees();
		}
		else if (advancing || resetting)
		{
			if (resetting)
			{
				ResetLeg();
				MatchLegsAndKnees();
			}
			if (advancing)
			{
				playerTR.position = Vector3.Lerp(flamingoV, destinyV, stepTimer);
				leg_leftTR.position = new Vector3(knee_left.position.x, knee_left.position.y, leg_leftTR.position.z);
				leg_rightTR.position = new Vector3(knee_right.position.x, knee_right.position.y, leg_rightTR.position.z);
				if (left)
				{
					destinyV = new Vector3(destinyX - 1.34375f, flamingoV.y, flamingoV.z);
					foot_rightTR.position = new Vector3(foot_right_jointTR.position.x + 0.0625f, foot_right_jointTR.position.y - 0.0625f, foot_rightTR.position.z);
					foot_leftTR.position = new Vector3(destinyX, -3f, foot_leftTR.position.z);
				}
				else
				{
					destinyV = new Vector3(destinyX - 29f / 32f, flamingoV.y, flamingoV.z);
					foot_leftTR.position = new Vector3(foot_left_jointTR.position.x + 0.0625f, foot_left_jointTR.position.y - 0.0625f, foot_leftTR.position.z);
					foot_rightTR.position = new Vector3(destinyX, -3f, foot_rightTR.position.z);
				}
				TransformStretchedLeg();
			}
		}
		else
		{
			MatchLegsAndKnees();
		}
	}

	private void MatchLegsAndKnees()
	{
		leg_leftTR.position = new Vector3(knee_left.position.x, knee_left.position.y, leg_leftTR.position.z);
		leg_rightTR.position = new Vector3(knee_right.position.x, knee_right.position.y, leg_rightTR.position.z);
		foot_leftTR.position = new Vector3(foot_left_jointTR.position.x + 0.0625f, foot_left_jointTR.position.y - 0.0625f, foot_leftTR.position.z);
		foot_rightTR.position = new Vector3(foot_right_jointTR.position.x + 0.0625f, foot_right_jointTR.position.y - 0.0625f, foot_rightTR.position.z);
	}

	private void ChangeSkin()
	{
		Color32[] array = new Color32[9] { spr0_left_c, spr1_left_c, spr2_left_c, spr3_left_c, spr4_left_c, spr5_left_c, spr6_left_c, spr7_left_c, spr8_left_c };
		Color32[] array2 = new Color32[9] { spr0_right_c, spr1_right_c, spr2_right_c, spr3_right_c, spr4_right_c, spr5_right_c, spr6_right_c, spr7_right_c, spr8_right_c };
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("FLA");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["FLA"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["FLA"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["FLA"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			playerSR.sprite = null;
			shadowSR.sprite = sprs_s[0];
			upper_leg_leftSR.sprite = null;
			leg_leftSR.sprite = null;
			upper_leg_rightSR.sprite = null;
			leg_rightSR.sprite = null;
			foot_leftSR.sprite = null;
			foot_rightSR.sprite = null;
		}
		else
		{
			playerSR.sprite = sprs[skin];
			shadowSR.sprite = sprs_s[skin];
			upper_leg_leftSR.color = array2[skin];
			leg_leftSR.color = array2[skin];
			upper_leg_rightSR.color = array[skin];
			leg_rightSR.color = array[skin];
			foot_leftSR.color = array2[skin];
			foot_rightSR.color = array[skin];
		}
	}

	public void ManageHit(Collider2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			if (MinigamesController.minigamesController.Hit())
			{
				FLA_Reset.FLA_reset.reset = true;
			}
			if (!outOfScreenHit)
			{
				hitParticles.transform.position = collision.transform.position;
			}
			else
			{
				hitParticles.transform.position = new Vector3(4f, hitParticles.transform.position.y, hitParticles.transform.position.z);
			}
			hitParticles.SetActive(true);
			moving = false;
			advancing = false;
			stepping = false;
			if (left)
			{
				leg_leftTR.localScale = new Vector3(leg_leftTR.localScale.x, 10f, leg_leftTR.localScale.z);
			}
			else
			{
				leg_rightTR.localScale = new Vector3(leg_rightTR.localScale.x, 10f, leg_rightTR.localScale.z);
			}
			screenShakeRemainingTime = 0.3f;
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

	private void PlayHitSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), playerTR.position.x / 4f);
		}
	}

	private void PlayStepSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			float pan = ((!left) ? (foot_rightTR.position.x / 4f) : (foot_leftTR.position.x / 4f));
			GlobalAudio.globalAudio.Play(stepSound, 0.9f, Random.Range(0.9f, 1.1f), pan);
		}
	}

	private void PlayHopSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			float pan = ((!left) ? (foot_rightTR.position.x / 4f) : (foot_leftTR.position.x / 4f));
			GlobalAudio.globalAudio.Play(stepSound, 0.3f, Random.Range(0.9f, 1.1f), pan);
		}
	}

	private void PlayStretchSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			if (audioSource == null)
			{
				DoPlayStretchSound();
			}
			else if (!audioSource.isPlaying || (float)audioSource.timeSamples >= (float)audioSource.clip.frequency * 0.1f)
			{
				DoPlayStretchSound();
			}
		}
	}

	private void DoPlayStretchSound()
	{
		float pan = ((!left) ? (foot_rightTR.position.x / 4f) : (foot_leftTR.position.x / 4f));
		audioSource = GlobalAudio.globalAudio.Play(stretchSound, Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f), pan);
	}
}
