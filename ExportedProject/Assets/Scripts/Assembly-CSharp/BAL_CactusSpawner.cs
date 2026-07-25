using System.Collections.Generic;
using UnityEngine;

public class BAL_CactusSpawner : MonoBehaviour
{
	public static BAL_CactusSpawner BAL_cactusSpawner;

	public GameObject bg;

	private List<GameObject> cactuses;

	private int cactusesPooledAmount = 6;

	public GameObject cactus;

	private List<GameObject> cactusesBranched;

	private int cactusesBranchedPooledAmount = 3;

	public GameObject cactusBranched;

	private GameObject obj;

	private Transform objTR;

	private Transform tr;

	private float frequency;

	private float timer;

	private bool branched = true;

	private Vector3 pos;

	private void Start()
	{
		BAL_cactusSpawner = this;
		tr = base.gameObject.transform;
		SetDifficultyParas();
		PoolCactuses();
		PoolCactusesBranched();
	}

	private void Update()
	{
		if (timer >= frequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			timer = 0f;
			SpawnCactus();
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	public void SetDifficultyParas()
	{
		frequency = BAL_Difficulty.BAL_difficulty.cactus_frequency;
		branched = BAL_Difficulty.BAL_difficulty.branched;
	}

	public void SpawnCactus()
	{
		if (!BAL_PlayerController.BAL_playerController.hit)
		{
			if (!branched || Random.Range(0f, 1f) < 0.5f)
			{
				obj = GetCactus();
				pos.z = obj.transform.position.z;
				pos.x = Random.Range(-7f, -5f);
				pos.y = Random.Range(-3.5f, -1.5f);
				objTR = obj.transform;
				objTR.position = pos;
				objTR.parent = bg.transform;
				obj.SetActive(true);
			}
			else
			{
				obj = GetCactusBranched();
				pos.z = obj.transform.position.z;
				pos.x = Random.Range(-7f, -5f);
				pos.y = Random.Range(-2.5f, -1.5f);
				objTR = obj.transform;
				objTR.position = pos;
				objTR.parent = bg.transform;
				obj.SetActive(true);
			}
		}
	}

	private void PoolCactuses()
	{
		cactuses = new List<GameObject>();
		for (int i = 0; i < cactusesPooledAmount; i++)
		{
			obj = Object.Instantiate(cactus);
			obj.SetActive(false);
			obj.transform.parent = tr;
			cactuses.Add(obj);
		}
	}

	private GameObject GetCactus()
	{
		for (int i = 0; i < cactusesPooledAmount; i++)
		{
			if (!cactuses[i].activeInHierarchy)
			{
				return cactuses[i];
			}
		}
		return null;
	}

	private void PoolCactusesBranched()
	{
		cactusesBranched = new List<GameObject>();
		for (int i = 0; i < cactusesBranchedPooledAmount; i++)
		{
			obj = Object.Instantiate(cactusBranched);
			obj.SetActive(false);
			obj.transform.parent = tr;
			cactusesBranched.Add(obj);
		}
	}

	private GameObject GetCactusBranched()
	{
		for (int i = 0; i < cactusesBranchedPooledAmount; i++)
		{
			if (!cactusesBranched[i].activeInHierarchy)
			{
				return cactusesBranched[i];
			}
		}
		return null;
	}
}
