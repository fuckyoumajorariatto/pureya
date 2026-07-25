using UnityEngine;

public class SEA_MarbleSpawner : MonoBehaviour
{
	public static SEA_MarbleSpawner SEA_marbleSpawner;

	private float marbleFrequency;

	private float marbleSpeed;

	private float marbleTimer;

	private Color32 shadowColor = new Color32(21, 21, 21, byte.MaxValue);

	private bool odd = true;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		SEA_marbleSpawner = this;
		SetDifficultyParas();
		marbleTimer = marbleFrequency;
	}

	private void Update()
	{
		ManageMarblesCreation();
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = SEA_Difficulty.SEA_difficulty.marbleFrequency;
		marbleSpeed = SEA_Difficulty.SEA_difficulty.speed;
		marbleSpeed = Random.Range(marbleSpeed - marbleSpeed / 4f, marbleSpeed + marbleSpeed / 4f);
	}

	private void ManageMarblesCreation()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		marbleTimer += Time.deltaTime;
		pos = new Vector3(0f, -5f, 1f);
		if (marbleTimer >= marbleFrequency && !SEA_PlayerController.SEA_playerController.hit)
		{
			marbleTimer = 0f;
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.y = -5f;
			pos.x = Random.Range(-3f, -1f);
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
		marbleController.speed = marbleSpeed;
		marbleController.direction = MarbleController.Direction.up;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}
}
