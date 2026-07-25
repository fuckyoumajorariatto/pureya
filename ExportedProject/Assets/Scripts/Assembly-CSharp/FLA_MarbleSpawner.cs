using UnityEngine;

public class FLA_MarbleSpawner : MonoBehaviour
{
	public static FLA_MarbleSpawner FLA_marbleSpawner;

	private float marbleSpeed;

	private Color32 shadowColor = new Color32(77, 77, 77, byte.MaxValue);

	private void Start()
	{
		FLA_marbleSpawner = this;
		SetDifficultyParas();
	}

	public void SetDifficultyParas()
	{
		marbleSpeed = FLA_Difficulty.FLA_difficulty.scroll_speed;
	}

	public void CreateMarble(Vector3 pos)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GameObject marble = MarbleCreator.marbleCreator.GetMarble();
			marble.transform.position = pos;
			SetMarbleConstants(marble);
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marble.GetComponent<MarbleController>().shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marble.GetComponent<MarbleController>().speed = marbleSpeed;
		marble.GetComponent<MarbleController>().direction = MarbleController.Direction.right;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marble.GetComponent<MarbleController>().maxOut = 8f;
		marble.SetActive(true);
	}
}
