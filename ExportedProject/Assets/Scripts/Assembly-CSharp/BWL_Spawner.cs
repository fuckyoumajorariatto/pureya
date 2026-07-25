using System.Collections.Generic;
using UnityEngine;

public class BWL_Spawner : MonoBehaviour
{
	public BWL_PlayerController playerController;

	private Transform tr;

	private List<GameObject> pinPrefabs;

	private int pinPooledAmount = 10;

	public GameObject pin;

	private GameObject obj;

	private Transform objTR;

	private float timer;

	private GameObject marble;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private int comboMarble;

	private int comboNoMarble;

	private Vector3 pos;

	private void Start()
	{
		tr = base.transform;
		CreatecactusPool();
	}

	private void Update()
	{
		if (!playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			timer += Time.deltaTime;
			if (timer >= BWL_Difficulty.BWL_difficulty.frequency)
			{
				CreateBatch();
				timer = 0f;
			}
		}
	}

	public void DoReset()
	{
		for (int i = 0; i < pinPrefabs.Count; i++)
		{
			pinPrefabs[i].SetActive(false);
		}
		timer = BWL_Difficulty.BWL_difficulty.frequency;
	}

	private void CreateBatch()
	{
		pos.y = -5.25f;
		pos.z = -1f;
		float num = Random.Range(0f, 1f);
		if (num < 1f - BWL_Difficulty.BWL_difficulty.marbleChance && (comboNoMarble < 2 || comboMarble > 1))
		{
			comboNoMarble++;
			comboMarble = 0;
			if (Random.Range(0f, 1f) < 0.5f || BWL_Difficulty.BWL_difficulty.max == 1)
			{
				obj = GetPin();
				if (!(obj == null))
				{
					objTR = obj.transform;
					objTR.position = new Vector3(Random.Range(-2.875f, 2.875f), pos.y, objTR.position.z);
					obj.SetActive(true);
				}
				return;
			}
			num = Random.Range(0f, 1f);
			if (num < 0.3f)
			{
				obj = GetPin();
				if (!(obj == null))
				{
					objTR = obj.transform;
					objTR.position = new Vector3(-2.5f, pos.y, objTR.position.z);
					obj.SetActive(true);
					obj = GetPin();
					if (!(obj == null))
					{
						objTR = obj.transform;
						objTR.position = new Vector3(0f, pos.y, objTR.position.z);
						obj.SetActive(true);
					}
				}
				return;
			}
			if (num < 0.6f)
			{
				obj = GetPin();
				if (!(obj == null))
				{
					objTR = obj.transform;
					objTR.position = new Vector3(2.5f, pos.y, objTR.position.z);
					obj.SetActive(true);
					obj = GetPin();
					if (!(obj == null))
					{
						objTR = obj.transform;
						objTR.position = new Vector3(0f, pos.y, objTR.position.z);
						obj.SetActive(true);
					}
				}
				return;
			}
			obj = GetPin();
			if (!(obj == null))
			{
				objTR = obj.transform;
				objTR.position = new Vector3(Random.Range(-2.875f, -1f), pos.y, objTR.position.z);
				obj.SetActive(true);
				obj = GetPin();
				if (!(obj == null))
				{
					objTR = obj.transform;
					objTR.position = new Vector3(Random.Range(1f, 2.875f), pos.y, objTR.position.z);
					obj.SetActive(true);
				}
			}
			return;
		}
		num = Random.Range(0f, 1f);
		comboNoMarble = 0;
		if (num < 0.5f && comboMarble < 1)
		{
			comboMarble++;
			marble = MarbleCreator.marbleCreator.GetMarble();
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x = Random.Range(-2.875f, -1f);
			}
			else
			{
				pos.x = Random.Range(1f, 2.875f);
			}
			SetMarbleConstants(marble);
			return;
		}
		num = Random.Range(0f, 1f);
		bool flag = false;
		if (num < 0.3f)
		{
			flag = true;
			obj = GetPin();
			if (obj == null)
			{
				return;
			}
			objTR = obj.transform;
			objTR.position = new Vector3(Random.Range(-2.875f, 0f), pos.y, objTR.position.z);
			obj.SetActive(true);
		}
		else if (num < 0.6f)
		{
			obj = GetPin();
			if (obj == null)
			{
				return;
			}
			objTR = obj.transform;
			objTR.position = new Vector3(Random.Range(0f, 2.875f), pos.y, objTR.position.z);
			obj.SetActive(true);
		}
		if (flag)
		{
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = Random.Range(1f, 2.875f);
			SetMarbleConstants(marble);
		}
		else
		{
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.x = Random.Range(-2.875f, -1f);
			SetMarbleConstants(marble);
		}
	}

	private void CreatecactusPool()
	{
		pinPrefabs = new List<GameObject>();
		for (int i = 0; i < pinPooledAmount; i++)
		{
			obj = Object.Instantiate(pin);
			obj.SetActive(false);
			obj.transform.parent = tr;
			pinPrefabs.Add(obj);
		}
	}

	private GameObject GetPin()
	{
		for (int i = 0; i < pinPooledAmount; i++)
		{
			if (!pinPrefabs[i].activeInHierarchy)
			{
				return pinPrefabs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.position = pos;
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.direction = MarbleController.Direction.up;
		marbleController.speed = BWL_Difficulty.BWL_difficulty.scroll;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 5.5f;
		marble.SetActive(true);
	}
}
