using UnityEngine;

public class PLU_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public AudioClip sting;

	public GameObject BgParticles_1;

	public GameObject BgParticles_2;

	public GameObject BgParticles_3;

	public PLU_RockSpawner rockSpawner;

	public PLU_MarbleSpawner marbleSpawner;

	private bool difficultyLoaded;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(76, 226, byte.MaxValue, byte.MaxValue);
		GlobalAudio.globalAudio.Play(sting, 1f, 1f, 0f);
	}

	private void Update()
	{
		LoadDifficulty();
	}

	private void LoadDifficulty()
	{
		if (!difficultyLoaded && PLU_Difficulty.PLU_difficulty.loaded)
		{
			difficultyLoaded = true;
			BgParticles_1.SetActive(true);
			BgParticles_2.SetActive(true);
			BgParticles_3.SetActive(true);
			rockSpawner.enabled = true;
			marbleSpawner.enabled = true;
		}
	}
}
