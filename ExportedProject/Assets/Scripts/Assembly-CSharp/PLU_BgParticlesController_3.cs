using UnityEngine;

public class PLU_BgParticlesController_3 : MonoBehaviour
{
	public ParticleSystem ps;

	private ParticleSystem.MainModule main;

	private float bgSpeed;

	private void Start()
	{
		SetDifficultyParas();
	}

	public void SetDifficultyParas()
	{
		main = ps.main;
		bgSpeed = PLU_Difficulty.PLU_difficulty.bg_speed_2;
		main.startSpeed = new ParticleSystem.MinMaxCurve(PLU_Difficulty.PLU_difficulty.rock_speed, bgSpeed);
		main.startLifetime = PLU_Difficulty.PLU_difficulty.bg_startLifetime_2;
	}
}
