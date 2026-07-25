using UnityEngine;

public class SQR_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	public GameObject bgParticles;

	private bool difficultyLoaded;

	private bool stingPlayed;

	public SQR_PineconeSpawner pineconeSpawner;

	public SQR_MarbleSpawner marbleSpawner;

	public SQR_BgBranchSpawner bgBranchSpawner;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(109, 186, byte.MaxValue, byte.MaxValue);
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
		if (!difficultyLoaded && SQR_Difficulty.SQR_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		pineconeSpawner.enabled = true;
		marbleSpawner.enabled = true;
		bgBranchSpawner.enabled = true;
		bgParticles.SetActive(true);
	}
}
