using UnityEngine;

public class PCK_SlotBattleController : MonoBehaviour
{
	public AudioClip punch1;

	public AudioClip punch2;

	public AudioClip rustle;

	public Animator renderCamAnim;

	private AsyncOperation async;

	private bool waitingForCinematic;

	private void Update()
	{
		if (waitingForCinematic && async.progress == 1f)
		{
			waitingForCinematic = false;
			renderCamAnim.Play("idle");
		}
	}

	private void PlayPunch1()
	{
		GlobalAudio.globalAudio.Play(punch1, 1f, Random.Range(0.9f, 1.1f), 0f);
	}

	private void PlayPunch2()
	{
		GlobalAudio.globalAudio.Play(punch2, 1f, Random.Range(0.9f, 1.1f), 0f);
	}

	private void PlayRustle()
	{
		GlobalAudio.globalAudio.Play(rustle, 0.6f, Random.Range(0.7f, 1f), 0f);
	}

	private void PlayRustleSoft()
	{
		GlobalAudio.globalAudio.Play(rustle, 0.2f, Random.Range(0.7f, 1f), 0f);
	}

	private void StartLoadingFightCinematic()
	{
		PCK_CinematicsController.PCK_cinematicsController.ChooseAndLoadCinematic();
	}

	private void ZoomAndShowCinematic()
	{
		renderCamAnim.Play("zoom in");
		Invoke("Fade", 0.6f);
		Invoke("ShowCinematic", 1f);
		PCK_CinematicsController.PCK_cinematicsController.PlayTransition();
		PCK_Controller.PCK_controller.FadePachinkoMusic();
		PauseController.pauseController.pauseCooldownTimer = 1f;
	}

	private void Fade()
	{
		PCK_CinematicsController.PCK_cinematicsController.FadeFast();
	}

	private void ShowCinematic()
	{
		waitingForCinematic = true;
		async = PCK_CinematicsController.PCK_cinematicsController.ActivateLoadedScene();
	}

	public void ZoomOut()
	{
		PauseController.pauseController.pauseCooldownTimer = 1f;
		if (PCK_SlotsController.PCK_slotsController.resultKind == "jackpot")
		{
			PCK_SlotsController.PCK_slotsController.animator.SetTrigger("cinematicFightWin");
		}
		else
		{
			PCK_SlotsController.PCK_slotsController.animator.SetTrigger("cinematicFightLose");
		}
		renderCamAnim.Play("zoom out");
	}
}
