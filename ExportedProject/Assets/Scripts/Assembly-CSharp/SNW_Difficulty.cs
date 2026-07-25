using System.Collections.Generic;
using UnityEngine;

public class SNW_Difficulty : MonoBehaviour
{
	public static SNW_Difficulty SNW_difficulty;

	public bool loaded;

	public float scroll_speed;

	public float frequency;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	public SNW_MarbleSpawner marbleSpawner;

	private List<GameObject> marbles;

	private void Awake()
	{
		SNW_difficulty = this;
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
		marbleSpawner.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll_speed, scroll_speed + scroll_speed / 3f);
		bgParticles1.SetActive(true);
		main = ps2.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll_speed, scroll_speed + scroll_speed / 3f);
		bgParticles2.SetActive(true);
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marblePooledAmount; i++)
		{
			if (marbles[i].activeInHierarchy)
			{
				marbles[i].GetComponent<MarbleController>().speed = scroll_speed;
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
				scroll_speed = 6f;
				frequency = 1f;
				break;
			case 1:
				scroll_speed = 7f;
				frequency = 0.5f;
				break;
			case 2:
				scroll_speed = 8f;
				frequency = 0.3f;
				break;
			case 3:
				scroll_speed = 9f;
				frequency = 0.25f;
				break;
			default:
				Debug.LogError("SNW difficulty level defaulted.");
				scroll_speed = 9f;
				frequency = 0.3f;
				break;
			}
			loaded = true;
		}
	}
}
