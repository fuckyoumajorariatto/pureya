using UnityEngine;

public class PNG_Reset : MonoBehaviour
{
	public static PNG_Reset PNG_reset;

	public PNG_PlayerController playerController;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		PNG_reset = this;
	}

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
		DisableByTag("wall");
		DisableByTag("prop");
		DisableByTag("marble");
		PNG_IceSpawner.PNG_iceSpawner.CreateInitialBatch();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.position = new Vector3(-2.5f, -2.686f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.rb.rotation = 0f;
		playerController.coll.enabled = true;
		playerController.hit = false;
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
