using UnityEngine;

public class FTL_Reset : MonoBehaviour
{
	public static FTL_Reset FTL_reset;

	public Transform playerTR;

	public FTL_PlayerController playerController;

	public Rigidbody2D playerRB;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		FTL_reset = this;
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
		DisableByTag("enemy");
		DisableByTag("marble");
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		FTL_WindowSpawner.FTL_windowSpawner.firstBatch = true;
		playerTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerTR.position = new Vector3(-2.9375f, -1.875f, playerTR.position.z);
		playerRB.velocity = new Vector2(0f, 0f);
		playerRB.angularVelocity = 0f;
		playerController.hitParticles.SetActive(false);
		playerController.ResetLadders();
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
