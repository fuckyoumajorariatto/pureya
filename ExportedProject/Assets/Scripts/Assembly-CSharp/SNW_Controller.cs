using UnityEngine;

public class SNW_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	public SNW_MarbleSpawner marbleSpawner;

	private bool difficultyLoaded;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(223, 223, 223, byte.MaxValue);
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
		if (!difficultyLoaded && SNW_Difficulty.SNW_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		marbleSpawner.enabled = true;
	}
}
