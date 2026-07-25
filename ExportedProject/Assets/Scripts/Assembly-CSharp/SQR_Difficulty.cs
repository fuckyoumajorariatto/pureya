using UnityEngine;

public class SQR_Difficulty : MonoBehaviour
{
	public static SQR_Difficulty SQR_difficulty;

	public bool loaded;

	public float scroll_speed;

	public float pinecone_frequency;

	public int pinecone_max;

	public float marble_frequency;

	public int level;

	private GlobalControl globalControl;

	public SQR_PineconeSpawner pineconeSpawner;

	public SQR_MarbleSpawner marbleSpawner;

	public SQR_BgBranchSpawner bgBranchSpawner;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		SQR_difficulty = this;
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
		pineconeSpawner.SetDifficultyParas();
		marbleSpawner.SetDifficultyParas();
		bgBranchSpawner.SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll_speed / 2f, scroll_speed);
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				scroll_speed = 4f;
				pinecone_frequency = 1.5f;
				pinecone_max = 1;
				marble_frequency = 2f;
				break;
			case 1:
				scroll_speed = 6f;
				pinecone_frequency = 1f;
				pinecone_max = 2;
				marble_frequency = 1f;
				break;
			case 2:
				scroll_speed = 7f;
				pinecone_frequency = 0.55f;
				pinecone_max = 2;
				marble_frequency = 0.68f;
				break;
			case 3:
				scroll_speed = 8f;
				pinecone_frequency = 0.45f;
				pinecone_max = 2;
				marble_frequency = 0.55f;
				break;
			default:
				Debug.LogError("SQR difficulty level defaulted.");
				scroll_speed = 7f;
				pinecone_frequency = 0.55f;
				pinecone_max = 2;
				marble_frequency = 0.68f;
				break;
			}
			loaded = true;
		}
	}
}
