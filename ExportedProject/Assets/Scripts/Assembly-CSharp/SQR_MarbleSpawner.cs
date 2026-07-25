using UnityEngine;

public class SQR_MarbleSpawner : MonoBehaviour
{
	public static SQR_MarbleSpawner SQR_marbleSpawner;

	private float marbleFrequency;

	private float marbleSpeed;

	private float marbleTimer;

	private Color32 shadowColor = new Color32(41, 41, 41, byte.MaxValue);

	private bool odd = true;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private float speed;

	private void Start()
	{
		SQR_marbleSpawner = this;
		SetDifficultyParas();
		marbleTimer = marbleFrequency;
	}

	private void Update()
	{
		ManageMarblesCreation();
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = SQR_Difficulty.SQR_difficulty.marble_frequency;
		marbleSpeed = SQR_Difficulty.SQR_difficulty.scroll_speed;
	}

	private void ManageMarblesCreation()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		marbleTimer += Time.deltaTime;
		pos = new Vector3(0f, -5f, 1f);
		if (marbleTimer >= marbleFrequency && !SceneChangeController.sceneChangeController.inTransition)
		{
			marbleTimer = 0f;
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = Random.Range(0f, 3.5f);
			if (odd)
			{
				pos.x *= -1f;
				odd = false;
			}
			else
			{
				odd = true;
			}
			marble.transform.position = pos;
			SetMarbleConstants(marble);
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		speed = marbleSpeed;
		if (Random.Range(0f, 1f) < 0.5f)
		{
			speed = marbleSpeed + 1f;
		}
		else
		{
			speed = marbleSpeed - 1f;
		}
		marbleController.speed = speed;
		marbleController.direction = MarbleController.Direction.up;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marbleController.maxOut = 5f;
		marble.SetActive(true);
	}
}
