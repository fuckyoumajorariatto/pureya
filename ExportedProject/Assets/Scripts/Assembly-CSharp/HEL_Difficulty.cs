using UnityEngine;

public class HEL_Difficulty : MonoBehaviour
{
	public static HEL_Difficulty HEL_difficulty;

	public bool loaded;

	public float speed;

	public float frequency;

	public int ballsMin;

	public int ballsMax;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		HEL_difficulty = this;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.75f, speed * 1.5f);
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
				speed = 2f;
				frequency = 2f;
				ballsMin = 1;
				ballsMax = 1;
				break;
			case 1:
				speed = 2.5f;
				frequency = 1.3f;
				ballsMin = 1;
				ballsMax = 2;
				break;
			case 2:
				speed = 3f;
				frequency = 1.1f;
				ballsMin = 1;
				ballsMax = 3;
				break;
			case 3:
				speed = 3.5f;
				frequency = 0.9f;
				ballsMin = 2;
				ballsMax = 3;
				break;
			default:
				Debug.LogError("HEL difficulty level defaulted.");
				speed = 2f;
				frequency = 1f;
				ballsMin = 1;
				ballsMax = 3;
				break;
			}
			loaded = true;
		}
	}
}
