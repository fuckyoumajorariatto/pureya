using UnityEngine;

public class SEA_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip sting;

	private bool difficultyLoaded;

	private bool stingPlayed;

	public SEA_BubbleSpawner bubbleSpawner;

	public SEA_MarbleSpawner marbleSpawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(27, 221, 225, byte.MaxValue);
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
		if (!difficultyLoaded && SEA_Difficulty.SEA_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		bubbleSpawner.enabled = true;
		marbleSpawner.enabled = true;
	}
}
