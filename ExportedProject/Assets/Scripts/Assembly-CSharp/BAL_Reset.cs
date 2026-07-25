using UnityEngine;

public class BAL_Reset : MonoBehaviour
{
	public static BAL_Reset BAL_reset;

	public GameObject player;

	public ParticleSystem hitParticles;

	private Transform tr;

	private Rigidbody2D rb;

	private GameObject[] gameObjects;

	private BAL_PlayerController playerController;

	public bool reset;

	private float timer;

	private void Awake()
	{
		BAL_reset = this;
	}

	private void Start()
	{
		tr = player.transform;
		rb = player.GetComponent<Rigidbody2D>();
		playerController = player.GetComponent<BAL_PlayerController>();
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
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			ResetPlayer();
			ResetHitParticles();
			DisableByTag("enemy");
			DisableByTag("marble");
			MarbleController.ResetMarblePitch();
		}
	}

	private void ResetPlayer()
	{
		tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		tr.position = new Vector3(0f, 2.5f, player.transform.position.z);
		rb.velocity = new Vector2(0f, 0f);
		rb.angularVelocity = 0f;
		player.GetComponent<SpriteRenderer>().enabled = true;
		playerController.shadow.GetComponent<SpriteRenderer>().enabled = true;
		player.GetComponent<Collider2D>().enabled = true;
		playerController.hit = false;
		playerController.firstBounce = true;
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
