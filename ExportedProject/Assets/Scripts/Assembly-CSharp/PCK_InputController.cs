using UnityEngine;

public class PCK_InputController : MonoBehaviour
{
	public static PCK_InputController PCK_inputController;

	public SpriteRenderer powerLeft;

	public SpriteRenderer powerRight;

	private Color powerColor;

	private Color32 powerColorHot = new Color32(254, 116, 30, byte.MaxValue);

	private Color32 powerColorRandoshoot = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	private float cooldown = 0.5f;

	private float velocityLeft;

	private bool chargingLeft;

	private bool shootLeft;

	private float leftCooldown;

	private float velocityRight;

	private bool shootRight;

	private bool chargingRight;

	private float rightCooldown;

	private int lastFramesAtMaxPowerLeft;

	private int lastFramesAtMaxPowerRight;

	public bool inputActive;

	public int marblesRemaining;

	public int visualMarblesRemaining;

	public AudioClip charge;

	public AudioClip release;

	public AudioClip empty;

	private float audioChargeCooldownLeft;

	private float audioChargeCooldownRight;

	private float audioEmptyCooldownLeft;

	private float audioEmptyCooldownRight;

	public Transform shootPointLeft;

	public Transform shootPointRight;

	private Vector3 pos;

	private bool shootLastTwo;

	private GameObject newMarble;

	private float vel;

	private float directionModifier;

	private float pan;

	private float pitch;

	private void Start()
	{
		PCK_inputController = this;
		powerColor = powerLeft.color;
		visualMarblesRemaining = marblesRemaining;
		shootLastTwo = false;
	}

	private void FixedUpdate()
	{
		if (!PauseController.pauseController.paused)
		{
			if (inputActive)
			{
				ManageInput();
			}
			MovePowerIndicators();
			ResetAudioChargeCooldown();
			ReduceAudioEmptyCooldown();
		}
	}

	public void CancelShoots()
	{
		EndShootLeft();
		EndShootRight();
		MovePowerIndicators();
	}

	private void ManageShoot(bool left)
	{
		if (left)
		{
			if (marblesRemaining <= 0)
			{
				chargingLeft = false;
			}
			else if (marblesRemaining >= 5)
			{
				marblesRemaining -= 3;
				ShootMarbleLeft();
				Invoke("ShootMarbleLeft", 0.15f);
				Invoke("ShootMarbleLeft", 0.3f);
				Invoke("EndShootLeft", 0.3f);
			}
			else if (marblesRemaining >= 3)
			{
				if (chargingRight)
				{
					marblesRemaining -= 2;
					ShootMarbleLeft();
					Invoke("ShootMarbleLeft", 0.15f);
					Invoke("EndShootLeft", 0.15f);
					shootLastTwo = true;
				}
				else
				{
					marblesRemaining -= 3;
					ShootMarbleLeft();
					Invoke("ShootMarbleLeft", 0.15f);
					Invoke("ShootMarbleLeft", 0.3f);
					Invoke("EndShootLeft", 0.3f);
				}
			}
			else if (shootLastTwo)
			{
				marblesRemaining -= 2;
				ShootMarbleLeft();
				Invoke("ShootMarbleLeft", 0.15f);
				Invoke("EndShootLeft", 0.15f);
				shootLastTwo = false;
			}
			else
			{
				if (marblesRemaining >= 0)
				{
					marblesRemaining--;
					ShootMarbleLeft();
				}
				EndShootLeft();
			}
		}
		else if (marblesRemaining <= 0)
		{
			chargingRight = false;
		}
		else if (marblesRemaining >= 5)
		{
			marblesRemaining -= 3;
			ShootMarbleRight();
			Invoke("ShootMarbleRight", 0.15f);
			Invoke("ShootMarbleRight", 0.3f);
			Invoke("EndShootRight", 0.3f);
		}
		else if (marblesRemaining >= 3)
		{
			if (chargingLeft)
			{
				marblesRemaining -= 2;
				ShootMarbleRight();
				Invoke("ShootMarbleRight", 0.15f);
				Invoke("EndShootRight", 0.15f);
				shootLastTwo = true;
			}
			else
			{
				marblesRemaining -= 3;
				ShootMarbleRight();
				Invoke("ShootMarbleRight", 0.15f);
				Invoke("ShootMarbleRight", 0.3f);
				Invoke("EndShootRight", 0.3f);
			}
		}
		else if (shootLastTwo)
		{
			marblesRemaining -= 2;
			ShootMarbleRight();
			Invoke("ShootMarbleRight", 0.15f);
			Invoke("EndShootRight", 0.15f);
			shootLastTwo = false;
		}
		else
		{
			if (marblesRemaining >= 0)
			{
				marblesRemaining--;
				ShootMarbleRight();
			}
			EndShootRight();
		}
	}

	private void ManageInput()
	{
		if (GlobalInput.globalInput.left && GlobalInput.globalInput.gameInputActive && leftCooldown <= 0f)
		{
			if (marblesRemaining == 0 || !PCK_MarbleCreator.PCK_marbleCreator.AreThereMarblesLeft())
			{
				PlayEmpty(true);
				EndShootLeft();
			}
			else
			{
				if (!shootLeft)
				{
					RaiseVelocity(true);
				}
				if (shootLeft)
				{
					ManageShoot(true);
				}
			}
		}
		else if (chargingLeft && leftCooldown <= 0f)
		{
			if (marblesRemaining == 0)
			{
				chargingLeft = false;
			}
			else
			{
				ManageShoot(true);
			}
		}
		else
		{
			chargingLeft = false;
			if (leftCooldown > 0f)
			{
				leftCooldown -= Time.deltaTime;
			}
			if (leftCooldown <= 0f)
			{
				velocityLeft = 0f;
			}
		}
		if (GlobalInput.globalInput.right && GlobalInput.globalInput.gameInputActive && rightCooldown <= 0f)
		{
			if (marblesRemaining == 0 || !PCK_MarbleCreator.PCK_marbleCreator.AreThereMarblesLeft())
			{
				PlayEmpty(false);
				EndShootRight();
				return;
			}
			if (!shootRight)
			{
				RaiseVelocity(false);
			}
			if (shootRight)
			{
				ManageShoot(false);
			}
		}
		else if (chargingRight && rightCooldown <= 0f)
		{
			if (marblesRemaining == 0)
			{
				chargingRight = false;
			}
			else
			{
				ManageShoot(false);
			}
		}
		else
		{
			chargingRight = false;
			if (rightCooldown > 0f)
			{
				rightCooldown -= Time.deltaTime;
			}
			if (rightCooldown <= 0f)
			{
				velocityRight = 0f;
			}
		}
	}

	private void EndShootLeft()
	{
		shootLeft = false;
		chargingLeft = false;
		velocityLeft = 0f;
	}

	private void EndShootRight()
	{
		shootRight = false;
		chargingRight = false;
		velocityRight = 0f;
	}

	private void ReduceAudioEmptyCooldown()
	{
		if (audioEmptyCooldownLeft > 0f)
		{
			audioEmptyCooldownLeft -= Time.deltaTime;
		}
		if (audioEmptyCooldownRight > 0f)
		{
			audioEmptyCooldownRight -= Time.deltaTime;
		}
	}

	private void PlayEmpty(bool left)
	{
		if (left)
		{
			if (audioEmptyCooldownLeft <= 0f)
			{
				audioEmptyCooldownLeft = 0.5f;
				GlobalAudio.globalAudio.Play(empty, 1f, 1f, -1f);
			}
		}
		else if (audioEmptyCooldownRight <= 0f)
		{
			audioEmptyCooldownRight = 0.5f;
			GlobalAudio.globalAudio.Play(empty, 1f, 1f, 1f);
		}
	}

	private void RaiseVelocity(bool left)
	{
		if (left)
		{
			chargingLeft = true;
			velocityLeft += Time.fixedDeltaTime * 4f;
			if (velocityLeft > 3f && lastFramesAtMaxPowerLeft < 10)
			{
				lastFramesAtMaxPowerLeft++;
				velocityLeft = 3f;
				powerLeft.color = powerColorHot;
			}
			else if (velocityLeft > 3f)
			{
				velocityLeft = Random.Range(0.2f, 3f);
				shootLeft = true;
			}
		}
		else
		{
			chargingRight = true;
			velocityRight += Time.fixedDeltaTime * 4f;
			if (velocityRight > 3f && lastFramesAtMaxPowerRight < 10)
			{
				lastFramesAtMaxPowerRight++;
				velocityRight = 3f;
				powerRight.color = powerColorHot;
			}
			else if (velocityRight > 3f)
			{
				velocityRight = Random.Range(0.2f, 3f);
				shootRight = true;
			}
		}
		PlayCharge(left);
	}

	private void MovePowerIndicators()
	{
		if (velocityLeft == 0f)
		{
			powerLeft.color = powerColor;
			powerLeft.transform.position = new Vector3(-6f, powerLeft.transform.position.y, powerLeft.transform.position.z);
		}
		else
		{
			powerLeft.transform.position = new Vector3(-5.825f + velocityLeft / 3f * 2.7625f, powerLeft.transform.position.y, powerLeft.transform.position.z);
		}
		if (velocityRight == 0f)
		{
			powerRight.color = powerColor;
			powerRight.transform.position = new Vector3(6f, powerRight.transform.position.y, powerRight.transform.position.z);
		}
		else
		{
			powerRight.transform.position = new Vector3(5.825f - velocityRight / 3f * 2.7625f, powerRight.transform.position.y, powerRight.transform.position.z);
		}
	}

	private void ShootMarbleLeft()
	{
		if (lastFramesAtMaxPowerLeft >= 10)
		{
			powerLeft.color = powerColorRandoshoot;
		}
		else if (lastFramesAtMaxPowerLeft > 0)
		{
			powerLeft.color = powerColor;
		}
		lastFramesAtMaxPowerLeft = 0;
		ShootMarble(true);
	}

	private void ShootFollowingMarbleLeft()
	{
		velocityLeft -= velocityLeft / 9f;
		ShootMarble(true);
	}

	private void ShootMarbleRight()
	{
		if (lastFramesAtMaxPowerRight >= 10)
		{
			powerRight.color = powerColorRandoshoot;
		}
		else if (lastFramesAtMaxPowerRight > 0)
		{
			powerRight.color = powerColor;
		}
		lastFramesAtMaxPowerRight = 0;
		ShootMarble(false);
	}

	private void ShootFollowingMarbleRight()
	{
		velocityRight -= velocityRight / 9f;
		ShootMarble(false);
	}

	private void ShootMarble(bool left)
	{
		if (visualMarblesRemaining <= 0)
		{
			return;
		}
		bool flag = false;
		if ((GlobalControl.globalControl.goldenMarbles > 0 && GlobalControl.globalControl.goldenMarbles >= marblesRemaining) || (GlobalControl.globalControl.goldenMarbles > 0 && Random.Range(0f, 1f) < (float)GlobalControl.globalControl.goldenMarbles * 100f / (float)marblesRemaining / 100f))
		{
			newMarble = PCK_MarbleCreator.PCK_marbleCreator.GetMarble(true);
			flag = true;
		}
		else
		{
			newMarble = PCK_MarbleCreator.PCK_marbleCreator.GetMarble(false);
		}
		if (newMarble == null)
		{
			marblesRemaining++;
			if (left)
			{
				leftCooldown = cooldown;
			}
			else
			{
				rightCooldown = cooldown;
			}
			return;
		}
		if (flag)
		{
			GlobalControl.globalControl.goldenMarbles--;
		}
		visualMarblesRemaining--;
		UpdateMarblesCounter();
		if (left)
		{
			directionModifier = -1f;
			vel = velocityLeft;
			leftCooldown = cooldown;
		}
		else
		{
			directionModifier = 1f;
			vel = velocityRight;
			rightCooldown = cooldown;
		}
		if (vel < 0.2f)
		{
			vel = 0.2f;
		}
		if (vel > 3f)
		{
			vel = 3f;
		}
		vel = 9f + vel * 1.2f;
		vel = 8.9f + (vel - 9.24f) * 1.1f;
		if (left)
		{
			pos = shootPointLeft.position;
		}
		else
		{
			pos = shootPointRight.position;
		}
		pos.z = newMarble.transform.position.z;
		newMarble.transform.position = pos;
		newMarble.SetActive(true);
		newMarble.GetComponent<Rigidbody2D>().velocity = new Vector2(vel * (0f - directionModifier), vel);
		PlayRelease(left);
	}

	private void ResetAudioChargeCooldown()
	{
		if (velocityLeft == 0f)
		{
			audioChargeCooldownLeft = 0f;
		}
		if (velocityRight == 0f)
		{
			audioChargeCooldownRight = 0f;
		}
	}

	private void PlayCharge(bool left)
	{
		if (left)
		{
			if (audioChargeCooldownLeft <= 0f)
			{
				pan = -1f;
				pitch = 1f + velocityLeft / 3f;
				audioChargeCooldownLeft = 0.05f;
				GlobalAudio.globalAudio.Play(charge, Random.Range(0.75f, 1f), pitch, pan);
			}
			else
			{
				audioChargeCooldownLeft -= Time.deltaTime;
			}
		}
		else if (audioChargeCooldownRight <= 0f)
		{
			pan = 1f;
			pitch = 1f + velocityRight / 3f;
			audioChargeCooldownRight = 0.05f;
			GlobalAudio.globalAudio.Play(charge, Random.Range(0.75f, 1f), pitch, pan);
		}
		else
		{
			audioChargeCooldownRight -= Time.deltaTime;
		}
	}

	public void UpdateMarblesCounter()
	{
		PCK_CounterController.PCK_counterController.ShowNumber(visualMarblesRemaining);
		GlobalControl.globalControl.marbles = visualMarblesRemaining;
		if (marblesRemaining < 100)
		{
			PauseController.pauseController.inPCK = false;
		}
	}

	private void PlayRelease(bool left)
	{
		if (left)
		{
			pan = -1f;
			pitch = 1f + velocityLeft / 3f;
		}
		else
		{
			pan = 1f;
			pitch = 1f + velocityRight / 3f;
		}
		GlobalAudio.globalAudio.Play(release, 1f, pitch, pan);
	}
}
