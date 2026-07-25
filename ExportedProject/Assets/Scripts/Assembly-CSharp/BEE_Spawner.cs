using System.Collections.Generic;
using UnityEngine;

public class BEE_Spawner : MonoBehaviour
{
	private List<GameObject> flowerPrefabs;

	private int flowersPooledAmount = 66;

	private List<GameObject> plantsPrefabs;

	private int plantsPooledAmount = 20;

	private List<GameObject> bgFlowerPrefabs;

	private int bgFlowersPooledAmount = 40;

	public GameObject flowerPrefab;

	public GameObject plantPrefab;

	public GameObject bgFlowerPrefab;

	private float frequency;

	private float timer;

	private GameObject obj;

	private Transform lastObjTR;

	private Transform tr;

	private Vector3 pos;

	private float z = -1f;

	private int flowerCombo;

	private int plantCombo;

	private int noMarbleCombo;

	private GameObject marble;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private int flowerComboMax;

	private bool initialBatch;

	private void Start()
	{
		tr = base.transform;
		CreateFlowersPool();
		CreatePlantsPool();
		CreateBgFlowersPool();
	}

	private void Update()
	{
		if (!initialBatch && BEE_Difficulty.BEE_difficulty != null)
		{
			initialBatch = true;
			CreateInitialBatch();
		}
	}

	private void LateUpdate()
	{
		if (lastObjTR.position.y < 4.5f && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
		}
	}

	private void CreateBatch()
	{
		pos.z = z;
		pos.y = lastObjTR.position.y + 1.0625f;
		if (lastObjTR.position.x.Equals(4.125f))
		{
			for (float num = -3.4375f; num < 4f; num += 1.375f)
			{
				if ((Random.Range(0f, 1f) < BEE_Difficulty.BEE_difficulty.plantChance || flowerCombo > BEE_Difficulty.BEE_difficulty.flowerComboMax) && plantCombo < 1)
				{
					obj = GetPlant();
					flowerCombo = 0;
					plantCombo++;
				}
				else
				{
					obj = GetFlower();
					plantCombo = 0;
					flowerCombo++;
					if ((Random.Range(0f, 1f) < BEE_Difficulty.BEE_difficulty.marbleChance || noMarbleCombo > 14) && num > -4f && num < 4f && noMarbleCombo > 1)
					{
						marble = MarbleCreator.marbleCreator.GetMarble();
						marble.transform.position = new Vector3(num, pos.y, pos.z - 2f);
						SetMarbleConstants(marble);
						noMarbleCombo = 0;
					}
					else
					{
						noMarbleCombo++;
					}
				}
				pos.x = num;
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		else
		{
			for (float num2 = -4.125f; num2 < 5f; num2 += 1.375f)
			{
				if ((Random.Range(0f, 1f) < BEE_Difficulty.BEE_difficulty.plantChance || flowerCombo > BEE_Difficulty.BEE_difficulty.flowerComboMax) && plantCombo < 1)
				{
					obj = GetPlant();
					flowerCombo = 0;
					plantCombo++;
				}
				else
				{
					obj = GetFlower();
					plantCombo = 0;
					flowerCombo++;
					if ((Random.Range(0f, 1f) < BEE_Difficulty.BEE_difficulty.marbleChance || noMarbleCombo > 14) && num2 > -4f && num2 < 4f && noMarbleCombo > 1)
					{
						marble = MarbleCreator.marbleCreator.GetMarble();
						marble.transform.position = new Vector3(num2, pos.y, pos.z - 2f);
						SetMarbleConstants(marble);
						noMarbleCombo = 0;
					}
					else
					{
						noMarbleCombo++;
					}
				}
				pos.x = num2;
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		lastObjTR = obj.transform;
		pos.z = 0f;
		for (float num3 = -4.125f; num3 < 5f; num3 += 2.75f)
		{
			if (Random.Range(0f, 1f) < 0.5f)
			{
				obj = GetBgFlower();
				if (obj != null)
				{
					pos.x = num3;
					obj.transform.position = pos;
					obj.SetActive(true);
				}
			}
		}
	}

	private void CreateFlowersPool()
	{
		flowerPrefabs = new List<GameObject>();
		for (int i = 0; i < flowersPooledAmount; i++)
		{
			obj = Object.Instantiate(flowerPrefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			flowerPrefabs.Add(obj);
		}
	}

	private void CreatePlantsPool()
	{
		plantsPrefabs = new List<GameObject>();
		for (int i = 0; i < plantsPooledAmount; i++)
		{
			obj = Object.Instantiate(plantPrefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			plantsPrefabs.Add(obj);
		}
	}

	private void CreateBgFlowersPool()
	{
		bgFlowerPrefabs = new List<GameObject>();
		for (int i = 0; i < bgFlowersPooledAmount; i++)
		{
			obj = Object.Instantiate(bgFlowerPrefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			bgFlowerPrefabs.Add(obj);
		}
	}

	private void CreateInitialBatch()
	{
		pos.z = z;
		for (float num = -2.25f; num < 5f; num += 2.125f)
		{
			for (float num2 = -3.4375f; num2 < 4f; num2 += 1.375f)
			{
				if (num >= 3f && Random.Range(0f, 1f) < BEE_Difficulty.BEE_difficulty.plantChance && plantCombo < 1)
				{
					obj = GetPlant();
					flowerCombo = 0;
					plantCombo++;
				}
				else
				{
					obj = GetFlower();
					plantCombo = 0;
					flowerCombo++;
				}
				pos.x = num2;
				pos.y = num;
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		for (float num3 = -3.3125f; num3 < 6f; num3 += 2.125f)
		{
			for (float num4 = -4.125f; num4 < 5f; num4 += 1.375f)
			{
				if (num3 >= 3f && Random.Range(0f, 1f) < BEE_Difficulty.BEE_difficulty.plantChance && plantCombo < 1)
				{
					obj = GetPlant();
					flowerCombo = 0;
					plantCombo++;
				}
				else
				{
					obj = GetFlower();
					plantCombo = 0;
					flowerCombo++;
				}
				pos.x = num4;
				pos.y = num3;
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		lastObjTR = obj.transform;
		pos.z = 0f;
		for (float num5 = -2.25f; num5 < 5f; num5 += 2.125f)
		{
			for (float num6 = -4.125f; num6 < 5f; num6 += 2.75f)
			{
				if (Random.Range(0f, 1f) < 0.5f)
				{
					obj = GetBgFlower();
					if (obj != null)
					{
						pos.x = num6;
						pos.y = num5;
						obj.transform.position = pos;
						obj.SetActive(true);
					}
				}
			}
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = BEE_Difficulty.BEE_difficulty.speed;
		marbleController.direction = MarbleController.Direction.down;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}

	public void DoReset()
	{
		for (int i = 0; i < flowerPrefabs.Count; i++)
		{
			flowerPrefabs[i].SetActive(false);
		}
		for (int j = 0; j < bgFlowerPrefabs.Count; j++)
		{
			bgFlowerPrefabs[j].SetActive(false);
		}
		for (int k = 0; k < plantsPrefabs.Count; k++)
		{
			plantsPrefabs[k].GetComponent<BEE_PlantController>().DoReset();
			plantsPrefabs[k].SetActive(false);
		}
		CreateInitialBatch();
	}

	private GameObject GetFlower()
	{
		for (int i = 0; i < flowersPooledAmount; i++)
		{
			if (!flowerPrefabs[i].activeInHierarchy)
			{
				return flowerPrefabs[i];
			}
		}
		return null;
	}

	private GameObject GetPlant()
	{
		for (int i = 0; i < plantsPooledAmount; i++)
		{
			if (!plantsPrefabs[i].activeInHierarchy)
			{
				plantsPrefabs[i].GetComponent<BEE_PlantController>().DoReset();
				return plantsPrefabs[i];
			}
		}
		return GetFlower();
	}

	private GameObject GetBgFlower()
	{
		for (int i = 0; i < bgFlowersPooledAmount; i++)
		{
			if (!bgFlowerPrefabs[i].activeInHierarchy)
			{
				return bgFlowerPrefabs[i];
			}
		}
		return null;
	}
}
