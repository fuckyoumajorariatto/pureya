using UnityEngine;

public class ZIP_Reset : MonoBehaviour
{
	public static ZIP_Reset ZIP_reset;

	public ZIP_PlayerController playerController;

	public GameObject hitParticles;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Awake()
	{
		ZIP_reset = this;
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
		ResetHitParticles();
		DisableByTag("enemy");
		DisableByTag("marble");
		ResetPlayer();
		ZIP_MarbleSpawner.ZIP_marbleSpawner.marbleTimer = ZIP_MarbleSpawner.ZIP_marbleSpawner.marbleFrequency;
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.lineTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.lineTR.localRotation = Quaternion.Euler(0f, 0f, 0f);
		playerController.hookJointTR.position = new Vector3(0f, -2.5f, 0.1f);
		playerController.hookJointTR.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.hookJointRB.velocity = new Vector2(0f, 0f);
		playerController.hookJointRB.angularVelocity = 0f;
		playerController.hookJointTR.localRotation = Quaternion.Euler(0f, 0f, 0f);
		playerController.hookTR.position = new Vector3(0f, -2.5f, 0.1f);
		playerController.hookTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.playerSprite.GetComponent<SpriteRenderer>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		ZIP_TreeSpawner.ZIP_treeSpawner.CreateInitialBatch();
		playerController.coll.enabled = true;
		playerController.hit = false;
	}

	private void ResetHitParticles()
	{
		hitParticles.SetActive(false);
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
