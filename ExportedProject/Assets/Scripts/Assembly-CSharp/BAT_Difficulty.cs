using UnityEngine;

public class BAT_Difficulty : MonoBehaviour
{
	public static BAT_Difficulty BAT_difficulty;

	public bool loaded;

	public float frequency;

	public float minHeight;

	public float maxHeight;

	public float speed;

	public float bgParticlesSpeed1;

	public float bgParticlesSpeed2;

	public float bgSquaresSpeed;

	public float bgSquaresFrequency;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public GameObject bgParticles3;

	private BAT_StalactitesSpawner BAT_stalactitesSpawner;

	private ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		BAT_difficulty = this;
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
		if (BAT_stalactitesSpawner == null)
		{
			BAT_stalactitesSpawner = BAT_StalactitesSpawner.BAT_stalactitesSpawner;
			if (BAT_stalactitesSpawner != null)
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
		BAT_StalactitesSpawner.BAT_stalactitesSpawner.SetDifficultyParas();
		BAT_BgDarknessSquareSpawner.BAT_bgDarknessSquareSpawner.SetDifficultyParas();
		ps = bgParticles1.GetComponent<ParticleSystem>();
		main = ps.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(bgParticlesSpeed1 * 0.75f, bgParticlesSpeed1);
		ps = bgParticles2.GetComponent<ParticleSystem>();
		main = ps.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(bgParticlesSpeed2 * 0.75f, bgParticlesSpeed2);
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				frequency = 1.25f;
				minHeight = 3.5f;
				maxHeight = 4.5f;
				speed = 3f;
				bgParticlesSpeed1 = 2f;
				bgParticlesSpeed2 = 1.5f;
				bgSquaresSpeed = 1f;
				bgSquaresFrequency = 5f;
				break;
			case 1:
				frequency = 1f;
				minHeight = 2f;
				maxHeight = 3.5f;
				speed = 4f;
				bgParticlesSpeed1 = 3f;
				bgParticlesSpeed2 = 2.25f;
				bgSquaresSpeed = 1.5f;
				bgSquaresFrequency = 4f;
				break;
			case 2:
				frequency = 0.75f;
				minHeight = 1.5f;
				maxHeight = 3f;
				speed = 5f;
				bgParticlesSpeed1 = 4f;
				bgParticlesSpeed2 = 3f;
				bgSquaresSpeed = 2f;
				bgSquaresFrequency = 3f;
				break;
			case 3:
				frequency = 0.7f;
				minHeight = 1.5f;
				maxHeight = 2.5f;
				speed = 5.5f;
				bgParticlesSpeed1 = 4.5f;
				bgParticlesSpeed2 = 3.5f;
				bgSquaresSpeed = 2.5f;
				bgSquaresFrequency = 2.5f;
				break;
			default:
				Debug.LogError("BAT difficulty level defaulted.");
				frequency = 0.75f;
				minHeight = 1.5f;
				maxHeight = 3f;
				speed = 5f;
				bgParticlesSpeed1 = 4f;
				bgParticlesSpeed2 = 3f;
				bgSquaresSpeed = 2f;
				bgSquaresFrequency = 3f;
				break;
			}
			loaded = true;
		}
	}
}
