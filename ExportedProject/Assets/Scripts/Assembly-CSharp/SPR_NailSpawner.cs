using System.Collections.Generic;
using UnityEngine;

public class SPR_NailSpawner : MonoBehaviour
{
	public static SPR_NailSpawner SPR_nailSpawner;

	private List<GameObject> straightNails;

	private int straightNailsPooledAmount = 6;

	public GameObject straightNail;

	private List<GameObject> bentUpLeftNails;

	private int bentUpLeftNailsPooledAmount = 6;

	public GameObject bentUpLeftNail;

	private List<GameObject> bentDownLeftNails;

	private int bentDownLeftNailsPooledAmount = 6;

	public GameObject bentDownLeftNail;

	private List<GameObject> bentUpRightNails;

	private int bentUpRightNailsPooledAmount = 6;

	public GameObject bentUpRightNail;

	private List<GameObject> bentDownRightNails;

	private int bentDownRightNailsPooledAmount = 6;

	public GameObject bentDownRightNail;

	private float frequency;

	private float timer;

	private bool bentNails = true;

	private bool doubleNails = true;

	private Vector3 pos;

	private GameObject obj;

	private Transform objTR;

	private bool left;

	private bool bentUp;

	private int count;

	private void Start()
	{
		SPR_nailSpawner = this;
		SetDifficultyParas();
		CreatePools();
	}

	private void Update()
	{
		count++;
		if (timer >= frequency && !SceneChangeController.sceneChangeController.transitioning)
		{
			SpawnNail();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		timer = 0f;
		frequency = SPR_Difficulty.SPR_difficulty.nailsFrequency;
		bentNails = SPR_Difficulty.SPR_difficulty.bentNails;
		doubleNails = SPR_Difficulty.SPR_difficulty.doubleNails;
	}

	private void SpawnNail()
	{
		pos = new Vector3(0f, -5f, 0f);
		if (!bentNails || Random.Range(0f, 1f) < 0.7f)
		{
			if (!doubleNails || Random.Range(0f, 1f) < 0.6f)
			{
				obj = GetStraightNail();
				if (!(obj == null))
				{
					objTR = obj.transform;
					pos.z = objTR.position.z;
					pos.x = Random.Range(1.5f, 3f);
					if (Random.Range(0f, 1f) < 0.5f)
					{
						objTR.localScale = new Vector3(1f, 1f, 1f);
						pos.x *= -1f;
						objTR.position = pos;
					}
					else
					{
						objTR.localScale = new Vector3(-1f, 1f, 1f);
						objTR.position = pos;
					}
					obj.SetActive(true);
				}
				return;
			}
			obj = GetStraightNail();
			if (obj == null)
			{
				return;
			}
			objTR = obj.transform;
			pos.z = objTR.position.z;
			pos.x = Random.Range(1.5f, 5f);
			objTR.localScale = new Vector3(1f, 1f, 1f);
			pos.x *= -1f;
			objTR.position = pos;
			obj.SetActive(true);
			obj = GetStraightNail();
			if (!(obj == null))
			{
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.x += Random.Range(7f, 8f);
				if (pos.x > 5f)
				{
					pos.x = 5f;
				}
				objTR.localScale = new Vector3(-1f, 1f, 1f);
				objTR.position = pos;
				obj.SetActive(true);
			}
			return;
		}
		if (!doubleNails || Random.Range(0f, 1f) < 0.6f)
		{
			switch (Random.Range(0, 2))
			{
			case 0:
				obj = GetBentDownLeftNail();
				bentUp = false;
				break;
			case 1:
				obj = GetBentUpLeftNail();
				bentUp = true;
				break;
			}
			if (obj == null)
			{
				return;
			}
			objTR = obj.transform;
			pos.z = objTR.position.z;
			pos.x = Random.Range(1.5f, 5f);
			pos.x *= -1f;
			objTR.position = pos;
			obj.SetActive(true);
			switch (Random.Range(0, 2))
			{
			case 0:
				obj = GetBentDownRightNail();
				break;
			case 1:
				obj = GetBentUpRightNail();
				break;
			}
			if (!(obj == null))
			{
				objTR = obj.transform;
				pos.z = objTR.position.z;
				pos.x += Random.Range(7f, 8f);
				if (pos.x > 5f)
				{
					pos.x = 5f;
				}
				objTR.position = pos;
				obj.SetActive(true);
			}
			return;
		}
		left = false;
		bentUp = false;
		switch (Random.Range(0, 4))
		{
		case 0:
			obj = GetBentDownLeftNail();
			left = true;
			bentUp = false;
			break;
		case 1:
			obj = GetBentUpLeftNail();
			left = true;
			bentUp = true;
			break;
		case 2:
			obj = GetBentDownRightNail();
			left = false;
			bentUp = false;
			break;
		case 3:
			obj = GetBentUpRightNail();
			left = false;
			bentUp = true;
			break;
		default:
			obj = GetBentDownLeftNail();
			left = true;
			bentUp = false;
			break;
		}
		if (!(obj == null))
		{
			objTR = obj.transform;
			pos.z = objTR.position.z;
			pos.x = Random.Range(1.5f, 3f);
			if (bentUp)
			{
				pos.x = Random.Range(2f, 3f);
			}
			if (left)
			{
				pos.x *= -1f;
				objTR.position = pos;
			}
			else
			{
				objTR.position = pos;
			}
			obj.SetActive(true);
		}
	}

	private void CreatePools()
	{
		straightNails = new List<GameObject>();
		for (int i = 0; i < straightNailsPooledAmount; i++)
		{
			obj = Object.Instantiate(straightNail);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			straightNails.Add(obj);
		}
		bentDownLeftNails = new List<GameObject>();
		for (int j = 0; j < bentDownLeftNailsPooledAmount; j++)
		{
			obj = Object.Instantiate(bentDownLeftNail);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			bentDownLeftNails.Add(obj);
		}
		bentUpLeftNails = new List<GameObject>();
		for (int k = 0; k < bentUpLeftNailsPooledAmount; k++)
		{
			obj = Object.Instantiate(bentUpLeftNail);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			bentUpLeftNails.Add(obj);
		}
		bentDownRightNails = new List<GameObject>();
		for (int l = 0; l < bentDownRightNailsPooledAmount; l++)
		{
			obj = Object.Instantiate(bentDownRightNail);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			bentDownRightNails.Add(obj);
		}
		bentUpRightNails = new List<GameObject>();
		for (int m = 0; m < bentUpRightNailsPooledAmount; m++)
		{
			obj = Object.Instantiate(bentUpRightNail);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			bentUpRightNails.Add(obj);
		}
	}

	private GameObject GetStraightNail()
	{
		for (int i = 0; i < straightNailsPooledAmount; i++)
		{
			if (!straightNails[i].activeInHierarchy)
			{
				return straightNails[i];
			}
		}
		return null;
	}

	private GameObject GetBentDownLeftNail()
	{
		for (int i = 0; i < bentDownLeftNailsPooledAmount; i++)
		{
			if (!bentDownLeftNails[i].activeInHierarchy)
			{
				return bentDownLeftNails[i];
			}
		}
		return null;
	}

	private GameObject GetBentUpLeftNail()
	{
		for (int i = 0; i < bentUpLeftNailsPooledAmount; i++)
		{
			if (!bentUpLeftNails[i].activeInHierarchy)
			{
				return bentUpLeftNails[i];
			}
		}
		return null;
	}

	private GameObject GetBentDownRightNail()
	{
		for (int i = 0; i < bentDownRightNailsPooledAmount; i++)
		{
			if (!bentDownRightNails[i].activeInHierarchy)
			{
				return bentDownRightNails[i];
			}
		}
		return null;
	}

	private GameObject GetBentUpRightNail()
	{
		for (int i = 0; i < bentUpRightNailsPooledAmount; i++)
		{
			if (!bentUpRightNails[i].activeInHierarchy)
			{
				return bentUpRightNails[i];
			}
		}
		return null;
	}
}
