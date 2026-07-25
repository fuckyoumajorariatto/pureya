using System.Collections.Generic;
using UnityEngine;

public class FRO_LilypadSpawner : MonoBehaviour
{
	public static FRO_LilypadSpawner FRO_lilypadSpawner;

	private List<GameObject> prefabs;

	private int pooledAmount = 12;

	public GameObject prefab;

	public float frequency;

	private float timer;

	public bool odd;

	private int noMarbleCombo;

	private Vector3 pos;

	public bool fistBatch = true;

	public Transform tr;

	private GameObject obj;

	private GameObject marble;

	private List<Transform> possibleLilys;

	private Transform prefabTR;

	private Transform closestTR;

	private void Start()
	{
		FRO_lilypadSpawner = this;
		odd = false;
		CreatePool();
		CreateInitialPrefabs();
	}

	private void Update()
	{
		if (!FRO_PlayerController.FRO_playerController.hit)
		{
			if (timer >= frequency)
			{
				CreateBatch();
				timer = 0f;
			}
			timer += Time.deltaTime;
		}
	}

	public void SetDifficultyParas()
	{
		frequency = FRO_Difficulty.FRO_difficulty.lilypad_frequency;
		if (fistBatch)
		{
			timer = frequency;
			fistBatch = false;
		}
		else
		{
			timer = 0f;
		}
	}

	private void CreatePool()
	{
		prefabs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			prefabs.Add(obj);
		}
	}

	private void CreateBatch()
	{
		pos.z = -1f;
		pos.y = 5f;
		if (odd)
		{
			pos.x = 0f;
			ActivatePrefab();
		}
		else
		{
			pos.x = -3.125f;
			ActivatePrefab();
			pos.x = 3.125f;
			ActivatePrefab();
			if ((Random.Range(0f, 1f) < 0.4f || noMarbleCombo > 1) && !SceneChangeController.sceneChangeController.transitioning)
			{
				noMarbleCombo = 0;
				if (Random.Range(0f, 1f) < 0.5f)
				{
					pos.x *= -1f;
				}
				marble = FRO_MarbleSpawner.FRO_marbleSpawner.CreateMarbleAlt();
				pos.z = -3f;
				marble.transform.position = pos;
			}
			else
			{
				noMarbleCombo++;
			}
		}
		odd = !odd;
	}

	public void CreateInitialPrefabs()
	{
		pos.z = -1f;
		pos.y = 3f;
		pos.x = 0f;
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

	public Transform GetTargetLilypad(Transform player, bool left)
	{
		possibleLilys = new List<Transform>();
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!prefabs[i].activeInHierarchy)
			{
				continue;
			}
			prefabTR = prefabs[i].transform;
			if (!(prefabTR.position.y > player.position.y))
			{
				continue;
			}
			if (left)
			{
				if (prefabTR.position.x < player.position.x)
				{
					possibleLilys.Add(prefabTR);
				}
			}
			else if (prefabTR.position.x > player.position.x)
			{
				possibleLilys.Add(prefabTR);
			}
		}
		if (possibleLilys.Count == 0)
		{
			return null;
		}
		closestTR = possibleLilys[0];
		for (int j = 1; j < possibleLilys.Count; j++)
		{
			if (possibleLilys[j].position.y < closestTR.position.y)
			{
				closestTR = possibleLilys[j];
			}
		}
		return closestTR;
	}
}
