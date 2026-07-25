using UnityEngine;

public class PPR_MarbleSpawner : MonoBehaviour
{
	public static PPR_MarbleSpawner PPR_marbleSpawner;

	private float marbleFrequency;

	private float marbleSpeed;

	private float marbleTimer;

	private Color32 shadowColor = new Color32(52, 52, 52, byte.MaxValue);

	private bool odd = true;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		PPR_marbleSpawner = this;
		SetDifficultyParas();
		marbleTimer = marbleFrequency;
	}

	private void Update()
	{
		ManageMarblesCreation();
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = PPR_Difficulty.PPR_difficulty.marble_frequency;
		marbleSpeed = PPR_Difficulty.PPR_difficulty.marble_speed;
	}

	private void ManageMarblesCreation()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		marbleTimer += Time.deltaTime;
		pos = new Vector3(0f, 5f, -1f);
		if (marbleTimer >= marbleFrequency && !SceneChangeController.sceneChangeController.inTransition)
		{
			marbleTimer = 0f;
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = 5f;
			pos.y = Random.Range(-1f, 3.5f);
			if (odd)
			{
				pos.y *= -1f;
				pos.y += 0.5f;
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
		marbleController.speed = marbleSpeed;
		marbleController.direction = MarbleController.Direction.left;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 5f;
		marble.SetActive(true);
	}
}
