using System.Collections.Generic;
using UnityEngine;

public class SWO_ArrowSpawner : MonoBehaviour
{
	public static SWO_ArrowSpawner SWO_arrowSpawner;

	private List<GameObject> prefabs;

	private int pooledAmount = 10;

	public GameObject prefab;

	public Transform target;

	private float frequency;

	private float timer;

	private float marbleTimer;

	private int lastArrow;

	private float arrowSpeed;

	private float nextArrowSpeed;

	private int arrowCombo;

	private bool doubleItem;

	private GameObject obj;

	private SWO_ArrowController arrowController;

	private Transform tr;

	private Vector3 pos = new Vector3(0f, 5f, -1f);

	private void Start()
	{
		SWO_arrowSpawner = this;
		tr = base.transform;
		CreatePool();
		SetDifficultyParas();
	}

	private void Update()
	{
		if (!SWO_PlayerController.SWO_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
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
		frequency = SWO_Difficulty.SWO_difficulty.arrowFrequency;
		arrowSpeed = SWO_Difficulty.SWO_difficulty.arrowSpeed;
		timer = frequency / 2f;
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
		pos.z = 1f;
		if (lastArrow == 0)
		{
			lastArrow++;
		}
		else if (lastArrow == 3)
		{
			lastArrow--;
		}
		else if (Random.Range(0f, 1f) < 0.5f)
		{
			lastArrow--;
		}
		else
		{
			lastArrow++;
		}
		int num = lastArrow;
		switch (num)
		{
		case 0:
			pos.x = -5f;
			pos.y = Random.Range(0f, 5f);
			nextArrowSpeed = arrowSpeed * 0.75f;
			break;
		case 1:
			pos.y = 5f;
			pos.x = Random.Range(-5f, 0f);
			nextArrowSpeed = arrowSpeed;
			break;
		case 2:
			pos.y = 5f;
			pos.x = Random.Range(0f, 5f);
			nextArrowSpeed = arrowSpeed;
			break;
		case 3:
			pos.x = 5f;
			pos.y = Random.Range(0f, 5f);
			nextArrowSpeed = arrowSpeed * 0.75f;
			break;
		}
		if ((Random.Range(0f, 1f) < 0.5f || arrowCombo > 3) && !doubleItem)
		{
			arrowCombo = 0;
			if (Random.Range(0f, 1f) < 0.5f && !doubleItem)
			{
				doubleItem = true;
				if (Random.Range(0f, 1f) < 0.25f)
				{
					if (num == 0 || num == 3)
					{
						pos.y = Random.Range(-3.375f, 5f);
					}
					SWO_MarbleSpawner.SWO_marbleSpawner.CreateMarble(nextArrowSpeed, pos);
				}
				else
				{
					ActivatePrefab();
					CreateMarbleCompanion();
				}
			}
			else
			{
				if (num == 0 || num == 3)
				{
					pos.y = Random.Range(-3.375f, 5f);
				}
				SWO_MarbleSpawner.SWO_marbleSpawner.CreateMarble(nextArrowSpeed, pos);
				doubleItem = false;
			}
		}
		else
		{
			arrowCombo++;
			ActivatePrefab();
			doubleItem = false;
		}
	}

	private void CreateMarbleCompanion()
	{
		pos.z = 1f;
		int num = ((lastArrow == 0) ? Random.Range(2, 4) : ((lastArrow == 3) ? Random.Range(0, 2) : ((lastArrow == 1) ? 3 : 0)));
		int num2 = num;
		switch (num2)
		{
		case 0:
			pos.x = -5f;
			pos.y = Random.Range(0f, 5f);
			nextArrowSpeed = arrowSpeed * 0.75f;
			break;
		case 1:
			pos.y = 5f;
			pos.x = Random.Range(-5f, 0f);
			nextArrowSpeed = arrowSpeed;
			break;
		case 2:
			pos.y = 5f;
			pos.x = Random.Range(0f, 5f);
			nextArrowSpeed = arrowSpeed;
			break;
		case 3:
			pos.x = 5f;
			pos.y = Random.Range(0f, 5f);
			nextArrowSpeed = arrowSpeed * 0.75f;
			break;
		}
		if (num2 == 0 || num2 == 3)
		{
			pos.y = Random.Range(-3.375f, 5f);
		}
		SWO_MarbleSpawner.SWO_marbleSpawner.CreateMarble(nextArrowSpeed, pos);
	}

	private void ActivatePrefab()
	{
		obj = GetPrefab();
		if (obj != null)
		{
			arrowController = obj.GetComponent<SWO_ArrowController>();
			arrowController.target = target;
			arrowController.moveSpeed = nextArrowSpeed;
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
