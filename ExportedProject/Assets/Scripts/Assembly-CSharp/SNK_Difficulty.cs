using UnityEngine;

public class SNK_Difficulty : MonoBehaviour
{
	public static SNK_Difficulty SNK_difficulty;

	public bool loaded;

	public float scroll;

	public float frequency;

	public float playerSpeed;

	public int comboNothingMax;

	public float[] spawnChances = new float[3];

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public GameObject bgParticles1;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		SNK_difficulty = this;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll + scroll / 4f, scroll * 1.5f);
		bgParticles1.SetActive(true);
		gameObjects = GameObject.FindGameObjectsWithTag("marble");
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<MarbleController>().speed = scroll;
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
				scroll = 1.75f;
				frequency = 1.75f;
				playerSpeed = 2.75f;
				comboNothingMax = 2;
				spawnChances = new float[3] { 0.4f, 0f, 0.1f };
				break;
			case 1:
				scroll = 2.1f;
				frequency = 1.4f;
				playerSpeed = 3.5f;
				comboNothingMax = 1;
				spawnChances = new float[3] { 0.8f, 0.2f, 0.1f };
				break;
			case 2:
				scroll = 2.5f;
				frequency = 1.1f;
				playerSpeed = 4f;
				comboNothingMax = 0;
				spawnChances = new float[3] { 1f, 0.7f, 0.2f };
				break;
			case 3:
				scroll = 3f;
				frequency = 0.9f;
				playerSpeed = 4f;
				comboNothingMax = 0;
				spawnChances = new float[3] { 1f, 0.7f, 0.2f };
				break;
			default:
				Debug.LogError("SNK difficulty level defaulted.");
				scroll = 2.5f;
				frequency = 1.15f;
				playerSpeed = 4f;
				spawnChances = new float[3] { 0.4f, 0.8f, 1f };
				break;
			}
			loaded = true;
		}
	}
}
