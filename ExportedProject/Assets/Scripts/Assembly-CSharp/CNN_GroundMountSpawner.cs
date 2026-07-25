using System.Collections.Generic;
using UnityEngine;

public class CNN_GroundMountSpawner : MonoBehaviour
{
	public Transform groundContainerTR;

	public GameObject mount;

	private Transform mountTR;

	private List<GameObject> mounts;

	private int pooledAmount = 4;

	private float frequency;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private int r;

	private Transform tr;

	private void Start()
	{
		SetDifficultyParas();
		timer = frequency;
		mountTR = mount.transform;
		mounts = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(mount, groundContainerTR);
			obj.SetActive(false);
			mounts.Add(obj);
		}
		CreateInitialMounts();
	}

	private void Update()
	{
		if (timer >= frequency)
		{
			CreateMount();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		frequency = CNN_Difficulty.CNN_difficulty.mount_frequency;
	}

	private void CreateMount()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!mounts[i].activeInHierarchy)
			{
				pos = new Vector3(-7f, -1.2f, mountTR.position.z);
				mounts[i].SetActive(true);
				tr = mounts[i].transform;
				tr.position = pos;
				r = Random.Range(16, 32);
				tr.localScale = new Vector2(r, 1f);
				break;
			}
		}
	}

	private void CreateInitialMounts()
	{
		pos = new Vector3(-3f, -1.2f, mountTR.position.z);
		mounts[0].SetActive(true);
		tr = mounts[0].transform;
		tr.position = pos;
		int num = Random.Range(16, 32);
		tr.localScale = new Vector2(num, 1f);
		pos = new Vector3(3f, -1.2f, mountTR.position.z);
		mounts[1].SetActive(true);
		tr = mounts[1].transform;
		tr.position = pos;
		num = Random.Range(16, 32);
		tr.localScale = new Vector2(num, 1f);
	}
}
