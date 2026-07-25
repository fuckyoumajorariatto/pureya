using UnityEngine;

public class DUC_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}
}
