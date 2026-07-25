using System.Collections.Generic;
using UnityEngine;

public class SQR_BgBranchSpawner : MonoBehaviour
{
	public static SQR_BgBranchSpawner SQR_bgBranchSpawner;

	private int pooledAmount = 24;

	private List<GameObject> branches;

	public GameObject branch;

	private int pooledAmount2 = 12;

	private List<GameObject> branches2;

	public GameObject branch2;

	private float frequency;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		SQR_bgBranchSpawner = this;
		SetDifficultyParas();
		timer = frequency;
		CreatePool();
		CreateInitialBatch();
	}

	private void Update()
	{
		if (timer >= frequency)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = SQR_Difficulty.SQR_difficulty.pinecone_frequency;
		timer = 0f;
	}

	private void CreatePool()
	{
		branches = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(branch);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			branches.Add(obj);
		}
		branches2 = new List<GameObject>();
		for (int j = 0; j < pooledAmount2; j++)
		{
			obj = Object.Instantiate(branch2);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			branches2.Add(obj);
		}
	}

	private void CreateBatch()
	{
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = Random.Range(-6f, -5f);
			pos.x = -3f;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				ActivateBranch(true);
			}
			else
			{
				ActivateBranch(false);
			}
		}
		else
		{
			pos.y = Random.Range(-6f, -5f);
			pos.x = -3f;
			ActivateBranch(true);
			pos.x = -3f;
			pos.y = Random.Range(-7f, -5f);
			ActivateBranch(false);
		}
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = Random.Range(-6f, -5f);
			pos.x = 3f;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				ActivateBranch(true);
			}
			else
			{
				ActivateBranch(false);
			}
		}
		else
		{
			pos.y = Random.Range(-6f, -5f);
			pos.x = 3f;
			ActivateBranch(true);
			pos.x = 3f;
			pos.y = Random.Range(-6f, -5f);
			ActivateBranch(false);
		}
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = Random.Range(-6f, -5f);
			pos.x = 0f;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				ActivateBranch2(true);
			}
			else
			{
				ActivateBranch2(false);
			}
		}
		else
		{
			pos.y = Random.Range(-6f, -5f);
			pos.x = 0f;
			ActivateBranch2(true);
			pos.x = 0f;
			pos.y = Random.Range(-6f, -5f);
			ActivateBranch2(false);
		}
	}

	private void CreateInitialBatch()
	{
		for (float num = -4f; num < 5f; num += 2f)
		{
			pos.y = num;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x = -3f;
				if (Random.Range(0f, 1f) < 0.5f)
				{
					ActivateBranch(true);
				}
				else
				{
					ActivateBranch(false);
				}
			}
			else
			{
				pos.x = -3f;
				ActivateBranch(true);
				pos.x = -3f;
				ActivateBranch(false);
			}
		}
		for (float num2 = -4f; num2 < 5f; num2 += 2f)
		{
			pos.y = num2;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x = 3f;
				if (Random.Range(0f, 1f) < 0.5f)
				{
					ActivateBranch(true);
				}
				else
				{
					ActivateBranch(false);
				}
			}
			else
			{
				pos.x = 3f;
				ActivateBranch(true);
				pos.x = 3f;
				ActivateBranch(false);
			}
		}
		for (float num3 = -4f; num3 <= 4f; num3 += 1.5f)
		{
			pos.y = num3;
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x = 0f;
				if (Random.Range(0f, 1f) < 0.5f)
				{
					ActivateBranch2(true);
				}
				else
				{
					ActivateBranch2(false);
				}
			}
			else
			{
				pos.x = 0f;
				ActivateBranch2(true);
				pos.x = 0f;
				ActivateBranch2(false);
			}
		}
	}

	private void ActivateBranch(bool left)
	{
		obj = GetBranch();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			if (left)
			{
				pos.x -= 1.5f;
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				pos.x += 1.5f;
				obj.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetBranch()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!branches[i].activeInHierarchy)
			{
				return branches[i];
			}
		}
		return null;
	}

	private void ActivateBranch2(bool left)
	{
		obj = GetBranch2();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			if (left)
			{
				pos.x -= 1.5f;
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				pos.x += 1.5f;
				obj.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetBranch2()
	{
		for (int i = 0; i < pooledAmount2; i++)
		{
			if (!branches2[i].activeInHierarchy)
			{
				return branches2[i];
			}
		}
		return null;
	}
}
