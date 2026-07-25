using System.Collections.Generic;
using UnityEngine;

public class ZIP_Difficulty : MonoBehaviour
{
	public static ZIP_Difficulty ZIP_difficulty;

	public bool loaded;

	public float scroll_speed;

	public float tree_frequency;

	public int tree_max;

	public float marble_frequency;

	public int marbles_spawn_type;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public ZIP_TreeSpawner treeSpawner;

	public ZIP_MarbleSpawner marbleSpawner;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private List<GameObject> marbles;

	private void Awake()
	{
		ZIP_difficulty = this;
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
		treeSpawner.SetDifficultyParas();
		marbleSpawner.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll_speed, scroll_speed + scroll_speed / 2f);
		bgParticles1.SetActive(true);
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marblePooledAmount; i++)
		{
			if (marbles[i].activeInHierarchy)
			{
				marbles[i].GetComponent<MarbleController>().speed = scroll_speed * Random.Range(1.1f, 1.3f);
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
				scroll_speed = 2f;
				tree_frequency = 1.5f;
				tree_max = 1;
				marble_frequency = 2f;
				marbles_spawn_type = 1;
				break;
			case 1:
				scroll_speed = 2.5f;
				tree_frequency = 1.5f;
				tree_max = 2;
				marble_frequency = 1.5f;
				marbles_spawn_type = 1;
				break;
			case 2:
				scroll_speed = 3f;
				tree_frequency = 1.25f;
				tree_max = 3;
				marble_frequency = 0.9f;
				marbles_spawn_type = 2;
				break;
			case 3:
				scroll_speed = 3.5f;
				tree_frequency = 0.9f;
				tree_max = 3;
				marble_frequency = 0.7f;
				marbles_spawn_type = 2;
				break;
			default:
				Debug.LogError("ZIP difficulty level defaulted.");
				scroll_speed = 3f;
				tree_frequency = 1.25f;
				tree_max = 3;
				marble_frequency = 0.9f;
				marbles_spawn_type = 2;
				break;
			}
			loaded = true;
		}
	}
}
