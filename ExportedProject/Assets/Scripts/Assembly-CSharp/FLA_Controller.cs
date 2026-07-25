using UnityEngine;

public class FLA_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(46, 219, byte.MaxValue, byte.MaxValue);
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
		if (!difficultyLoaded && FLA_Difficulty.FLA_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		base.gameObject.GetComponent<FLA_ShellSpawner>().enabled = true;
		base.gameObject.GetComponent<FLA_MarbleSpawner>().enabled = true;
		player.GetComponent<FLA_PlayerController>().enabled = true;
	}
}
