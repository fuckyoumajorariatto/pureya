using System.Collections.Generic;
using UnityEngine;

public class PLA_Spawner : MonoBehaviour
{
	public PLA_PlayerController playerController;

	private Transform tr;

	private List<GameObject> missilePrefabs;

	private int missilePooledAmount = 20;

	public GameObject missile;

	private List<GameObject> missile2Prefabs;

	private int missile2PooledAmount = 10;

	public GameObject missile2;

	private int missile2Combo;

	private GameObject obj;

	private Transform objTR;

	private float timer;

	private float marbleTimer;

	private GameObject marble;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private void Start()
	{
		tr = base.transform;
		CreateMissilePool();
		CreateMissile2Pool();
		timer = PLA_Difficulty.PLA_difficulty.missileFrequency;
	}

	private void Update()
	{
		if (!playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			timer += Time.deltaTime;
			if (timer >= PLA_Difficulty.PLA_difficulty.missileFrequency)
			{
				CreateMissileBatch();
				timer = 0f;
			}
			marbleTimer += Time.deltaTime;
			if (marbleTimer > PLA_Difficulty.PLA_difficulty.marbleFrequency)
			{
				CreateMarbleBatch();
				marbleTimer = 0f;
			}
		}
	}

	public void DoReset()
	{
		for (int i = 0; i < missilePrefabs.Count; i++)
		{
			missilePrefabs[i].SetActive(false);
		}
		for (int j = 0; j < missile2Prefabs.Count; j++)
		{
			missile2Prefabs[j].SetActive(false);
		}
		timer = PLA_Difficulty.PLA_difficulty.missileFrequency / 2f;
		marbleTimer = -0.5f;
	}

	private void CreateMissileBatch()
	{
		if (Random.Range(0f, 1f) > PLA_Difficulty.PLA_difficulty.missile2Chance || missile2Combo > 1)
		{
			missile2Combo = 0;
			obj = GetMissile();
			if (!(obj == null))
			{
				objTR = obj.transform;
				objTR.position = new Vector3(0f, 0f, objTR.position.z);
				objTR.rotation = Quaternion.identity;
				objTR.eulerAngles = tr.forward * Random.Range(0f, 360f);
				obj.SetActive(true);
			}
		}
		else
		{
			missile2Combo++;
			obj = GetMissile2();
			if (!(obj == null))
			{
				objTR = obj.transform;
				objTR.position = new Vector3(0f, 0f, objTR.position.z);
				objTR.rotation = Quaternion.identity;
				objTR.eulerAngles = tr.forward * Random.Range(0f, 360f);
				obj.SetActive(true);
			}
		}
	}

	private void CreateMarbleBatch()
	{
		marble = MarbleCreator.marbleCreator.GetMarble();
		float num = Random.Range(0f, 1f);
		float num2 = 0f;
		num2 = ((num < 0.1f) ? 3.6875f : ((num < 0.2f) ? (-3.6875f) : ((num < 0.4f) ? Random.Range(2.8f, 3.6875f) : ((!(num < 0.6f)) ? Random.Range(-3.6875f, 3.6875f) : Random.Range(-2.8f, -3.6875f)))));
		marble.transform.position = new Vector3(-5f, num2, -4f);
		SetMarbleConstants(marble);
	}

	private void CreateMissilePool()
	{
		missilePrefabs = new List<GameObject>();
		for (int i = 0; i < missilePooledAmount; i++)
		{
			obj = Object.Instantiate(missile);
			obj.SetActive(false);
			obj.transform.parent = tr;
			missilePrefabs.Add(obj);
		}
	}

	private GameObject GetMissile()
	{
		for (int i = 0; i < missilePooledAmount; i++)
		{
			if (!missilePrefabs[i].activeInHierarchy)
			{
				return missilePrefabs[i];
			}
		}
		return null;
	}

	private void CreateMissile2Pool()
	{
		missile2Prefabs = new List<GameObject>();
		for (int i = 0; i < missile2PooledAmount; i++)
		{
			obj = Object.Instantiate(missile2);
			obj.SetActive(false);
			obj.transform.parent = tr;
			missile2Prefabs.Add(obj);
		}
	}

	private GameObject GetMissile2()
	{
		for (int i = 0; i < missile2PooledAmount; i++)
		{
			if (!missile2Prefabs[i].activeInHierarchy)
			{
				return missile2Prefabs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.direction = MarbleController.Direction.right;
		marbleController.speed = Random.Range(PLA_Difficulty.PLA_difficulty.bgSpeed * 1.1f, PLA_Difficulty.PLA_difficulty.bgSpeed * 1.5f);
		marbleController.tr.localScale = new Vector3(1f, 1f, 1f);
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}
}
