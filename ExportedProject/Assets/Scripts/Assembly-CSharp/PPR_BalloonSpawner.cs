using System.Collections.Generic;
using UnityEngine;

public class PPR_BalloonSpawner : MonoBehaviour
{
	public static PPR_BalloonSpawner PPR_balloonSpawner;

	private List<GameObject> balloons;

	private int pooledAmount = 6;

	public GameObject balloon;

	private float frequency;

	private float timer;

	private int maxBallons;

	private Color32[] colors = new Color32[4]
	{
		new Color32(byte.MaxValue, 224, 46, byte.MaxValue),
		new Color32(byte.MaxValue, 71, 71, byte.MaxValue),
		new Color32(71, 112, byte.MaxValue, byte.MaxValue),
		new Color32(byte.MaxValue, 78, 173, byte.MaxValue)
	};

	private Vector3 pos;

	private GameObject obj;

	private float y;

	private void Start()
	{
		PPR_balloonSpawner = this;
		SetDifficultyParas();
		timer = frequency;
		CreatePool();
	}

	private void Update()
	{
		if (timer >= frequency && !PPR_PlayerController.PPR_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = PPR_Difficulty.PPR_difficulty.balloon_frequency;
		maxBallons = PPR_Difficulty.PPR_difficulty.balloon_max;
		timer = 0f;
	}

	private void CreatePool()
	{
		balloons = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(balloon);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			balloons.Add(obj);
		}
	}

	private void CreateBatch()
	{
		pos.y = Random.Range(-6f, 3.5f);
		pos.x = Random.Range(5f, 8f);
		ActivateBalloon();
		if (maxBallons != 2)
		{
			return;
		}
		y = Random.Range(-5f, 3.5f);
		if ((y > pos.y && y < pos.y + 2f) || (y < pos.y && y > pos.y - 2f) || y == pos.y)
		{
			if (pos.y <= 0f)
			{
				y = pos.y + 3f;
			}
			else
			{
				y = pos.y - 3f;
			}
		}
		pos.y = y;
		pos.x = Random.Range(5f, 9f);
		ActivateBalloon();
	}

	private void ActivateBalloon()
	{
		obj = GetBalloon();
		if (obj != null)
		{
			obj.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetBalloon()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!balloons[i].activeInHierarchy)
			{
				return balloons[i];
			}
		}
		return null;
	}
}
