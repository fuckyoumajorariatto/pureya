using System.Collections.Generic;
using UnityEngine;

public class PCK_BoardCreator : MonoBehaviour
{
	public static PCK_BoardCreator PCK_boardCreator;

	public GameObject pin;

	public AudioClip changeBoardSound;

	private List<GameObject> pins;

	private int pooledAmount = 50;

	public GameObject pinsContainer;

	public GameObject hole;

	public GameObject boardContainer;

	private int[,] matrix;

	private bool boardCreated;

	private bool changingDesign;

	private void Start()
	{
		PCK_boardCreator = this;
		CreatePool();
	}

	private void Update()
	{
		if (GlobalControl.globalControl != null && !boardCreated)
		{
			boardCreated = true;
			if (SceneChangeController.sceneChangeController.previousScene == "MMU")
			{
				CreateBoard();
			}
			else
			{
				ManageChangeDesign();
			}
		}
	}

	private void CreatePool()
	{
		pins = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject gameObject = Object.Instantiate(pin);
			gameObject.SetActive(false);
			gameObject.transform.parent = base.gameObject.transform;
			pins.Add(gameObject);
		}
	}

	private void CreateBoard()
	{
		switch (GlobalControl.globalControl.pachinkoDesign)
		{
		case 1:
			PlacePinsDesign1();
			break;
		case 2:
			PlacePinsDesign2();
			break;
		case 3:
			PlacePinsDesign3();
			break;
		case 4:
			PlacePinsDesign4();
			break;
		default:
			PlacePinsDesign1();
			break;
		}
		if (changingDesign)
		{
			PlaceHole(true);
		}
		else
		{
			PlaceHole(false);
		}
	}

	public void ChangeBoard()
	{
		changingDesign = true;
		GlobalAudio.globalAudio.Play(changeBoardSound, 1f, 1f, 0f);
		for (int i = 0; i < pins.Count; i++)
		{
			pins[i].GetComponent<Animator>().SetBool("visible", false);
		}
		Invoke("DisableAllPins", 0.5f);
		Invoke("CreateBoard", 0.6f);
	}

	private void DisableAllPins()
	{
		for (int i = 0; i < pins.Count; i++)
		{
			pins[i].SetActive(false);
		}
	}

	private void ManageChangeDesign()
	{
		int num = GlobalControl.globalControl.pachinkoDesign;
		float num2 = Random.Range(0f, 1f);
		if (num <= 1 || !(num2 < 0.5f))
		{
			num2 = Random.Range(0f, 1f);
			if (num2 < 0.99f)
			{
				num = 1;
			}
			else
			{
				num2 = Random.Range(0f, 1f);
				num = ((num2 < 0.5f) ? 2 : ((!(num2 < 0.75f)) ? 4 : 3));
			}
		}
		if (num != GlobalControl.globalControl.pachinkoDesign)
		{
			GlobalControl.globalControl.pachinkoDesign = num;
			ChangeBoard();
		}
		else
		{
			changingDesign = true;
			CreateBoard();
		}
	}

	public void PlaceHole(bool random)
	{
		if (GlobalControl.globalControl.storyLevel == 0)
		{
			GlobalControl.globalControl.pachinkoDesign = 1;
			GlobalControl.globalControl.pachinkoHole = new float[2] { 0f, -3f };
			CreateHole(false);
		}
		else
		{
			CreateHole(random);
		}
	}

	private void CreateHole(bool random)
	{
		int num = 0;
		int num2 = 0;
		float num3 = 0f;
		float num4 = 0f;
		if (random)
		{
			int[] array = new int[0];
			if (GlobalControl.globalControl.pachinkoDesign == 1)
			{
				num = Random.Range(3, 7);
				switch (num)
				{
				case 3:
					array = new int[6] { -5, -3, -1, 1, 3, 5 };
					break;
				case 4:
					array = new int[5] { -4, -2, 0, 2, 4 };
					break;
				case 5:
					array = new int[4] { -3, -1, 1, 3 };
					break;
				case 6:
					array = new int[1];
					break;
				default:
					Debug.Log("ERROR: Hole row is incorrect = " + num);
					array = new int[1];
					break;
				}
				num2 = array[Random.Range(0, array.Length)];
				num -= 3;
				num3 = (float)num2 * 0.5f;
				num4 = (float)num * -1f;
			}
			else if (GlobalControl.globalControl.pachinkoDesign == 2)
			{
				num = Random.Range(3, 6);
				switch (num)
				{
				case 3:
					array = new int[6] { -5, -3, -1, 1, 3, 5 };
					break;
				case 4:
					array = new int[4] { -3, -1, 1, 3 };
					break;
				case 5:
					array = new int[4] { -3, -1, 1, 3 };
					break;
				default:
					Debug.Log("ERROR: Hole row is incorrect = " + num);
					array = new int[1];
					break;
				}
				num2 = array[Random.Range(0, array.Length)];
				num -= 3;
				num3 = (float)num2 * 0.5f;
				num4 = (float)num * -1f;
			}
			else if (GlobalControl.globalControl.pachinkoDesign == 3)
			{
				switch (Random.Range(0, 6))
				{
				case 0:
					num3 = -2.5f;
					num4 = -0.0625f;
					break;
				case 1:
					num3 = 2.5f;
					num4 = -0.0625f;
					break;
				case 2:
					num3 = 0f;
					num4 = -0.25f;
					break;
				case 3:
					num3 = -1.4375f;
					num4 = -2.125f;
					break;
				case 4:
					num3 = 1.4375f;
					num4 = -2.125f;
					break;
				case 5:
					num3 = 0f;
					num4 = -3f;
					break;
				}
			}
			else if (GlobalControl.globalControl.pachinkoDesign == 4)
			{
				switch (Random.Range(0, 5))
				{
				case 0:
					num3 = -2.25f;
					num4 = -1.25f;
					break;
				case 1:
					num3 = 2.25f;
					num4 = -1.25f;
					break;
				case 2:
					num3 = 0f;
					num4 = -1.6875f;
					break;
				case 3:
					num3 = -1f;
					num4 = -2.375f;
					break;
				case 4:
					num3 = 1f;
					num4 = -2.375f;
					break;
				}
			}
			hole.transform.position = new Vector3(num3, num4, hole.transform.position.z);
			GlobalControl.globalControl.pachinkoHole = new float[2] { num3, num4 };
		}
		else
		{
			hole.transform.position = new Vector3(GlobalControl.globalControl.pachinkoHole[0], GlobalControl.globalControl.pachinkoHole[1], hole.transform.position.z);
		}
		if (changingDesign)
		{
			Invoke("DelayedReappearance", 0.75f);
		}
		else
		{
			hole.GetComponent<Animator>().SetBool("change_out", false);
		}
		changingDesign = false;
		GlobalControl.globalControl.SaveData();
	}

	private void DelayedReappearance()
	{
		hole.GetComponent<Animator>().SetBool("change_out", false);
	}

	private void MoveHoleTo(float x, float y)
	{
		hole.transform.position = new Vector3(x, y, hole.transform.position.z);
		hole.GetComponent<Animator>().SetBool("change_out", false);
	}

	private void PlacePinsDesign1()
	{
		matrix = new int[6, 15];
		for (int i = 1; i < matrix.GetLength(0) + 1; i++)
		{
			for (int j = 1; j < matrix.GetLength(1) + 1; j++)
			{
				switch (i)
				{
				case 1:
					if (j % 2 == 0)
					{
						CreatePin(i, j);
					}
					break;
				case 2:
					if (j % 2 != 0)
					{
						CreatePin(i, j);
					}
					break;
				case 3:
					if (j % 2 == 0 && Mathf.Abs(j - 8) <= 7)
					{
						CreatePin(i, j);
					}
					break;
				case 4:
					if (j % 2 != 0 && Mathf.Abs(j - 8) <= 6)
					{
						CreatePin(i, j);
					}
					break;
				case 5:
					if (j % 2 == 0 && Mathf.Abs(j - 8) <= 5)
					{
						CreatePin(i, j);
					}
					break;
				case 6:
					if (j % 2 != 0 && Mathf.Abs(j - 8) <= 2)
					{
						CreatePin(i, j);
					}
					break;
				}
			}
		}
	}

	private void PlacePinsDesign2()
	{
		matrix = new int[6, 15];
		for (int i = 1; i < matrix.GetLength(0) + 1; i++)
		{
			for (int j = 1; j < matrix.GetLength(1) + 1; j++)
			{
				switch (i)
				{
				case 1:
					if (j % 2 == 0)
					{
						CreatePin(i, j);
					}
					break;
				case 2:
					if (j % 2 == 0 && (j < 6 || j > 10))
					{
						CreatePin(i, j);
					}
					break;
				case 3:
					if (j % 2 == 0 && Mathf.Abs(j - 8) <= 7)
					{
						CreatePin(i, j);
					}
					break;
				case 4:
					if (j % 2 == 0 && Mathf.Abs(j - 8) <= 5)
					{
						CreatePin(i, j);
					}
					break;
				case 5:
					if (j % 2 == 0 && Mathf.Abs(j - 8) <= 5)
					{
						CreatePin(i, j);
					}
					break;
				case 6:
					if (j % 2 == 0 && Mathf.Abs(j - 8) <= 2)
					{
						CreatePin(i, j);
					}
					break;
				}
			}
		}
	}

	private void PlacePinsDesign3()
	{
		SpawnPin(0f, 2f);
		SpawnPin(-1f, 2f);
		SpawnPin(-1.3125f, 1.6875f);
		SpawnPin(-1.5f, 1.3125f);
		SpawnPin(-1.5f, 0.875f);
		SpawnPin(1f, 2f);
		SpawnPin(1.3125f, 1.6875f);
		SpawnPin(1.5f, 1.3125f);
		SpawnPin(1.5f, 0.875f);
		SpawnPin(-0.5f, 1f);
		SpawnPin(0.5f, 1f);
		SpawnPin(-2.75f, 0.8125f);
		SpawnPin(-2.9375f, 0.375f);
		SpawnPin(-3f, -0.0625f);
		SpawnPin(-2f, -0.0625f);
		SpawnPin(-2.375f, -1f);
		SpawnPin(2.75f, 0.8125f);
		SpawnPin(2.9375f, 0.375f);
		SpawnPin(3f, -0.0625f);
		SpawnPin(2f, -0.0625f);
		SpawnPin(2.375f, -1f);
		SpawnPin(-0.5f, 0.0625f);
		SpawnPin(-0.75f, -0.5f);
		SpawnPin(0.5f, 0.0625f);
		SpawnPin(0.75f, -0.5f);
		SpawnPin(0f, -1f);
		SpawnPin(-1.9375f, -2.125f);
		SpawnPin(1.9375f, -2.125f);
		SpawnPin(0f, -2f);
		SpawnPin(-0.9375f, -2.125f);
		SpawnPin(-0.6875f, -2.5625f);
		SpawnPin(-0.5f, -3f);
		SpawnPin(0.9375f, -2.125f);
		SpawnPin(0.6875f, -2.5625f);
		SpawnPin(0.5f, -3f);
	}

	private void PlacePinsDesign4()
	{
		SpawnPin(-0.625f, 2f);
		SpawnPin(-0.8125f, 1.8125f);
		SpawnPin(-0.4375f, 1.8125f);
		SpawnPin(0.625f, 2f);
		SpawnPin(0.8125f, 1.8125f);
		SpawnPin(0.4375f, 1.8125f);
		SpawnPin(-2.5f, 1.4375f);
		SpawnPin(-2.6875f, 1.25f);
		SpawnPin(-2.3125f, 1.25f);
		SpawnPin(2.5f, 1.4375f);
		SpawnPin(2.6875f, 1.25f);
		SpawnPin(2.3125f, 1.25f);
		SpawnPin(-2.5f, 0.125f);
		SpawnPin(-2.6875f, -0.0625f);
		SpawnPin(-2.3125f, -0.0625f);
		SpawnPin(-2.125f, -0.25f);
		SpawnPin(-2.875f, -0.25f);
		SpawnPin(2.5f, 0.125f);
		SpawnPin(2.6875f, -0.0625f);
		SpawnPin(2.3125f, -0.0625f);
		SpawnPin(2.125f, -0.25f);
		SpawnPin(2.875f, -0.25f);
		SpawnPin(0f, 0.125f);
		SpawnPin(-0.1875f, -0.0625f);
		SpawnPin(0.1875f, -0.0625f);
		SpawnPin(-0.375f, -0.25f);
		SpawnPin(0.375f, -0.25f);
		SpawnPin(-1.25f, -1.3125f);
		SpawnPin(-1.4375f, -1.5f);
		SpawnPin(-1.0625f, -1.5f);
		SpawnPin(1.25f, -1.3125f);
		SpawnPin(1.4375f, -1.5f);
		SpawnPin(1.0625f, -1.5f);
		SpawnPin(0f, -2.5f);
		SpawnPin(-0.1875f, -2.6875f);
		SpawnPin(0.1875f, -2.6875f);
		SpawnPin(-0.375f, -2.875f);
		SpawnPin(0.375f, -2.875f);
		SpawnPin(-1.25f, -0.3125f);
		SpawnPin(1.25f, -0.3125f);
	}

	private void CreatePin(int row, int column)
	{
		row -= 3;
		column -= 8;
		GameObject gameObject = GetPin();
		gameObject.transform.position = new Vector3((float)column * 0.5f, (float)row * -1f, gameObject.transform.position.z);
		gameObject.transform.parent = pinsContainer.transform;
		gameObject.SetActive(true);
		gameObject.GetComponent<Animator>().SetBool("visible", true);
	}

	private void SpawnPin(float x, float y)
	{
		GameObject gameObject = GetPin();
		gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
		gameObject.transform.parent = pinsContainer.transform;
		gameObject.SetActive(true);
		gameObject.GetComponent<Animator>().SetBool("visible", true);
	}

	private GameObject GetPin()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!pins[i].activeInHierarchy)
			{
				return pins[i];
			}
		}
		Debug.Log("No disabled pins. Returned null on GetPin().");
		return null;
	}
}
