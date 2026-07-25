using System.Collections.Generic;
using UnityEngine;

public class PCK_MarbleCreator : MonoBehaviour
{
	public static PCK_MarbleCreator PCK_marbleCreator;

	public List<GameObject> marbles;

	public List<SpriteRenderer> marblesSR;

	public List<PCK_MarbleController> marblesControllers;

	public GameObject marble;

	private int marblePooledAmount = 30;

	public List<GameObject> marbleParticles;

	public GameObject marbleParticle;

	private int marbleParticlesPooledAmount = 4;

	private GameObject obj;

	public Transform boardContainer;

	private void Start()
	{
		PCK_marbleCreator = this;
		PoolMarbles();
		PoolMarbleParticles();
	}

	private void PoolMarbles()
	{
		marbles = new List<GameObject>();
		marblesSR = new List<SpriteRenderer>();
		marblesControllers = new List<PCK_MarbleController>();
		for (int i = 0; i < marblePooledAmount; i++)
		{
			obj = Object.Instantiate(marble);
			obj.SetActive(false);
			obj.transform.parent = boardContainer;
			marbles.Add(obj);
			marblesSR.Add(obj.GetComponent<SpriteRenderer>());
			marblesControllers.Add(obj.GetComponent<PCK_MarbleController>());
		}
	}

	private void PoolMarbleParticles()
	{
		marbleParticles = new List<GameObject>();
		for (int i = 0; i < marbleParticlesPooledAmount; i++)
		{
			GameObject gameObject = Object.Instantiate(marbleParticle);
			gameObject.SetActive(false);
			gameObject.transform.parent = boardContainer;
			marbleParticles.Add(gameObject);
		}
	}

	public GameObject GetMarble(bool golden)
	{
		for (int i = 0; i < marblePooledAmount; i++)
		{
			if (!marbles[i].activeInHierarchy)
			{
				MarbleCreator.marbleCreator.PickSpritePCK(marblesSR[i], marblesControllers[i], golden);
				return marbles[i];
			}
		}
		return null;
	}

	public GameObject GetMarbleParticles()
	{
		for (int i = 0; i < marbleParticlesPooledAmount; i++)
		{
			if (!marbleParticles[i].activeInHierarchy)
			{
				marbleParticles[i].GetComponent<MarbleParticlesController>().timer = 0f;
				marbleParticles[i].transform.localScale = new Vector3(1f, 1f, 1f);
				return marbleParticles[i];
			}
		}
		return null;
	}

	public bool AreThereMarblesLeft()
	{
		for (int i = 0; i < marblePooledAmount; i++)
		{
			if (!marbles[i].activeInHierarchy)
			{
				return true;
			}
		}
		return false;
	}
}
