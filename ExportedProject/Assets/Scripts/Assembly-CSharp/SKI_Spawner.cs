using UnityEngine;

public class SKI_Spawner : MonoBehaviour
{
	public GameObject[] ships;

	public SKI_PlayerController playerController;

	private float timer;

	private Vector3 pos;

	private GameObject obj;

	private bool up;

	private float[] posX = new float[7] { -3.75f, -2.5f, -1.25f, 0f, 1.25f, 2.5f, 3.75f };

	private MarbleController marbleController;

	public Color32 shadowColor;

	private GameObject marble;

	private int marbleCombo;

	private int noMarbleCombo;

	public bool firstShip;

	private bool spawnShip;

	private void Start()
	{
		if (Random.Range(0f, 1f) < 0.5f)
		{
			up = false;
		}
		pos.z = -5.1f;
		firstShip = true;
		spawnShip = true;
	}

	private void Update()
	{
		if (timer >= SKI_Difficulty.SKI_difficulty.frequency && !playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			CreateBatch();
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void DoReset()
	{
		for (int i = 0; i < ships.Length; i++)
		{
			ships[i].SetActive(false);
		}
		firstShip = true;
		timer = SKI_Difficulty.SKI_difficulty.frequency;
	}

	private void CreateBatch()
	{
		if (spawnShip)
		{
			int num = 0;
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i].activeInHierarchy)
				{
					num++;
				}
			}
			if (num < 3)
			{
				spawnShip = false;
				CreateShip();
				if (Random.Range(0f, 1f) < 0.4f && !firstShip)
				{
					SpawnMarble();
				}
			}
			else
			{
				spawnShip = true;
				SpawnMarble();
			}
		}
		else
		{
			spawnShip = true;
			SpawnMarble();
		}
	}

	private void CreateShip()
	{
		noMarbleCombo++;
		marbleCombo = 0;
		obj = GetShip();
		if (!(obj == null))
		{
			if (up)
			{
				pos.y = 5.4f;
				obj.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				up = false;
			}
			else
			{
				pos.y = -5.4f;
				obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				up = true;
			}
			SetPosX();
			obj.transform.position = pos;
			obj.SetActive(true);
			firstShip = false;
		}
	}

	private void SpawnMarble()
	{
		marbleCombo++;
		noMarbleCombo = 0;
		marble = MarbleCreator.marbleCreator.GetMarble();
		marbleController = marble.GetComponent<MarbleController>();
		if (up)
		{
			pos.y = 4.5f;
			up = false;
			marbleController.direction = MarbleController.Direction.down;
		}
		else
		{
			pos.y = -4.5f;
			up = true;
			marbleController.direction = MarbleController.Direction.up;
		}
		SetPosX();
		marble.transform.position = new Vector3(pos.x, pos.y, 0f);
		SetMarbleConstants(marble);
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = Random.Range(2f, 5f) * SKI_Difficulty.SKI_difficulty.boatSpeedModifier;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marbleController.maxOut = 4.6f;
		marble.SetActive(true);
	}

	private void SetPosX()
	{
		int num = Random.Range(0, posX.Length);
		pos.x = posX[num];
		int num2 = 0;
		bool flag = false;
		while (!flag && num2 < 100)
		{
			flag = true;
			num2++;
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i].activeInHierarchy && Mathf.Abs(pos.x - ships[i].transform.position.x) < 0.1f)
				{
					flag = false;
					num++;
					if (num >= posX.Length)
					{
						num = 0;
					}
					pos.x = posX[num];
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			if (flag)
			{
				for (int j = 0; j < ships.Length; j++)
				{
					if (ships[j].activeInHierarchy)
					{
						if (ships[j].transform.position.x == pos.x - 1.25f)
						{
							flag2 = true;
						}
						else if (ships[j].transform.position.x == pos.x + 1.25f)
						{
							flag3 = true;
						}
					}
				}
				if (flag2 && flag3)
				{
					flag = false;
				}
			}
			if (flag && firstShip && posX[num] <= 1.25f)
			{
				flag = false;
				num++;
				if (num >= posX.Length)
				{
					num = 0;
				}
				pos.x = posX[num];
			}
		}
		if (num2 >= 100)
		{
			pos.x = 5f;
		}
	}

	private GameObject GetShip()
	{
		int num = Random.Range(0, ships.Length);
		if (!ships[num].activeInHierarchy)
		{
			return ships[num];
		}
		for (int i = 0; i < ships.Length; i++)
		{
			if (!ships[i].activeInHierarchy)
			{
				return ships[i];
			}
		}
		return null;
	}
}
