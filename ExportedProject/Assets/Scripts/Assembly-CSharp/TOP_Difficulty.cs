using System.Collections.Generic;
using UnityEngine;

public class TOP_Difficulty : MonoBehaviour
{
	public static TOP_Difficulty TOP_difficulty;

	public bool loaded;

	public float scroll;

	public float ballChance;

	public float marbleChance;

	public int level;

	public GameObject bgParticles1;

	private GlobalControl globalControl;

	private List<GameObject> marbles;

	public ParticleSystem ps1;

	private ParticleSystem.MainModule main;

	private GameObject[] objs;

	private void Awake()
	{
		TOP_difficulty = this;
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
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll * 0.7f, scroll * 1.5f);
		bgParticles1.SetActive(true);
		objs = GameObject.FindGameObjectsWithTag("marble");
		for (int i = 0; i < objs.Length; i++)
		{
			objs[i].GetComponent<MarbleController>().speed = scroll;
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
				scroll = 1.5f;
				ballChance = 0.4f;
				marbleChance = 0.4f;
				break;
			case 1:
				scroll = 2f;
				ballChance = 0.5f;
				marbleChance = 0.5f;
				break;
			case 2:
				scroll = 2.5f;
				ballChance = 0.5f;
				marbleChance = 0.5f;
				break;
			case 3:
				scroll = 3f;
				ballChance = 0.5f;
				marbleChance = 0.5f;
				break;
			default:
				Debug.LogError("TOP difficulty level defaulted.");
				scroll = 2.5f;
				ballChance = 0.6f;
				marbleChance = 0.5f;
				break;
			}
			loaded = true;
		}
	}
}
