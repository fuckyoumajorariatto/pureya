using System.Collections.Generic;
using UnityEngine;

public class SQD_UrchinSpawner : MonoBehaviour
{
	private List<GameObject> urchins;

	private int pooledAmount = 9;

	public GameObject urchin;

	public float frequency;

	private int urchinMax;

	public float timer;

	private int max;

	private float r;

	private float xmax;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		SetDifficultyParas();
		CreatePool();
		timer = frequency;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= frequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateUrchinBatch();
			timer = 0f;
		}
	}

	public void SetDifficultyParas()
	{
		frequency = SQD_Difficulty.SQD_difficulty.urchin_frequency;
	}

	private void CreatePool()
	{
		urchins = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(urchin);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			urchins.Add(obj);
		}
	}

	private void CreateUrchinBatch()
	{
		max = 1;
		r = Random.Range(0f, 1f);
		if (r < SQD_Difficulty.SQD_difficulty.double_urchin_chance)
		{
			max = 2;
		}
		SpawnUrchins(max);
	}

	private void SpawnUrchins(int n)
	{
		xmax = 7.5f;
		switch (n)
		{
		case 0:
			break;
		case 1:
			pos.x = Random.Range(4.5f, xmax);
			pos.y = Random.Range(-3.5f, 3.5f);
			ActivateUrchin(pos);
			break;
		case 2:
			pos.x = Random.Range(4.5f, xmax);
			pos.y = Random.Range(0.5f, 3.5f);
			ActivateUrchin(pos);
			pos.x = Random.Range(4.5f, xmax);
			pos.y = Random.Range(-3.5f, -0.5f);
			ActivateUrchin(pos);
			break;
		case 3:
		{
			pos.x = Random.Range(5f, xmax);
			pos.y = Random.Range(-3f, 3f);
			ActivateUrchin(pos);
			float y = pos.y;
			pos.x = Random.Range(5f, xmax);
			if (pos.y <= 2f)
			{
				pos.y = Random.Range(pos.y + 1f, 3f);
			}
			else if (pos.y > -2f)
			{
				pos.y = Random.Range(-3f, pos.y - 1f);
			}
			ActivateUrchin(pos);
			float y2 = pos.y;
			pos.x = Random.Range(5f, xmax);
			int num = 1000;
			pos.y = Random.Range(-3f, 3f);
			while (((pos.y <= y + 1f && pos.y >= y - 1f) || (pos.y <= y2 + 1f && pos.y >= y2 - 1f)) && num > 0)
			{
				num--;
			}
			if (num > 0)
			{
				ActivateUrchin(pos);
			}
			break;
		}
		}
	}

	private void ActivateUrchin(Vector3 pos)
	{
		obj = GetUrchin();
		if (obj != null)
		{
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetUrchin()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!urchins[i].activeInHierarchy)
			{
				return urchins[i];
			}
		}
		return null;
	}
}
