using System.Collections.Generic;
using UnityEngine;

public class MNK_LightSpawner : MonoBehaviour
{
	public static MNK_LightSpawner MNK_lightSpawner;

	public List<GameObject> items;

	public int pooledAmount;

	public GameObject item;

	private GameObject lastLight;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		MNK_lightSpawner = this;
		pooledAmount = 12;
		CreatePool();
		CreateInitialBatch();
	}

	private void Update()
	{
		ManageItemCreation();
	}

	private void ManageItemCreation()
	{
		if (lastLight.transform.position.x >= -4.5f)
		{
			CreateItem();
			CreateItem();
			CreateItem();
		}
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

	private void CreateItem()
	{
		pos.y = Random.Range(-4f, 3f);
		pos.x = Random.Range(-7f, -5f);
		ActivateItem();
	}

	public void CreateInitialBatch()
	{
		for (int i = 0; i < 10; i++)
		{
			pos.y = Random.Range(-4f, 3f);
			pos.x = Random.Range(-4f, 4f);
			ActivateItem();
		}
		CreateItem();
	}

	private void ActivateItem()
	{
		obj = GetItem();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
			lastLight = obj;
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
