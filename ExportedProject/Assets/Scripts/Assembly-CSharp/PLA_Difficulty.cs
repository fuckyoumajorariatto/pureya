using UnityEngine;

public class PLA_Difficulty : MonoBehaviour
{
	public static PLA_Difficulty PLA_difficulty;

	public PLA_Spawner spawner;

	public bool loaded;

	public float bgSpeed;

	public float missileSpeed;

	public float missileFrequency;

	public float missile2Chance;

	public float marbleFrequency;

	public float laserFrequency;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		PLA_difficulty = this;
	}

	private void Start()
	{
		LoadLevel();
		spawner.enabled = true;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(bgSpeed * 0.5f, bgSpeed);
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				bgSpeed = 2f;
				missileSpeed = 5f;
				missileFrequency = 3f;
				missile2Chance = 0f;
				marbleFrequency = 1.8f;
				laserFrequency = 5f;
				break;
			case 1:
				bgSpeed = 2.5f;
				missileSpeed = 7f;
				missileFrequency = 1f;
				missile2Chance = 0.1f;
				marbleFrequency = 1.3f;
				laserFrequency = 4f;
				break;
			case 2:
				bgSpeed = 3f;
				missileSpeed = 8f;
				missileFrequency = 0.6f;
				missile2Chance = 0.2f;
				marbleFrequency = 1f;
				laserFrequency = 3f;
				break;
			case 3:
				bgSpeed = 4f;
				missileSpeed = 9.5f;
				missileFrequency = 0.5f;
				missile2Chance = 0.4f;
				marbleFrequency = 0.5f;
				laserFrequency = 2f;
				break;
			default:
				Debug.LogError("PLA difficulty level defaulted.");
				bgSpeed = 3f;
				missileSpeed = 8f;
				missileFrequency = 0.6f;
				missile2Chance = 0.2f;
				marbleFrequency = 1.2f;
				laserFrequency = 3f;
				break;
			}
			loaded = true;
		}
	}
}
