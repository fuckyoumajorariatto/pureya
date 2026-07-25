using UnityEngine;

public class FRO_MarbleSpawner : MonoBehaviour
{
	public static FRO_MarbleSpawner FRO_marbleSpawner;

	private float marbleSpeed;

	private Color32 shadowColor = new Color32(55, 55, 55, byte.MaxValue);

	private float speed;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		FRO_marbleSpawner = this;
	}

	public GameObject CreateMarble()
	{
		marble = MarbleCreator.marbleCreator.GetMarble();
		SetMarbleConstants(marble);
		return marble;
	}

	public void SetDifficultyParas()
	{
		speed = FRO_Difficulty.FRO_difficulty.speed;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = speed * 2.5f;
		marbleController.direction = MarbleController.Direction.down;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}

	public GameObject CreateMarbleAlt()
	{
		marble = MarbleCreator.marbleCreator.GetMarble();
		SetMarbleConstantsAlt(marble);
		return marble;
	}

	private void SetMarbleConstantsAlt(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = speed;
		marbleController.direction = MarbleController.Direction.down;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}
}
