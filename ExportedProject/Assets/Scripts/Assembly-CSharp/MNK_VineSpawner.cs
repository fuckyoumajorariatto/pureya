using System.Collections.Generic;
using UnityEngine;

public class MNK_VineSpawner : MonoBehaviour
{
	public static MNK_VineSpawner MNK_vineSpawner;

	public List<GameObject> items;

	public int pooledAmount;

	public GameObject item;

	private GameObject lastVine;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		MNK_vineSpawner = this;
		pooledAmount = 9;
		CreatePool();
		CreateInitialBatch();
	}

	private void Update()
	{
		ManageItemCreation();
	}

	private void ManageItemCreation()
	{
		if (lastVine.transform.position.x >= -3.75f)
		{
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
		pos.y = 0f;
		pos.x = lastVine.transform.position.x - 1.25f;
		ActivateItem();
	}

	public void CreateInitialBatch()
	{
		pos.y = 0f;
		pos.x = 4.375f;
		ActivateItem();
		pos.x = 3.125f;
		ActivateItem();
		pos.x = 1.875f;
		ActivateItem();
		pos.x = 0.625f;
		ActivateItem();
		pos.x = -0.625f;
		ActivateItem();
		pos.x = -1.875f;
		ActivateItem();
		pos.x = -3.125f;
		ActivateItem();
		pos.x = -4.375f;
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
			lastVine = obj;
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
