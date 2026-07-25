using UnityEngine;

public class RAT_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(138, 164, 216, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}
}
