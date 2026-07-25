using UnityEngine;

public class BAT_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	private bool difficultyLoaded;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(78, 51, 147, byte.MaxValue);
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
		if (!difficultyLoaded && BAT_Difficulty.BAT_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		base.gameObject.GetComponent<BAT_StalactitesSpawner>().enabled = true;
		base.gameObject.GetComponent<BAT_BgDarknessSquareSpawner>().enabled = true;
		bgParticles1.SetActive(true);
		bgParticles2.SetActive(true);
	}
}
