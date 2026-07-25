using UnityEngine;

public class PLU_MarbleSpawner : MonoBehaviour
{
	public PLU_PlayerController playerController;

	private float marbleFrequency;

	private int marbleMax;

	private float marbleSpeed;

	private float marbleTimer;

	private Color32 shadowColor = new Color32(120, 120, 120, byte.MaxValue);

	private float lastMarblePosX;

	private int direction = 1;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		SetDifficultyParas();
		marbleTimer = marbleFrequency;
	}

	private void Update()
	{
		ManageMarblesCreation();
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = PLU_Difficulty.PLU_difficulty.marble_frequency;
		marbleSpeed = PLU_Difficulty.PLU_difficulty.marble_speed;
	}

	private void ManageMarblesCreation()
	{
		if (SceneChangeController.sceneChangeController.transitioning || playerController.hit)
		{
			return;
		}
		if (Random.Range(0, 7) == 0 && lastMarblePosX > -2f && lastMarblePosX < 2f)
		{
			direction *= -1;
		}
		marbleTimer += Time.deltaTime;
		pos = new Vector3(0f, -5f, 1f);
		if (marbleTimer >= marbleFrequency)
		{
			marbleTimer = 0f;
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = Random.Range(0f, 2f * (float)direction);
			pos.x = lastMarblePosX + (float)direction;
			if (pos.x <= -3.5f)
			{
				pos.x = -3.5f;
				direction *= -1;
			}
			else if (pos.x >= 3.5f)
			{
				pos.x = 3.5f;
				direction *= -1;
			}
			marble.transform.position = pos;
			SetMarbleConstants(marble);
			lastMarblePosX = pos.x;
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = marbleSpeed;
		marbleController.direction = MarbleController.Direction.up;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marble.SetActive(true);
	}
}
