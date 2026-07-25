using UnityEngine;

public class PLA_Reset : MonoBehaviour
{
	public PLA_PlayerController playerController;

	public PLA_DishController dishController;

	public PLA_Spawner spawner;

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
		playerController.ResetPlayer();
		spawner.DoReset();
		dishController.DoReset();
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
