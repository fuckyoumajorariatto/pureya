using System.Collections.Generic;
using UnityEngine;

public class FRO_Difficulty : MonoBehaviour
{
	public static FRO_Difficulty FRO_difficulty;

	public bool loaded;

	public float speed;

	public float lilypad_frequency;

	public float log_frequency;

	public float bg_startLifetime_1;

	public float bg_rateOverTime_1;

	public float bg_startLifetime_2;

	public float bg_rateOverTime_2;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private List<GameObject> marbles;

	private void Awake()
	{
		FRO_difficulty = this;
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
		FRO_PlayerController.FRO_playerController.SetDifficultyParas();
		base.gameObject.GetComponent<FRO_RockSpawner>().SetDifficultyParas();
		base.gameObject.GetComponent<FRO_LilypadSpawner>().SetDifficultyParas();
		base.gameObject.GetComponent<FRO_MarbleSpawner>().SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed - speed / 3f, speed);
		main.startLifetime = bg_startLifetime_1;
		emission = ps1.emission;
		emission.rateOverTime = bg_rateOverTime_1;
		bgParticles1.SetActive(true);
		main = ps2.main;
		main.startLifetime = bg_startLifetime_2;
		emission = ps2.emission;
		emission.rateOverTime = bg_rateOverTime_2;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed / 3f, speed - speed / 3f);
		bgParticles2.SetActive(true);
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < marbles.Count; i++)
		{
			if (marbles[i].transform.position.x < -1.6f || marbles[i].transform.position.x > 1.6f)
			{
				marbles[i].GetComponent<MarbleController>().speed = speed;
			}
			else
			{
				marbles[i].GetComponent<MarbleController>().speed = speed * 2f;
			}
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
				lilypad_frequency = 1f;
				log_frequency = 2f;
				bg_startLifetime_1 = 10f;
				bg_rateOverTime_1 = 10f;
				bg_startLifetime_2 = 20f;
				bg_rateOverTime_2 = 5f;
				break;
			case 1:
				speed = 3f;
				lilypad_frequency = 0.6f;
				log_frequency = 0.6f;
				bg_startLifetime_1 = 7f;
				bg_rateOverTime_1 = 15f;
				bg_startLifetime_2 = 15f;
				bg_rateOverTime_2 = 7f;
				break;
			case 2:
				speed = 4f;
				lilypad_frequency = 0.5f;
				log_frequency = 0.5f;
				bg_startLifetime_1 = 5f;
				bg_rateOverTime_1 = 20f;
				bg_startLifetime_2 = 10f;
				bg_rateOverTime_2 = 10f;
				break;
			case 3:
				speed = 4.5f;
				lilypad_frequency = 0.45f;
				log_frequency = 0.45f;
				bg_startLifetime_1 = 4.5f;
				bg_rateOverTime_1 = 18f;
				bg_startLifetime_2 = 8f;
				bg_rateOverTime_2 = 14f;
				break;
			default:
				Debug.LogError("FRO difficulty level defaulted.");
				speed = 4f;
				lilypad_frequency = 0.5f;
				log_frequency = 0.4f;
				bg_startLifetime_1 = 5f;
				bg_rateOverTime_1 = 20f;
				bg_startLifetime_2 = 10f;
				bg_rateOverTime_2 = 10f;
				break;
			}
			loaded = true;
		}
	}
}
