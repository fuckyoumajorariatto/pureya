using UnityEngine;

public class BWL_Difficulty : MonoBehaviour
{
	public static BWL_Difficulty BWL_difficulty;

	public bool loaded;

	public float scroll;

	public float frequency;

	public float marbleChance;

	public int max;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public GameObject bgParticles1;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private int numParticles;

	private ParticleSystem.Particle[] particles;

	private void Awake()
	{
		BWL_difficulty = this;
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
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll);
		bgParticles1.SetActive(true);
		UpdateParticlesSpeed();
		gameObjects = GameObject.FindGameObjectsWithTag("marble");
		for (int i = 0; i < gameObjects.Length; i++)
		{
			gameObjects[i].GetComponent<MarbleController>().speed = scroll;
		}
	}

	private void UpdateParticlesSpeed()
	{
		numParticles = bgParticles1.GetComponent<ParticleSystem>().particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps1.GetParticles(particles);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = particles[i].velocity.normalized * scroll;
		}
		ps1.SetParticles(particles, numParticles);
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				scroll = 5f;
				frequency = 1.25f;
				marbleChance = 0.4f;
				max = 1;
				break;
			case 1:
				scroll = 6f;
				frequency = 0.9f;
				marbleChance = 0.4f;
				max = 2;
				break;
			case 2:
				scroll = 7f;
				frequency = 0.62f;
				marbleChance = 0.4f;
				max = 2;
				break;
			case 3:
				scroll = 8f;
				frequency = 0.52f;
				marbleChance = 0.4f;
				max = 2;
				break;
			default:
				Debug.LogError("BWL difficulty level defaulted.");
				scroll = 8f;
				frequency = 0.45f;
				marbleChance = 0.4f;
				max = 2;
				break;
			}
			loaded = true;
		}
	}
}
