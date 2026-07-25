using System.Collections.Generic;
using UnityEngine;

public class BAL_Difficulty : MonoBehaviour
{
	public static BAL_Difficulty BAL_difficulty;

	public GameObject bgParticles;

	public bool loaded;

	public float scrollSpeed;

	public float cactus_frequency;

	public bool branched;

	public float marbleSpeed;

	public float marbleFrequency;

	public float bgParticlesRate;

	public int level;

	private GlobalControl globalControl;

	private BAL_CactusSpawner BAL_cactusSpawner;

	private ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private List<GameObject> items;

	private int numParticles;

	private ParticleSystem.Particle[] particles;

	private void Awake()
	{
		BAL_difficulty = this;
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
			}
		}
		if (BAL_cactusSpawner == null)
		{
			BAL_cactusSpawner = BAL_CactusSpawner.BAL_cactusSpawner;
			if (BAL_cactusSpawner != null)
			{
				UpdateDifficulty();
			}
		}
		if (level != GlobalControl.globalControl.GetLevel())
		{
			UpdateDifficulty();
		}
	}

	public void UpdateDifficulty()
	{
		LoadLevel();
		BAL_CactusSpawner.BAL_cactusSpawner.SetDifficultyParas();
		BAL_MarbleSpawner.BAL_marbleSpawner.SetDifficultyParas();
		ps = bgParticles.GetComponent<ParticleSystem>();
		main = ps.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scrollSpeed * 0.5f, scrollSpeed);
		emission = ps.emission;
		emission.rateOverTime = bgParticlesRate;
		UpdateParticlesSpeed();
		items = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marbles.Count; i++)
		{
			if (items[i].activeInHierarchy)
			{
				items[i].transform.GetComponent<MarbleController>().speed = marbleSpeed;
			}
		}
	}

	private void UpdateParticlesSpeed()
	{
		ps = bgParticles.GetComponent<ParticleSystem>();
		numParticles = ps.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps.GetParticles(particles);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = new Vector3(0f, Random.Range(scrollSpeed * 0.5f, scrollSpeed), 0f);
		}
		ps.SetParticles(particles, numParticles);
	}

	public void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				scrollSpeed = 3f;
				cactus_frequency = 2f;
				branched = false;
				marbleSpeed = 5f;
				marbleFrequency = 1.8f;
				bgParticlesRate = 1.5f;
				break;
			case 1:
				scrollSpeed = 4f;
				cactus_frequency = 1.5f;
				branched = true;
				marbleSpeed = 6f;
				marbleFrequency = 1.2f;
				bgParticlesRate = 2f;
				break;
			case 2:
				scrollSpeed = 5.5f;
				cactus_frequency = 1f;
				branched = true;
				marbleSpeed = 7.5f;
				marbleFrequency = 0.8f;
				bgParticlesRate = 2.5f;
				break;
			case 3:
				scrollSpeed = 6.5f;
				cactus_frequency = 0.8f;
				branched = true;
				marbleSpeed = 8.5f;
				marbleFrequency = 0.6f;
				bgParticlesRate = 3f;
				break;
			default:
				Debug.LogError("BAL difficulty defaulted.");
				scrollSpeed = 5.5f;
				cactus_frequency = 1f;
				branched = true;
				marbleSpeed = 7.5f;
				marbleFrequency = 0.8f;
				bgParticlesRate = 2.5f;
				break;
			}
			loaded = true;
		}
	}
}
