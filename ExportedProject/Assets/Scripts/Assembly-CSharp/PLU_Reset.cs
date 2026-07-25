using UnityEngine;

public class PLU_Reset : MonoBehaviour
{
	public static PLU_Reset PLU_reset;

	public PLU_PlayerController playerController;

	public ParticleSystem hitParticles;

	public Transform hitParticlesTR;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Awake()
	{
		PLU_reset = this;
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
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(0f, 3.25f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.coll.enabled = true;
		playerController.hit = false;
	}

	private void ResetHitParticles()
	{
		hitParticles.Stop();
		hitParticlesTR.position = new Vector3(0f, 3.25f, hitParticlesTR.position.z);
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
