using UnityEngine;

public class SHI_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(24, 174, 228, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}
}
