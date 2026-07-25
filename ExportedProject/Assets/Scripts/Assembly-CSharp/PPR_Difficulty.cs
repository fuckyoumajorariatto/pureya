using UnityEngine;

public class PPR_Difficulty : MonoBehaviour
{
	public static PPR_Difficulty PPR_difficulty;

	public bool loaded;

	public float balloon_speed;

	public float balloon_frequency;

	public int balloon_max;

	public float marble_frequency;

	public float marble_speed;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public PPR_BalloonSpawner balloonSpawner;

	public PPR_MarbleSpawner marbleSpawner;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		PPR_difficulty = this;
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
		balloonSpawner.SetDifficultyParas();
		marbleSpawner.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(balloon_speed, balloon_speed + balloon_speed / 3f);
		bgParticles1.SetActive(true);
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(balloon_speed - balloon_speed / 2f, balloon_speed);
		bgParticles2.SetActive(true);
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				balloon_speed = 3f;
				balloon_frequency = 2f;
				balloon_max = 1;
				marble_frequency = 2.5f;
				marble_speed = 2f;
				break;
			case 1:
				balloon_speed = 4f;
				balloon_frequency = 1.5f;
				balloon_max = 2;
				marble_frequency = 1.75f;
				marble_speed = 3f;
				break;
			case 2:
				balloon_speed = 6f;
				balloon_frequency = 1f;
				balloon_max = 2;
				marble_frequency = 0.9f;
				marble_speed = 5f;
				break;
			case 3:
				balloon_speed = 6.5f;
				balloon_frequency = 0.9f;
				balloon_max = 2;
				marble_frequency = 0.8f;
				marble_speed = 5.5f;
				break;
			default:
				Debug.LogError("PPR difficulty level defaulted.");
				balloon_speed = 6f;
				balloon_frequency = 1f;
				balloon_max = 2;
				marble_frequency = 0.9f;
				marble_speed = 5f;
				break;
			}
			loaded = true;
		}
	}
}
