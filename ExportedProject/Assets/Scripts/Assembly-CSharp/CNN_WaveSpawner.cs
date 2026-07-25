using System.Collections.Generic;
using UnityEngine;

public class CNN_WaveSpawner : MonoBehaviour
{
	public Transform wavesContainerTR;

	public GameObject wave;

	private List<GameObject> waves;

	private int pooledAmount = 20;

	private float frequency;

	private float timer;

	private float[] ys = new float[13];

	private Vector3 pos;

	private GameObject obj;

	private Transform tr;

	private int r;

	private void Start()
	{
		SetDifficultyParas();
		timer = frequency;
		int num = 0;
		for (float num2 = -3.85f; num2 <= -1.25f; num2 += 0.2f)
		{
			ys[num] = num2;
			num++;
		}
		waves = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(wave, wavesContainerTR);
			obj.SetActive(false);
			waves.Add(obj);
		}
		CreateInitialWaves();
	}

	private void Update()
	{
		if (timer >= frequency)
		{
			CreateWave();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = CNN_Difficulty.CNN_difficulty.wave_frequency;
	}

	private int CreateWave()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!waves[i].activeInHierarchy)
			{
				pos = new Vector3(-5f, 0f, wavesContainerTR.position.z);
				r = Random.Range(0, ys.Length);
				pos.y = ys[r];
				if (pos.y == ys[0])
				{
					pos.z = -5f;
				}
				waves[i].SetActive(true);
				tr = waves[i].transform;
				tr.position = pos;
				if (pos.y > -2f)
				{
					tr.localScale = new Vector2(0.5f, 0.5f);
				}
				else
				{
					tr.localScale = new Vector2(1f, 1f);
				}
				return i;
			}
		}
		return 0;
	}

	private void CreateInitialWaves()
	{
		for (int i = 0; i < 20; i++)
		{
			int index = CreateWave();
			pos = new Vector3(Random.Range(-4, 4), 0f, wavesContainerTR.position.z);
			int num = Random.Range(0, 12);
			pos.y = ys[num];
			if (pos.y == ys[0])
			{
				pos.z = -5f;
			}
			waves[index].SetActive(true);
			tr = waves[index].transform;
			tr.position = pos;
			if (pos.y > -2f)
			{
				tr.localScale = new Vector2(0.5f, 0.5f);
			}
			else
			{
				tr.localScale = new Vector2(1f, 1f);
			}
		}
	}
}
