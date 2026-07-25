using UnityEngine;

public class ZIP_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	public ZIP_TreeSpawner treeSpawner;

	public ZIP_MarbleSpawner marbleSpawner;

	public ZIP_GrassSpawner grassSpawner;

	private bool difficultyLoaded;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(108, 212, 24, byte.MaxValue);
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
		if (!difficultyLoaded && ZIP_Difficulty.ZIP_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		base.gameObject.GetComponent<ZIP_TreeSpawner>().enabled = true;
		base.gameObject.GetComponent<ZIP_MarbleSpawner>().enabled = true;
		base.gameObject.GetComponent<ZIP_GrassSpawner>().enabled = true;
	}
}
