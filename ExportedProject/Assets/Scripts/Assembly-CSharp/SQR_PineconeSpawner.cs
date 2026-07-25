using System.Collections.Generic;
using UnityEngine;

public class SQR_PineconeSpawner : MonoBehaviour
{
	public static SQR_PineconeSpawner SQR_pineconeSpawner;

	private List<GameObject> pinecones;

	private int pooledAmount = 6;

	public GameObject pinecone;

	private List<GameObject> branches;

	public GameObject branch;

	private float frequency;

	private float timer;

	private int maxItems;

	private Vector3 pos;

	private GameObject obj;

	private float n;

	private GameObject pine;

	private float originalPosX;

	private void Start()
	{
		SQR_pineconeSpawner = this;
		SetDifficultyParas();
		timer = frequency;
		CreatePool();
	}

	private void Update()
	{
		if (timer >= frequency && !SQR_PlayerController.SQR_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = SQR_Difficulty.SQR_difficulty.pinecone_frequency;
		maxItems = SQR_Difficulty.SQR_difficulty.pinecone_max;
		timer = 0f;
	}

	private void CreatePool()
	{
		pinecones = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(pinecone);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			pinecones.Add(obj);
		}
		branches = new List<GameObject>();
		for (int j = 0; j < pooledAmount; j++)
		{
			GameObject gameObject = Object.Instantiate(branch);
			gameObject.SetActive(false);
			gameObject.transform.parent = base.gameObject.transform;
			branches.Add(gameObject);
		}
	}

	private void CreateBatch()
	{
		pos.y = Random.Range(-7f, -5f);
		pos.x = Random.Range(-4f, 4f);
		ActivateItem();
		if (maxItems == 2)
		{
			pos.y = Random.Range(-7f, -5f);
			n = Random.Range(1f, 8f);
			if (n <= 2f)
			{
				n = 1f;
			}
			else if (n < 5f)
			{
				n = 5f;
			}
			if (pos.x < 0f)
			{
				pos.x += n;
			}
			else
			{
				pos.x -= n;
			}
			if (pos.x > 4f)
			{
				pos.x = 4f;
			}
			else if (pos.x < -4f)
			{
				pos.x = -4f;
			}
			ActivateItem();
		}
	}

	private void ActivateItem()
	{
		int itemIndex = GetItemIndex();
		if (itemIndex != -1)
		{
			pine = pinecones[itemIndex];
			pos.z = pine.transform.position.z;
			pine.transform.position = pos;
			pine.SetActive(true);
			originalPosX = pos.x;
			obj = branches[itemIndex];
			pos.z = obj.transform.position.z;
			pos.y += 0.0625f;
			if (pos.x > 0f)
			{
				pos.x += 2.125f;
				obj.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				pos.x -= 2.125f;
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			obj.transform.position = pos;
			obj.SetActive(true);
			pos.x = originalPosX;
		}
	}

	private int GetItemIndex()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!pinecones[i].activeInHierarchy)
			{
				return i;
			}
		}
		return -1;
	}
}
