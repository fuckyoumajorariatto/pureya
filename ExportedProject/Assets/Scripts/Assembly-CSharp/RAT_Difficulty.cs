using UnityEngine;

public class RAT_Difficulty : MonoBehaviour
{
	public static RAT_Difficulty RAT_difficulty;

	public bool loaded;

	public float speed;

	public float frequency;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	public ParticleSystem ps3;

	private ParticleSystem.MainModule main;

	private ParticleSystem.Particle[] particles;

	private int numParticles;

	private void Awake()
	{
		RAT_difficulty = this;
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
		main.startSpeed = speed;
		numParticles = ps1.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps1.GetParticles(particles);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = new Vector3(0f, speed, 0f);
		}
		ps1.SetParticles(particles, numParticles);
		main = ps2.main;
		main.startSpeed = speed * 1.25f;
		numParticles = ps2.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps2.GetParticles(particles);
		for (int j = 0; j < particles.Length; j++)
		{
			particles[j].velocity = new Vector3(0f, speed * 1.25f, 0f);
		}
		ps2.SetParticles(particles, numParticles);
		main = ps3.main;
		main.startSpeed = speed * 1.25f;
		numParticles = ps3.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps3.GetParticles(particles);
		for (int k = 0; k < particles.Length; k++)
		{
			particles[k].velocity = new Vector3(0f, speed * 1.25f, 0f);
		}
		ps3.SetParticles(particles, numParticles);
		ps1.Play();
		ps2.Play();
		ps3.Play();
		gameObjects = GameObject.FindGameObjectsWithTag("marble");
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<MarbleController>().speed = speed;
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
				speed = 2.5f;
				frequency = 1.25f;
				break;
			case 1:
				speed = 3.5f;
				frequency = 0.9f;
				break;
			case 2:
				speed = 5f;
				frequency = 0.7f;
				break;
			case 3:
				speed = 6f;
				frequency = 0.6f;
				break;
			default:
				Debug.LogError("RAT difficulty level defaulted.");
				speed = 5f;
				frequency = 0.7f;
				break;
			}
			loaded = true;
		}
	}
}
