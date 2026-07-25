using UnityEngine;

public class FTL_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	public GameObject bgParticles;

	private bool difficultyLoaded;

	private bool stingPlayed;

	public FTL_WindowSpawner windowSpawner;

	public FTL_MarbleSpawner marbleSpawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(196, 147, 109, byte.MaxValue);
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
		if (!difficultyLoaded && FTL_Difficulty.FTL_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		windowSpawner.enabled = true;
		marbleSpawner.enabled = true;
		bgParticles.SetActive(true);
	}
}
