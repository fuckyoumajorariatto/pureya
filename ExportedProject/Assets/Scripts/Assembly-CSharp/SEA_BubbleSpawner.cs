using System.Collections.Generic;
using UnityEngine;

public class SEA_BubbleSpawner : MonoBehaviour
{
	public static SEA_BubbleSpawner SEA_bubbleSpawner;

	private List<GameObject> prefabs;

	private int pooledAmount = 20;

	public GameObject prefab;

	private float frequency;

	private float timer;

	private Vector3 pos = new Vector3(0f, 5f, -1f);

	private GameObject obj;

	private int r;

	private void Start()
	{
		SEA_bubbleSpawner = this;
		SetDifficultyParas();
		CreatePool();
		CreateInitialBatch();
	}

	private void Update()
	{
		if (!SEA_PlayerController.SEA_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (timer >= frequency)
			{
				CreateBatch();
				CreateBatch();
				timer = 0f;
			}
			timer += Time.deltaTime;
		}
	}

	public void SetDifficultyParas()
	{
		frequency = SEA_Difficulty.SEA_difficulty.bubbleFrequency;
		timer = frequency;
	}

	private void CreatePool()
	{
		prefabs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			prefabs.Add(obj);
		}
	}

	public void CreateInitialBatch()
	{
		pos.y = -3f;
		pos.x = 0f;
		ActivatePrefab();
	}

	private void CreateBatch()
	{
		r = Random.Range(0, 57);
		if (Random.Range(0f, 1f) < 0.5f)
		{
			r *= -1;
		}
		float num;
		for (num = 0.0625f * (float)r; num > pos.x - 1f && num < pos.x + 1f; num += 0.5f)
		{
		}
		if (num > 3.5f)
		{
			num -= 7f;
		}
		pos.x = num;
		pos.y = Random.Range(-7f, -6f);
		ActivatePrefab();
	}

	private void ActivatePrefab()
	{
		obj = GetPrefab();
		if (obj != null)
		{
			obj.transform.position = pos;
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

	public void DisableAllPrefabs()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (prefabs[i].activeInHierarchy)
			{
				prefabs[i].SetActive(false);
			}
		}
	}
}
