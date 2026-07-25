using System.Collections.Generic;
using UnityEngine;

public class TOP_Spawner : MonoBehaviour
{
	public static TOP_Spawner TOP_spawner;

	public TOP_PlayerController playerController;

	public List<GameObject> prefabs;

	public List<SpriteRenderer> prefabsSR;

	public List<Transform> prefabsTR;

	public GameObject prefab;

	private int pooledAmount = 50;

	public GameObject ballPrefab;

	public List<GameObject> ballPrefabs;

	public List<SpriteRenderer> ballPrefabsSR;

	public List<Transform> ballPrefabsTR;

	private int ballPooledAmount = 6;

	public Sprite[] ballSprites;

	private GameObject obj;

	private Vector3 pos;

	private int lastFloorBlock;

	private float floorCooldown;

	public Color32 color1;

	public Color32 color2;

	private float[] positionsY = new float[4] { -2.125f, -1.375f, -0.625f, 0.125f };

	private int lastObstacleYindex;

	private int noBrick;

	private int noBrickCombo = 5;

	private int sameBrickCombo;

	public int noBallCombo;

	private int noBallsYet = 1;

	private GameObject marble;

	private MarbleController marbleController;

	private Color32 shadowColor = new Color32(69, 69, 69, byte.MaxValue);

	private int noMarbleCombo;

	private void Start()
	{
		TOP_spawner = this;
		noBrick = 5;
		CreatePool();
		CreateInitialFloor();
		CreateBallPool();
	}

	private void LateUpdate()
	{
		ManageFloorCreation();
	}

	private void ManageSpawnBall()
	{
		pos.z = -2f;
		if (playerController.hit || SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		if (noBallsYet > 0)
		{
			noBallsYet--;
		}
		else if (Random.Range(0f, 1f) < TOP_Difficulty.TOP_difficulty.ballChance || noBallCombo > 1)
		{
			int num = GetBallPrefab();
			if (num == -1)
			{
				noBallCombo++;
				return;
			}
			pos.x = prefabsTR[lastFloorBlock].position.x;
			if (noBrickCombo > 0)
			{
				pos.y = -1.95f;
			}
			else
			{
				pos.y = positionsY[lastObstacleYindex] + 1f;
			}
			ballPrefabsTR[num].position = pos;
			ballPrefabs[num].GetComponentInChildren<TOP_BallController>().Initialize();
			ballPrefabs[num].GetComponentInChildren<SpriteRenderer>().sprite = ballSprites[Random.Range(0, 2)];
			ballPrefabs[num].SetActive(true);
			noBallCombo = 0;
		}
		else
		{
			noBallCombo++;
		}
	}

	private void SpawnBrick()
	{
		ManageSpawnBall();
		int num = Random.Range(0, positionsY.Length);
		if (noBrick > 0)
		{
			float num2 = Random.Range(0f, 1f);
			if (noBrick == 1)
			{
				noBrick++;
			}
			else if (num2 < 0.5f && noBrickCombo < 3)
			{
				noBrick++;
			}
			else
			{
				noBrick = 0;
				num = 0;
			}
		}
		else
		{
			float num3 = Random.Range(0f, 1f);
			if (num3 < 0.4f || sameBrickCombo > 1)
			{
				noBrick = 0;
				num = lastObstacleYindex + 1;
			}
			else if (num3 < 0.6f)
			{
				noBrick = 0;
				num = lastObstacleYindex;
			}
			else
			{
				noBrick++;
			}
		}
		if (num >= positionsY.Length || num < 0)
		{
			noBrick++;
		}
		if (noBrick > 0)
		{
			noBrickCombo++;
			lastObstacleYindex = 0;
			ManageSpawnMarble(0);
			return;
		}
		noBrickCombo = 0;
		if (num == lastObstacleYindex)
		{
			sameBrickCombo++;
		}
		else
		{
			sameBrickCombo = 0;
		}
		SpawnBricks(num);
		ManageSpawnMarble(num);
	}

	private void SpawnBricks(int index)
	{
		bool flag = false;
		if (prefabsSR[lastFloorBlock].color == color1)
		{
			flag = true;
		}
		for (int i = 0; i <= index; i++)
		{
			pos.y = positionsY[i];
			pos.x = prefabsTR[lastFloorBlock].position.x - 2.25f;
			int num = GetPrefab();
			if (num == -1)
			{
				return;
			}
			if (flag)
			{
				prefabsSR[num].color = color1;
			}
			else
			{
				prefabsSR[num].color = color2;
			}
			prefabsTR[num].position = pos;
			prefabs[num].SetActive(true);
			flag = !flag;
		}
		lastObstacleYindex = index;
	}

	private void ManageSpawnMarble(int index)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			if (Random.Range(0f, 1f) < TOP_Difficulty.TOP_difficulty.marbleChance || noMarbleCombo > 2)
			{
				noMarbleCombo = 0;
				marble = MarbleCreator.marbleCreator.GetMarble();
				SetMarbleConstants(marble);
				pos.y = positionsY[index] + 0.75f * (float)Random.Range(1, 4);
				pos.x = prefabsTR[lastFloorBlock].position.x - 2.25f;
				pos.z = -4.1f;
				marble.transform.position = pos;
			}
			else
			{
				noMarbleCombo++;
			}
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = TOP_Difficulty.TOP_difficulty.scroll;
		marbleController.direction = MarbleController.Direction.right;
		marbleController.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_right;
		marbleController.maxOut = 9f;
		marble.SetActive(true);
	}

	private void ManageFloorCreation()
	{
		pos.z = -2f;
		if (prefabsTR[lastFloorBlock].position.x > -4.5f && floorCooldown > 0.2f)
		{
			floorCooldown = 0f;
			pos.y = -2.875f;
			pos.x = prefabsTR[lastFloorBlock].position.x - 2.25f;
			int num = GetPrefab();
			if (num == -1)
			{
				return;
			}
			bool flag = true;
			if (prefabsSR[lastFloorBlock].color == color1)
			{
				flag = false;
			}
			if (flag)
			{
				prefabsSR[num].color = color1;
				flag = false;
			}
			else
			{
				prefabsSR[num].color = color2;
				flag = true;
			}
			prefabsTR[num].position = pos;
			prefabs[num].SetActive(true);
			pos.y = -3.625f;
			num = GetPrefab();
			if (num == -1)
			{
				return;
			}
			if (flag)
			{
				prefabsSR[num].color = color1;
				flag = false;
			}
			else
			{
				prefabsSR[num].color = color2;
				flag = true;
			}
			prefabsTR[num].position = pos;
			prefabs[num].SetActive(true);
			lastFloorBlock = num;
			pos.y = -4.375f;
			num = GetPrefab();
			if (num == -1)
			{
				return;
			}
			if (flag)
			{
				prefabsSR[num].color = color1;
				flag = false;
			}
			else
			{
				prefabsSR[num].color = color2;
				flag = true;
			}
			prefabsTR[num].position = pos;
			prefabs[num].SetActive(true);
			lastFloorBlock = num;
			SpawnBrick();
		}
		floorCooldown += Time.deltaTime;
	}

	public void CreateInitialFloor()
	{
		pos.z = -2f;
		pos.y = -2.875f;
		bool flag = true;
		for (float num = 4.5f; num > -5f; num -= 2.25f)
		{
			pos.x = num;
			int num2 = GetPrefab();
			if (num2 == -1)
			{
				break;
			}
			prefabsTR[num2].position = pos;
			if (flag)
			{
				prefabsSR[num2].color = color1;
				flag = false;
			}
			else
			{
				prefabsSR[num2].color = color2;
				flag = true;
			}
			prefabs[num2].SetActive(true);
		}
		pos.y = -3.625f;
		for (float num3 = 4.5f; num3 > -5f; num3 -= 2.25f)
		{
			pos.x = num3;
			int num4 = GetPrefab();
			if (num4 == -1)
			{
				break;
			}
			prefabsTR[num4].position = pos;
			if (flag)
			{
				prefabsSR[num4].color = color1;
				flag = false;
			}
			else
			{
				prefabsSR[num4].color = color2;
				flag = true;
			}
			prefabs[num4].SetActive(true);
		}
		pos.y = -4.375f;
		for (float num5 = 4.5f; num5 > -5f; num5 -= 2.25f)
		{
			pos.x = num5;
			int num6 = GetPrefab();
			if (num6 == -1)
			{
				break;
			}
			prefabsTR[num6].position = pos;
			if (flag)
			{
				prefabsSR[num6].color = color1;
				flag = false;
			}
			else
			{
				prefabsSR[num6].color = color2;
				flag = true;
			}
			prefabs[num6].SetActive(true);
			lastFloorBlock = num6;
		}
		SpawnBrick();
	}

	private void CreatePool()
	{
		prefabs = new List<GameObject>();
		prefabsSR = new List<SpriteRenderer>();
		prefabsTR = new List<Transform>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			prefabs.Add(obj);
			prefabsSR.Add(obj.GetComponent<SpriteRenderer>());
			prefabsTR.Add(obj.GetComponent<Transform>());
		}
	}

	private void CreateBallPool()
	{
		ballPrefabs = new List<GameObject>();
		ballPrefabsSR = new List<SpriteRenderer>();
		ballPrefabsTR = new List<Transform>();
		for (int i = 0; i < ballPooledAmount; i++)
		{
			obj = Object.Instantiate(ballPrefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			ballPrefabs.Add(obj);
			ballPrefabsSR.Add(obj.GetComponent<SpriteRenderer>());
			ballPrefabsTR.Add(obj.GetComponent<Transform>());
		}
	}

	private int GetPrefab()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!prefabs[i].activeInHierarchy)
			{
				return i;
			}
		}
		return -1;
	}

	private int GetBallPrefab()
	{
		for (int i = 0; i < ballPooledAmount; i++)
		{
			if (!ballPrefabs[i].activeInHierarchy)
			{
				return i;
			}
		}
		return -1;
	}

	public void DoReset()
	{
		noBrickCombo = 5;
		noBallCombo = 0;
		sameBrickCombo = 0;
		lastObstacleYindex = 0;
		floorCooldown = 0f;
		noBrick = 5;
		noBallsYet = 1;
		noMarbleCombo = 0;
		for (int i = 0; i < ballPooledAmount; i++)
		{
			ballPrefabs[i].SetActive(false);
		}
		for (int j = 0; j < pooledAmount; j++)
		{
			if (prefabs[j].activeInHierarchy)
			{
				prefabs[j].SetActive(false);
			}
		}
	}
}
