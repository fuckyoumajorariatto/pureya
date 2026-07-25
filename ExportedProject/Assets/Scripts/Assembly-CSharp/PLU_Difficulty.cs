using UnityEngine;

public class PLU_Difficulty : MonoBehaviour
{
	public static PLU_Difficulty PLU_difficulty;

	public bool loaded;

	public float rock_frequency;

	public float rock_speed;

	public int rock_max;

	public float marble_frequency;

	public float marble_speed;

	public float bg_speed_1;

	public float bg_startLifetime_1;

	public float bg_rateOverTime_1;

	public float bg_speed_2;

	public float bg_startLifetime_2;

	public float bg_rateOverTime_2;

	public int level;

	public GameObject BgParticles_1;

	public GameObject BgParticles_2;

	public GameObject BgParticles_3;

	public PLU_MarbleSpawner marbleSpawner;

	public PLU_RockSpawner rockSpawner;

	public PLU_BgParticlesController_1 bgpc1;

	public PLU_BgParticlesController_2 bgpc2;

	public PLU_BgParticlesController_3 bgpc3;

	private void Awake()
	{
		PLU_difficulty = this;
	}

	private void Start()
	{
		LoadLevel();
	}

	private void Update()
	{
		if (level != GlobalControl.globalControl.GetLevel())
		{
			UpdateDifficulty();
		}
	}

	private void UpdateDifficulty()
	{
		LoadLevel();
		marbleSpawner.SetDifficultyParas();
		rockSpawner.SetDifficultyParas();
		bgpc1.SetDifficultyParas();
		bgpc2.SetDifficultyParas();
		bgpc3.SetDifficultyParas();
	}

	private void LoadLevel()
	{
		level = GlobalControl.globalControl.GetLevel();
		switch (level)
		{
		case 0:
			rock_frequency = 1.5f;
			rock_speed = 8f;
			rock_max = 1;
			marble_frequency = 2f;
			marble_speed = 6f;
			bg_speed_1 = 20f;
			bg_startLifetime_1 = 1f;
			bg_rateOverTime_1 = 50f;
			bg_speed_2 = 10f;
			bg_startLifetime_2 = 1.5f;
			bg_rateOverTime_2 = 30f;
			break;
		case 1:
			rock_frequency = 1.3f;
			rock_speed = 9f;
			rock_max = 2;
			marble_frequency = 1.75f;
			marble_speed = 7f;
			bg_speed_1 = 30f;
			bg_startLifetime_1 = 0.75f;
			bg_rateOverTime_1 = 75f;
			bg_speed_2 = 12.5f;
			bg_startLifetime_2 = 1.25f;
			bg_rateOverTime_2 = 35f;
			break;
		case 2:
			rock_frequency = 0.8f;
			rock_speed = 11f;
			rock_max = 2;
			marble_frequency = 1f;
			marble_speed = 8f;
			bg_speed_1 = 40f;
			bg_startLifetime_1 = 0.5f;
			bg_rateOverTime_1 = 100f;
			bg_speed_2 = 15f;
			bg_startLifetime_2 = 1f;
			bg_rateOverTime_2 = 40f;
			break;
		case 3:
			rock_frequency = 0.7f;
			rock_speed = 12f;
			rock_max = 2;
			marble_frequency = 0.8f;
			marble_speed = 10f;
			bg_speed_1 = 45f;
			bg_startLifetime_1 = 0.45f;
			bg_rateOverTime_1 = 100f;
			bg_speed_2 = 17f;
			bg_startLifetime_2 = 0.9f;
			bg_rateOverTime_2 = 40f;
			break;
		default:
			Debug.LogError("PLU difficulty level defaulted.");
			rock_frequency = 0.9f;
			rock_speed = 10f;
			rock_max = 2;
			marble_frequency = 1.1f;
			marble_speed = 8f;
			bg_speed_1 = 40f;
			bg_startLifetime_1 = 0.5f;
			bg_rateOverTime_1 = 100f;
			bg_speed_2 = 15f;
			bg_startLifetime_2 = 1f;
			bg_rateOverTime_2 = 40f;
			break;
		}
		loaded = true;
	}
}
