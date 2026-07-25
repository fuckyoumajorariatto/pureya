using System.Collections.Generic;
using UnityEngine;

public class SNW_RockCreator : MonoBehaviour
{
	public static SNW_RockCreator SNW_rockCreator;

	public List<GameObject> rocks;

	public int pooledAmount;

	public GameObject rock;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		SNW_rockCreator = this;
		pooledAmount = 20;
		CreatePool();
	}

	private void CreatePool()
	{
		rocks = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(rock);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			rocks.Add(obj);
		}
	}

	public GameObject GetRock()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!rocks[i].activeInHierarchy)
			{
				return rocks[i];
			}
		}
		return null;
	}
}
