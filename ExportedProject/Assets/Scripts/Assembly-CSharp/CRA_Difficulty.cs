using UnityEngine;

public class CRA_Difficulty : MonoBehaviour
{
	public static CRA_Difficulty CRA_difficulty;

	public bool loaded;

	public float speed;

	public float frequency;

	public float coralFrequency;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public GameObject bgParticles1;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		CRA_difficulty = this;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed / 8f, speed / 2f);
		bgParticles1.SetActive(true);
		gameObjects = GameObject.FindGameObjectsWithTag("marble");
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<MarbleController>().speed = speed * 2f;
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
				speed = 2f;
				frequency = 1f;
				coralFrequency = 6f;
				break;
			case 1:
				speed = 3f;
				frequency = 0.6f;
				coralFrequency = 4f;
				break;
			case 2:
				speed = 4f;
				frequency = 0.3f;
				coralFrequency = 3f;
				break;
			case 3:
				speed = 5f;
				frequency = 0.2f;
				coralFrequency = 3f;
				break;
			default:
				Debug.LogError("CRA difficulty level defaulted.");
				speed = 4f;
				frequency = 0.3f;
				coralFrequency = 3f;
				break;
			}
			loaded = true;
		}
	}
}
