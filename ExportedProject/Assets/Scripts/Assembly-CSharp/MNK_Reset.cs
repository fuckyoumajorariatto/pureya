using System.Collections.Generic;
using UnityEngine;

public class MNK_Reset : MonoBehaviour
{
	public static MNK_Reset MNK_reset;

	public Transform playerTR;

	public MNK_PlayerController playerController;

	public Rigidbody2D playerRB;

	public bool reset;

	private float timer;

	private void Awake()
	{
		MNK_reset = this;
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
		DisableCoconuts();
		DisableByTag("wall");
		DisableByTag("marble");
		GetComponent<MNK_CoconutSpawner>().CreateInitialBatch();
		MNK_Leaf1Spawner.MNK_leaf1Spawner.CreateInitialBatch();
		MNK_Leaf2Spawner.MNK_leaf2Spawner.CreateInitialBatch();
		MNK_LightSpawner.MNK_lightSpawner.CreateInitialBatch();
		MNK_VineSpawner.MNK_vineSpawner.CreateInitialBatch();
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		playerTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		playerTR.position = new Vector3(-2.5f, -2.75f, playerTR.position.z);
		playerTR.localScale = new Vector3(1f, 1f, 1f);
		playerRB.velocity = new Vector2(0f, 0f);
		playerRB.angularVelocity = 0f;
		playerController.sr.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		playerController.hitParticles.SetActive(false);
		playerController.ResetSprite();
		playerController.inputCooldown = 0.125f;
		playerController.coll.enabled = true;
		playerController.hit = false;
	}

	private void DisableByTag(string tag)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			gameObject.SetActive(false);
		}
	}

	private void DisableCoconuts()
	{
		List<GameObject> items = MNK_CoconutSpawner.MNK_coconutSpawner.items;
		for (int i = 0; i < MNK_CoconutSpawner.MNK_coconutSpawner.pooledAmount; i++)
		{
			items[i].GetComponent<MNK_CoconutController>().Disable();
		}
	}
}
