using System.Collections.Generic;
using UnityEngine;

public class ZIP_TreeSpawner : MonoBehaviour
{
	public static ZIP_TreeSpawner ZIP_treeSpawner;

	private List<GameObject> items;

	private int pooledAmount = 10;

	public GameObject item;

	private float frequency;

	private int maxItems;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private float x;

	private void Start()
	{
		ZIP_treeSpawner = this;
		SetDifficultyParas();
		CreatePool();
		CreateInitialBatch();
		timer = frequency;
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
		frequency = ZIP_Difficulty.ZIP_difficulty.tree_frequency;
		maxItems = ZIP_Difficulty.ZIP_difficulty.tree_max;
		timer = 0f;
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

	public void CreateInitialBatch()
	{
		pos.y = Random.Range(2f, 4f);
		pos.x = Random.Range(-4f, 4f);
		RemoveEdgeUncertainty();
		ActivateItem();
		if (maxItems > 1)
		{
			pos.y = Random.Range(2f, 4f);
			if (pos.x > 1f)
			{
				pos.x = Random.Range(-4f, -0.5f);
			}
			else if (pos.x < -1f)
			{
				pos.x = Random.Range(0.5f, 4f);
			}
			else if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x = Random.Range(2.5f, 4f);
			}
			else
			{
				pos.x = Random.Range(-4f, -2.5f);
			}
			RemoveEdgeUncertainty();
			ActivateItem();
		}
	}

	private void CreateBatch()
	{
		if (maxItems == 1)
		{
			pos.y = 6.5f;
			pos.x = Random.Range(-4f, 4f);
			RemoveEdgeUncertainty();
			ActivateItem();
			return;
		}
		if (maxItems < 3 || Random.Range(0f, 1f) < 0.75f)
		{
			pos.y = Random.Range(6.5f, 7.5f);
			pos.x = Random.Range(-4f, 4f);
			RemoveEdgeUncertainty();
			ActivateItem();
			pos.y = Random.Range(6.5f, 8.5f);
			if (pos.x > 0f)
			{
				pos.x = Random.Range(-4f, pos.x - 2f);
			}
			else if (pos.x < 0f)
			{
				pos.x = Random.Range(pos.x + 2f, 4f);
			}
			else if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x = Random.Range(2f, 4f);
			}
			else
			{
				pos.x = Random.Range(-4f, -2f);
			}
			RemoveEdgeUncertainty();
			ActivateItem();
			return;
		}
		pos.y = Random.Range(6.5f, 7.5f);
		pos.x = Random.Range(-4f, -1.5f);
		RemoveEdgeUncertainty();
		ActivateItem();
		pos.y = Random.Range(6.5f, 7.5f);
		x = Random.Range(pos.x + 2f, 1.5f);
		if (x < pos.x + 1.5f)
		{
			x = pos.x + 1.5f;
		}
		pos.x = x;
		ActivateItem();
		pos.y = Random.Range(6.5f, 7.5f);
		x = Random.Range(pos.x + 2f, 4f);
		if (x < pos.x + 1.5f)
		{
			x = pos.x + 1.5f;
		}
		pos.x = x;
		RemoveEdgeUncertainty();
		ActivateItem();
	}

	private void RemoveEdgeUncertainty()
	{
		if (pos.x < -2.75f)
		{
			pos.x = -3.5f;
		}
		else if (pos.x > 2.75f)
		{
			pos.x = 3.5f;
		}
	}

	private void ActivateItem()
	{
		obj = GetItem();
		if (obj != null)
		{
			pos.z = 5f;
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
