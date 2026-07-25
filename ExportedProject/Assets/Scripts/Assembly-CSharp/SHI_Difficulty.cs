using UnityEngine;

public class SHI_Difficulty : MonoBehaviour
{
	public static SHI_Difficulty SHI_difficulty;

	public SHI_PlayerController playerController;

	public bool loaded;

	public float speed;

	public float acceleration;

	public float maxSpeed;

	public float linearDrag;

	public int marbleCooldown;

	public int islandCooldown;

	public bool marbleOddChance;

	public float chanceOfSecondIsland;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		SHI_difficulty = this;
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
		playerController.rb.drag = linearDrag;
		main = ps1.main;
		main.gravityModifier = 0.15f * speed;
		main = ps2.main;
		main.gravityModifier = 0.15f * speed;
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
				acceleration = 500f;
				maxSpeed = 2f;
				linearDrag = 1.6f;
				marbleCooldown = 1;
				islandCooldown = 1;
				chanceOfSecondIsland = 0f;
				break;
			case 1:
				speed = 1.6f;
				acceleration = 600f;
				maxSpeed = 2.6f;
				linearDrag = 1.6f;
				marbleCooldown = 1;
				islandCooldown = 1;
				chanceOfSecondIsland = 0.4f;
				break;
			case 2:
				speed = 1.8f;
				acceleration = 680f;
				maxSpeed = 3.1f;
				linearDrag = 1.8f;
				marbleCooldown = 1;
				islandCooldown = 1;
				marbleOddChance = true;
				chanceOfSecondIsland = 0.75f;
				break;
			case 3:
				speed = 2f;
				acceleration = 720f;
				maxSpeed = 3.5f;
				linearDrag = 2f;
				marbleCooldown = 1;
				islandCooldown = 1;
				chanceOfSecondIsland = 0.75f;
				marbleOddChance = true;
				break;
			default:
				Debug.LogError("SHI difficulty level defaulted.");
				speed = 2f;
				acceleration = 700f;
				linearDrag = 1.8f;
				marbleCooldown = 1;
				islandCooldown = 1;
				marbleOddChance = true;
				break;
			}
			loaded = true;
		}
	}
}
