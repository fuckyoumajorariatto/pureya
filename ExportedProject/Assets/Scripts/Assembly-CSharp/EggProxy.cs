using UnityEngine;

public class EggProxy : MonoBehaviour
{
	public EggController controller;

	private void ShowScene02()
	{
		controller.ShowScene02();
	}

	private void ShowScene03()
	{
		controller.ShowScene02();
	}

	private void ShowFinalScene()
	{
		controller.ShowFinalScene();
	}

	private void FinalNumber()
	{
		controller.FinalNumber();
	}

	private void EndFade()
	{
		controller.EndFade();
	}

	private void End()
	{
		controller.End();
	}

	private void FinalSceneOut()
	{
		controller.FinalSceneOut();
	}

	public void PlayWind()
	{
		controller.PlayWind();
	}

	public void PlayStep()
	{
		controller.PlayStep();
	}

	public void PlayEggMovement()
	{
		controller.PlayEggMovement();
	}

	public void PlayEggPush()
	{
		controller.PlayEggPush();
	}

	public void PlayWings()
	{
		controller.PlayWings();
	}

	public void PlayHeadMovement()
	{
		controller.PlayHeadMovement();
	}

	public void PlayHeadMovement2()
	{
		controller.PlayHeadMovement2();
	}

	public void PlayPenguinVoice()
	{
		controller.PlayPenguinVoice();
	}

	public void PlayNumberSound()
	{
		controller.PlayNumberSound();
	}

	public void PlayFinalNumberSound()
	{
		controller.PlayFinalNumberSound();
	}
}
