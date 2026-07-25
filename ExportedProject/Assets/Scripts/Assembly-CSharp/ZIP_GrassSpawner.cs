using System.Collections.Generic;
using UnityEngine;

public class ZIP_GrassSpawner : MonoBehaviour
{
	public static ZIP_GrassSpawner ZIP_grassSpawner;

	private List<GameObject> items;

	private int pooledAmount = 50;

	public GameObject item;

	private float frequency;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		ZIP_grassSpawner = this;
		SetDifficultyParas();
		CreatePool();
		CreateInitialBatch();
	}

	private void Update()
	{
		if (timer >= frequency && !ZIP_PlayerController.ZIP_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = 0.1f;
		timer = frequency;
	}

	private void CreatePool()
	{
		items = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(item);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			items.Add(obj);
		}
	}

	private void CreateInitialBatch()
	{
		for (float num = -4f; num < 5f; num += 0.3f)
		{
			pos.y = num;
			pos.x = Random.Range(-4f, 4f);
			ActivateItem();
		}
	}

	private void CreateBatch()
	{
		pos.y = 5f;
		pos.x = Random.Range(-4f, 4f);
		ActivateItem();
	}

	private void ActivateItem()
	{
		obj = GetItem();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetItem()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!items[i].activeInHierarchy)
			{
				return items[i];
			}
		}
		return null;
	}
}
