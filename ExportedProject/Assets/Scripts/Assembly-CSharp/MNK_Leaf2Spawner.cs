using System.Collections.Generic;
using UnityEngine;

public class MNK_Leaf2Spawner : MonoBehaviour
{
	public static MNK_Leaf2Spawner MNK_leaf2Spawner;

	public List<GameObject> items;

	public int pooledAmount;

	public GameObject item;

	private GameObject lastLeaf;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		MNK_leaf2Spawner = this;
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
		if (lastLeaf.transform.position.x >= -4f)
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
		pos.x = lastLeaf.transform.position.x - 1f;
		ActivateItem();
	}

	public void CreateInitialBatch()
	{
		pos.x = 4.5f;
		ActivateItem();
		pos.x = 3.5f;
		ActivateItem();
		pos.x = 2.5f;
		ActivateItem();
		pos.x = 1.5f;
		ActivateItem();
		pos.x = 0.5f;
		ActivateItem();
		pos.x = -0.5f;
		ActivateItem();
		pos.x = -1.5f;
		ActivateItem();
		pos.x = -2.5f;
		ActivateItem();
		pos.x = -3.5f;
		ActivateItem();
		pos.x = -4.5f;
		ActivateItem();
	}

	private void ActivateItem()
	{
		obj = GetItem();
		if (obj != null)
		{
			pos.y = obj.transform.position.y;
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
			lastLeaf = obj;
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
