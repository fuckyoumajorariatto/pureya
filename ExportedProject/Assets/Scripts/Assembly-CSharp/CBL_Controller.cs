using UnityEngine;

public class CBL_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public AudioClip clip;

	public GameObject player;

	public GameObject goal;

	private bool difficultyLoaded;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(128, 201, 7, byte.MaxValue);
		GlobalAudio.globalAudio.Play(clip, 1f, 1f, 0f);
	}

	private void Update()
	{
		InitializeDifficulty();
	}

	private void InitializeDifficulty()
	{
		if (!difficultyLoaded && CBL_Difficulty.CBL_difficulty.loaded)
		{
			ActivateDifficultyDependents();
		}
	}

	private void ActivateDifficultyDependents()
	{
		difficultyLoaded = true;
		base.gameObject.GetComponent<CBL_FieldObjectsSpawner>().enabled = true;
		player.GetComponent<CBL_PlayerController>().enabled = true;
		goal.GetComponent<CBL_GoalController>().enabled = true;
	}
}
