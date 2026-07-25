using UnityEngine;

public class MMUController : MonoBehaviour
{
	public GameObject exitButton;

	public GameObject rotationButton;

	private void Start()
	{
		if (GlobalControl.globalControl.mobile)
		{
			exitButton.SetActive(false);
			rotationButton.SetActive(true);
		}
		else
		{
			exitButton.SetActive(true);
			rotationButton.SetActive(false);
		}
		GlobalControl.globalControl.playing = false;
		Screen.sleepTimeout = -2;
		GlobalControl.globalControl.CalculateProgress();
	}
}
