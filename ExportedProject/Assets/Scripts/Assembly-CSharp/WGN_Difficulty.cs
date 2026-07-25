using UnityEngine;

public class WGN_Difficulty : MonoBehaviour
{
	public static WGN_Difficulty WGN_difficulty;

	public bool loaded;

	public float speed;

	public float rockSpeed;

	public float rockFrequency;

	public float marbleFrequency;

	public int max;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public GameObject bgParticles1;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		WGN_difficulty = this;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed, speed * 1.25f);
		bgParticles1.SetActive(true);
		gameObjects = GameObject.FindGameObjectsWithTag("marble");
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<MarbleController>().speed = Random.Range(rockSpeed, rockSpeed * 1.25f);
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
				speed = 4f;
				rockSpeed = 4f;
				rockFrequency = 1.5f;
				marbleFrequency = 2.4f;
				max = 1;
				break;
			case 1:
				speed = 5f;
				rockSpeed = 6f;
				rockFrequency = 1f;
				marbleFrequency = 1.7f;
				max = 1;
				break;
			case 2:
				speed = 6f;
				rockSpeed = 7f;
				rockFrequency = 0.6f;
				marbleFrequency = 0.9f;
				max = 2;
				break;
			case 3:
				speed = 6.5f;
				rockSpeed = 7.5f;
				rockFrequency = 0.5f;
				marbleFrequency = 0.7f;
				max = 2;
				break;
			default:
				Debug.LogError("WGN difficulty level defaulted.");
				speed = 6f;
				rockSpeed = 7f;
				rockFrequency = 0.6f;
				marbleFrequency = 0.9f;
				max = 2;
				break;
			}
			loaded = true;
		}
	}
}
