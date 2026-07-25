using UnityEngine;

public class SPR_Difficulty : MonoBehaviour
{
	public static SPR_Difficulty SPR_difficulty;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public bool loaded;

	public float scrollSpeed;

	public float nailsFrequency;

	public bool bentNails;

	public bool doubleNails;

	public float marbleSpeed;

	public float marbleFrequency;

	public float bgParticles2Rate;

	public int level;

	private GlobalControl globalControl;

	private SPR_NailSpawner SPR_nailSpawner;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private int numParticles;

	private ParticleSystem.Particle[] particles;

	private Vector3 v;

	private void Awake()
	{
		SPR_difficulty = this;
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
		if (SPR_nailSpawner == null)
		{
			SPR_nailSpawner = SPR_NailSpawner.SPR_nailSpawner;
			if (SPR_nailSpawner != null)
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
		SPR_NailSpawner.SPR_nailSpawner.SetDifficultyParas();
		SPR_MarbleSpawner.SPR_marbleSpawner.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scrollSpeed * 0.6f, scrollSpeed);
		main = ps2.main;
		main.startSpeed = scrollSpeed / 2.5f;
		emission = ps2.emission;
		emission.rateOverTime = bgParticles2Rate;
		UpdateParticlesSpeed();
	}

	private void UpdateParticlesSpeed()
	{
		numParticles = ps2.particleCount;
		particles = new ParticleSystem.Particle[numParticles];
		ps2.GetParticles(particles);
		v = new Vector3(0f, scrollSpeed / 2.5f, 0f);
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].velocity = v;
		}
		ps2.SetParticles(particles, numParticles);
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
				nailsFrequency = 2.5f;
				bentNails = false;
				doubleNails = false;
				marbleSpeed = 4f;
				marbleFrequency = 1.6f;
				bgParticles2Rate = 0.75f;
				break;
			case 1:
				scrollSpeed = 4f;
				nailsFrequency = 2f;
				bentNails = true;
				doubleNails = false;
				marbleSpeed = 5f;
				marbleFrequency = 1.1f;
				bgParticles2Rate = 1f;
				break;
			case 2:
				scrollSpeed = 6f;
				nailsFrequency = 1.25f;
				bentNails = true;
				doubleNails = true;
				marbleSpeed = 7.5f;
				marbleFrequency = 0.8f;
				bgParticles2Rate = 1.1f;
				break;
			case 3:
				scrollSpeed = 7f;
				nailsFrequency = 1f;
				bentNails = true;
				doubleNails = true;
				marbleSpeed = 8.5f;
				marbleFrequency = 0.7f;
				bgParticles2Rate = 1.4f;
				break;
			default:
				Debug.LogError("SPR difficulty defaulted.");
				scrollSpeed = 7f;
				nailsFrequency = 0.95f;
				bentNails = true;
				doubleNails = true;
				marbleSpeed = 8f;
				marbleFrequency = 0.6f;
				bgParticles2Rate = 1.3f;
				break;
			}
			loaded = true;
		}
	}
}
