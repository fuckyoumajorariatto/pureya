using UnityEngine;

public class Ending2ControllerProxy : MonoBehaviour
{
	public Ending2Controller controller;

	private void JumpToScene02()
	{
		controller.ActivateScene02();
	}

	private void JumpToScene03()
	{
		controller.ActivateScene03();
	}

	private void PlayShine()
	{
		controller.PlayShine();
	}

	private void PlayTurn()
	{
		controller.PlayTurn();
	}

	private void PlayBirdWing()
	{
		controller.PlayBirdWing();
	}

	private void PlayAmbience()
	{
		controller.PlayAmbience();
	}

	private void PlayWindow()
	{
		controller.PlayWindow();
	}

	private void PlayGirlMovement1()
	{
		controller.PlayGirlMovement1();
	}

	private void PlayGirlMovement2()
	{
		controller.PlayGirlMovement2();
	}

	private void PlayMarblesThrow()
	{
		controller.PlayMarblesThrow();
	}

	private void PlayMarbleFly()
	{
		controller.PlayMarbleFly();
	}

	private void PlayMusic()
	{
		controller.PlayMusic();
	}

	private void End()
	{
		controller.End();
	}
}
