using System.Collections.Generic;
using UnityEngine;

public class MTO_BulletSpawner : MonoBehaviour
{
	private List<GameObject> bullets;

	private GameObject obj;

	private Rigidbody2D rb;

	private Transform tr;

	private Transform playerTR;

	private int pooledAmount = 4;

	public GameObject bullet;

	public AudioClip clip;

	private float frequency = 0.15f;

	private int bulletSpeed = 4000;

	private float timer;

	private Vector3 pos;

	private Quaternion rot = new Quaternion(0f, 0f, 0f, 0f);

	private void Start()
	{
		playerTR = base.transform;
		CreatePool();
	}

	private void CreatePool()
	{
		bullets = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(bullet);
			obj.SetActive(false);
			bullets.Add(obj);
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= frequency)
		{
			ActivateBullet();
			timer = 0f;
		}
	}

	private void ActivateBullet()
	{
		pos = new Vector3(playerTR.position.x, playerTR.position.y, playerTR.position.z + 1f);
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!bullets[i].activeInHierarchy)
			{
				tr = bullets[i].transform;
				rb = bullets[i].GetComponent<Rigidbody2D>();
				rb.velocity = new Vector2(0f, 0f);
				tr.position = pos;
				tr.rotation = rot;
				bullets[i].SetActive(true);
				rb.AddForce(new Vector2(0f, bulletSpeed));
				PlaySound();
				break;
			}
		}
	}

	private void PlaySound()
	{
		GlobalAudio.globalAudio.Play(clip, Random.Range(0.05f, 0.2f), Random.Range(0.5f, 1.5f), base.transform.position.x / 4f);
	}
}
