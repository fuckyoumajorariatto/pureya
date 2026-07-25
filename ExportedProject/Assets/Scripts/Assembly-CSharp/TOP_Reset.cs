using UnityEngine;

public class TOP_Reset : MonoBehaviour
{
	public TOP_PlayerController playerController;

	public TOP_Spawner spawner;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

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
		playerController.DoReset();
		spawner.DoReset();
		DisableByTag("marble");
		spawner.CreateInitialFloor();
		MarbleController.ResetMarblePitch();
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
