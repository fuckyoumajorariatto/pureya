using UnityEngine;
using UnityEngine.SceneManagement;

public class PCK_CinematicsController : MonoBehaviour
{
	public static PCK_CinematicsController PCK_cinematicsController;

	public GameObject fadeToWhite;

	public GameObject cinematicNumbers;

	public AudioClip transition;

	private AsyncOperation async;

	public PCK_SlotBattleController slotsBattleController;

	private void Start()
	{
		PCK_cinematicsController = this;
	}

	public void Intro()
	{
		Fade();
		SceneChangeController.sceneChangeController.pausableScene = false;
		async = SceneChangeController.sceneChangeController.LoadSceneInBackground("Intro");
		Invoke("ActivateScene", 1f);
	}

	private void ActivateScene()
	{
		SceneChangeController.sceneChangeController.ActivateScene();
		async.allowSceneActivation = true;
		SceneChangeController.sceneChangeController.pausableScene = true;
	}

	public void Fade()
	{
		fadeToWhite.GetComponent<Animator>().SetTrigger("action");
	}

	public void FadeFast()
	{
		fadeToWhite.GetComponent<Animator>().SetTrigger("actionFast");
	}

	public void Ending1()
	{
		Fade();
		SceneChangeController.sceneChangeController.pausableScene = false;
		async = SceneChangeController.sceneChangeController.LoadSceneInBackground("Ending1");
		Invoke("ActivateScene", 1f);
	}

	public void Ending2()
	{
		Fade();
		SceneChangeController.sceneChangeController.pausableScene = false;
		async = SceneChangeController.sceneChangeController.LoadSceneInBackground("Ending2");
		Invoke("ActivateScene", 1f);
	}

	private void ActivateCinematicNumbers()
	{
		cinematicNumbers.GetComponent<Animator>().SetTrigger("show");
		PCK_CinematicNumbersController.PCK_cinematicNumbersController.ChangeNumbers();
	}

	private void CallPauseMarbles()
	{
		PCK_Controller.PCK_controller.PauseMarbles();
	}

	public void ChooseAndLoadCinematic()
	{
		if (PauseController.pauseController.exiting)
		{
			return;
		}
		PCK_InputController.PCK_inputController.inputActive = false;
		ActivateCinematicNumbers();
		PCK_InputController.PCK_inputController.CancelShoots();
		string empty = string.Empty;
		int num = Random.Range(1, 5);
		while (num == GlobalControl.globalControl.recentCinematics[0] || num == GlobalControl.globalControl.recentCinematics[1])
		{
			num++;
			if (num > 4)
			{
				num = 1;
			}
		}
		switch (num)
		{
		case 1:
			empty = "Horseboys";
			break;
		case 2:
			empty = "Racecars";
			break;
		case 3:
			empty = "Baseball";
			break;
		case 4:
			empty = "Egg";
			break;
		default:
			empty = "Racecars";
			break;
		}
		GlobalControl.globalControl.recentCinematics[1] = GlobalControl.globalControl.recentCinematics[0];
		GlobalControl.globalControl.recentCinematics[0] = num;
		async = SceneManager.LoadSceneAsync(empty, LoadSceneMode.Additive);
		async.allowSceneActivation = false;
	}

	public AsyncOperation ActivateLoadedScene()
	{
		async.allowSceneActivation = true;
		return async;
	}

	public void EndCinematic()
	{
		fadeToWhite.GetComponent<Animator>().SetTrigger("fadeOut");
		slotsBattleController.ZoomOut();
		PCK_Controller.PCK_controller.PlayMusic();
		Invoke("EndCinematic2", 0.25f);
		PCK_InputController.PCK_inputController.inputActive = true;
	}

	private void EndCinematic2()
	{
		PauseController.pauseController.inCinematic = false;
		PauseController.pauseController.inCinematicSkipped = false;
	}

	public void PlayTransition()
	{
		GlobalAudio.globalAudio.Play(transition, 1f, 1f, 0f);
	}
}
