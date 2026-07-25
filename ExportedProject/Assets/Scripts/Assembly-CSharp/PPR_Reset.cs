using UnityEngine;

public class PPR_Reset : MonoBehaviour
{
	public static PPR_Reset PPR_reset;

	public PPR_PlayerController playerController;

	public GameObject hitParticles;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		PPR_reset = this;
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
		playerController.tr.position = new Vector3(-2.75f, 0f, playerController.tr.position.z);
		playerController.playerSpriteTR.localPosition = new Vector3(0f, 0f, playerController.playerSpriteTR.localPosition.z);
		playerController.playerSpriteRB.gravityScale = 0f;
		playerController.playerSpriteRB.velocity = new Vector2(0f, 0f);
		playerController.playerSpriteRB.angularVelocity = 0f;
		playerController.playerSpriteTR.localRotation = Quaternion.Euler(0f, 0f, 0f);
		playerController.marblesCollider.enabled = true;
		playerController.tipCollider.enabled = true;
		playerController.marblesColliderTR.localPosition = new Vector3(0f, 0f, playerController.playerSpriteTR.localPosition.z);
		playerController.marblesColliderRB.velocity = new Vector2(0f, 0f);
		playerController.marblesColliderRB.angularVelocity = 0f;
		playerController.marblesColliderTR.localRotation = Quaternion.Euler(0f, 0f, 0f);
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
