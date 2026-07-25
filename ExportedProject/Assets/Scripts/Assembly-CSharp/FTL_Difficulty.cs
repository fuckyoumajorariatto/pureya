using System.Collections.Generic;
using UnityEngine;

public class FTL_Difficulty : MonoBehaviour
{
	public static FTL_Difficulty FTL_difficulty;

	public bool loaded;

	public float speed;

	public float frequency;

	public int level;

	public GameObject bgParticles;

	private GlobalControl globalControl;

	public FTL_WindowSpawner windowSpawner;

	public FTL_MarbleSpawner marbleSpawner;

	public ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private List<GameObject> marbles;

	private void Awake()
	{
		FTL_difficulty = this;
	}

	private void Start()
	{
		LoadLevel();
	}

	private void Update()
	{
		if (globalControl == null)
		{
			globalControl = GlobalControl.globalControl;
			if (globalControl != null)
			{
				LoadLevel();
				UpdateDifficulty();
			}
		}
		if (level != GlobalControl.globalControl.GetLevel())
		{
			UpdateDifficulty();
		}
	}

	private void UpdateDifficulty()
	{
		LoadLevel();
		windowSpawner.SetDifficultyParas();
		marbleSpawner.SetDifficultyParas();
		main = ps.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.75f, speed * 1.25f);
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marblePooledAmount; i++)
		{
			if (marbles[i].activeInHierarchy)
			{
				marbles[i].GetComponent<MarbleController>().speed = speed;
			}
		}
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				speed = 3f;
				frequency = 2f;
				break;
			case 1:
				speed = 4f;
				frequency = 1f;
				break;
			case 2:
				speed = 5f;
				frequency = 0.55f;
				break;
			case 3:
				speed = 6f;
				frequency = 0.43f;
				break;
			default:
				Debug.LogError("FTL difficulty level defaulted.");
				speed = 5f;
				frequency = 0.5f;
				break;
			}
			loaded = true;
		}
	}
}
