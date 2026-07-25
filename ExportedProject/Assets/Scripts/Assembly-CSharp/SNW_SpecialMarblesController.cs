using System.Collections.Generic;
using UnityEngine;

public class SNW_SpecialMarblesController : MonoBehaviour
{
	private float timer;

	private List<GameObject> marbles;

	private void Update()
	{
		if (timer > 1f)
		{
			DisableIfOut();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	private void DisableIfOut()
	{
		marbles = MarbleCreator.marbleCreator.marbles;
		for (int i = 0; i < MarbleCreator.marbleCreator.marblePooledAmount; i++)
		{
			if (marbles[i].activeInHierarchy && marbles[i].transform.position.y > 5f)
			{
				marbles[i].GetComponent<MarbleController>().Disable();
			}
		}
	}
}
