using System.Collections.Generic;
using UnityEngine;

public class SWO_ArrowFarSpawner : MonoBehaviour
{
	public Transform containerTR;

	private List<GameObject> prefabs;

	private int pooledAmount = 10;

	public GameObject prefab;

	private GameObject obj;

	private SWO_ArrowFarController arrowController;

	private float frequency = 0.5f;

	private float timer;

	private void Start()
	{
		CreatePool();
		timer = frequency;
	}

	private void Update()
	{
		if (!SWO_PlayerController.SWO_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (timer >= frequency)
			{
				ActivatePrefab();
				timer = 0f;
			}
			timer += Time.deltaTime;
		}
	}

	private void CreatePool()
	{
		prefabs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = containerTR;
			prefabs.Add(obj);
		}
	}

	private void ActivatePrefab()
	{
		obj = GetPrefab();
		if (obj != null)
		{
			arrowController = obj.GetComponent<SWO_ArrowFarController>();
			arrowController.speed = Random.Range(SWO_Difficulty.SWO_difficulty.arrowSpeed / 2f, SWO_Difficulty.SWO_difficulty.arrowSpeed * 2f);
			arrowController.tr.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, -1.5f), arrowController.tr.position.z);
			arrowController.tr.rotation = Quaternion.Euler(0f, 0f, Random.Range(-10f, 10f));
			obj.SetActive(true);
		}
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
		return null;
	}
}
