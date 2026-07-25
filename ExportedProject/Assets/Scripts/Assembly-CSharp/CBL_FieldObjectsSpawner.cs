using System.Collections.Generic;
using UnityEngine;

public class CBL_FieldObjectsSpawner : MonoBehaviour
{
	public static CBL_FieldObjectsSpawner CBL_fieldObjectsSpawner;

	public List<GameObject> balls;

	public GameObject ball;

	private int ballPooledAmount = 2;

	public List<GameObject> pennants;

	public GameObject pennant;

	private int pennantPooledAmount = 10;

	private GameObject obj;

	private Transform objTR;

	private Rigidbody2D rb;

	private float frequency;

	private float speed;

	private float timer;

	private float maxPennants;

	private bool odd;

	private Vector3 pos;

	private Vector3 lastBallPos;

	private int r;

	private void Start()
	{
		CBL_fieldObjectsSpawner = this;
		PoolBalls();
		PoolPennants();
		SetDifficultyParas();
		pos = new Vector3(0f, 5f, 0f);
	}

	private void Update()
	{
		ManageSpawn();
	}

	public void SetDifficultyParas()
	{
		speed = CBL_Difficulty.CBL_difficulty.scrollSpeed;
		frequency = CBL_Difficulty.CBL_difficulty.objectFrequency;
		maxPennants = CBL_Difficulty.CBL_difficulty.maxPennants;
	}

	private void ManageSpawn()
	{
		if (timer >= frequency && !CBL_PlayerController.CBL_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			SpawnObject();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	private void SpawnObject()
	{
		obj = GetBall();
		if (!odd && obj != null)
		{
			odd = true;
			objTR = obj.transform;
			pos.z = objTR.position.z;
			pos.y = 4.5f;
			pos.x = Random.Range(-3f, 3f);
			objTR.position = pos;
			lastBallPos = pos;
			obj.SetActive(true);
			rb = obj.GetComponent<Rigidbody2D>();
			rb.velocity = new Vector2(0f, 0f - speed);
			rb.angularVelocity = 0f;
			rb.rotation = 0f;
			obj.GetComponent<CBL_BallController>().Enable();
			if (maxPennants > 1f)
			{
				if (pos.x > 1f || pos.x < -1f)
				{
					obj = GetPennant();
					pos.x *= -1f;
					obj.transform.position = pos;
					obj.SetActive(true);
					return;
				}
				pos.x = Random.Range(-3f, -2f);
				obj = GetPennant();
				obj.transform.position = pos;
				obj.SetActive(true);
				pos.x = Random.Range(2f, 3f);
				obj = GetPennant();
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			return;
		}
		odd = false;
		obj = GetPennant();
		objTR = obj.transform;
		if (maxPennants == 1f)
		{
			pos.z = objTR.position.z;
			pos.y = 5f;
			pos.x = Random.Range(-2.5f, 2.5f);
			EnsurePennantIsNotOnBallsWay();
			objTR.position = pos;
			obj.SetActive(true);
		}
		else if (maxPennants == 2f)
		{
			pos.z = objTR.position.z;
			pos.y = 5f;
			pos.x = Random.Range(-2.5f, -1f);
			EnsurePennantIsNotOnBallsWay();
			objTR.position = pos;
			obj.SetActive(true);
			obj = GetPennant();
			objTR = obj.transform;
			pos.z = objTR.position.z;
			pos.y = 5f;
			pos.x = Random.Range(1f, 2.5f);
			EnsurePennantIsNotOnBallsWay();
			objTR.position = pos;
			obj.SetActive(true);
		}
		else if (maxPennants == 3f)
		{
			r = Random.Range(0, 7);
			if (r == 0)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 0f;
				objTR.position = pos;
				obj.SetActive(true);
			}
			else if (r == 1)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -3f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 3f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = Random.Range(-1f, 1f);
				objTR.position = pos;
				obj.SetActive(true);
			}
			else if (r == 2)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -1.1f;
				objTR.position = pos;
				obj.SetActive(true);
			}
			else if (r == 3)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 1.1f;
				objTR.position = pos;
				obj.SetActive(true);
			}
			else if (r == 4)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -0.1f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -1.1f;
				objTR.position = pos;
				obj.SetActive(true);
			}
			else if (r == 5)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 2.2f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 0.1f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 1.1f;
				objTR.position = pos;
				obj.SetActive(true);
			}
			else if (r == 6)
			{
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = -1.1f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 1.1f;
				objTR.position = pos;
				obj.SetActive(true);
				obj = GetPennant();
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.y = 5f;
				pos.x = 0f;
				objTR.position = pos;
				obj.SetActive(true);
			}
		}
	}

	private void EnsurePennantIsNotOnBallsWay()
	{
		if (pos.x >= lastBallPos.x - 1f && pos.x <= lastBallPos.x + 1f)
		{
			if (pos.x >= 1f)
			{
				pos.x += 1f;
			}
			else if (pos.x <= 1f)
			{
				pos.x -= 1f;
			}
		}
		if (pos.x > 2f)
		{
			pos.x = 3.3f;
		}
		else if (pos.x < -2f)
		{
			pos.x = -3.3f;
		}
	}

	private void PoolBalls()
	{
		balls = new List<GameObject>();
		for (int i = 0; i < ballPooledAmount; i++)
		{
			obj = Object.Instantiate(ball);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			balls.Add(obj);
		}
	}

	public GameObject GetBall()
	{
		for (int i = 0; i < ballPooledAmount; i++)
		{
			if (!balls[i].activeInHierarchy)
			{
				return balls[i];
			}
		}
		return null;
	}

	private void PoolPennants()
	{
		pennants = new List<GameObject>();
		for (int i = 0; i < pennantPooledAmount; i++)
		{
			obj = Object.Instantiate(pennant);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			pennants.Add(obj);
		}
	}

	public GameObject GetPennant()
	{
		for (int i = 0; i < pennantPooledAmount; i++)
		{
			if (!pennants[i].activeInHierarchy)
			{
				return pennants[i];
			}
		}
		Debug.Log("ERROR: GetPennant returned null on CBL_BallSpawner.");
		return null;
	}
}
