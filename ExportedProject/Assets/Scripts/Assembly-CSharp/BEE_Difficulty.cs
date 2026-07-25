using UnityEngine;

public class BEE_Difficulty : MonoBehaviour
{
	public static BEE_Difficulty BEE_difficulty;

	public bool loaded;

	public float speed;

	public float plantChance;

	public float marbleChance;

	public int flowerComboMax;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public GameObject bgParticles1;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	public BEE_PlayerController playerController;

	private void Awake()
	{
		BEE_difficulty = this;
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
		playerController.UpdateDifficulty();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed + speed / 4f, speed * 2f);
		bgParticles1.SetActive(true);
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
				speed = 1f;
				plantChance = 0.05f;
				marbleChance = 0.05f;
				flowerComboMax = 30;
				break;
			case 1:
				speed = 1.5f;
				plantChance = 0.1f;
				marbleChance = 0.075f;
				flowerComboMax = 14;
				break;
			case 2:
				speed = 2f;
				plantChance = 0.15f;
				marbleChance = 0.1f;
				flowerComboMax = 7;
				break;
			case 3:
				speed = 2.5f;
				plantChance = 0.2f;
				marbleChance = 0.125f;
				flowerComboMax = 7;
				break;
			default:
				Debug.LogError("BEE difficulty level defaulted.");
				speed = 2f;
				plantChance = 0.15f;
				marbleChance = 0.1f;
				flowerComboMax = 7;
				break;
			}
			loaded = true;
		}
	}
}
