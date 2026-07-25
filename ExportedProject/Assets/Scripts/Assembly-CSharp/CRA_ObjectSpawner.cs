using System.Collections.Generic;
using UnityEngine;

public class CRA_ObjectSpawner : MonoBehaviour
{
	public static CRA_ObjectSpawner CRA_objectSpawner;

	private List<GameObject> objs;

	private int pooledAmount = 20;

	public GameObject prefab;

	private List<GameObject> objs2;

	private int pooledAmount2 = 8;

	public GameObject prefab2;

	private List<GameObject> objs3;

	private int pooledAmount3 = 8;

	public GameObject prefab3;

	private Vector3 pos = new Vector3(0f, -5f, -0.1f);

	private GameObject obj;

	private float timer;

	private float coralTimer;

	private Vector3 pos1;

	public CRA_PlayerController playerController;

	private bool odd;

	private Vector2 lastCoralArea;

	private void Start()
	{
		CRA_objectSpawner = this;
		CreatePool();
		CreatePool2();
		CreatePool3();
		DoReset();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		coralTimer += Time.deltaTime;
		if (coralTimer >= CRA_Difficulty.CRA_difficulty.coralFrequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateCoral();
			coralTimer = 0f;
		}
		if (timer >= CRA_Difficulty.CRA_difficulty.frequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatchAlgae();
			timer = 0f;
		}
	}

	public void DoReset()
	{
		timer = CRA_Difficulty.CRA_difficulty.frequency;
		coralTimer = 0f;
	}

	private void CreateCoral()
	{
		if (!playerController.hit)
		{
			pos.x = Random.Range(-2f, 2f);
			pos.y = Random.Range(-5.5f, -7f);
			pos.z = -1f;
			obj = GetObj3();
			if (obj != null)
			{
				obj.transform.position = pos;
				lastCoralArea = new Vector2(pos.x - 1f, pos.x + 1f);
				obj.SetActive(true);
			}
			else
			{
				lastCoralArea = new Vector2(-10f, -10f);
			}
		}
	}

	private void CreateBatchAlgae()
	{
		pos.z = -0.1f;
		if (odd)
		{
			pos.x = Random.Range(-4f, 0f);
			pos1 = pos;
			pos.y = Random.Range(-5f, -4.5f);
			obj = GetObj();
			if (obj != null && (pos.x < lastCoralArea.x || pos.x > lastCoralArea.y))
			{
				obj.transform.position = pos;
				obj.SetActive(true);
				obj.GetComponent<CRA_AlgaeController>().Initialize();
			}
		}
		else
		{
			if (pos1.x > -1f)
			{
				pos.x = Random.Range(pos1.x * -1f, 4f);
			}
			else
			{
				pos.x = Random.Range(0f, 4f);
			}
			pos.y = Random.Range(-5f, -4.5f);
			obj = GetObj();
			if (obj != null && (pos.x < lastCoralArea.x || pos.x > lastCoralArea.y))
			{
				obj.transform.position = pos;
				obj.SetActive(true);
				obj.GetComponent<CRA_AlgaeController>().Initialize();
			}
		}
		odd = !odd;
	}

	private void CreatePool()
	{
		objs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			objs.Add(obj);
		}
	}

	public GameObject GetObj()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!objs[i].activeInHierarchy)
			{
				return objs[i];
			}
		}
		return null;
	}

	private void CreatePool2()
	{
		objs2 = new List<GameObject>();
		for (int i = 0; i < pooledAmount2; i++)
		{
			obj = Object.Instantiate(prefab2);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			objs2.Add(obj);
		}
	}

	public GameObject GetObj2()
	{
		for (int i = 0; i < pooledAmount2; i++)
		{
			if (!objs2[i].activeInHierarchy)
			{
				return objs2[i];
			}
		}
		return null;
	}

	private void CreatePool3()
	{
		objs3 = new List<GameObject>();
		for (int i = 0; i < pooledAmount3; i++)
		{
			obj = Object.Instantiate(prefab3);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			objs3.Add(obj);
		}
	}

	public GameObject GetObj3()
	{
		for (int i = 0; i < pooledAmount3; i++)
		{
			if (!objs3[i].activeInHierarchy)
			{
				return objs3[i];
			}
		}
		return null;
	}
}
