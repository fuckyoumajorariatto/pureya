using System.Collections.Generic;
using UnityEngine;

public class MarbleCreator : MonoBehaviour
{
	public static MarbleCreator marbleCreator;

	public List<GameObject> marbles;

	private List<Transform> marblesTR;

	private List<SpriteRenderer> marblesSR;

	private List<MarbleController> marblesControllers;

	public GameObject marble;

	public int marblePooledAmount;

	public List<GameObject> marbleParticles;

	public GameObject marbleParticle;

	private int marbleParticlesPooledAmount = 5;

	public List<MarbleParticlesController> marbleParticlesControllers;

	public List<GameObject> marbleNumbers;

	public GameObject marbleNumbersGO;

	private int marbleNumbersPooledAmount = 5;

	public List<MarbleNumbersController> marbleNumbersControllers;

	private int marbleNumbersIndex;

	private Transform thisTR;

	public Sprite[] marbleSkins;

	public Sprite[] numberSprites;

	private GameObject obj;

	private int r;

	private void Start()
	{
		marbleCreator = this;
		marblePooledAmount = 15;
		PoolMarbles();
		PoolMarbleParticles();
		PoolMarbleNumbers();
		thisTR = base.gameObject.transform;
	}

	private void PoolMarbles()
	{
		marbles = new List<GameObject>();
		marblesTR = new List<Transform>();
		marblesControllers = new List<MarbleController>();
		marblesSR = new List<SpriteRenderer>();
		for (int i = 0; i < marblePooledAmount; i++)
		{
			obj = Object.Instantiate(marble);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			marbles.Add(obj);
			marblesTR.Add(obj.transform);
			marblesControllers.Add(obj.GetComponent<MarbleController>());
			marblesSR.Add(obj.GetComponent<SpriteRenderer>());
		}
	}

	private void PoolMarbleParticles()
	{
		marbleParticles = new List<GameObject>();
		for (int i = 0; i < marbleParticlesPooledAmount; i++)
		{
			obj = Object.Instantiate(marbleParticle);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			marbleParticles.Add(obj);
			marbleParticlesControllers.Add(obj.GetComponent<MarbleParticlesController>());
		}
	}

	private void PoolMarbleNumbers()
	{
		marbleNumbers = new List<GameObject>();
		for (int i = 0; i < marbleNumbersPooledAmount; i++)
		{
			obj = Object.Instantiate(marbleNumbersGO);
			obj.transform.parent = base.gameObject.transform;
			marbleNumbers.Add(obj);
			marbleNumbersControllers.Add(obj.GetComponent<MarbleNumbersController>());
		}
	}

	public GameObject GetMarble()
	{
		for (int i = 0; i < marblePooledAmount; i++)
		{
			if (marbles[i] != null)
			{
				if (!marbles[i].activeInHierarchy)
				{
					PickSprite(i);
					marblesControllers[i].maxOut = 5f;
					marblesControllers[i].direction = MarbleController.Direction.none;
					marblesControllers[i].ResetMarble();
					return marbles[i];
				}
				continue;
			}
			Debug.Log("-pry ERROR: Marbles #" + i + " was null, but it was replaced.");
			obj = Object.Instantiate(marble);
			marbles[i] = obj;
			marblesTR[i] = obj.transform;
			marblesControllers[i] = obj.GetComponent<MarbleController>();
			PickSprite(i);
			marblesControllers[i].maxOut = 5f;
			marblesControllers[i].direction = MarbleController.Direction.none;
			marblesControllers[i].ResetMarble();
			return obj;
		}
		obj = Object.Instantiate(marble);
		marblePooledAmount++;
		marbles.Add(obj);
		marblesTR.Add(obj.transform);
		marblesControllers.Add(obj.GetComponent<MarbleController>());
		marblesSR.Add(obj.GetComponent<SpriteRenderer>());
		PickSprite(marbles.Count - 1);
		marblesControllers[marbles.Count - 1].maxOut = 5f;
		marblesControllers[marbles.Count - 1].direction = MarbleController.Direction.none;
		marblesControllers[marbles.Count - 1].ResetMarble();
		return obj;
	}

	public GameObject GetMarbleParticles()
	{
		for (int i = 0; i < marbleParticlesPooledAmount; i++)
		{
			if (!marbleParticles[i].activeInHierarchy)
			{
				marbleParticlesControllers[i].timer = 0f;
				return marbleParticles[i];
			}
		}
		return null;
	}

	public GameObject GetMarbleNumbers()
	{
		marbleNumbersIndex++;
		if (marbleNumbersIndex >= marbleNumbersPooledAmount)
		{
			marbleNumbersIndex = 0;
		}
		marbleNumbersControllers[marbleNumbersIndex].UpdateNumbers();
		return marbleNumbers[marbleNumbersIndex];
	}

	public void PickSprite(int i)
	{
		if (!GlobalControl.globalControl.golden)
		{
			r = Random.Range(0, 10);
		}
		else
		{
			r = Random.Range(0, 11);
		}
		marblesSR[i].sprite = marbleSkins[r];
		if (r == 10)
		{
			marblesControllers[i].golden = true;
		}
		else
		{
			marblesControllers[i].golden = false;
		}
	}

	public void PickSpritePCK(SpriteRenderer mSR, PCK_MarbleController mController, bool golden)
	{
		if (!golden)
		{
			r = Random.Range(0, 10);
		}
		else
		{
			r = 10;
		}
		mSR.sprite = marbleSkins[r];
		if (r == 10)
		{
			mController.golden = true;
		}
		else
		{
			mController.golden = false;
		}
	}

	public void ResetMarbles()
	{
		for (int i = 0; i < marbles.Count; i++)
		{
			obj = marbles[i];
			if (obj != null)
			{
				marblesTR[i].parent = thisTR;
				marblesControllers[i].direction = MarbleController.Direction.none;
				marblesControllers[i].speed = 0f;
				marblesControllers[i].maxOut = 5f;
				marblesControllers[i].golden = false;
				marblesTR[i].localScale = new Vector3(1f, 1f, 1f);
				obj.SetActive(false);
			}
			else
			{
				Debug.LogError("pry- Null marbles on ResetMarbles() on marble #" + i);
			}
		}
		MarbleController.ResetMarblePitch();
	}
}
