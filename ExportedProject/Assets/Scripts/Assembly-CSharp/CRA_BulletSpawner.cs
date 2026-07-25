using System.Collections.Generic;
using UnityEngine;

public class CRA_BulletSpawner : MonoBehaviour
{
	private List<GameObject> objs;

	private int pooledAmount = 40;

	public GameObject prefab;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		CreatePool();
	}

	private void CreatePool()
	{
		objs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			objs.Add(obj);
		}
	}

	public GameObject GetObj()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!objs[i].activeInHierarchy)
			{
				return objs[i];
			}
		}
		return null;
	}
}
