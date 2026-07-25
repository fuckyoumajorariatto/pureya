using System.Collections.Generic;
using UnityEngine;

public class BAT_BgDarknessSquareSpawner : MonoBehaviour
{
	public static BAT_BgDarknessSquareSpawner BAT_bgDarknessSquareSpawner;

	public List<GameObject> squares;

	public GameObject square;

	private int squarePooledAmount = 4;

	private GameObject obj;

	private Vector3 pos;

	private Transform tr;

	public float timer;

	public float frequency;

	private bool odd = true;

	private void Start()
	{
		BAT_bgDarknessSquareSpawner = this;
		PoolSquares();
		SpawnInitialSquares();
		SetDifficultyParas();
		tr = base.gameObject.transform;
		timer = frequency;
	}

	private void Update()
	{
		ManageSpawns();
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = BAT_Difficulty.BAT_difficulty.bgSquaresFrequency;
	}

	private void ManageSpawns()
	{
		if (timer >= frequency)
		{
			timer = 0f;
			SpawnSquare();
		}
	}

	private void SpawnSquare()
	{
		obj = GetSquare();
		if (!(obj == null))
		{
			pos = new Vector3(6f, 0f, obj.transform.position.z);
			pos.x = Random.Range(6f, 7f);
			pos.y = Random.Range(1f, 2f);
			if (odd)
			{
				pos.y *= -1f;
			}
			odd = !odd;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	public void SpawnInitialSquares()
	{
		obj = GetSquare();
		if (!(obj == null))
		{
			pos = new Vector3(0f, 3f, obj.transform.position.z);
			pos.y = Random.Range(1f, 2f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.y *= -1f;
				odd = false;
			}
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private void PoolSquares()
	{
		squares = new List<GameObject>();
		for (int i = 0; i < squarePooledAmount; i++)
		{
			obj = Object.Instantiate(square);
			obj.SetActive(false);
			obj.transform.parent = tr;
			squares.Add(obj);
		}
	}

	public GameObject GetSquare()
	{
		for (int i = 0; i < squarePooledAmount; i++)
		{
			if (!squares[i].activeInHierarchy)
			{
				return squares[i];
			}
		}
		return null;
	}
}
