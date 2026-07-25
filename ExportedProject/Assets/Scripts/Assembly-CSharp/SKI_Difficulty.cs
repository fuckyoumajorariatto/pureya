using UnityEngine;

public class SKI_Difficulty : MonoBehaviour
{
	public static SKI_Difficulty SKI_difficulty;

	public bool loaded;

	public float speed;

	public float boatSpeedModifier;

	public float frequency;

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
		SKI_difficulty = this;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed);
		main.startLifetime = new ParticleSystem.MinMaxCurve(2f / speed, 20f / speed);
		bgParticles1.SetActive(true);
		UpdateParticlesSpeed();
	}

	private void UpdateParticlesSpeed()
	{
		numParticles = bgParticles1.GetComponent<ParticleSystem>().particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps1.GetParticles(particles);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = particles[i].velocity.normalized * speed;
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
				speed = 1f;
				frequency = 1f;
				boatSpeedModifier = 0.75f;
				break;
			case 1:
				speed = 2f;
				frequency = 0.7f;
				boatSpeedModifier = 0.9f;
				break;
			case 2:
				speed = 3f;
				frequency = 0.5f;
				boatSpeedModifier = 1f;
				break;
			case 3:
				speed = 3.5f;
				frequency = 0.4f;
				boatSpeedModifier = 1f;
				break;
			default:
				Debug.LogError("SKI difficulty level defaulted.");
				speed = 3f;
				frequency = 0.35f;
				boatSpeedModifier = 1f;
				break;
			}
			loaded = true;
		}
	}
}
