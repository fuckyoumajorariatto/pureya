using System.Collections.Generic;
using UnityEngine;

public class PLU_RockSpawner : MonoBehaviour
{
	public PLU_PlayerController playerController;

	private List<GameObject> rocks;

	private int pooledAmount = 4;

	public GameObject rock;

	private float frequency;

	private int rockMax;

	private float speed;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		SetDifficultyParas();
		CreatePool();
	}

	private void Update()
	{
		if (timer >= frequency && !SceneChangeController.sceneChangeController.transitioning && !playerController.hit)
		{
			CreateRockBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = PLU_Difficulty.PLU_difficulty.rock_frequency;
		rockMax = PLU_Difficulty.PLU_difficulty.rock_max;
		speed = PLU_Difficulty.PLU_difficulty.rock_speed;
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
		CreateRocks(Random.Range(1, rockMax + 1));
	}

	private void CreateRocks(int n)
	{
		float max = -8f;
		switch (n)
		{
		case 1:
			pos.y = Random.Range(-5.5f, max);
			pos.x = Random.Range(-3.5f, 3.5f);
			RemoveEdgeUncertainty();
			ActivateRock();
			break;
		case 2:
			pos.y = Random.Range(-5.5f, max);
			pos.x = Random.Range(-3.5f, 3.5f);
			RemoveEdgeUncertainty();
			ActivateRock();
			pos.y = Random.Range(-5.5f, max);
			if (pos.x <= 1f)
			{
				pos.x = Random.Range(pos.x + 3f, 4f);
			}
			else if (pos.x > -1f)
			{
				pos.x = Random.Range(-4f, pos.x - 3f);
			}
			RemoveEdgeUncertainty();
			ActivateRock();
			break;
		}
	}

	private void RemoveEdgeUncertainty()
	{
		if (pos.x > 2.8f)
		{
			pos.x = 4f;
		}
		else if (pos.x < -2.8f)
		{
			pos.x = -4f;
		}
	}

	private void ActivateRock()
	{
		obj = GetRock();
		if (obj != null)
		{
			obj.transform.position = pos;
			obj.SetActive(true);
			obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, speed);
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
}
