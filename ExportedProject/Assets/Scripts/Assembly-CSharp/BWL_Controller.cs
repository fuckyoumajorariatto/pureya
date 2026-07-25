using UnityEngine;

public class BWL_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	public BWL_Spawner BWL_spawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(233, 192, 100, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}

	private void Update()
	{
		if (BWL_Difficulty.BWL_difficulty.loaded)
		{
			BWL_spawner.enabled = true;
			base.enabled = false;
		}
	}
}
