using UnityEngine;

public class RacecarsProxy : MonoBehaviour
{
	public RacecarsController controller;

	private void DuelWin()
	{
		controller.DuelWin();
	}

	private void DuelLose()
	{
		controller.DuelLose();
	}

	private void FinalNumber()
	{
		controller.FinalNumber();
	}

	private void EndFade()
	{
		controller.EndFade();
	}

	private void EnableParticles1()
	{
		controller.EnableParticles1();
	}

	private void EnableParticles2()
	{
		controller.EnableParticles2();
	}

	private void DisableParticles1()
	{
		controller.DisableParticles1();
	}

	private void DisableParticles2()
	{
		controller.DisableParticles2();
	}

	private void FinalSceneOut()
	{
		controller.FinalSceneOut();
	}

	private void End()
	{
		controller.End();
	}

	private void PlayEngine1()
	{
		controller.PlayEngine1();
	}

	private void PlayEngine2()
	{
		controller.PlayEngine2();
	}

	private void LowerEngine()
	{
		controller.LowerEngine();
	}

	private void PlayDriftL()
	{
		controller.PlayDriftL();
	}

	private void PlayDriftR()
	{
		controller.PlayDriftR();
	}

	private void PlayCrash()
	{
		controller.PlayCrash();
	}

	private void PlayFire()
	{
		controller.PlayFire();
	}

	private void PlayCarTouch()
	{
		controller.PlayCarTouch();
	}

	private void PlayEngineRevs()
	{
		controller.PlayEngineRevs();
	}

	private void PlayClaps()
	{
		controller.PlayClaps();
	}

	private void PlayShoot()
	{
		controller.PlayShoot();
	}
}
