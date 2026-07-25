using UnityEngine;

public class Ending1ControllerProxy : MonoBehaviour
{
	public Ending1Controller controller;

	private void JumpToScene02()
	{
		controller.ActivateScene02();
	}

	private void JumpToScene03()
	{
		controller.ActivateScene03();
	}

	private void JumpToScene04()
	{
		controller.ActivateScene04();
	}

	private void JumpToScene04_2()
	{
		controller.ActivateScene04_2();
	}

	private void JumpToScene05()
	{
		controller.ActivateScene05();
	}

	private void ActivateCreditsBG()
	{
		controller.ActivateCreditsBG();
	}

	private void ShowTitle()
	{
		controller.ShowTitle();
	}

	private void HideTitle()
	{
		controller.HideTitle();
	}

	private void StartCredits()
	{
		controller.StartCredits();
	}

	private void EndCredits()
	{
		controller.EndCredits();
	}

	public void PlayShine()
	{
		controller.PlayShine();
	}

	public void PlayTurn()
	{
		controller.PlayTurn();
	}

	public void PlayStretchArms()
	{
		controller.PlayStretchArms();
	}

	public void PlayLeaveBowl()
	{
		controller.PlayLeaveBowl();
	}

	public void PlayTurn2()
	{
		controller.PlayTurn2();
	}

	public void PlayStep()
	{
		controller.PlayStep();
	}

	public void PlayCreak()
	{
		controller.PlayCreak();
	}

	public void PlaySlide()
	{
		controller.PlaySlide();
	}

	public void PlayTurnBowl()
	{
		controller.PlayTurnBowl();
	}

	public void PlayDropBowl()
	{
		controller.PlayDropBowl();
	}

	public void PlayFallingMarbles()
	{
		controller.PlayFallingMarbles();
	}

	public void PlayCreditsMusic()
	{
		controller.PlayCreditsMusic();
	}

	public void PlayMusic()
	{
		controller.PlayMusic();
	}

	public void End()
	{
		controller.End();
	}
}
