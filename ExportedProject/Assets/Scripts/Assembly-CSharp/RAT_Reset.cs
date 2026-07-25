using UnityEngine;

public class RAT_Reset : MonoBehaviour
{
	public RAT_PlayerController playerController;

	public RAT_Spawner spawner;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Update()
	{
		if (reset)
		{
			timer += Time.deltaTime;
			if (timer >= GlobalControl.globalControl.minigamesResetTime && !SceneChangeController.sceneChangeController.transitioning)
			{
				DoReset();
			}
		}
	}

	private void DoReset()
	{
		reset = false;
		timer = 0f;
		DisableByTag("wall");
		DisableByTag("marble");
		ResetPlayer();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		spawner.InitialSpawn();
		playerController.ResetPlayer();
	}

	private void DisableByTag(string tag)
	{
		gameObjects = GameObject.FindGameObjectsWithTag(tag);
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
	}
}
