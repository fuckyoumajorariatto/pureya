using UnityEngine;

public class PLU_BgParticlesController_1 : MonoBehaviour
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
		bgSpeed = PLU_Difficulty.PLU_difficulty.bg_speed_1;
		main.startSpeed = new ParticleSystem.MinMaxCurve(bgSpeed / 2f, bgSpeed);
		main.startLifetime = PLU_Difficulty.PLU_difficulty.bg_startLifetime_1;
		emission.rateOverTime = PLU_Difficulty.PLU_difficulty.bg_rateOverTime_1;
	}
}
