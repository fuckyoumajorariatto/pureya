using UnityEngine;

public class CNN_Difficulty : MonoBehaviour
{
	public static CNN_Difficulty CNN_difficulty;

	public bool loaded;

	public int cannonballMax;

	public float speedUp;

	public float speedDown;

	public float wave_frequency;

	public float bg_elements_speed;

	public float mount_frequency;

	public int level;

	public CNN_BallSpawner ballSpawner;

	public CNN_GroundMountSpawner groundMountSpawner;

	public CNN_WaveSpawner waveSpawner;

	private void Awake()
	{
		CNN_difficulty = this;
	}

	private void Start()
	{
		LoadLevel();
	}

	private void Update()
	{
		if (level != GlobalControl.globalControl.GetLevel())
		{
			UpdateDifficulty();
		}
	}

	private void UpdateDifficulty()
	{
		LoadLevel();
		ballSpawner.SetDifficultyParas();
		groundMountSpawner.SetDifficultyParas();
		waveSpawner.SetDifficultyParas();
	}

	private void LoadLevel()
	{
		level = GlobalControl.globalControl.GetLevel();
		switch (level)
		{
		case 0:
			cannonballMax = 1;
			speedUp = 4f;
			speedDown = 6f;
			wave_frequency = 0.05f;
			bg_elements_speed = 1f;
			mount_frequency = 1.5f;
			break;
		case 1:
			cannonballMax = 2;
			speedUp = 5f;
			speedDown = 8f;
			wave_frequency = 0.1f;
			bg_elements_speed = 2f;
			mount_frequency = 3f;
			break;
		case 2:
			cannonballMax = 2;
			speedUp = 8f;
			speedDown = 11f;
			wave_frequency = 0.1f;
			bg_elements_speed = 2f;
			mount_frequency = 3f;
			break;
		case 3:
			cannonballMax = 2;
			speedUp = 9f;
			speedDown = 14f;
			wave_frequency = 0.1f;
			bg_elements_speed = 2.5f;
			mount_frequency = 3.75f;
			break;
		default:
			Debug.LogError("CNN difficulty level defaulted.");
			cannonballMax = 2;
			speedUp = 8f;
			speedDown = 11f;
			wave_frequency = 0.1f;
			bg_elements_speed = 2f;
			mount_frequency = 3f;
			break;
		}
		loaded = true;
	}
}
