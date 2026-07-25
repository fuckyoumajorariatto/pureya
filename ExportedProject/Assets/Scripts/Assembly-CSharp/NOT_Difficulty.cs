using UnityEngine;

public class NOT_Difficulty : MonoBehaviour
{
	public static NOT_Difficulty NOT_difficulty;

	public NOT_BallController ballController;

	public bool loaded;

	public float ballSpeed;

	public float frequency;

	public int level;

	private GlobalControl globalControl;

	private void Awake()
	{
		NOT_difficulty = this;
	}

	private void Start()
	{
		LoadLevel();
		ballController.enabled = true;
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
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				ballSpeed = 5f;
				frequency = 1f;
				break;
			case 1:
				ballSpeed = 6f;
				frequency = 1f;
				break;
			case 2:
				ballSpeed = 7f;
				frequency = 1f;
				break;
			case 3:
				ballSpeed = 8f;
				frequency = 1f;
				break;
			default:
				Debug.LogError("NOT difficulty level defaulted.");
				ballSpeed = 7f;
				frequency = 1f;
				break;
			}
			loaded = true;
		}
	}
}
