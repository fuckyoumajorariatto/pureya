using System.Collections.Generic;
using UnityEngine;

public class GOT_Difficulty : MonoBehaviour
{
	public static GOT_Difficulty GOT_difficulty;

	public GOT_PlayerController playerController;

	public bool loaded;

	public float speed;

	public float rock_frequency;

	public float marble_chance;

	public float gravity;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public GameObject bgParticles3;

	public GOT_RockSpawner rockSpawner;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	public ParticleSystem ps3;

	private ParticleSystem.MainModule main;

	private List<GameObject> marbles;

	private int numParticles;

	private ParticleSystem.Particle[] particles;

	private void Awake()
	{
		GOT_difficulty = this;
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
		rockSpawner.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed, speed + 2f);
		bgParticles1.SetActive(true);
		main = ps2.main;
		main.startSpeed = speed;
		bgParticles2.SetActive(true);
		numParticles = ps2.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps2.GetParticles(particles);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = new Vector3(0f, speed, 0f);
		}
		ps2.SetParticles(particles, numParticles);
		main = ps3.main;
		main.startSpeed = speed;
		bgParticles3.SetActive(true);
		numParticles = ps3.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps3.GetParticles(particles);
		for (int j = 0; j < particles.Length; j++)
		{
			particles[j].velocity = new Vector3(0f, speed, 0f);
		}
		ps3.SetParticles(particles, numParticles);
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int k = 0; k < marbles.Count; k++)
		{
			marbles[k].GetComponent<MarbleController>().speed = speed;
		}
		playerController.UpdateDifficulty();
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				speed = 2.5f;
				rock_frequency = 0.5f;
				marble_chance = 0.25f;
				gravity = 4f;
				break;
			case 1:
				speed = 3f;
				rock_frequency = 0.5f;
				marble_chance = 0.3f;
				gravity = 4f;
				break;
			case 2:
				speed = 4f;
				rock_frequency = 0.46f;
				marble_chance = 0.4f;
				gravity = 4.5f;
				break;
			case 3:
				speed = 5f;
				rock_frequency = 0.36f;
				marble_chance = 0.5f;
				gravity = 5f;
				break;
			default:
				Debug.LogError("GOT difficulty level defaulted.");
				speed = 4f;
				rock_frequency = 0.46f;
				marble_chance = 0.4f;
				break;
			}
			loaded = true;
		}
	}
}
