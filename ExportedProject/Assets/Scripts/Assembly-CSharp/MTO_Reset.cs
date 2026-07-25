using UnityEngine;

public class MTO_Reset : MonoBehaviour
{
	public static MTO_Reset MTO_reset;

	public Transform playerTR;

	public Rigidbody2D playerRB;

	public MTO_BulletSpawner MTO_bulletSpawner;

	public MTO_PlayerController MTO_playerController;

	public SpriteRenderer playerSR;

	public GameObject hitParticles;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Awake()
	{
		MTO_reset = this;
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
		DisableByTag("bullet");
		DisableByTag("enemy");
		DisableByTag("marble");
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerTR.position = new Vector3(0f, -3.25f, playerTR.position.z);
		playerRB.velocity = new Vector2(0f, 0f);
		playerRB.angularVelocity = 0f;
		MTO_bulletSpawner.enabled = true;
		playerSR.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		MTO_playerController.hit = false;
		MTO_playerController.coll.enabled = true;
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
