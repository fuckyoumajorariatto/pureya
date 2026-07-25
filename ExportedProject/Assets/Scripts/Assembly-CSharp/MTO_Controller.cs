using UnityEngine;

public class MTO_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip clip;

	public GameObject bgParticles_1;

	public GameObject bgParticles_2;

	public MTO_MeteorSpawner MTO_meteorSpawner;

	private bool difficultyLoaded;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(35, 59, 100, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 1f, 1f, 0f);
	}

	private void Update()
	{
		InitializeDifficulty();
	}

	private void InitializeDifficulty()
	{
		if (!difficultyLoaded && MTO_Difficulty.MTO_difficulty.loaded)
		{
			difficultyLoaded = true;
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		bgParticles_1.SetActive(true);
		bgParticles_2.SetActive(true);
		MTO_meteorSpawner.enabled = true;
	}
}
