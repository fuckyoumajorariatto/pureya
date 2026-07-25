using System.Collections.Generic;
using UnityEngine;

public class SHI_Spawner : MonoBehaviour
{
	public Transform tr;

	private GameObject obj;

	private Transform objTR;

	private List<GameObject> waterPrefabs;

	private int waterPooledAmount = 63;

	public GameObject water;

	public Color32 originalWaterColor;

	private List<GameObject> islandsPrefabs;

	private int islandsPooledAmount = 12;

	public GameObject island;

	private float tileOddX = -3.9375f;

	private float tileEvenX = -2.625f;

	private float tileOddY = -4f;

	private float tileEvenY = -3.25f;

	private GameObject marble;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private int marbleCooldownCounter = 5;

	private int islandCooldownCounter = 5;

	private void Start()
	{
		CreateWaterPool();
		CreateIslandsPool();
		CreateInitialBatch();
	}

	private void LateUpdate()
	{
		if (objTR.position.y < 5f && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
		}
	}

	public void DoReset()
	{
		for (int i = 0; i < waterPooledAmount; i++)
		{
			waterPrefabs[i].SetActive(false);
		}
		for (int j = 0; j < islandsPooledAmount; j++)
		{
			islandsPrefabs[j].SetActive(false);
		}
		CreateInitialBatch();
	}

	private void CreateBatch()
	{
		float num = tileOddX;
		float num2 = objTR.position.y + 0.75f;
		for (int i = 0; i < 4; i++)
		{
			obj = GetWater();
			if (obj != null)
			{
				obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
				num += 2.625f;
				obj.SetActive(true);
			}
		}
		num = tileEvenX;
		num2 += 0.75f;
		for (int j = 0; j < 3; j++)
		{
			obj = GetWater();
			if (obj != null)
			{
				obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
				num += 2.625f;
				obj.SetActive(true);
				objTR = obj.transform;
			}
		}
		if (SHI_Difficulty.SHI_difficulty.islandCooldown > 0)
		{
			if (islandCooldownCounter < SHI_Difficulty.SHI_difficulty.islandCooldown)
			{
				islandCooldownCounter++;
				SpawnIsland();
			}
			else
			{
				islandCooldownCounter = 0;
			}
		}
		else
		{
			SpawnIsland();
		}
		if (SHI_Difficulty.SHI_difficulty.marbleCooldown > 0)
		{
			if (marbleCooldownCounter < SHI_Difficulty.SHI_difficulty.marbleCooldown)
			{
				marbleCooldownCounter++;
				SpawnMarble();
				return;
			}
			marbleCooldownCounter = 0;
			if (SHI_Difficulty.SHI_difficulty.marbleOddChance && Random.Range(0f, 1f) > 0.5f)
			{
				SpawnMarble();
			}
		}
		else
		{
			SpawnMarble();
		}
	}

	private void SpawnIsland()
	{
		float num = 0f;
		float num2 = 0f;
		obj = GetIsland();
		if (obj != null)
		{
			if (Random.Range(0f, 1f) < 0.5f)
			{
				num2 = objTR.position.y - 0.75f;
				num = tileOddX + 2.625f * (float)Random.Range(0, 4);
			}
			else
			{
				num2 = objTR.position.y - 1.5f;
				num = tileEvenX + 2.625f * (float)Random.Range(0, 3);
			}
			obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
			obj.SetActive(true);
		}
		if (!(Random.Range(0f, 1f) < SHI_Difficulty.SHI_difficulty.chanceOfSecondIsland))
		{
			return;
		}
		obj = GetIsland();
		if (!(obj != null))
		{
			return;
		}
		if (num > 1f)
		{
			if (Random.Range(0f, 1f) < 0.5f)
			{
				num2 = objTR.position.y - 0.75f;
				num = tileOddX + 2.625f * (float)Random.Range(0, 1);
			}
			else
			{
				num2 = objTR.position.y - 1.5f;
				num = tileEvenX + 2.625f * (float)Random.Range(0, 1);
			}
			Debug.Log(num);
			obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
			obj.SetActive(true);
		}
		else if (num < -1f)
		{
			if (Random.Range(0f, 1f) < 0.5f)
			{
				num2 = objTR.position.y - 0.75f;
				num = tileOddX + 2.625f * (float)Random.Range(2, 4);
			}
			else
			{
				num2 = objTR.position.y - 1.5f;
				num = tileEvenX + 2.625f * (float)Random.Range(1, 3);
			}
			Debug.Log(num);
			obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
			obj.SetActive(true);
		}
	}

	private void SpawnMarble()
	{
		float y;
		float num;
		if (Random.Range(0f, 1f) < 0.5f)
		{
			y = objTR.position.y - 0.75f;
			num = -1.3125f + 2.625f * (float)Random.Range(0, 2);
		}
		else
		{
			y = objTR.position.y - 1.5f;
			num = tileEvenX + 2.625f * (float)Random.Range(0, 3);
		}
		marble = MarbleCreator.marbleCreator.GetMarble();
		marble.transform.position = new Vector3(num, y, -2f);
		SetMarbleConstants(marble);
		if ((num < -2.5f || num > 2.5f) && Random.Range(0f, 1f) > 0.5f)
		{
			num *= -1f;
			marble = MarbleCreator.marbleCreator.GetMarble();
			marble.transform.position = new Vector3(num, y, -2f);
			SetMarbleConstants(marble);
		}
	}

	private void CreateInitialBatch()
	{
		float num = tileOddX;
		float num2 = tileOddY;
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				obj = GetWater();
				if (obj != null)
				{
					obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
					num += 2.625f;
					obj.SetActive(true);
				}
			}
			num = tileOddX;
			num2 += 1.5f;
		}
		num = tileEvenX;
		num2 = tileEvenY;
		for (int k = 0; k < 7; k++)
		{
			for (int l = 0; l < 3; l++)
			{
				obj = GetWater();
				if (obj != null)
				{
					obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
					num += 2.625f;
					obj.SetActive(true);
				}
			}
			num = tileEvenX;
			num2 += 1.5f;
		}
		objTR = obj.transform;
		obj = GetIsland();
		if (obj != null)
		{
			if (Random.Range(0f, 1f) < 0.5f)
			{
				num2 = objTR.position.y - 2.25f;
				num = tileOddX + 2.625f * (float)Random.Range(0, 4);
			}
			else
			{
				num2 = objTR.position.y - 3f;
				num = ((!(Random.Range(0f, 1f) < 0.5f)) ? (0f - tileEvenX) : tileEvenX);
			}
			obj.transform.position = new Vector3(num, num2, obj.transform.position.z);
			obj.SetActive(true);
		}
		num2 = objTR.position.y - 0.75f;
		num = -1.3125f + 2.625f * (float)Random.Range(0, 2);
		marble = MarbleCreator.marbleCreator.GetMarble();
		marble.transform.position = new Vector3(num, num2, -2f);
		SetMarbleConstants(marble);
	}

	private void CreateWaterPool()
	{
		waterPrefabs = new List<GameObject>();
		for (int i = 0; i < waterPooledAmount; i++)
		{
			obj = Object.Instantiate(water);
			obj.SetActive(false);
			obj.transform.parent = tr;
			obj.GetComponent<SpriteRenderer>().color = new Color32(originalWaterColor.r, (byte)(174 + Random.Range(-16, 10)), originalWaterColor.b, originalWaterColor.a);
			waterPrefabs.Add(obj);
		}
	}

	private GameObject GetWater()
	{
		for (int i = 0; i < waterPooledAmount; i++)
		{
			if (!waterPrefabs[i].activeInHierarchy)
			{
				return waterPrefabs[i];
			}
		}
		return null;
	}

	private void CreateIslandsPool()
	{
		islandsPrefabs = new List<GameObject>();
		for (int i = 0; i < islandsPooledAmount; i++)
		{
			obj = Object.Instantiate(island);
			obj.SetActive(false);
			obj.transform.parent = tr;
			islandsPrefabs.Add(obj);
		}
	}

	private GameObject GetIsland()
	{
		for (int i = 0; i < islandsPooledAmount; i++)
		{
			if (!islandsPrefabs[i].activeInHierarchy)
			{
				return islandsPrefabs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = SHI_Difficulty.SHI_difficulty.speed;
		marbleController.direction = MarbleController.Direction.down;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}
}
