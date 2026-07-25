using System.Collections.Generic;
using UnityEngine;

public class BAT_StalactitesSpawner : MonoBehaviour
{
	public static BAT_StalactitesSpawner BAT_stalactitesSpawner;

	public List<GameObject> stalactites;

	public GameObject stalactite;

	private int stalactitePooledAmount = 30;

	public List<GameObject> BgStalactites;

	public GameObject BgStalactite;

	private int BgStalactitePooledAmount = 10;

	private GameObject obj;

	private MarbleController marbleController;

	private Vector3 pos;

	private Transform objTR;

	private Transform tr;

	public float timer;

	public float frequency;

	private float minHeight;

	private float maxHeight;

	private float marbleSpeed;

	public float timerBg;

	public float frequencyBg;

	private float y;

	private float r;

	private bool firstStalactite = true;

	private bool odd = true;

	private void Start()
	{
		BAT_stalactitesSpawner = this;
		tr = base.transform;
		PoolStalactites();
		PoolBgStalactites();
		SpawnInitialBgStalactite();
		SpawnInitialMiniStalactite();
		SetDifficultyParas();
	}

	private void Update()
	{
		ManageSpawns();
		timer += Time.deltaTime;
		timerBg += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = BAT_Difficulty.BAT_difficulty.frequency;
		minHeight = BAT_Difficulty.BAT_difficulty.minHeight;
		maxHeight = BAT_Difficulty.BAT_difficulty.maxHeight;
		marbleSpeed = BAT_Difficulty.BAT_difficulty.speed;
		timer = frequency - 0.5f;
		frequencyBg = frequency / 2f;
	}

	private void ManageSpawns()
	{
		if (timer >= frequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			timer = 0f;
			if (odd)
			{
				SpawnMarble();
				odd = false;
			}
			else
			{
				SpawnStalactite();
				odd = true;
			}
		}
		if (timerBg >= frequencyBg)
		{
			timerBg = 0f;
			SpawnBgStalactite();
			SpawnMiniStalactites();
		}
	}

	private void SpawnMarble()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			obj = MarbleCreator.marbleCreator.GetMarble();
			marbleController = obj.GetComponent<MarbleController>();
			marbleController.speed = marbleSpeed;
			marbleController.direction = MarbleController.Direction.left;
			marbleController.maxOut = 7f;
			marbleController.shadow.GetComponent<SpriteRenderer>().color = Color.black;
			marbleController.shadow.GetComponent<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
			objTR = obj.transform;
			objTR.position = new Vector3(Random.Range(4.5f, 5.5f), Random.Range(-2f, 2f), 0f);
			obj.SetActive(true);
			if (Random.Range(0f, 1f) < 0.33f && (objTR.position.y > 1f || objTR.position.y < -1f))
			{
				y = objTR.position.y;
				obj = MarbleCreator.marbleCreator.GetMarble();
				marbleController = obj.GetComponent<MarbleController>();
				marbleController.speed = marbleSpeed;
				marbleController.direction = MarbleController.Direction.left;
				marbleController.maxOut = 7f;
				marbleController.shadow.GetComponent<SpriteRenderer>().color = Color.black;
				marbleController.shadow.GetComponent<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
				obj.transform.position = new Vector3(Random.Range(4.5f, 5.5f), 0f - y, 0f);
				obj.SetActive(true);
			}
		}
	}

	private void SpawnStalactite()
	{
		obj = GetStalactite();
		if (obj == null)
		{
			return;
		}
		objTR = obj.transform;
		pos = new Vector3(5f, 0f, objTR.position.z);
		r = Random.Range(0f, 1f);
		if ((double)r <= 0.7)
		{
			firstStalactite = false;
			pos.y = Random.Range(minHeight, maxHeight);
			objTR.localScale = new Vector3(2f, 1f, 1f);
			if ((double)Random.Range(0f, 1f) < 0.5)
			{
				pos.y *= -1f;
				objTR.localScale = new Vector3(2f, -1f, 1f);
			}
			objTR.position = pos;
			obj.SetActive(true);
		}
		else if (!firstStalactite)
		{
			pos.y = Random.Range(1.5f, 5f);
			objTR.localScale = new Vector3(2f, 1f, 1f);
			objTR.position = pos;
			obj.SetActive(true);
			obj = GetStalactite();
			if (!(obj == null))
			{
				objTR = obj.transform;
				pos.y -= Random.Range(6.75f, 6.75f + minHeight / 2f);
				objTR.localScale = new Vector3(2f, -1f, 1f);
				objTR.position = pos;
				obj.SetActive(true);
			}
		}
	}

	private void SpawnMiniStalactites()
	{
		CreateMiniStalactite(true, Random.Range(5f, 6f));
		CreateMiniStalactite(false, Random.Range(5f, 6f));
	}

	private void CreateMiniStalactite(bool up, float x)
	{
		obj = GetStalactite();
		if (!(obj == null))
		{
			objTR = obj.transform;
			pos = new Vector3(x, 0f, obj.transform.position.z);
			pos.y = Random.Range(5.5f, 6f);
			if (up)
			{
				objTR.localScale = new Vector3(2f, 1f, 1f);
			}
			else
			{
				pos.y *= -1f;
				objTR.localScale = new Vector3(2f, -1f, 1f);
			}
			objTR.position = pos;
			obj.SetActive(true);
		}
	}

	private void SpawnBgStalactite()
	{
		obj = GetBgStalactite();
		if (obj == null)
		{
			return;
		}
		objTR = obj.transform;
		pos = new Vector3(5f, 0f, obj.transform.position.z);
		r = Random.Range(0f, 1f);
		if ((double)r <= 0.7)
		{
			pos.y = Random.Range(1f, 4f);
			objTR.localScale = new Vector3(2f, 1f, 1f);
			if ((double)Random.Range(0f, 1f) < 0.5)
			{
				pos.y *= -1f;
				objTR.localScale = new Vector3(2f, -1f, 1f);
			}
			objTR.position = pos;
			obj.SetActive(true);
			return;
		}
		pos.y = Random.Range(3f, 4f);
		objTR.localScale = new Vector3(2f, 1f, 1f);
		objTR.position = pos;
		obj.SetActive(true);
		obj = GetBgStalactite();
		if (!(obj == null))
		{
			objTR = obj.transform;
			pos.y *= -1f;
			objTR.localScale = new Vector3(2f, -1f, 1f);
			objTR.position = pos;
			obj.SetActive(true);
		}
	}

	public void SpawnInitialMiniStalactite()
	{
		CreateMiniStalactite(true, Random.Range(-4f, -3f));
		CreateMiniStalactite(false, Random.Range(-4f, -3f));
		CreateMiniStalactite(true, Random.Range(-2f, -1f));
		CreateMiniStalactite(false, Random.Range(-2f, -1f));
		CreateMiniStalactite(true, Random.Range(1f, 2f));
		CreateMiniStalactite(false, Random.Range(1f, 2f));
		CreateMiniStalactite(true, Random.Range(3f, 4f));
		CreateMiniStalactite(false, Random.Range(3f, 4f));
	}

	public void SpawnInitialBgStalactite()
	{
		pos = new Vector3(-3f, 0f, 0f);
		CreateBgStalactite(GetBgStalactite(), pos);
		pos = new Vector3(-1f, 0f, 0f);
		CreateBgStalactite(GetBgStalactite(), pos);
		pos = new Vector3(1f, 0f, 0f);
		CreateBgStalactite(GetBgStalactite(), pos);
		pos = new Vector3(3f, 0f, 0f);
		CreateBgStalactite(GetBgStalactite(), pos);
	}

	private void CreateBgStalactite(GameObject obj, Vector3 pos)
	{
		if (!(obj == null))
		{
			objTR = obj.transform;
			pos.z = obj.transform.position.z;
			pos.y = Random.Range(1f, 4f);
			objTR.localScale = new Vector3(2f, 1f, 1f);
			if ((double)Random.Range(0f, 1f) < 0.5)
			{
				pos.y *= -1f;
				objTR.localScale = new Vector3(2f, -1f, 1f);
			}
			objTR.position = pos;
			obj.SetActive(true);
		}
	}

	private void PoolStalactites()
	{
		stalactites = new List<GameObject>();
		for (int i = 0; i < stalactitePooledAmount; i++)
		{
			obj = Object.Instantiate(stalactite);
			obj.SetActive(false);
			obj.transform.parent = tr;
			stalactites.Add(obj);
		}
	}

	public GameObject GetStalactite()
	{
		for (int i = 0; i < stalactitePooledAmount; i++)
		{
			if (!stalactites[i].activeInHierarchy)
			{
				return stalactites[i];
			}
		}
		return null;
	}

	private void PoolBgStalactites()
	{
		BgStalactites = new List<GameObject>();
		for (int i = 0; i < BgStalactitePooledAmount; i++)
		{
			obj = Object.Instantiate(BgStalactite);
			obj.SetActive(false);
			obj.transform.parent = tr;
			BgStalactites.Add(obj);
		}
	}

	public GameObject GetBgStalactite()
	{
		for (int i = 0; i < BgStalactitePooledAmount; i++)
		{
			if (!BgStalactites[i].activeInHierarchy)
			{
				return BgStalactites[i];
			}
		}
		return null;
	}
}
