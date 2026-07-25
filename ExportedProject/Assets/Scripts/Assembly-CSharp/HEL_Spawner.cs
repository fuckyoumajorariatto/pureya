using System.Collections.Generic;
using UnityEngine;

public class HEL_Spawner : MonoBehaviour
{
	public GameObject ballPrefab;

	private List<GameObject> ballPrefabs;

	private int ballPooledAmount = 30;

	public GameObject craneArmPrefab;

	private List<GameObject> craneArmPrefabs;

	private int craneArmsPooledAmount = 10;

	private float timer;

	private GameObject obj;

	private Transform tr;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	public Color32 shadowColor;

	private float ballY1;

	private float ballY2;

	private float ballY3;

	private bool addCraneArm = true;

	private void Start()
	{
		CreateBallsPool();
		CreateCraneArmsPool();
		tr = base.transform;
		timer = HEL_Difficulty.HEL_difficulty.frequency;
		obj = GetCraneArm();
		if (obj != null)
		{
			obj.transform.position = new Vector3(Random.Range(-5f, 1f), -4.46875f, 0f);
			obj.SetActive(true);
		}
	}

	private void Update()
	{
		if (timer > HEL_Difficulty.HEL_difficulty.frequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	private void CreateBatch()
	{
		if (addCraneArm)
		{
			if (Random.Range(0f, 1f) < 0.7f)
			{
				obj = GetCraneArm();
				if (obj != null)
				{
					obj.transform.position = new Vector3(Random.Range(4f, 5f), -4.46875f, 0f);
					obj.SetActive(true);
				}
			}
			else
			{
				obj = GetCraneArm();
				if (obj != null)
				{
					obj.transform.position = new Vector3(4f, -4.46875f, 0f);
					obj.SetActive(true);
				}
				obj = GetCraneArm();
				if (obj != null)
				{
					obj.transform.position = new Vector3(5f, -4.46875f, 0f);
					obj.SetActive(true);
				}
			}
		}
		addCraneArm = !addCraneArm;
		float min = 0f;
		float max = 1f;
		if (HEL_Difficulty.HEL_difficulty.ballsMin == 2)
		{
			min = 0.33f;
		}
		else if (HEL_Difficulty.HEL_difficulty.ballsMin == 3)
		{
			min = 0.66f;
		}
		if (HEL_Difficulty.HEL_difficulty.ballsMax == 1)
		{
			max = 0.38f;
		}
		else if (HEL_Difficulty.HEL_difficulty.ballsMax == 2)
		{
			max = 0.75f;
		}
		float num = Random.Range(min, max);
		if (num < 0.38f)
		{
			obj = GetBall();
			if (obj != null)
			{
				pos.z = obj.transform.position.z;
				pos.x = 5.03125f + 0.0625f * (float)Random.Range(0, 9);
				pos.y = Random.Range(-2f, 2f);
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			marble = MarbleCreator.marbleCreator.GetMarble();
			if (pos.y > 0f)
			{
				pos.y = Random.Range(-1f, -3f);
			}
			else
			{
				pos.y = Random.Range(1f, 3f);
			}
			marble.transform.position = new Vector3(5f + 0.0625f * (float)Random.Range(0, 33), pos.y, -4.1f);
			SetMarbleConstants(marble);
			return;
		}
		if (num < 0.75f)
		{
			obj = GetBall();
			if (obj != null)
			{
				pos.z = obj.transform.position.z;
				pos.x = 5.03125f + 0.0625f * (float)Random.Range(0, 9);
				pos.y = Random.Range(-4f, 4f);
				obj.transform.position = pos;
				obj.SetActive(true);
				ballY1 = pos.y;
				if (Random.Range(0f, 1f) < 0.7f && (pos.y <= -1.25f || pos.y >= 1.25f))
				{
					if (pos.y > 1f || pos.y < -1f)
					{
						pos.y = 0f - pos.y;
					}
					else if (Random.Range(0f, 1f) < 0.5f)
					{
						pos.y = Random.Range(2f, 4f);
					}
					else
					{
						pos.y = Random.Range(-2f, -4f);
					}
					pos.x += 0.6875f;
				}
				else
				{
					if (pos.y >= 0f)
					{
						pos.y -= 0.75f;
					}
					else
					{
						pos.y += 0.75f;
					}
					pos.x += 0.6875f;
				}
				obj = GetBall();
				if (obj != null)
				{
					obj.transform.position = pos;
					obj.SetActive(true);
				}
				ballY2 = pos.y;
			}
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.y = Random.Range(-3.5f, 3.5f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				while (Mathf.Abs(ballY1 - pos.y) < 0.75f || Mathf.Abs(ballY2 - pos.y) < 0.75f)
				{
					pos.y += 0.1f;
				}
			}
			else
			{
				while (Mathf.Abs(ballY1 - pos.y) < 0.75f || Mathf.Abs(ballY2 - pos.y) < 0.75f)
				{
					pos.y -= 0.1f;
				}
			}
			marble.transform.position = new Vector3(5f + 0.0625f * (float)Random.Range(0, 33), pos.y, -4.1f);
			SetMarbleConstants(marble);
			return;
		}
		float[] array = new float[3];
		num = Random.Range(0f, 3f);
		if (num == 0f)
		{
			array[0] = 0f;
		}
		else if (num == 1f)
		{
			array[0] = 0.6875f;
		}
		else if (num == 2f)
		{
			array[0] = 1.375f;
		}
		num = Random.Range(0f, 3f);
		if (num == 0f)
		{
			array[1] = 0f;
		}
		else if (num == 1f)
		{
			array[1] = 0.6875f;
		}
		else if (num == 2f)
		{
			array[1] = 1.375f;
		}
		if (array[1] == array[0])
		{
			array[1] += 0.6875f;
			if (array[1] > 1.375f)
			{
				array[1] = 0f;
			}
		}
		if ((array[0] == 0f || array[1] == 0f) && (array[0] == 0.6875f || array[1] == 0.6875f))
		{
			array[2] = 1.375f;
		}
		else if ((array[0] == 0f || array[1] == 0f) && (array[0] == 1.375f || array[1] == 1.375f))
		{
			array[2] = 0.6875f;
		}
		else
		{
			array[2] = 0f;
		}
		if (Random.Range(0f, 1f) < 0.7f)
		{
			obj = GetBall();
			pos.z = obj.transform.position.z;
			if (obj != null)
			{
				pos.x = 5.03125f + array[0];
				pos.y = Random.Range(3f, 4f);
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			ballY1 = pos.y;
			obj = GetBall();
			pos.z = obj.transform.position.z;
			if (obj != null)
			{
				pos.x = 5.03125f + array[1];
				pos.y = Random.Range(-1f, 1f);
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			ballY2 = pos.y;
			obj = GetBall();
			pos.z = obj.transform.position.z;
			if (obj != null)
			{
				pos.x = 5.03125f + array[2];
				pos.y = Random.Range(-3f, -4f);
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			ballY3 = pos.y;
		}
		else
		{
			obj = GetBall();
			pos.z = obj.transform.position.z;
			if (obj != null)
			{
				pos.x = 5.03125f + array[0];
				pos.y = Random.Range(-4f, 2.5f);
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			ballY1 = pos.y;
			pos.y += 0.75f;
			pos.x = 5.03125f + array[1];
			obj = GetBall();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			ballY2 = pos.y;
			pos.y += 0.75f;
			pos.x = 5.03125f + array[2];
			obj = GetBall();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			ballY3 = pos.y;
		}
		marble = MarbleCreator.marbleCreator.GetMarble();
		pos.y = Random.Range(-3.5f, 3.5f);
		if (Random.Range(0f, 1f) < 0.5f)
		{
			while (Mathf.Abs(ballY1 - pos.y) < 0.75f || Mathf.Abs(ballY2 - pos.y) < 0.75f || Mathf.Abs(ballY3 - pos.y) < 0.75f)
			{
				pos.y += 0.1f;
			}
		}
		else
		{
			while (Mathf.Abs(ballY1 - pos.y) < 0.75f || Mathf.Abs(ballY2 - pos.y) < 0.75f || Mathf.Abs(ballY3 - pos.y) < 0.75f)
			{
				pos.y -= 0.1f;
			}
		}
		marble.transform.position = new Vector3(5f + 0.0625f * (float)Random.Range(0, 33), pos.y, -4.1f);
		SetMarbleConstants(marble);
	}

	private void AddCraneArm()
	{
		obj = GetCraneArm();
		if (obj != null)
		{
			obj.transform.position = new Vector3(4f, -3.6875f, obj.transform.position.z);
			obj.SetActive(true);
		}
	}

	private void CreateBallsPool()
	{
		ballPrefabs = new List<GameObject>();
		for (int i = 0; i < ballPooledAmount; i++)
		{
			obj = Object.Instantiate(ballPrefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			ballPrefabs.Add(obj);
		}
	}

	private GameObject GetBall()
	{
		for (int i = 0; i < ballPooledAmount; i++)
		{
			if (!ballPrefabs[i].activeInHierarchy)
			{
				return ballPrefabs[i];
			}
		}
		return null;
	}

	private void CreateCraneArmsPool()
	{
		craneArmPrefabs = new List<GameObject>();
		for (int i = 0; i < craneArmsPooledAmount; i++)
		{
			obj = Object.Instantiate(craneArmPrefab);
			obj.SetActive(false);
			obj.transform.parent = tr;
			craneArmPrefabs.Add(obj);
		}
	}

	private GameObject GetCraneArm()
	{
		for (int i = 0; i < craneArmsPooledAmount; i++)
		{
			if (!craneArmPrefabs[i].activeInHierarchy)
			{
				return craneArmPrefabs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = HEL_Difficulty.HEL_difficulty.speed;
		marbleController.direction = MarbleController.Direction.left;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 7.1f;
		marble.SetActive(true);
	}
}
