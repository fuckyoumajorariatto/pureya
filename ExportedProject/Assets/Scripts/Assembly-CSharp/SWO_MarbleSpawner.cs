using System.Collections.Generic;
using UnityEngine;

public class SWO_MarbleSpawner : MonoBehaviour
{
	public static SWO_MarbleSpawner SWO_marbleSpawner;

	private List<GameObject> prefabs;

	private int pooledAmount = 10;

	public GameObject prefab;

	public Transform target;

	private float speed;

	private Vector3 pos;

	private GameObject container;

	private SWO_MarbleContainerController controller;

	private GameObject marble;

	private Transform marbleTR;

	private MarbleController marbleController;

	private GameObject obj;

	private Color32 shadowColor = new Color32(52, 52, 52, byte.MaxValue);

	private void Start()
	{
		SWO_marbleSpawner = this;
		CreatePool();
	}

	public void CreateMarble(float spd, Vector3 position)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			speed = spd;
			pos = position;
			container = ActivatePrefab();
			controller = container.GetComponent<SWO_MarbleContainerController>();
			marble = MarbleCreator.marbleCreator.GetMarble();
			marbleTR = marble.transform;
			marbleTR.parent = container.transform;
			SetMarbleConstants();
			marbleTR.localPosition = Vector3.zero;
			controller.marble = marble;
		}
	}

	private void SetMarbleConstants()
	{
		marbleTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = 0f;
		marbleController.direction = MarbleController.Direction.none;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}

	private void CreatePool()
	{
		prefabs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject gameObject = Object.Instantiate(prefab);
			gameObject.SetActive(false);
			gameObject.transform.parent = base.gameObject.transform;
			prefabs.Add(gameObject);
		}
	}

	private GameObject ActivatePrefab()
	{
		obj = GetPrefab();
		if (obj != null)
		{
			controller = obj.GetComponent<SWO_MarbleContainerController>();
			controller.target = target;
			controller.moveSpeed = speed;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
		return obj;
	}

	private GameObject GetPrefab()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!prefabs[i].activeInHierarchy)
			{
				return prefabs[i];
			}
		}
		Debug.Log("SWO: No more marble-container available. Returned null.");
		return null;
	}
}
