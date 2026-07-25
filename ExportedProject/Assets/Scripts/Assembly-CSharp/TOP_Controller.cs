using UnityEngine;

public class TOP_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip sting;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(132, 234, 238, byte.MaxValue);
	}

	private void Update()
	{
		PlaySting();
	}

	private void PlaySting()
	{
		if (GlobalAudio.globalAudio != null && !stingPlayed)
		{
			GlobalAudio.globalAudio.Play(sting, 1f, 1f, 0f);
			stingPlayed = true;
		}
	}
}
