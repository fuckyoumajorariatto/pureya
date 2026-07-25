using UnityEngine;

public class MNK_MarbleSpawner : MonoBehaviour
{
	public static MNK_MarbleSpawner MNK_marbleSpawner;

	private Color32 shadowColor = new Color32(38, 38, 38, byte.MaxValue);

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		MNK_marbleSpawner = this;
	}

	public GameObject CreateMarble(float x, float y)
	{
		pos = new Vector3(0f, 5f, -1f);
		if (!SceneChangeController.sceneChangeController.inTransition)
		{
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = x;
			pos.y = y;
			marble.transform.position = pos;
			SetMarbleConstants(marble);
		}
		return marble;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = 0f;
		marbleController.direction = MarbleController.Direction.down;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 8f;
		marbleController.tr.localScale = new Vector3(1f, 1f, 1f);
		marble.SetActive(true);
	}
}
