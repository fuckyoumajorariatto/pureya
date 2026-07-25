using UnityEngine;

public class BAT_Reset : MonoBehaviour
{
	public static BAT_Reset BAT_reset;

	public bool reset;

	private float timer;

	public BAT_StalactitesSpawner ss;

	public Transform playerTransform;

	public Rigidbody2D playerRB;

	public SpriteRenderer playerSR;

	public BAT_PlayerController playerController;

	private GameObject[] gameObjects;

	private void Awake()
	{
		BAT_reset = this;
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
		ss.SpawnInitialBgStalactite();
		ss.SpawnInitialMiniStalactite();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerTransform.position = new Vector3(-2.5f, 0f, playerTransform.position.z);
		playerTransform.localScale = new Vector3(1f, 1f, 1f);
		playerRB.velocity = new Vector2(0f, 0f);
		playerRB.angularVelocity = 0f;
		playerSR.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		playerController.hitParticles.SetActive(false);
		playerController.hit = false;
		playerController.firstInput = false;
		playerController.coll.enabled = true;
		ResetStalactitesSpawner();
	}

	private void ResetStalactitesSpawner()
	{
		ss.timer = GetComponent<BAT_StalactitesSpawner>().frequency;
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
