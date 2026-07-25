using UnityEngine;

public class CRA_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(58, 66, 197, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}
}
