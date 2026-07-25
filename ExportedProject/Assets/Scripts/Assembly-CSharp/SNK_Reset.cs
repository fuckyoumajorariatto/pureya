using UnityEngine;

public class SNK_Reset : MonoBehaviour
{
	public SNK_PlayerController playerController;

	public SNK_Spawner spawner;

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
		playerController.DoReset();
		spawner.DoReset();
		DisableByTag("marble");
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
