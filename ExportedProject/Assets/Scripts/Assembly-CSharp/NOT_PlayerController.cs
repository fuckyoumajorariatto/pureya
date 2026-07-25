using UnityEngine;

public class NOT_PlayerController : MonoBehaviour
{
	public NOT_Spawner spawner;

	public NOT_Reset reset;

	public Transform renderCameraTR;

	private bool bothButtons;

	private float doubleMoveSoundCooldown;

	private int maxSpeed = 15;

	private int acceleration = 60;

	public float currentSpeed;

	private float deceleration = 70f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	public Collider2D coll;

	private Vector3 pos;

	private void Start()
	{
		tr = base.transform;
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
			if (doubleMoveSoundCooldown > 0f)
			{
				doubleMoveSoundCooldown -= Time.deltaTime;
			}
		}
	}

	private void ManageInput()
	{
		receivingInput = false;
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right) || hit)
		{
			bothButtons = true;
			if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
			{
				doubleMoveSoundCooldown = 0.25f;
			}
			return;
		}
		if (GlobalInput.globalInput.left)
		{
			if (currentSpeed > (float)(-maxSpeed))
			{
				currentSpeed -= (float)acceleration * Time.deltaTime;
			}
		}
		else if (GlobalInput.globalInput.right)
		{
			if (currentSpeed < (float)maxSpeed)
			{
				currentSpeed += (float)acceleration * Time.deltaTime;
			}
		}
		else
		{
			tr.localScale = new Vector3(1f, 1f, tr.localScale.z);
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			receivingInput = true;
		}
		if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
		{
			bothButtons = false;
		}
		else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
		{
			bothButtons = false;
			if (!(doubleMoveSoundCooldown > 0f))
			{
			}
		}
	}

	private void MovePlayer()
	{
		if (!hit)
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
			if (tr.position.x < -4.625f)
			{
				tr.position = new Vector2(tr.position.x + 9.25f, tr.position.y);
			}
			else if (tr.position.x > 4.625f)
			{
				tr.position = new Vector2(tr.position.x - 9.25f, tr.position.y);
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

	public void ManageCollision()
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				reset.reset = true;
			}
			screenShakeRemainingTime = 0.4f;
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
			pos = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	public void DoReset()
	{
		tr.position = new Vector3(0f, -3.3125f, 0f);
		spawner.existingMarbles = 0;
		spawner.marbleTimer = 0f;
		hit = false;
	}
}
