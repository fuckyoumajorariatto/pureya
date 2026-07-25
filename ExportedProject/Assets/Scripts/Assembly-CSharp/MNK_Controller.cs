using UnityEngine;

public class MNK_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	private bool stingPlayed;

	public MNK_CoconutSpawner coconutSpawner;

	public MNK_ScrollController scrollController;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(54, 111, 36, byte.MaxValue);
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
		if (!difficultyLoaded && MNK_Difficulty.MNK_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		coconutSpawner.enabled = true;
		scrollController.enabled = true;
	}
}
