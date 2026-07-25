using System.Collections.Generic;
using UnityEngine;

public class PNG_Difficulty : MonoBehaviour
{
	public static PNG_Difficulty PNG_difficulty;

	public bool loaded;

	public float speed;

	public int minTemplate;

	public int maxTemplate;

	public float backwardsMaxSpeed;

	public int level;

	public GameObject bgParticles1;

	private GlobalControl globalControl;

	private List<GameObject> marbles;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private void Awake()
	{
		PNG_difficulty = this;
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
				bgParticles1.SetActive(true);
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
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < marbles.Count; i++)
		{
			marbles[i].GetComponent<MarbleController>().speed = speed;
		}
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed, speed * 1.5f);
		bgParticles1.SetActive(true);
	}

	private void LoadLevel()
	{
		if (!(GlobalControl.globalControl == null))
		{
			level = GlobalControl.globalControl.GetLevel();
			switch (level)
			{
			case 0:
				speed = 2.5f;
				minTemplate = 1;
				maxTemplate = 2;
				backwardsMaxSpeed = 5f;
				break;
			case 1:
				speed = 3f;
				minTemplate = 1;
				maxTemplate = 6;
				backwardsMaxSpeed = 6f;
				break;
			case 2:
				speed = 4f;
				minTemplate = 3;
				maxTemplate = 11;
				backwardsMaxSpeed = 7.5f;
				break;
			case 3:
				speed = 5f;
				minTemplate = 3;
				maxTemplate = 13;
				backwardsMaxSpeed = 8f;
				break;
			default:
				Debug.LogError("PNG difficulty level defaulted.");
				speed = 4f;
				minTemplate = 3;
				maxTemplate = 11;
				break;
			}
			loaded = true;
		}
	}
}
