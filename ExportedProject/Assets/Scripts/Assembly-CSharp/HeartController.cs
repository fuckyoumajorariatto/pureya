using UnityEngine;

public class HeartController : MonoBehaviour
{
	public static HeartController heartController;

	public AudioClip heartBreak;

	public bool animationRunning;

	private void Start()
	{
		heartController = this;
	}

	public void ShowBreak()
	{
		if (GlobalControl.globalControl.mode == "normal")
		{
			MinigamesController.minigamesController.brokenHearts[MinigamesController.minigamesController.currentMinigameNumber - 1] = 1;
		}
	}

	public void CancelAnimation()
	{
		animationRunning = false;
		base.gameObject.GetComponent<Animator>().SetTrigger("cancel");
	}

	private void PlayHeartBreak()
	{
		GlobalAudio.globalAudio.Play(heartBreak, 1f, 1f, 0f);
	}

	private void End()
	{
		animationRunning = false;
	}
}
