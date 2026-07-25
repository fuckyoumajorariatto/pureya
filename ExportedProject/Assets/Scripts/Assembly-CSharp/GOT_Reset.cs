using UnityEngine;

public class GOT_Reset : MonoBehaviour
{
	public static GOT_Reset GOT_reset;

	public GOT_PlayerController playerController;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Awake()
	{
		GOT_reset = this;
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
		DisableByTag("marble");
		GOT_RockSpawner.GOT_rockSpawner.CreateInitialRocks();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(0f, -2f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.initialCooldown = 0f;
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
