using System.Collections.Generic;
using UnityEngine;

public class SQD_Difficulty : MonoBehaviour
{
	public static SQD_Difficulty SQD_difficulty;

	public bool loaded;

	public float urchin_frequency;

	public float urchin_speed;

	public float double_urchin_chance;

	public float marble_frequency;

	public float marble_speed;

	public int marble_max;

	public float scrollSpeed;

	public int level;

	public SQD_Controller controller;

	public SQD_UrchinSpawner urchinSpawner;

	public ParticleSystem bgParticles1;

	public ParticleSystem bgParticles2;

	private ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private List<GameObject> marbles;

	private void Awake()
	{
		SQD_difficulty = this;
	}

	private void Start()
	{
		LoadLevel();
	}

	private void Update()
	{
		if (level != GlobalControl.globalControl.GetLevel())
		{
			UpdateDifficulty();
		}
	}

	private void UpdateDifficulty()
	{
		LoadLevel();
		controller.SetDifficultyParas();
		urchinSpawner.SetDifficultyParas();
		ps = bgParticles1;
		main = ps.main;
		main.startSpeed = scrollSpeed;
		ps = bgParticles2;
		main = ps.main;
		main.startSpeed = scrollSpeed / 3f;
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < marbles.Count; i++)
		{
			marbles[i].GetComponent<MarbleController>().speed = marble_speed;
		}
	}

	private void LoadLevel()
	{
		level = GlobalControl.globalControl.GetLevel();
		switch (level)
		{
		case 0:
			scrollSpeed = 1.5f;
			urchin_frequency = 3f;
			urchin_speed = 2f;
			double_urchin_chance = 0f;
			marble_frequency = 3f;
			marble_speed = 1f;
			marble_max = 1;
			break;
		case 1:
			scrollSpeed = 2f;
			urchin_frequency = 2.1f;
			urchin_speed = 2.5f;
			double_urchin_chance = 0.4f;
			marble_frequency = 2.25f;
			marble_speed = 2f;
			marble_max = 1;
			break;
		case 2:
			scrollSpeed = 2.5f;
			urchin_frequency = 1.5f;
			urchin_speed = 3f;
			double_urchin_chance = 0.6f;
			marble_frequency = 1.5f;
			marble_speed = 2.5f;
			marble_max = 2;
			break;
		case 3:
			scrollSpeed = 3f;
			urchin_frequency = 1f;
			urchin_speed = 3.5f;
			double_urchin_chance = 0.75f;
			marble_frequency = 1.2f;
			marble_speed = 3f;
			marble_max = 2;
			break;
		default:
			Debug.LogError("SQD difficulty level defaulted.");
			scrollSpeed = 2.5f;
			urchin_frequency = 1.5f;
			urchin_speed = 3f;
			double_urchin_chance = 0.6f;
			marble_frequency = 1.5f;
			marble_speed = 2.5f;
			marble_max = 2;
			break;
		}
		loaded = true;
	}
}
