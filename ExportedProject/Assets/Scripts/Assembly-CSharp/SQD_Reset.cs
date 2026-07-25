using UnityEngine;

public class SQD_Reset : MonoBehaviour
{
	public static SQD_Reset SQD_reset;

	public SQD_PlayerController playerController;

	public bool reset;

	private float timer;

	private GameObject[] gameObjects;

	private void Awake()
	{
		SQD_reset = this;
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
		ResetUrchinSpawner();
		DisableByTag("enemy");
		DisableByTag("marble");
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerController.tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.tr.position = new Vector3(-2f, 0f, playerController.tr.position.z);
		playerController.rb.velocity = new Vector2(0f, 0f);
		playerController.rb.angularVelocity = 0f;
		playerController.sr.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		playerController.bubbles.SetActive(true);
		playerController.bubblesHit.SetActive(false);
		playerController.animator.SetBool("dead", false);
		playerController.coll.enabled = true;
		playerController.hit = false;
	}

	private void ResetUrchinSpawner()
	{
		GetComponent<SQD_UrchinSpawner>().timer = GetComponent<SQD_UrchinSpawner>().frequency;
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
