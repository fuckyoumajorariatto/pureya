using UnityEngine;

public class PLU_BgParticlesController_2 : MonoBehaviour
{
	public ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private float bgSpeed;

	private void Start()
	{
		SetDifficultyParas();
	}

	public void SetDifficultyParas()
	{
		main = ps.main;
		emission = ps.emission;
		bgSpeed = PLU_Difficulty.PLU_difficulty.bg_speed_2;
		main.startSpeed = new ParticleSystem.MinMaxCurve(bgSpeed * 0.67f, bgSpeed);
		main.startLifetime = PLU_Difficulty.PLU_difficulty.bg_startLifetime_2;
		emission.rateOverTime = PLU_Difficulty.PLU_difficulty.bg_rateOverTime_2;
	}
}
