using UnityEngine;

public class CNN_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	public CNN_WaveSpawner waveSpawner;

	public CNN_GroundMountSpawner groundMountSpawner;

	public CNN_BallSpawner ballSpawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(125, 226, byte.MaxValue, byte.MaxValue);
		GlobalAudio.globalAudio.Play(sting, 1f, 1f, 0f);
	}

	private void Update()
	{
		LoadDifficulty();
	}

	private void LoadDifficulty()
	{
		if (!difficultyLoaded && CNN_Difficulty.CNN_difficulty.loaded)
		{
			difficultyLoaded = true;
			waveSpawner.enabled = true;
			groundMountSpawner.enabled = true;
			ballSpawner.enabled = true;
		}
	}
}
