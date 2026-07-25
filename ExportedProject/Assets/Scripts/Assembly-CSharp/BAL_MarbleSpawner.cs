using UnityEngine;

public class BAL_MarbleSpawner : MonoBehaviour
{
	public static BAL_MarbleSpawner BAL_marbleSpawner;

	private float marbleFrequency;

	private float marbleSpeed;

	private float marbleTimer;

	private Color32 shadowColor = new Color32(61, 61, 61, byte.MaxValue);

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		BAL_marbleSpawner = this;
		SetDifficultyParas();
		marbleTimer = marbleFrequency;
	}

	private void Update()
	{
		ManageMarblesCreation();
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = BAL_Difficulty.BAL_difficulty.marbleFrequency;
		marbleSpeed = BAL_Difficulty.BAL_difficulty.marbleSpeed;
	}

	private void ManageMarblesCreation()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			marbleTimer += Time.deltaTime;
			pos = new Vector3(0f, -5f, 0f);
			if (marbleTimer >= marbleFrequency && !SceneChangeController.sceneChangeController.inTransition && !BAL_PlayerController.BAL_playerController.hit)
			{
				marbleTimer = 0f;
				marble = MarbleCreator.marbleCreator.GetMarble();
				pos.x = Random.Range(-8f, -5f);
				pos.y = Random.Range(-2.5f, 2.5f);
				marble.transform.position = pos;
				SetMarbleConstants(marble);
			}
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = Random.Range(marbleSpeed * 0.75f, marbleSpeed * 1.25f);
		marbleController.direction = MarbleController.Direction.right;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 8f;
		marble.SetActive(true);
	}
}
