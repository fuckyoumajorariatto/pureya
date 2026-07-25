using System.Collections.Generic;
using UnityEngine;

public class SNK_Spawner : MonoBehaviour
{
	public SNK_PlayerController playerController;

	private Transform tr;

	private List<GameObject> cactusPrefabs;

	private int cactusPooledAmount = 20;

	public GameObject cactus;

	private List<GameObject> cactus2Prefabs;

	private int cactus2PooledAmount = 10;

	public GameObject cactus2;

	private GameObject obj;

	private Transform objTR;

	private float timer;

	private GameObject marble;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private int comboCactus;

	private int comboMarble;

	private int comboNothing;

	private int comboNoMarble;

	private void Start()
	{
		tr = base.transform;
		CreatecactusPool();
		Createcactus2Pool();
		timer = SNK_Difficulty.SNK_difficulty.frequency;
	}

	private void Update()
	{
		if (!playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			timer += Time.deltaTime;
			if (timer >= SNK_Difficulty.SNK_difficulty.frequency)
			{
				CreateBatch();
				timer = 0f;
			}
		}
	}

	public void DoReset()
	{
		for (int i = 0; i < cactusPrefabs.Count; i++)
		{
			cactusPrefabs[i].SetActive(false);
		}
		for (int j = 0; j < cactus2Prefabs.Count; j++)
		{
			cactus2Prefabs[j].SetActive(false);
		}
		timer = SNK_Difficulty.SNK_difficulty.frequency;
	}

	private void CreateCactus(Vector2 pos)
	{
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = 5.5f;
		}
		obj = Getcactus();
		if (!(obj == null))
		{
			objTR = obj.transform;
			objTR.position = new Vector3(pos.x, pos.y, objTR.position.z);
			objTR.rotation = Quaternion.identity;
			obj.SetActive(true);
		}
	}

	private void CreateCactus2(Vector2 pos)
	{
		obj = Getcactus2();
		if (!(obj == null))
		{
			objTR = obj.transform;
			objTR.position = new Vector3(pos.x, pos.y, objTR.position.z);
			objTR.rotation = Quaternion.identity;
			obj.SetActive(true);
		}
	}

	private void CreateMarble(Vector2 pos)
	{
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = 5.5f;
		}
		marble = MarbleCreator.marbleCreator.GetMarble();
		marble.transform.position = new Vector3(pos.x, pos.y, -5f);
		SetMarbleConstants(marble);
	}

	private void CallItemCreation(Vector2 pos)
	{
		float num = Random.Range(0f, 1f);
		float[] spawnChances = SNK_Difficulty.SNK_difficulty.spawnChances;
		if ((num <= spawnChances[2] || comboNoMarble >= 3) && comboMarble < 2)
		{
			if (pos.x >= 3.5f)
			{
				pos.x = 3.5f;
			}
			else if (pos.x <= -3.5f)
			{
				pos.x = -3.5f;
			}
			comboNoMarble = 0;
			comboCactus = 0;
			comboNothing = 0;
			comboMarble++;
			CreateMarble(pos);
		}
		else if (num <= spawnChances[1] && comboCactus < 6)
		{
			comboNoMarble++;
			comboCactus++;
			comboMarble = 0;
			comboNothing = 0;
			CreateCactus2(pos);
		}
		else if ((num <= spawnChances[0] || comboMarble > 1 || comboNothing >= SNK_Difficulty.SNK_difficulty.comboNothingMax) && comboCactus < 6)
		{
			comboNoMarble++;
			comboCactus++;
			comboMarble = 0;
			comboNothing = 0;
			CreateCactus(pos);
		}
		else
		{
			comboNoMarble++;
			comboNothing++;
		}
	}

	private void CreateBatch()
	{
		float y = 4.5f;
		float num = Random.Range(-4f, -2f);
		float num2 = Random.Range(2f, 4f);
		CallItemCreation(new Vector2(num, y));
		CallItemCreation(new Vector2(num2, y));
		CallItemCreation(new Vector2(Random.Range(num + 2f, num2 - 2f), y));
	}

	public void CreateFaceCactus()
	{
		obj = Getcactus();
		if (!(obj == null))
		{
			objTR = obj.transform;
			objTR.position = new Vector3(playerController.tr.position.x, playerController.tr.position.y + 0.75f, objTR.position.z);
			objTR.rotation = Quaternion.identity;
			obj.SetActive(true);
		}
	}

	private void CreatecactusPool()
	{
		cactusPrefabs = new List<GameObject>();
		for (int i = 0; i < cactusPooledAmount; i++)
		{
			obj = Object.Instantiate(cactus);
			obj.SetActive(false);
			obj.transform.parent = tr;
			cactusPrefabs.Add(obj);
		}
	}

	private GameObject Getcactus()
	{
		for (int i = 0; i < cactusPooledAmount; i++)
		{
			if (!cactusPrefabs[i].activeInHierarchy)
			{
				return cactusPrefabs[i];
			}
		}
		return null;
	}

	private void Createcactus2Pool()
	{
		cactus2Prefabs = new List<GameObject>();
		for (int i = 0; i < cactus2PooledAmount; i++)
		{
			obj = Object.Instantiate(cactus2);
			obj.SetActive(false);
			obj.transform.parent = tr;
			cactus2Prefabs.Add(obj);
		}
	}

	private GameObject Getcactus2()
	{
		for (int i = 0; i < cactus2PooledAmount; i++)
		{
			if (!cactus2Prefabs[i].activeInHierarchy)
			{
				return cactus2Prefabs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.direction = MarbleController.Direction.down;
		marbleController.speed = SNK_Difficulty.SNK_difficulty.scroll;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 5.6f;
		marble.SetActive(true);
	}
}
