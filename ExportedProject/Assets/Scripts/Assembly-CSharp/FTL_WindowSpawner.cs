using System.Collections.Generic;
using UnityEngine;

public class FTL_WindowSpawner : MonoBehaviour
{
	public static FTL_WindowSpawner FTL_windowSpawner;

	public List<GameObject> windows;

	public List<GameObject> fireWindows;

	public int pooledAmount;

	public GameObject window;

	public GameObject fireWindow;

	private float frequency;

	public float timer;

	private bool firstMarble;

	private Vector3 pos;

	public bool firstBatch = true;

	private GameObject obj;

	private int fire1;

	private int fire2;

	private float diff;

	private void Start()
	{
		pooledAmount = 36;
		FTL_windowSpawner = this;
		SetDifficultyParas();
		timer = frequency;
		CreatePool();
	}

	private void Update()
	{
		if (timer >= frequency && !FTL_PlayerController.FTL_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = FTL_Difficulty.FTL_difficulty.frequency;
	}

	private void CreatePool()
	{
		windows = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(window);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			windows.Add(obj);
		}
		fireWindows = new List<GameObject>();
		for (int j = 0; j < pooledAmount; j++)
		{
			obj = Object.Instantiate(fireWindow);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			fireWindows.Add(obj);
		}
	}

	private void CreateBatch()
	{
		fire1 = Random.Range(0, 4);
		fire2 = Random.Range(0, 4);
		if (fire2 == fire1)
		{
			fire2++;
			if (fire2 == 4)
			{
				fire2 = 0;
			}
		}
		pos.x = 5f;
		diff = 0f;
		if (firstBatch)
		{
			firstBatch = false;
			diff = 1f;
		}
		else if (Random.Range(0f, 1f) < 0.5f)
		{
			diff = 1f;
		}
		pos.y = -2f + diff;
		if (fire1 == 0 || fire2 == 0)
		{
			ActivateFireWindow();
		}
		else
		{
			ActivateWindow();
			if (Random.Range(0f, 1f) < 0.5f)
			{
				firstMarble = true;
				FTL_MarbleSpawner.FTL_marbleSpawner.CreateMarble(pos.x, pos.y);
			}
		}
		pos.y = -0.5f + diff;
		if (fire1 == 1 || fire2 == 1)
		{
			ActivateFireWindow();
		}
		else
		{
			ActivateWindow();
			if (Random.Range(0f, 1f) < 0.5f)
			{
				firstMarble = true;
				FTL_MarbleSpawner.FTL_marbleSpawner.CreateMarble(pos.x, pos.y);
			}
			else if (!firstMarble)
			{
				firstMarble = true;
				FTL_MarbleSpawner.FTL_marbleSpawner.CreateMarble(pos.x, pos.y);
			}
		}
		pos.y = 1f + diff;
		if (fire1 == 2 || fire2 == 2)
		{
			ActivateFireWindow();
		}
		else
		{
			ActivateWindow();
			if (Random.Range(0f, 1f) < 0.5f)
			{
				firstMarble = true;
				FTL_MarbleSpawner.FTL_marbleSpawner.CreateMarble(pos.x, pos.y);
			}
		}
		pos.y = 2.5f + diff;
		if (fire1 == 3 || fire2 == 3)
		{
			ActivateFireWindow();
			return;
		}
		ActivateWindow();
		if (Random.Range(0f, 1f) < 0.5f)
		{
			firstMarble = true;
			FTL_MarbleSpawner.FTL_marbleSpawner.CreateMarble(pos.x, pos.y);
		}
	}

	private float MakeNumberPixelPerfect(float n)
	{
		if ((double)n % 0.0625 != 0.0)
		{
			for (float num = -1.5f; (double)num < 3.5; num += 0.0625f)
			{
				if (n < num)
				{
					n = num;
					break;
				}
			}
		}
		if ((double)n % 0.0625 != 0.0)
		{
			n = 3.5f;
		}
		if (n < -0.75f)
		{
			n = -1.5f;
		}
		else if (n > 2.25f)
		{
			n = 3.5f;
		}
		return n;
	}

	private void ActivateWindow()
	{
		obj = GetWindow();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private void ActivateFireWindow()
	{
		obj = GetFireWindow();
		if (obj != null)
		{
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetWindow()
	{
		for (int i = 0; i < pooledAmount * 2; i++)
		{
			if (!windows[i].activeInHierarchy)
			{
				return windows[i];
			}
		}
		return null;
	}

	private GameObject GetFireWindow()
	{
		for (int i = 0; i < pooledAmount * 2; i++)
		{
			if (!fireWindows[i].activeInHierarchy)
			{
				return fireWindows[i];
			}
		}
		return null;
	}
}
