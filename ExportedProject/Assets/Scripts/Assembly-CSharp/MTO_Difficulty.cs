using UnityEngine;

public class MTO_Difficulty : MonoBehaviour
{
	public static MTO_Difficulty MTO_difficulty;

	public MTO_MeteorSpawner MTO_meteorSpawner;

	public MTO_StartsParticlesController_1 MTO_startsParticlesController_1;

	public MTO_StartsParticlesController_2 MTO_startsParticlesController_2;

	public bool loaded;

	public float meteor_frequency;

	public float meteor_speed;

	public int meteor_life;

	public float bg_speed_1;

	public float bg_startLifetime_1;

	public float bg_rateOverTime_1;

	public float bg_speed_2;

	public float bg_startLifetime_2;

	public float bg_rateOverTime_2;

	public int level;

	private void Awake()
	{
		MTO_difficulty = this;
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

	public void UpdateDifficulty()
	{
		LoadLevel();
		MTO_meteorSpawner.SetDifficultyParas();
		MTO_startsParticlesController_1.SetDifficultyParas();
		MTO_startsParticlesController_2.SetDifficultyParas();
	}

	public void LoadLevel()
	{
		level = GlobalControl.globalControl.GetLevel();
		switch (level)
		{
		case 0:
			meteor_frequency = 1f;
			meteor_speed = 4000f;
			meteor_life = 2;
			bg_speed_1 = 3f;
			bg_startLifetime_1 = 10f;
			bg_rateOverTime_1 = 3f;
			bg_speed_2 = 1f;
			bg_startLifetime_2 = 30f;
			bg_rateOverTime_2 = 2f;
			break;
		case 1:
			meteor_frequency = 0.7f;
			meteor_speed = 4000f;
			meteor_life = 3;
			bg_speed_1 = 4f;
			bg_startLifetime_1 = 6f;
			bg_rateOverTime_1 = 6f;
			bg_speed_2 = 1.5f;
			bg_startLifetime_2 = 14f;
			bg_rateOverTime_2 = 4f;
			break;
		case 2:
			meteor_frequency = 0.5f;
			meteor_speed = 5000f;
			meteor_life = 3;
			bg_speed_1 = 5f;
			bg_startLifetime_1 = 4f;
			bg_rateOverTime_1 = 7f;
			bg_speed_2 = 2f;
			bg_startLifetime_2 = 10f;
			bg_rateOverTime_2 = 5f;
			break;
		case 3:
			meteor_frequency = 0.3f;
			meteor_speed = 6000f;
			meteor_life = 3;
			bg_speed_1 = 6f;
			bg_startLifetime_1 = 4f;
			bg_rateOverTime_1 = 7f;
			bg_speed_2 = 2.5f;
			bg_startLifetime_2 = 10f;
			bg_rateOverTime_2 = 4f;
			break;
		default:
			Debug.LogError("MTO difficulty defaulted.");
			meteor_frequency = 0.5f;
			meteor_speed = 5000f;
			meteor_life = 3;
			bg_speed_1 = 5f;
			bg_startLifetime_1 = 4f;
			bg_rateOverTime_1 = 7f;
			bg_speed_2 = 2f;
			bg_startLifetime_2 = 10f;
			bg_rateOverTime_2 = 5f;
			break;
		}
		loaded = true;
	}
}
