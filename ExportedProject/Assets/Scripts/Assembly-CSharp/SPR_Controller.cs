using UnityEngine;

public class SPR_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public GameObject playerCenter;

	public GameObject hitPlayerCenter;

	public AudioClip sting;

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public SPR_NailSpawner nailSpawner;

	public SPR_MarbleSpawner marbleSpawner;

	public ParticleSystem ps1;

	public ParticleSystem ps2;

	private ParticleSystem.MainModule main;

	private ParticleSystem.EmissionModule emission;

	private bool difficultyLoaded;

	private bool stingPlayed;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(194, 132, 80, byte.MaxValue);
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
		if (!difficultyLoaded && SPR_Difficulty.SPR_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		nailSpawner.enabled = true;
		marbleSpawner.enabled = true;
		main = ps1.main;
		main.startSpeed = SPR_Difficulty.SPR_difficulty.scrollSpeed;
		main = ps2.main;
		main.startSpeed = SPR_Difficulty.SPR_difficulty.scrollSpeed / 2.5f;
		emission = ps2.emission;
		emission.rateOverTime = SPR_Difficulty.SPR_difficulty.bgParticles2Rate;
		bgParticles1.SetActive(true);
		bgParticles2.SetActive(true);
	}
}
