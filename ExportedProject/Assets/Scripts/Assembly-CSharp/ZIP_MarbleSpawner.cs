using UnityEngine;

public class ZIP_MarbleSpawner : MonoBehaviour
{
	public static ZIP_MarbleSpawner ZIP_marbleSpawner;

	public float marbleFrequency;

	private float marbleSpeed;

	private float marbleSpeedSelected;

	private int marblesSpawnType;

	public float marbleTimer;

	private Color32 shadowColor = new Color32(73, 73, 73, byte.MaxValue);

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		ZIP_marbleSpawner = this;
		SetDifficultyParas();
		marbleTimer = marbleFrequency;
	}

	private void Update()
	{
		ManageMarblesCreation();
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = ZIP_Difficulty.ZIP_difficulty.marble_frequency;
		marbleSpeed = ZIP_Difficulty.ZIP_difficulty.scroll_speed;
		marblesSpawnType = ZIP_Difficulty.ZIP_difficulty.marbles_spawn_type;
	}

	private void ManageMarblesCreation()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		marbleTimer += Time.deltaTime;
		if (!(marbleTimer >= marbleFrequency))
		{
			return;
		}
		marbleTimer = 0f;
		if (marblesSpawnType == 1)
		{
			pos = new Vector3(0f, 0f, 3f);
			pos.x = Random.Range(-3.5f, 3.5f);
			pos.y = Random.Range(4.5f, 6.9f);
			marbleSpeedSelected = marbleSpeed * Random.Range(1.1f, 1.3f);
			marble = MarbleCreator.marbleCreator.GetMarble();
			marble.transform.position = pos;
			SetMarbleConstants(marble);
		}
		else if (marblesSpawnType == 2)
		{
			if (pos.x > 0f)
			{
				pos.x = Random.Range(-3.5f, pos.x - 2f);
			}
			else
			{
				pos.x = Random.Range(pos.x + 2f, 3.5f);
			}
			pos.y = Random.Range(4.5f, 6.9f);
			marbleSpeedSelected = marbleSpeed * Random.Range(1.1f, 1.3f);
			marble = MarbleCreator.marbleCreator.GetMarble();
			marble.transform.position = pos;
			SetMarbleConstants(marble);
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = marbleSpeedSelected;
		marbleController.direction = MarbleController.Direction.down;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_right;
		marbleController.maxOut = 7f;
		marble.SetActive(true);
	}
}
