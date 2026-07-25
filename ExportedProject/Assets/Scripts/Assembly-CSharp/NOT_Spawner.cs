using UnityEngine;

public class NOT_Spawner : MonoBehaviour
{
	public float marbleTimer;

	private Color32 shadowColor = new Color32(36, 36, 36, byte.MaxValue);

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	public NOT_PlayerController playerController;

	public int existingMarbles;

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
		if (marbleTimer >= 2f)
		{
			marbleTimer = 0f;
			if (GameObject.FindGameObjectsWithTag("marble").Length < 4)
			{
				marble = MarbleCreator.marbleCreator.GetMarble();
				pos.x = Random.Range(-3.5f, 3.5f);
				pos.y = Random.Range(0f, 3.5f);
				marble.transform.position = pos;
				SetMarbleConstants(marble);
				existingMarbles++;
			}
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = 0f;
		marbleController.direction = MarbleController.Direction.down;
		marbleController.maxOut = 4.5f;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marble.SetActive(true);
	}
}
