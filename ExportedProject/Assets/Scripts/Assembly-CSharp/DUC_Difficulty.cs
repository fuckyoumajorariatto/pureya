using UnityEngine;

public class DUC_Difficulty : MonoBehaviour
{
	public static DUC_Difficulty DUC_difficulty;

	public bool loaded;

	public float speed;

	public float marbleChance;

	public float frequency;

	public int maxObjs;

	public int level;

	private GlobalControl globalControl;

	private GameObject[] gameObjects;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public GameObject bgParticles3;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	public ParticleSystem ps3;

	private ParticleSystem.MainModule main;

	private ParticleSystem.ColorBySpeedModule colorBySpeed;

	private void Awake()
	{
		DUC_difficulty = this;
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
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.75f, speed);
		colorBySpeed = ps1.colorBySpeed;
		colorBySpeed.range = new Vector2(speed * 0.75f, speed);
		bgParticles1.SetActive(true);
		main = ps2.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.75f, speed);
		colorBySpeed = ps2.colorBySpeed;
		colorBySpeed.range = new Vector2(speed * 0.75f, speed);
		bgParticles2.SetActive(true);
		main = ps3.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.75f, speed);
		colorBySpeed = ps3.colorBySpeed;
		colorBySpeed.range = new Vector2(speed * 0.75f, speed);
		bgParticles3.SetActive(true);
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
				speed = 3f;
				marbleChance = 1f;
				frequency = 2f;
				maxObjs = 1;
				break;
			case 1:
				speed = 4f;
				marbleChance = 1f;
				frequency = 1f;
				maxObjs = 2;
				break;
			case 2:
				speed = 5f;
				marbleChance = 1f;
				frequency = 0.8f;
				maxObjs = 2;
				break;
			case 3:
				speed = 6f;
				marbleChance = 1f;
				frequency = 0.7f;
				maxObjs = 2;
				break;
			default:
				Debug.LogError("DUC difficulty level defaulted.");
				speed = 5f;
				marbleChance = 1f;
				frequency = 0.8f;
				maxObjs = 2;
				break;
			}
			loaded = true;
		}
	}
}
