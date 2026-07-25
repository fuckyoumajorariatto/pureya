using UnityEngine;

public class SNK_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	public SNK_Spawner SNK_spawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(byte.MaxValue, 225, 102, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 0.9f, 1f, 0f);
	}

	private void Update()
	{
		if (SNK_Difficulty.SNK_difficulty.loaded)
		{
			SNK_spawner.enabled = true;
			base.enabled = false;
		}
	}
}
