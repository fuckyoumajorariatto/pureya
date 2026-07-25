using UnityEngine;

public class WGN_Reset : MonoBehaviour
{
	public WGN_PlayerController playerController;

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
		ResetPlayer();
		DisableByTag("marble");
		DisableRocks();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.DoReset();
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

	private void DisableRocks()
	{
		gameObjects = GameObject.FindGameObjectsWithTag("enemy");
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<WGN_RockController>().DoReset();
		}
	}
}
