using System.Collections.Generic;
using UnityEngine;

public class DUC_SpongeSpawner : MonoBehaviour
{
	private List<GameObject> objs;

	private int pooledAmount = 10;

	public GameObject prefab;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private float y;

	private int selectedY;

	private int selectedY2;

	private float x = 5.75f;

	private float[] positionsY;

	public Sprite[] spongeSprites;

	public DUC_PlayerController playerController;

	private Color32 shadowColor = new Color32(122, 122, 122, byte.MaxValue);

	private GameObject marble;

	private MarbleController marbleController;

	private int selectedYmarble;

	private int selectedYmarble2;

	private int noMarbleCombo;

	private void Start()
	{
		positionsY = new float[6];
		y = 1.5f;
		for (int i = 0; i < positionsY.Length; i++)
		{
			positionsY[i] = y;
			y -= 1f;
		}
		timer = DUC_Difficulty.DUC_difficulty.frequency;
		CreatePool();
	}

	private void Update()
	{
		if (timer >= DUC_Difficulty.DUC_difficulty.frequency && !playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	private void CreatePool()
	{
		objs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			objs.Add(obj);
		}
	}

	private void CreateBatch()
	{
		pos.x = x;
		pos.z = 0.09f;
		if (Random.Range(0f, 1f) < 0.35f)
		{
			selectedY = 1;
		}
		else if (Random.Range(0f, 1f) < 0.25f)
		{
			selectedY = positionsY.Length - 1;
		}
		else
		{
			selectedY = Random.Range(0, positionsY.Length);
		}
		pos.y = positionsY[selectedY];
		ActivateObj();
		selectedYmarble = -1;
		selectedYmarble2 = -1;
		if (Random.Range(0f, 1f) < DUC_Difficulty.DUC_difficulty.marbleChance || noMarbleCombo > 4)
		{
			CreateMarble();
			noMarbleCombo = 0;
		}
		else
		{
			noMarbleCombo++;
		}
		if (DUC_Difficulty.DUC_difficulty.maxObjs != 2)
		{
			return;
		}
		selectedY2 = selectedY;
		selectedY = Random.Range(0, positionsY.Length);
		while (selectedY == selectedY2)
		{
			selectedY++;
			if (selectedY > positionsY.Length - 1)
			{
				selectedY = 0;
			}
		}
		pos.y = positionsY[selectedY];
		pos.z = 0.09f;
		ActivateObj();
	}

	private void CreateMarble()
	{
		marble = MarbleCreator.marbleCreator.GetMarble();
		selectedYmarble = Random.Range(0, positionsY.Length);
		while (positionsY[selectedYmarble] == pos.y || selectedYmarble == selectedYmarble2)
		{
			selectedYmarble++;
			if (selectedYmarble > positionsY.Length - 1)
			{
				selectedYmarble = 0;
			}
		}
		pos.z = 1f;
		pos.y = positionsY[selectedYmarble];
		marble.transform.position = pos;
		SetMarbleConstants(marble);
		selectedYmarble2 = selectedYmarble;
	}

	private void ActivateObj()
	{
		obj = GetObj();
		if (obj != null)
		{
			obj.GetComponent<SpriteRenderer>().sprite = spongeSprites[Random.Range(0, spongeSprites.Length)];
			pos.z = obj.transform.position.z;
			obj.transform.position = pos;
			obj.SetActive(true);
		}
	}

	private GameObject GetObj()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!objs[i].activeInHierarchy)
			{
				return objs[i];
			}
		}
		return null;
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = DUC_Difficulty.DUC_difficulty.speed;
		marbleController.direction = MarbleController.Direction.left;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marbleController.maxOut = 6f;
		marble.SetActive(true);
	}

	public void DoReset()
	{
		for (int i = 0; i < objs.Count; i++)
		{
			objs[i].SetActive(false);
		}
		timer = 0f;
	}
}
