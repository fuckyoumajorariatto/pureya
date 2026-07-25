using System.Collections.Generic;
using UnityEngine;

public class GOT_RockSpawner : MonoBehaviour
{
	public static GOT_RockSpawner GOT_rockSpawner;

	private List<GameObject> rocks;

	private int pooledAmount = 12;

	public GameObject rock;

	private float frequency;

	private float timer;

	private float[] horizontalPositions = new float[5] { -3.25f, -1.625f, 0f, 1.625f, 3.5f };

	private int lastX1;

	private int lastX2;

	private int index1;

	private int index2;

	private Vector3 pos;

	private GameObject obj;

	private GameObject marble;

	private void Start()
	{
		GOT_rockSpawner = this;
		SetDifficultyParas();
		CreatePool();
		CreateInitialRocks();
	}

	private void Update()
	{
		if (timer >= frequency && !GOT_PlayerController.GOT_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateRockBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = GOT_Difficulty.GOT_difficulty.rock_frequency;
		timer = frequency;
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

	private void CreateRockBatch()
	{
		pos.y = 5f;
		index1 = Random.Range(0, 5);
		while (index1 == lastX1 || index1 == lastX2 || Mathf.Abs(lastX1 - index1) == 4)
		{
			index1++;
			if (index1 > 4)
			{
				index1 = 0;
			}
		}
		pos.x = horizontalPositions[index1];
		ActivateRock();
		lastX2 = lastX1;
		lastX1 = index1;
		if (Random.Range(0f, 1f) < GOT_Difficulty.GOT_difficulty.marble_chance && !SceneChangeController.sceneChangeController.transitioning)
		{
			marble = GOT_MarbleSpawner.GOT_marbleSpawner.CreateMarble();
			marble.transform.position = new Vector3(pos.x, 6f, -3f);
		}
	}

	public void CreateInitialRocks()
	{
		pos.y = -3.5f;
		pos.x = 0f;
		ActivateRock();
		switch (GOT_Difficulty.GOT_difficulty.level)
		{
		case 0:
			pos.y = -1f;
			break;
		case 1:
			pos.y = 0f;
			break;
		case 2:
			pos.y = 0f;
			break;
		case 3:
			pos.y = 0f;
			break;
		default:
			pos.y = -1f;
			break;
		}
		pos.x = -2f;
		ActivateRock();
		pos.x = 0f;
		ActivateRock();
		pos.x = 2f;
		ActivateRock();
		lastX1 = 2;
		if (GOT_Difficulty.GOT_difficulty.level == 0)
		{
			CreateInitialRockBatch(pos.y + 2f);
			CreateInitialRockBatch(pos.y + 2.5f);
		}
		else if (GOT_Difficulty.GOT_difficulty.level == 1)
		{
			CreateInitialRockBatch(pos.y + 3f);
		}
		else if (GOT_Difficulty.GOT_difficulty.level == 2)
		{
			CreateInitialRockBatch(pos.y + 3f);
		}
		else if (GOT_Difficulty.GOT_difficulty.level == 3)
		{
			CreateInitialRockBatch(pos.y + 3f);
		}
	}

	private void CreateInitialRockBatch(float y)
	{
		pos.y = Random.Range(y - 0.5f, y + 0.5f);
		index1 = Random.Range(0, 5);
		while (index1 == lastX1 || index1 == lastX2 || index1 == 2)
		{
			index1++;
			if (index1 > 4)
			{
				index1 = 0;
			}
		}
		pos.x = horizontalPositions[index1];
		ActivateRock();
		pos.y = Random.Range(y - 0.5f, y + 0.5f);
		index2 = Random.Range(0, 5);
		while (index2 == lastX1 || index2 == lastX2 || index2 == index1 || index2 == 2)
		{
			index2++;
			if (index2 > 4)
			{
				index2 = 0;
			}
		}
		pos.x = horizontalPositions[index2];
		ActivateRock();
		lastX1 = index1;
		lastX2 = index2;
	}

	private void ActivateRock()
	{
		obj = GetRock();
		if (obj != null)
		{
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetRock()
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

	public void CrumbleAllRocks()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (rocks[i].activeInHierarchy)
			{
				rocks[i].GetComponent<Animator>().SetTrigger("action");
			}
		}
	}
}
