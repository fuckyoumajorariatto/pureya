using System.Collections.Generic;
using UnityEngine;

public class PNG_IceController : MonoBehaviour
{
	public static PNG_IceController PNG_iceController;

	public List<GameObject> prefabs;

	private float speed;

	private Transform tr;

	private void Start()
	{
		PNG_iceController = this;
		if (PNG_IceSpawner.PNG_iceSpawner != null)
		{
			prefabs = PNG_IceSpawner.PNG_iceSpawner.prefabs;
		}
	}

	private void Update()
	{
		speed = PNG_Difficulty.PNG_difficulty.speed;
		foreach (GameObject prefab in prefabs)
		{
			if (prefab.activeSelf)
			{
				tr = prefab.transform;
				tr.Translate(speed * Time.deltaTime * -1f, 0f, 0f);
				if (tr.position.x < -5f)
				{
					Disable(prefab);
				}
			}
		}
	}

	private void Disable(GameObject go)
	{
		go.SetActive(false);
	}
}
