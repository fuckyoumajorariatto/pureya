using UnityEngine;

public class CNN_Reset : MonoBehaviour
{
	public static CNN_Reset CNN_reset;

	public Transform playerTR;

	public Rigidbody2D playerRB;

	public CNN_PlayerController playerController;

	public GameObject hitParticles;

	public CNN_BallSpawner ballSpawner;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Awake()
	{
		CNN_reset = this;
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
		ballSpawner.firstBall = true;
		ballSpawner.newBatch = true;
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerTR.position = new Vector3(2f, -2.75f, playerTR.position.z);
		playerRB.velocity = new Vector2(0f, 0f);
		playerRB.angularVelocity = 0f;
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
