using System.Collections.Generic;
using UnityEngine;

public class SNW_PennantSpawner : MonoBehaviour
{
	public static SNW_PennantSpawner SNW_pennantSpawner;

	public List<GameObject> pennants_left;

	public List<GameObject> pennants_right;

	public int pooledAmount;

	public GameObject pennant_left;

	public GameObject pennant_right;

	private float frequency;

	private float timer;

	private bool odd;

	private Vector3 pos;

	private GameObject obj;

	private float x;

	private float marblePos;

	private float diff;

	private void Start()
	{
		pooledAmount = 12;
		SNW_pennantSpawner = this;
		SetDifficultyParas();
		CreatePool();
	}

	private void Update()
	{
		if (timer >= frequency && !SNW_PlayerController.SNW_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (odd)
			{
				CreateRock();
				odd = false;
			}
			else
			{
				CreateBatch();
				odd = true;
			}
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = SNW_Difficulty.SNW_difficulty.frequency;
	}

	private void CreatePool()
	{
		pennants_left = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(pennant_left);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			pennants_left.Add(obj);
		}
		pennants_right = new List<GameObject>();
		for (int j = 0; j < pooledAmount; j++)
		{
			obj = Object.Instantiate(pennant_right);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			pennants_right.Add(obj);
		}
	}

	private void CreateBatch()
	{
		diff = 2.5f;
		if (Random.Range(0f, 1f) < 0.7f)
		{
			pos.y = Random.Range(-5f, -6f);
			x = Random.Range(1f, 3f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				x *= -1f;
			}
			pos.x = x;
			if (x > 0f)
			{
				ActivatePennantLeft();
				marblePos = pos.x + diff;
				SNW_MarbleSpawner.SNW_marbleSpawner.CreateMarble(marblePos, pos.y);
			}
			else
			{
				ActivatePennantRight();
				marblePos = pos.x - diff;
				SNW_MarbleSpawner.SNW_marbleSpawner.CreateMarble(marblePos, pos.y);
			}
		}
		else
		{
			pos.y = -5f;
			x = Random.Range(-2f, 2f);
			pos.x = x;
			ActivatePennantLeft();
			pos.x += diff;
			SNW_MarbleSpawner.SNW_marbleSpawner.CreateMarble(pos.x, pos.y);
			pos.x += diff;
			ActivatePennantRight();
		}
	}

	private void ActivatePennantLeft()
	{
		obj = GetPennatLeft();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private void ActivatePennantRight()
	{
		obj = GetPennatRight();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private void CreateRock()
	{
		pos.y = -5f;
		pos.x = Random.Range(-3f, 3f);
		obj = SNW_RockCreator.SNW_rockCreator.GetRock();
		if (obj != null)
		{
			obj.transform.position = pos;
			obj.SetActive(true);
		}
		if (pos.x < -1.5f || pos.x > 1.5f)
		{
			pos.y = -6f;
			pos.x *= -1f;
			obj = SNW_RockCreator.SNW_rockCreator.GetRock();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		for (int i = 0; i < 7; i++)
		{
			pos.y = Random.Range(-5f, -8f);
			pos.x = Random.Range(5f, 20f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x *= -1f;
			}
			obj = SNW_RockCreator.SNW_rockCreator.GetRock();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
	}

	private GameObject GetPennatLeft()
	{
		for (int i = 0; i < pooledAmount * 2; i++)
		{
			if (!pennants_left[i].activeInHierarchy)
			{
				return pennants_left[i];
			}
		}
		return null;
	}

	private GameObject GetPennatRight()
	{
		for (int i = 0; i < pooledAmount * 2; i++)
		{
			if (!pennants_right[i].activeInHierarchy)
			{
				return pennants_right[i];
			}
		}
		return null;
	}
}
