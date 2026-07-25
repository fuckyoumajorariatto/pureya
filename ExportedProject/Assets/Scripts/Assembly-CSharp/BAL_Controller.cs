using UnityEngine;

public class BAL_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(88, 232, byte.MaxValue, byte.MaxValue);
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
		if (!difficultyLoaded && BAL_Difficulty.BAL_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		base.gameObject.GetComponent<BAL_CactusSpawner>().enabled = true;
		base.gameObject.GetComponent<BAL_MarbleSpawner>().enabled = true;
	}
}
