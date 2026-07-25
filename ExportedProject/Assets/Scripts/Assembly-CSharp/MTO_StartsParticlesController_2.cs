using UnityEngine;

public class MTO_StartsParticlesController_2 : MonoBehaviour
{
	public ParticleSystem ps;

	private void Start()
	{
		SetDifficultyParas();
	}

	public void SetDifficultyParas()
	{
		ps = base.gameObject.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = ps.main;
		ParticleSystem.EmissionModule emission = ps.emission;
		MTO_Difficulty mTO_difficulty = MTO_Difficulty.MTO_difficulty;
		float bg_speed_ = mTO_difficulty.bg_speed_2;
		main.startSpeed = new ParticleSystem.MinMaxCurve(bg_speed_ / 2f, bg_speed_);
		main.startLifetime = mTO_difficulty.bg_startLifetime_2;
		emission.rateOverTime = mTO_difficulty.bg_rateOverTime_2;
	}
}
