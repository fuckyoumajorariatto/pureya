using System.Collections.Generic;
using UnityEngine;

public class MNK_ScrollController : MonoBehaviour
{
	public GameObject player;

	private float speed;

	private List<GameObject> items;

	private void Update()
	{
		speed = MNK_Difficulty.MNK_difficulty.scroll_speed;
		speed *= Time.deltaTime;
		if (player.GetComponent<MNK_PlayerController>().hit)
		{
			player.transform.Translate(speed, -5f * Time.deltaTime, 0f);
		}
		else
		{
			player.transform.Translate(speed, 0f, 0f);
		}
		items = MNK_VineSpawner.MNK_vineSpawner.items;
		for (int i = 0; i < MNK_VineSpawner.MNK_vineSpawner.pooledAmount; i++)
		{
			items[i].transform.Translate(speed, 0f, 0f);
		}
		items = MNK_Leaf1Spawner.MNK_leaf1Spawner.items;
		for (int j = 0; j < MNK_Leaf1Spawner.MNK_leaf1Spawner.pooledAmount; j++)
		{
			items[j].transform.Translate(speed, 0f, 0f);
		}
		items = MNK_Leaf2Spawner.MNK_leaf2Spawner.items;
		for (int k = 0; k < MNK_Leaf2Spawner.MNK_leaf2Spawner.pooledAmount; k++)
		{
			items[k].transform.Translate(speed, 0f, 0f);
		}
		items = MNK_LightSpawner.MNK_lightSpawner.items;
		for (int l = 0; l < MNK_LightSpawner.MNK_lightSpawner.pooledAmount; l++)
		{
			items[l].transform.Translate(speed * 0.7f, 0f, 0f);
		}
		items = MNK_CoconutSpawner.MNK_coconutSpawner.items;
		for (int m = 0; m < MNK_CoconutSpawner.MNK_coconutSpawner.pooledAmount; m++)
		{
			items[m].transform.Translate(speed, 0f, 0f);
		}
		items = MarbleCreator.marbleCreator.marbles;
		for (int n = 0; n < MarbleCreator.marbleCreator.marbles.Count; n++)
		{
			if (items[n].activeInHierarchy)
			{
				items[n].transform.Translate(speed, 0f, 0f);
			}
		}
	}
}
