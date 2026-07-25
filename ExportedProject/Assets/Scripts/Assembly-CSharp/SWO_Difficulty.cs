using UnityEngine;

public class SWO_Difficulty : MonoBehaviour
{
	public static SWO_Difficulty SWO_difficulty;

	public bool loaded;

	public float arrowSpeed;

	public float arrowFrequency;

	public float bgSpeed;

	public int level;

	private GlobalControl globalControl;

	private void Awake()
	{
		SWO_difficulty = this;
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
		if (SWO_ArrowSpawner.SWO_arrowSpawner != null)
		{
			SWO_ArrowSpawner.SWO_arrowSpawner.SetDifficultyParas();
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
				arrowSpeed = 4f;
				arrowFrequency = 1.5f;
				bgSpeed = 1f;
				break;
			case 1:
				arrowSpeed = 6f;
				arrowFrequency = 0.75f;
				bgSpeed = 1.5f;
				break;
			case 2:
				arrowSpeed = 9f;
				arrowFrequency = 0.5f;
				bgSpeed = 2f;
				break;
			case 3:
				arrowSpeed = 9.5f;
				arrowFrequency = 0.45f;
				bgSpeed = 2.5f;
				break;
			default:
				Debug.LogError("SWO difficulty level defaulted.");
				arrowSpeed = 9f;
				arrowFrequency = 0.5f;
				bgSpeed = 2f;
				break;
			}
			loaded = true;
		}
	}
}
