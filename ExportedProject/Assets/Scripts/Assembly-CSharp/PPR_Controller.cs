using UnityEngine;

public class PPR_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	private bool stingPlayed;

	public PPR_BalloonSpawner balloonSpawner;

	public PPR_MarbleSpawner marbleSpawner;

	public PPR_CloudSpawner cloudSpawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(88, 190, byte.MaxValue, byte.MaxValue);
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
		if (!difficultyLoaded && PPR_Difficulty.PPR_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		balloonSpawner.enabled = true;
		marbleSpawner.enabled = true;
		cloudSpawner.enabled = true;
	}
}
