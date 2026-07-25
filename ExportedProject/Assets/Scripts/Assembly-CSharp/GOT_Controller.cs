using UnityEngine;

public class GOT_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	private bool stingPlayed;

	public GOT_RockSpawner rockSpawner;

	public GOT_MarbleSpawner marbleSpawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(155, 155, 155, byte.MaxValue);
	}

	private void Update()
	{
		InitializeDifficulty();
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

	private void InitializeDifficulty()
	{
		if (!difficultyLoaded && GOT_Difficulty.GOT_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		rockSpawner.enabled = true;
		marbleSpawner.enabled = true;
	}
}
