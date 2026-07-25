using UnityEngine;

public class GOT_MarbleSpawner : MonoBehaviour
{
	public static GOT_MarbleSpawner GOT_marbleSpawner;

	private float marbleSpeed;

	private Color32 shadowColor = new Color32(62, 62, 62, byte.MaxValue);

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		GOT_marbleSpawner = this;
	}

	public GameObject CreateMarble()
	{
		marble = MarbleCreator.marbleCreator.GetMarble();
		SetMarbleConstants(marble);
		return marble;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = GOT_Difficulty.GOT_difficulty.speed;
		marbleController.direction = MarbleController.Direction.down;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}
}
