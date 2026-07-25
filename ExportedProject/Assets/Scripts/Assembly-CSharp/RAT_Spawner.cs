using System.Collections.Generic;
using UnityEngine;

public class RAT_Spawner : MonoBehaviour
{
	public RAT_PlayerController playerController;

	public GameObject platformPrefab;

	private List<GameObject> platformPrefabs;

	private int platformPooledAmount = 14;

	private GameObject obj;

	private GameObject obj2;

	private bool apart;

	private Transform tr;

	private Vector3 pos;

	private int fragileCombo;

	private float timer;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private GameObject marble;

	private int marbleCombo;

	private int noMarbleCombo;

	private void Start()
	{
		tr = base.transform;
		CreatePlatformsPool();
		timer = 10f;
		InitialSpawn();
	}

	private void Update()
	{
		if (timer > RAT_Difficulty.RAT_difficulty.frequency && !playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			SpawnPlatform();
		}
		timer += Time.deltaTime;
	}

	public void InitialSpawn()
	{
		fragileCombo = 0;
		obj = GetPlatform();
		if (obj != null)
		{
			pos = new Vector3(-21f / 32f, -2.53125f, obj.transform.position.z);
			obj.transform.position = pos;
			obj.SetActive(true);
		}
		obj = GetPlatform();
		if (obj != null)
		{
			pos = new Vector3(21f / 32f, -2.53125f, obj.transform.position.z);
			obj.transform.position = pos;
			obj.SetActive(true);
		}
		marbleCombo = 0;
		noMarbleCombo = 0;
	}

	private void SpawnPlatform()
	{
		timer = 0f;
		obj = GetPlatform();
		if (obj != null)
		{
			pos = new Vector3(Random.Range(-2.75f, 1.4375f), -5.5f, obj.transform.position.z);
			obj.transform.position = pos;
			obj.SetActive(true);
			ManageSpawnMarble();
		}
		obj2 = GetPlatform();
		if (obj2 != null)
		{
			if (Random.Range(0f, 1f) < 0.7f && pos.x < -1.5f)
			{
				pos.x = 0f - pos.x;
				apart = true;
			}
			else
			{
				pos.x += 1.3125f;
				apart = false;
			}
			obj2.transform.position = pos;
			obj2.SetActive(true);
			ManageSpawnMarble();
		}
		if (!(obj != null) || !(obj2 != null))
		{
			return;
		}
		if (apart)
		{
			float num = Random.Range(0f, 1f);
			if (num < 0.33f)
			{
				fragileCombo++;
				obj.transform.GetComponent<RAT_PlatformController>().Fragile(false);
				obj2.transform.GetComponent<RAT_PlatformController>().Fragile(true);
			}
			else if (num < 0.66f)
			{
				fragileCombo++;
				obj.transform.GetComponent<RAT_PlatformController>().Fragile(true);
				obj2.transform.GetComponent<RAT_PlatformController>().Fragile(false);
			}
			else
			{
				fragileCombo = 0;
				obj.transform.GetComponent<RAT_PlatformController>().Fragile(false);
				obj2.transform.GetComponent<RAT_PlatformController>().Fragile(false);
			}
			return;
		}
		float num2 = Random.Range(0f, 1f);
		if (num2 < 0.4f || fragileCombo > 0)
		{
			fragileCombo = 0;
			obj.transform.GetComponent<RAT_PlatformController>().Fragile(false);
			obj2.transform.GetComponent<RAT_PlatformController>().Fragile(false);
		}
		else if (num2 < 0.75f)
		{
			fragileCombo++;
			obj.transform.GetComponent<RAT_PlatformController>().Fragile(true);
			obj2.transform.GetComponent<RAT_PlatformController>().Fragile(true);
		}
		else if (num2 < 0.875f)
		{
			fragileCombo++;
			obj.transform.GetComponent<RAT_PlatformController>().Fragile(false);
			obj2.transform.GetComponent<RAT_PlatformController>().Fragile(true);
		}
		else
		{
			fragileCombo++;
			obj.transform.GetComponent<RAT_PlatformController>().Fragile(true);
			obj2.transform.GetComponent<RAT_PlatformController>().Fragile(false);
		}
	}

	private void ManageSpawnMarble()
	{
		if (marbleCombo == 0 && Random.Range(0f, 1f) < 0.5f)
		{
			SpawnMarble();
			return;
		}
		if (noMarbleCombo > 6)
		{
			SpawnMarble();
			return;
		}
		noMarbleCombo++;
		marbleCombo = 0;
	}

	private void SpawnMarble()
	{
		marbleCombo++;
		noMarbleCombo = 0;
		marble = MarbleCreator.marbleCreator.GetMarble();
		pos.y += 0.75f;
		marble.transform.position = pos;
		SetMarbleConstants(marble);
		pos.y -= 0.75f;
	}

	private void CreatePlatformsPool()
	{
		platformPrefabs = new List<GameObject>();
		for (int i = 0; i < platformPooledAmount; i++)
		{
			obj = Object.Instantiate(platformPrefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			platformPrefabs.Add(obj);
		}
	}

	private GameObject GetPlatform()
	{
		for (int i = 0; i < platformPooledAmount; i++)
		{
			if (!platformPrefabs[i].activeInHierarchy)
			{
				return platformPrefabs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = RAT_Difficulty.RAT_difficulty.speed;
		marbleController.direction = MarbleController.Direction.up;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 7.1f;
		marble.SetActive(true);
	}
}
