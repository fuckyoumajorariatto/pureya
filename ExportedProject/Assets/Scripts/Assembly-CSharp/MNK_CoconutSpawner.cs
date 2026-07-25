using System.Collections.Generic;
using UnityEngine;

public class MNK_CoconutSpawner : MonoBehaviour
{
	public static MNK_CoconutSpawner MNK_coconutSpawner;

	public List<GameObject> items;

	public int pooledAmount;

	public GameObject item;

	private GameObject lastItem;

	private float lastDrop1;

	private float lastDrop2;

	private float frequency;

	private int dropsSinceMarble;

	private float timer;

	private Vector3 pos;

	private int maxCoconutsWithoutMarble = 10;

	private int level;

	private bool dropped;

	private int control;

	private int n;

	private float posX;

	private GameObject marble;

	private GameObject obj;

	private float dropMargin = 0.25f;

	private Transform hitTR;

	private void Start()
	{
		MNK_coconutSpawner = this;
		pooledAmount = 20;
		CreatePool();
		CreateInitialBatch();
		SetDifficultyParas();
	}

	private void Update()
	{
		if (!MNK_PlayerController.MNK_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			ManageItemCreation();
			if (timer >= frequency)
			{
				Drop();
				Drop();
				timer = 0f;
			}
			else
			{
				timer += Time.deltaTime;
			}
		}
	}

	private void ManageItemCreation()
	{
		if (lastItem.transform.position.x >= -6f)
		{
			CreateItem();
		}
	}

	public void SetDifficultyParas()
	{
		frequency = MNK_Difficulty.MNK_difficulty.frequency;
	}

	private void Drop()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		dropped = false;
		control = 0;
		level = GlobalControl.globalControl.GetLevel();
		if (level == 2)
		{
			maxCoconutsWithoutMarble = 7;
		}
		else if (level == 3)
		{
			maxCoconutsWithoutMarble = 10;
		}
		else
		{
			maxCoconutsWithoutMarble = 5;
		}
		if (Random.Range(0f, 1f) < 0.85f && dropsSinceMarble < maxCoconutsWithoutMarble)
		{
			n = Random.Range(0, pooledAmount);
			while (!dropped && control < 100)
			{
				control++;
				if (items[n].activeInHierarchy && !items[n].GetComponent<MNK_CoconutController>().fall && CheckDropMargin(items[n].transform.position.x))
				{
					dropsSinceMarble++;
					items[n].GetComponent<MNK_CoconutController>().fall = true;
					lastDrop2 = lastDrop1;
					lastDrop1 = items[n].transform.position.x;
					dropped = true;
					ReplaceCoconut(items[n].transform.position.x);
				}
				n++;
				if (n >= pooledAmount)
				{
					n = 0;
				}
			}
		}
		else
		{
			n = Random.Range(0, pooledAmount);
			while (!dropped && control < 100)
			{
				control++;
				if (items[n].activeInHierarchy && !items[n].GetComponent<MNK_CoconutController>().fall && CheckDropMargin(items[n].transform.position.x) && items[n].transform.position.x > -5.5f && items[n].transform.position.x < 1f && !SceneChangeController.sceneChangeController.transitioning)
				{
					dropsSinceMarble = 0;
					posX = items[n].transform.position.x;
					marble = MNK_MarbleSpawner.MNK_marbleSpawner.CreateMarble(posX, 5f);
					marble.GetComponent<MarbleController>().speed = MNK_Difficulty.MNK_difficulty.fall_speed;
					lastDrop2 = lastDrop1;
					lastDrop1 = marble.transform.position.x;
					dropped = true;
				}
				n++;
				if (n >= pooledAmount)
				{
					n = 0;
				}
			}
		}
		control = 0;
	}

	private bool CheckDropMargin(float _posX)
	{
		if (_posX > lastDrop1 - dropMargin && _posX < lastDrop1 + dropMargin)
		{
			return false;
		}
		if (_posX > lastDrop2 - dropMargin && _posX < lastDrop2 + dropMargin)
		{
			return false;
		}
		hitTR = Physics2D.Raycast(new Vector2(_posX, 4.5f), new Vector2(0f, -1f), 2f).transform;
		if (hitTR == null)
		{
			return true;
		}
		return false;
	}

	public void ReplaceCoconut(float posX)
	{
		pos.x = posX;
		ActivateReplaceItem();
	}

	private void ReplaceMarble(float posX)
	{
		MNK_MarbleSpawner.MNK_marbleSpawner.CreateMarble(posX, 5f);
	}

	private void CreatePool()
	{
		items = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(item);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			items.Add(obj);
		}
	}

	private void CreateItem()
	{
		pos.x = lastItem.transform.position.x - 1.25f;
		ActivateItem();
	}

	public void CreateInitialBatch()
	{
		pos.x = 3.75f;
		ActivateItem();
		pos.x = 2.5f;
		ActivateItem();
		pos.x = 1.25f;
		ActivateItem();
		pos.x = 0f;
		ActivateItem();
		pos.x = -1.25f;
		ActivateItem();
		pos.x = -2.5f;
		ActivateItem();
		pos.x = -3.75f;
		ActivateItem();
		pos.x = -5f;
		ActivateItem();
		pos.x = -6.25f;
		ActivateItem();
	}

	private void ActivateItem()
	{
		obj = GetItem();
		if (obj != null)
		{
			pos.y = 5f;
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.GetComponent<MNK_CoconutController>().replacement = false;
			obj.SetActive(true);
			lastItem = obj;
		}
	}

	private void ActivateReplaceItem()
	{
		obj = GetItem();
		if (obj != null)
		{
			pos.y = 5f;
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.GetComponent<MNK_CoconutController>().replacement = true;
			obj.SetActive(true);
		}
	}

	private GameObject GetItem()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!items[i].activeInHierarchy)
			{
				return items[i];
			}
		}
		return null;
	}
}
