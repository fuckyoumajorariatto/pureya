using UnityEngine;

public class PNG_MarbleSpawner : MonoBehaviour
{
	public static PNG_MarbleSpawner PNG_marbleSpawner;

	private float marbleSpeed;

	private Color32 shadowColor = new Color32(55, 55, 55, byte.MaxValue);

	private float speed;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		PNG_marbleSpawner = this;
	}

	public GameObject CreateMarble()
	{
		marble = MarbleCreator.marbleCreator.GetMarble();
		SetMarbleConstants(marble);
		return marble;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		speed = PNG_Difficulty.PNG_difficulty.speed;
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = speed;
		marbleController.direction = MarbleController.Direction.left;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_right;
		marbleController.maxOut = 18f;
		marble.SetActive(true);
	}
}
