using System.Collections.Generic;
using UnityEngine;

public class MNK_Difficulty : MonoBehaviour
{
	public static MNK_Difficulty MNK_difficulty;

	public bool loaded;

	public float scroll_speed;

	public float fall_speed;

	public float frequency;

	public int level;

	private GlobalControl globalControl;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	private List<GameObject> items;

	private void Awake()
	{
		MNK_difficulty = this;
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
		base.gameObject.GetComponent<MNK_CoconutSpawner>().SetDifficultyParas();
		main = ps1.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll_speed * 2f, scroll_speed * 4f);
		bgParticles1.SetActive(true);
		main = ps2.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(scroll_speed, scroll_speed * 2f);
		bgParticles2.SetActive(true);
		items = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marbles.Count; i++)
		{
			if (items[i].activeInHierarchy)
			{
				items[i].transform.GetComponent<MarbleController>().speed = fall_speed;
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
				scroll_speed = 1.5f;
				fall_speed = 3f;
				frequency = 0.8f;
				break;
			case 1:
				scroll_speed = 1.75f;
				fall_speed = 4.5f;
				frequency = 0.4f;
				break;
			case 2:
				scroll_speed = 2f;
				fall_speed = 5.5f;
				frequency = 0.27f;
				break;
			case 3:
				scroll_speed = 2.25f;
				fall_speed = 6f;
				frequency = 0.24f;
				break;
			default:
				Debug.LogError("MNK difficulty level defaulted.");
				scroll_speed = 2f;
				fall_speed = 5.5f;
				frequency = 0.27f;
				break;
			}
			loaded = true;
		}
	}
}
