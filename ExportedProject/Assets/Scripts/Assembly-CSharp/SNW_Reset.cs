using UnityEngine;

public class SNW_Reset : MonoBehaviour
{
	public static SNW_Reset SNW_reset;

	public SNW_PlayerController playerController;

	public Rigidbody2D playerSpriteRB;

	public GameObject trailParticles;

	public GameObject hitParticles;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		SNW_reset = this;
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
		playerController.tr.position = new Vector3(0f, 2.75f, playerController.tr.position.z);
		playerController.playerSpriteTR.localPosition = new Vector3(0f, 0f, -2f);
		playerSpriteRB.velocity = new Vector2(0f, 0f);
		playerSpriteRB.angularVelocity = 0f;
		playerController.playerSpriteSR.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		playerController.coll.enabled = true;
		playerController.hit = false;
		trailParticles.SetActive(true);
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
