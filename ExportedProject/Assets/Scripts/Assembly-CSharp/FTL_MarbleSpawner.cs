using UnityEngine;

public class FTL_MarbleSpawner : MonoBehaviour
{
	public static FTL_MarbleSpawner FTL_marbleSpawner;

	private float marbleSpeed;

	private Color32 shadowColor = new Color32(19, 19, 19, byte.MaxValue);

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		FTL_marbleSpawner = this;
		SetDifficultyParas();
	}

	public void SetDifficultyParas()
	{
		marbleSpeed = FTL_Difficulty.FTL_difficulty.speed;
	}

	public void CreateMarble(float x, float y)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			pos = new Vector3(0f, 5f, 0f);
			if (!SceneChangeController.sceneChangeController.inTransition)
			{
				marble = MarbleCreator.marbleCreator.GetMarble();
				pos.x = x;
				pos.y = y;
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
		marbleController.speed = marbleSpeed;
		marbleController.direction = MarbleController.Direction.left;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 5f;
		marble.SetActive(true);
	}
}
