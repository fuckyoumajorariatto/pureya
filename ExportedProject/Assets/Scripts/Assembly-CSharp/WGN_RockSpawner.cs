using System.Collections.Generic;
using UnityEngine;

public class WGN_RockSpawner : MonoBehaviour
{
	public static WGN_RockSpawner WGN_rockSpawner;

	private List<GameObject> rocks;

	private int pooledAmount = 10;

	public GameObject rock;

	private float timer;

	public WGN_PlayerController playerController;

	public AudioClip rockSound;

	private Vector3 pos;

	private GameObject obj;

	private void Start()
	{
		WGN_rockSpawner = this;
		CreatePool();
		timer = WGN_Difficulty.WGN_difficulty.rockFrequency;
	}

	private void Update()
	{
		if (timer >= WGN_Difficulty.WGN_difficulty.rockFrequency && !playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateRockBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	private void CreatePool()
	{
		rocks = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(rock);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			rocks.Add(obj);
		}
	}

	private void CreateRockBatch()
	{
		CreateRock(Random.Range(1, WGN_Difficulty.WGN_difficulty.max + 1));
	}

	private void CreateRock(int n)
	{
		float max = 7f;
		pos.z = -2f;
		switch (n)
		{
		case 1:
			pos.y = Random.Range(5f, max);
			pos.x = Random.Range(-3.5f, 3.5f);
			RemoveEdgeUncertainty();
			ActivateRock();
			break;
		case 2:
			pos.y = Random.Range(5f, max);
			pos.x = Random.Range(-3.5f, 3.5f);
			RemoveEdgeUncertainty();
			ActivateRock();
			pos.y = Random.Range(5f, max);
			if (pos.x <= 1f)
			{
				pos.x = Random.Range(pos.x + 3f, 4f);
			}
			else if (pos.x > -1f)
			{
				pos.x = Random.Range(-4f, pos.x - 3f);
			}
			RemoveEdgeUncertainty();
			ActivateRock();
			break;
		}
	}

	private void RemoveEdgeUncertainty()
	{
		if (pos.x > 2.8f)
		{
			pos.x = 3.5f;
		}
		else if (pos.x < -2.8f)
		{
			pos.x = -3.5f;
		}
	}

	private void ActivateRock()
	{
		obj = GetRock();
		if (obj != null)
		{
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetRock()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!rocks[i].activeInHierarchy)
			{
				return rocks[i];
			}
		}
		return null;
	}

	public void PlayRockSound(Transform rockTR)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(rockSound, Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), rockTR.position.x / 4f);
		}
	}
}
