using UnityEngine;

public class FRO_Reset : MonoBehaviour
{
	public static FRO_Reset FRO_reset;

	public Rigidbody2D playerRB;

	public FRO_PlayerController playerController;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		FRO_reset = this;
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
		DisableByTag("prop");
		DisableByTag("enemy");
		DisableByTag("marble");
		FRO_LilypadSpawner.FRO_lilypadSpawner.CreateInitialPrefabs();
		FRO_LilypadSpawner.FRO_lilypadSpawner.odd = false;
		FRO_LilypadSpawner.FRO_lilypadSpawner.SetDifficultyParas();
		FRO_RockSpawner.FRO_rockSpawner.SetDifficultyParas();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(0f, 3f, playerController.tr.position.z);
		playerRB.velocity = new Vector2(0f, 0f);
		playerRB.angularVelocity = 0f;
		playerRB.rotation = 0f;
		playerController.SR.enabled = true;
		playerController.SR_s.enabled = true;
		playerController.ChangeToSprite0();
		playerController.coll.enabled = true;
		playerController.hit = false;
		FRO_LilypadSpawner.FRO_lilypadSpawner.fistBatch = true;
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
