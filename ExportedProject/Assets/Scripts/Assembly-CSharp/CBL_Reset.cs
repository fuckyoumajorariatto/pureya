using UnityEngine;

public class CBL_Reset : MonoBehaviour
{
	public static CBL_Reset CBL_reset;

	public Transform playerTR;

	public CBL_PlayerController playerController;

	public GameObject hitParticles;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private void Awake()
	{
		CBL_reset = this;
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
		DisableByTag("ball");
		DisableByTag("enemy");
		DisableByTag("marble");
		ResetPlayer();
		ResetHitParticles();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerTR.position = new Vector3(0f, -3.125f, playerTR.position.z);
		playerController.playerSpriteTR.localPosition = new Vector3(0f, 0f, 0f);
		playerController.playerSpriteTR.localRotation = Quaternion.identity;
		playerController.playerSprite.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		playerController.currentSpeed = 0f;
		playerController.playerSpriteSR.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		playerController.coll1.enabled = true;
		playerController.coll2.enabled = true;
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
