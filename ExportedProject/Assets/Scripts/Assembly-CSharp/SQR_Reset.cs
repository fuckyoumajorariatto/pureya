using UnityEngine;

public class SQR_Reset : MonoBehaviour
{
	public static SQR_Reset SQR_reset;

	public SQR_PlayerController playerController;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		SQR_reset = this;
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
		DisableByTag("prop");
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(0f, 3f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.rb.gravityScale = 0f;
		playerController.sr.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		playerController.hitParticles.SetActive(false);
		playerController.animator.SetBool("left", true);
		playerController.ChangeToSprite0();
		playerController.coll.enabled = true;
		playerController.hit = false;
	}

	private void DisableByTag(string tag)
	{
		gameObjects = GameObject.FindGameObjectsWithTag(tag);
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			if (CompareTag("enemy"))
			{
				gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
				Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
				component.gravityScale = 0f;
				component.velocity = new Vector2(0f, 0f);
				component.angularVelocity = 0f;
			}
			gameObject.SetActive(false);
		}
	}
}
