using UnityEngine;

public class SWO_Reset : MonoBehaviour
{
	public static SWO_Reset SWO_reset;

	public Transform playerTR;

	public SWO_PlayerController playerController;

	public SWO_ArrowSpawner arrowSpawner;

	private GameObject[] gameObjects;

	public bool reset;

	private float timer;

	private bool resetPlayerNextFrame;

	private void Awake()
	{
		SWO_reset = this;
	}

	private void Update()
	{
		if (resetPlayerNextFrame && !SceneChangeController.sceneChangeController.transitioning)
		{
			resetPlayerNextFrame = false;
			ResetPlayer();
			SWO_HeartController.SWO_heartController.Reset();
		}
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
		DisableByTag("enemy");
		DisableByTag("marble");
		resetPlayerNextFrame = true;
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerController.hit = false;
		playerController.coll.enabled = true;
		arrowSpawner.SetDifficultyParas();
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
