using UnityEngine;

public class SKI_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	public SKI_Spawner SKI_spawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(0, 198, 218, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}

	private void Update()
	{
		if (SKI_Difficulty.SKI_difficulty.loaded)
		{
			SKI_spawner.enabled = true;
			base.enabled = false;
		}
	}
}
