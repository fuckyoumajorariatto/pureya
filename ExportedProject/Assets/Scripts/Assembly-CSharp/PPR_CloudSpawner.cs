using System.Collections.Generic;
using UnityEngine;

public class PPR_CloudSpawner : MonoBehaviour
{
	public static PPR_CloudSpawner PPR_cloudSpawner;

	private List<GameObject> clouds;

	private int pooledAmount = 8;

	public GameObject cloud;

	private float frequency;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private float y;

	private void Start()
	{
		PPR_cloudSpawner = this;
		SetDifficultyParas();
		CreatePool();
		CreateInitialBatch();
	}

	private void Update()
	{
		if (timer >= frequency && !PPR_PlayerController.PPR_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = PPR_Difficulty.PPR_difficulty.balloon_frequency;
		timer = frequency;
	}

	private void CreatePool()
	{
		clouds = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(cloud);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			clouds.Add(obj);
		}
	}

	private void CreateBatch()
	{
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(6f, 8f);
		ActivateCloud();
		if (!(Random.Range(0f, 1f) < 0.5f))
		{
			return;
		}
		y = Random.Range(-4f, 4f);
		if ((y > pos.y && y < pos.y + 2f) || (y < pos.y && y > pos.y - 2f) || y == pos.y)
		{
			if (pos.y <= 0f)
			{
				y = pos.y + 3f;
			}
			else
			{
				y = pos.y - 3f;
			}
		}
		pos.y = y;
		pos.x = Random.Range(5f, 7f);
		ActivateCloud();
	}

	private void CreateInitialBatch()
	{
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(-5f, -2f);
		ActivateCloud();
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(-5f, -2f);
		ActivateCloud();
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(-1f, 2f);
		ActivateCloud();
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(-1f, 2f);
		ActivateCloud();
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(3f, 6f);
		ActivateCloud();
		pos.y = Random.Range(-4f, 4f);
		pos.x = Random.Range(3f, 6f);
		ActivateCloud();
	}

	private void ActivateCloud()
	{
		obj = GetCloud();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetCloud()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!clouds[i].activeInHierarchy)
			{
				return clouds[i];
			}
		}
		return null;
	}
}
