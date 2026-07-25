using UnityEngine;

public class SEA_Difficulty : MonoBehaviour
{
	public static SEA_Difficulty SEA_difficulty;

	public bool loaded;

	public float speed;

	public float bubbleFrequency;

	public float marbleFrequency;

	public int level;

	public float gravity;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public GameObject fishContainer;

	public SEA_BubbleSpawner bubbleSpawner;

	public SEA_MarbleSpawner marbleSpawner;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	public ParticleSystem.MainModule main;

	private void Awake()
	{
		SEA_difficulty = this;
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
		bubbleSpawner.SetDifficultyParas();
		marbleSpawner.SetDifficultyParas();
		SEA_FishController.SEA_fishController.SetDifficultyParas();
		SEA_PlayerController.SEA_playerController.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed - speed / 2f, speed - speed / 4f);
		bgParticles1.SetActive(true);
		main = ps2.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed / 3f, speed - speed / 2f);
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
				speed = 1.5f;
				bubbleFrequency = 1f;
				marbleFrequency = 1.75f;
				gravity = 0.75f;
				break;
			case 1:
				speed = 2.5f;
				bubbleFrequency = 0.75f;
				marbleFrequency = 1f;
				gravity = 0.75f;
				break;
			case 2:
				speed = 3.5f;
				bubbleFrequency = 0.75f;
				marbleFrequency = 0.75f;
				gravity = 0.8f;
				break;
			case 3:
				speed = 4.5f;
				bubbleFrequency = 0.575f;
				marbleFrequency = 0.6f;
				gravity = 0.85f;
				break;
			default:
				Debug.LogError("SEA difficulty level defaulted.");
				speed = 3.5f;
				bubbleFrequency = 0.75f;
				marbleFrequency = 0.75f;
				break;
			}
			loaded = true;
		}
	}
}
