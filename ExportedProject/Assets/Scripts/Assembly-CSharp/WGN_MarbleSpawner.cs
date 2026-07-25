using UnityEngine;

public class WGN_MarbleSpawner : MonoBehaviour
{
	private float marbleTimer;

	private Color32 shadowColor = new Color32(36, 36, 36, byte.MaxValue);

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	public WGN_PlayerController playerController;

	private void Update()
	{
		ManageMarblesCreation();
	}

	private void ManageMarblesCreation()
	{
		if (SceneChangeController.sceneChangeController.transitioning || playerController.hit)
		{
			return;
		}
		marbleTimer += Time.deltaTime;
		pos = new Vector3(0f, 5f, -1f);
		if (marbleTimer >= WGN_Difficulty.WGN_difficulty.marbleFrequency)
		{
			marbleTimer = 0f;
			if (Random.Range(1, WGN_Difficulty.WGN_difficulty.max + 1) < 2)
			{
				marble = MarbleCreator.marbleCreator.GetMarble();
				pos.x = Random.Range(-3.75f, 3.75f);
				marble.transform.position = pos;
				SetMarbleConstants(marble);
				return;
			}
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = Random.Range(-3.75f, -1f);
			pos.y = Random.Range(5f, 6.9f);
			marble.transform.position = pos;
			SetMarbleConstants(marble);
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = Random.Range(1f, 3.75f);
			pos.y = Random.Range(5f, 6.9f);
			marble.transform.position = pos;
			SetMarbleConstants(marble);
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = Random.Range(WGN_Difficulty.WGN_difficulty.rockSpeed, WGN_Difficulty.WGN_difficulty.rockSpeed * 1.25f);
		marbleController.direction = MarbleController.Direction.down;
		marbleController.maxOut = 7f;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_right;
		marble.SetActive(true);
	}
}
