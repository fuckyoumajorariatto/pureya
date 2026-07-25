using System.Collections.Generic;
using UnityEngine;

public class FLA_ShellSpawner : MonoBehaviour
{
	public static FLA_ShellSpawner FLA_shellSpawner;

	private List<GameObject> shells1;

	private List<GameObject> shells2;

	private List<GameObject> shells3;

	private int pooledAmount = 3;

	public GameObject shell1;

	public GameObject shell2;

	public GameObject shell3;

	private float frequency;

	private float timer;

	private GameObject obj;

	private Vector3 pos;

	private Transform lastShellTR;

	public int noMarbleCombo;

	public int marbleCombo;

	private void Start()
	{
		FLA_shellSpawner = this;
		SetDifficultyParas();
		CreatePool();
	}

	private void Update()
	{
		if (timer >= frequency && !FLA_PlayerController.FLA_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			timer = 0f;
			CreateBatch();
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = FLA_Difficulty.FLA_difficulty.frequency;
		timer = 0f;
	}

	private void CreatePool()
	{
		shells1 = new List<GameObject>();
		shells2 = new List<GameObject>();
		shells3 = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(shell1);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			shells1.Add(obj);
			obj = Object.Instantiate(shell2);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			shells2.Add(obj);
			obj = Object.Instantiate(shell3);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			shells3.Add(obj);
		}
	}

	private void CreateBatch()
	{
		if ((Random.Range(0f, 1f) < 0.5f && noMarbleCombo < 3) || marbleCombo > 2)
		{
			noMarbleCombo++;
			marbleCombo = 0;
			pos.x = Random.Range(-6.5f, -5.5f);
			if (lastShellTR != null && pos.x > lastShellTR.position.x - 3f)
			{
				pos.x = lastShellTR.position.x - 3f;
				timer = 0f - FLA_Difficulty.FLA_difficulty.frequency;
			}
			ActivateItem();
		}
		else
		{
			marbleCombo++;
			noMarbleCombo = 0;
			SpawnMarble();
		}
	}

	private void SpawnMarble()
	{
		pos.x = Random.Range(-6.5f, -5.5f);
		pos.y = -2.875f;
		if (lastShellTR != null && pos.x > lastShellTR.position.x - 1.5f)
		{
			pos.x = lastShellTR.position.x - 1.5f;
			timer = 0f - FLA_Difficulty.FLA_difficulty.frequency;
		}
		FLA_MarbleSpawner.FLA_marbleSpawner.CreateMarble(pos);
	}

	private void ActivateItem()
	{
		float num = Random.Range(0f, 1f);
		if (num < 0.33f)
		{
			obj = GetShell1();
			if (obj == null)
			{
				obj = GetShell2();
			}
		}
		else if (num < 0.66f)
		{
			obj = GetShell2();
			if (obj == null)
			{
				obj = GetShell1();
			}
		}
		else
		{
			obj = GetShell3();
			if (obj == null)
			{
				obj = GetShell1();
			}
			if (obj == null)
			{
				obj = GetShell2();
			}
		}
		if (obj != null)
		{
			lastShellTR = obj.transform;
			pos.y = -3.125f;
			pos.z = lastShellTR.position.z;
			lastShellTR.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetShell1()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!shells1[i].activeInHierarchy)
			{
				return shells1[i];
			}
		}
		return null;
	}

	private GameObject GetShell2()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!shells2[i].activeInHierarchy)
			{
				return shells2[i];
			}
		}
		return null;
	}

	private GameObject GetShell3()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!shells3[i].activeInHierarchy)
			{
				return shells3[i];
			}
		}
		return null;
	}
}
