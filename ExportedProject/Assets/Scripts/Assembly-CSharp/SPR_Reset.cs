using UnityEngine;

public class SPR_Reset : MonoBehaviour
{
	public static SPR_Reset SPR_reset;

	public SPR_PlayerController playerController;

	public ParticleSystem hitParticles;

	public bool reset;

	private float timer;

	public SPR_NailSpawner nailSpawner;

	public SPR_MarbleSpawner marbleSpawner;

	private GameObject[] gameObjects;

	private void Awake()
	{
		SPR_reset = this;
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
		ResetHitParticles();
		DisableByTag("enemy");
		DisableByTag("marble");
		nailSpawner.enabled = true;
		marbleSpawner.enabled = true;
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(0f, 3f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.playerSprite.SetActive(true);
		playerController.thread.SetActive(true);
		playerController.coll.enabled = true;
		playerController.hit = false;
	}

	private void ResetHitParticles()
	{
		hitParticles.Stop();
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
