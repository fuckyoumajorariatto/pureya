using System.Collections.Generic;
using UnityEngine;

public class FRO_RockSpawner : MonoBehaviour
{
	public static FRO_RockSpawner FRO_rockSpawner;

	private List<GameObject> prefabs;

	private int pooledAmount = 12;

	public GameObject prefab;

	private float frequency;

	private float currentFrequency;

	private float timer;

	private float marbleTimer;

	private int leftCombo;

	private int rightCombo;

	private GameObject obj;

	private GameObject marble;

	private float r;

	private Vector3 pos = new Vector3(0f, 5.5f, -1f);

	private void Start()
	{
		FRO_rockSpawner = this;
		SetDifficultyParas();
		CreatePool();
	}

	private void Update()
	{
		if (FRO_PlayerController.FRO_playerController.hit || SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		if (timer >= currentFrequency)
		{
			CreateBatch();
			timer = 0f;
		}
		if (marbleTimer >= currentFrequency)
		{
			if (Random.Range(0f, 1f) < 0.5f)
			{
				CreateMarble();
			}
			marbleTimer = 0f;
		}
		timer += Time.deltaTime;
		marbleTimer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = FRO_Difficulty.FRO_difficulty.log_frequency;
		currentFrequency = frequency * Random.Range(0.9f, 1.1f);
		timer = 0f;
		marbleTimer = frequency / 2f;
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

	private void CreateBatch()
	{
		pos.z = -2f;
		pos.y = 5.5f;
		r = Random.Range(0f, 1f);
		if (leftCombo >= 2)
		{
			r = 1f;
		}
		else if (rightCombo >= 2)
		{
			r = 0f;
		}
		if (r < 0.5f)
		{
			leftCombo++;
			rightCombo = 0;
			pos.x = -1.5625f;
			ActivatePrefab();
		}
		else
		{
			rightCombo++;
			leftCombo = 0;
			pos.x = 1.5625f;
			ActivatePrefab();
		}
		currentFrequency = frequency * Random.Range(0.9f, 1.1f);
	}

	private void CreateMarble()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			pos.z = -1f;
			float num = Random.Range(0f, 1f);
			if (num < 0.5f)
			{
				pos.x = -1.5625f;
			}
			else
			{
				pos.x = 1.5625f;
			}
			marble = FRO_MarbleSpawner.FRO_marbleSpawner.CreateMarble();
			marble.transform.position = pos;
		}
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
