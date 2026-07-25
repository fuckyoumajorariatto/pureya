using UnityEngine;

public class CBL_Difficulty : MonoBehaviour
{
	public static CBL_Difficulty CBL_difficulty;

	public GameObject bgParticles;

	public GameObject bgParticles3;

	public ParticleSystem bgParticlesPS;

	public ParticleSystem bgParticles3PS;

	public bool loaded;

	public float scrollSpeed;

	public float objectFrequency;

	public float marbleSpeed;

	public int maxPennants;

	public float bgParticles2Rate;

	public int level;

	private GlobalControl globalControl;

	private CBL_FieldObjectsSpawner CBL_fieldObjectsSpawner;

	private ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private int numParticles;

	private ParticleSystem.Particle[] particles;

	private void Awake()
	{
		CBL_difficulty = this;
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
		if (CBL_fieldObjectsSpawner == null)
		{
			CBL_fieldObjectsSpawner = CBL_FieldObjectsSpawner.CBL_fieldObjectsSpawner;
			if (CBL_fieldObjectsSpawner != null)
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
		CBL_FieldObjectsSpawner.CBL_fieldObjectsSpawner.SetDifficultyParas();
		CBL_GoalController.CBL_goalController.SetDifficultyParas();
		CBL_PlayerController.CBL_playerController.SetDifficultyParas();
		ps = bgParticlesPS;
		main = ps.main;
		main.startSpeed = scrollSpeed;
		ps = bgParticles3PS;
		main = ps.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scrollSpeed, scrollSpeed + scrollSpeed / 2f);
		UpdateParticlesSpeed();
	}

	private void UpdateParticlesSpeed()
	{
		numParticles = bgParticles.GetComponent<ParticleSystem>().particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		bgParticlesPS.GetParticles(particles);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = particles[i].velocity.normalized * scrollSpeed;
		}
		bgParticlesPS.SetParticles(particles, numParticles);
		bgParticles.SetActive(true);
		bgParticles3.SetActive(true);
	}

	public void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				scrollSpeed = 4f;
				bgParticles2Rate = 0.7f;
				marbleSpeed = 9f;
				maxPennants = 1;
				objectFrequency = 1f;
				break;
			case 1:
				scrollSpeed = 5f;
				bgParticles2Rate = 0.9f;
				marbleSpeed = 10f;
				maxPennants = 2;
				objectFrequency = 0.9f;
				break;
			case 2:
				scrollSpeed = 7f;
				bgParticles2Rate = 1.1f;
				marbleSpeed = 11f;
				maxPennants = 3;
				objectFrequency = 0.7f;
				break;
			case 3:
				scrollSpeed = 8f;
				bgParticles2Rate = 1.3f;
				marbleSpeed = 12f;
				maxPennants = 3;
				objectFrequency = 0.6f;
				break;
			default:
				Debug.LogError("CBL difficulty defaulted.");
				scrollSpeed = 7f;
				bgParticles2Rate = 1.1f;
				marbleSpeed = 11f;
				maxPennants = 3;
				objectFrequency = 0.65f;
				break;
			}
			loaded = true;
		}
	}
}
