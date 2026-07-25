using System.Collections.Generic;
using UnityEngine;

public class FLA_Difficulty : MonoBehaviour
{
	public static FLA_Difficulty FLA_difficulty;

	public bool loaded;

	public float scroll_speed;

	public float frequency;

	public int level;

	private GlobalControl globalControl;

	private void Awake()
	{
		FLA_difficulty = this;
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
		base.gameObject.GetComponent<FLA_ShellSpawner>().SetDifficultyParas();
		base.gameObject.GetComponent<FLA_MarbleSpawner>().SetDifficultyParas();
		base.gameObject.GetComponent<FLA_Controller>().player.GetComponent<FLA_PlayerController>().SetDifficultyParas();
		List<GameObject> marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < marbles.Count; i++)
		{
			marbles[i].GetComponent<MarbleController>().speed = scroll_speed;
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
				scroll_speed = 3f;
				frequency = 1f;
				break;
			case 1:
				scroll_speed = 4f;
				frequency = 0.8f;
				break;
			case 2:
				scroll_speed = 5f;
				frequency = 0.6f;
				break;
			case 3:
				scroll_speed = 5.5f;
				frequency = 0.55f;
				break;
			default:
				Debug.LogError("FLA difficulty level defaulted.");
				scroll_speed = 5f;
				frequency = 0.6f;
				break;
			}
			loaded = true;
		}
	}
}
