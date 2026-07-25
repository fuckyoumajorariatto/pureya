using UnityEngine;

public class SEA_Reset : MonoBehaviour
{
	public static SEA_Reset SEA_reset;

	public SEA_PlayerController playerController;

	public GameObject hitParticles;

	public bool reset;

	private float timer;

	private void Awake()
	{
		SEA_reset = this;
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
		DisableByTag("marble");
		SEA_BubbleSpawner.SEA_bubbleSpawner.CreateInitialBatch();
		SEA_BubbleSpawner.SEA_bubbleSpawner.SetDifficultyParas();
		SEA_MarbleSpawner.SEA_marbleSpawner.SetDifficultyParas();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(0f, -1.75f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.rb.rotation = 0f;
		playerController.rb.gravityScale = SEA_Difficulty.SEA_difficulty.gravity;
		playerController.coll.enabled = true;
		playerController.hit = false;
		hitParticles.SetActive(false);
	}

	private void DisableByTag(string tag)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			gameObject.SetActive(false);
		}
	}
}
