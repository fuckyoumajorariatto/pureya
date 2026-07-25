using UnityEngine;

public class FRO_BgParticlesController_2 : MonoBehaviour
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
		FRO_Difficulty fRO_difficulty = FRO_Difficulty.FRO_difficulty;
		float speed = fRO_difficulty.speed;
		main.startSpeed = new ParticleSystem.MinMaxCurve(speed / 3f, speed - speed / 3f);
		main.startLifetime = fRO_difficulty.bg_startLifetime_2;
		emission.rateOverTime = fRO_difficulty.bg_rateOverTime_2;
	}
}
