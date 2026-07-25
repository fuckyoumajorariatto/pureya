using System.Collections.Generic;
using UnityEngine;

public class PNG_IceSpawner : MonoBehaviour
{
	public static PNG_IceSpawner PNG_iceSpawner;

	public Sprite[] iceSprites;

	public List<GameObject> prefabs;

	public List<SpriteRenderer> prefabsSR;

	public List<Transform> prefabsTR;

	private int pooledAmount = 100;

	public GameObject prefab;

	private float frequency;

	private float timer;

	private float marbleTimer;

	private Vector3 pos = new Vector3(0f, 5f, -1f);

	private float[] vPos;

	private float[] hPos;

	private int lastBlock;

	private int n;

	private List<string[]> map = new List<string[]>();

	private bool iceOnTop;

	private string s;

	private GameObject obj;

	private GameObject marble;

	private int r;

	private int c;

	private float marbleChance;

	private bool firstMarble;

	private void Start()
	{
		PNG_iceSpawner = this;
		SetDifficultyParas();
		CreatePool();
		InitializatePosArrays();
		CreateInitialBatch();
	}

	private void Update()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			if (prefabsTR[lastBlock].position.x <= 4.5f && !PNG_PlayerController.PNG_playerController.hit)
			{
				CreateBatch();
				timer = 0f;
			}
			timer += Time.deltaTime;
			marbleTimer += Time.deltaTime;
		}
	}

	public void SetDifficultyParas()
	{
		frequency = 10f;
		timer = 0f;
		marbleTimer = frequency / 2f;
	}

	private void InitializatePosArrays()
	{
		vPos = new float[9];
		hPos = new float[20];
		c = 0;
		for (float num = 2.25f; num > -4f; num -= 0.75f)
		{
			vPos[c] = num;
			c++;
		}
		c = 0;
		for (float num2 = 4.5f; num2 < 19f; num2 += 0.75f)
		{
			hPos[c] = num2;
			c++;
		}
	}

	private void CreatePool()
	{
		prefabs = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(prefab);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			prefabs.Add(obj);
			prefabsSR.Add(obj.GetComponent<SpriteRenderer>());
			prefabsTR.Add(obj.GetComponent<Transform>());
		}
		if (PNG_IceController.PNG_iceController != null)
		{
			PNG_IceController.PNG_iceController.prefabs = prefabs;
		}
	}

	public void CreateInitialBatch()
	{
		pos.z = 0f;
		pos.y = -3.75f;
		for (float num = -3.75f; num < 6f; num += 0.75f)
		{
			pos.x = num;
			ActivatePrefab(true);
		}
		firstMarble = true;
	}

	private void CreateMarble()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			marble = PNG_MarbleSpawner.PNG_marbleSpawner.CreateMarble();
			marble.transform.position = pos;
		}
	}

	private void ActivatePrefab(bool top)
	{
		n = GetPrefab();
		if (n >= 0)
		{
			prefabsTR[n].position = pos;
			prefabs[n].SetActive(true);
		}
		lastBlock = n;
		if (top)
		{
			prefabsSR[n].sprite = iceSprites[1];
			prefabsTR[n].tag = "wall";
		}
		else
		{
			prefabsSR[n].sprite = iceSprites[0];
			prefabsTR[n].tag = "prop";
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

	public void DisableAllPrefabs()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			if (prefabs[i].activeInHierarchy)
			{
				prefabs[i].SetActive(false);
			}
		}
	}

	private void CreateBatch()
	{
		pos.z = 0f;
		r = Random.Range(PNG_Difficulty.PNG_difficulty.minTemplate, PNG_Difficulty.PNG_difficulty.maxTemplate + 1);
		switch (r)
		{
		case 1:
			SetMap1();
			break;
		case 2:
			SetMap2();
			break;
		case 3:
			SetMap3();
			break;
		case 4:
			SetMap4();
			break;
		case 5:
			SetMap5();
			break;
		case 6:
			SetMap6();
			break;
		case 7:
			SetMap7();
			break;
		case 8:
			SetMap8();
			break;
		case 9:
			SetMap9();
			break;
		case 10:
			SetMap10();
			break;
		case 11:
			SetMap11();
			break;
		case 12:
			SetMap12();
			break;
		case 13:
			SetMap13();
			break;
		}
		CreateMap();
	}

	private void CreateMap()
	{
		float num = 4.5f - prefabsTR[lastBlock].position.x;
		for (int i = 0; i < map[0].Length; i++)
		{
			iceOnTop = false;
			for (int j = 0; j < 9; j++)
			{
				pos.x = hPos[i] - num;
				pos.y = vPos[j];
				s = map[j][i];
				if (s == "o")
				{
					ActivatePrefab(!iceOnTop);
					iceOnTop = true;
				}
				else if (s == "x")
				{
					iceOnTop = false;
					if (Random.Range(0f, 1f) < marbleChance || firstMarble)
					{
						firstMarble = false;
						marbleChance = 0.5f;
						CreateMarble();
					}
					else
					{
						marbleChance += 0.1f;
					}
				}
				else
				{
					iceOnTop = false;
				}
			}
		}
	}

	private void SetMap1()
	{
		map = new List<string[]>();
		map.Add(new string[11]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", "x",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "o", "o",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", "o", "o", "o", " ", " ", " ", " ", " ",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", "o", "o", "o", " ", " ", " ", " ", " ",
			" "
		});
		map.Add(new string[11]
		{
			" ", " ", "o", "o", "o", " ", " ", " ", " ", " ",
			" "
		});
		map.Add(new string[11]
		{
			"o", "o", "o", "o", "o", " ", "o", "o", "o", "o",
			"o"
		});
	}

	private void SetMap2()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", "x",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", "o", "o",
			" ", " ", " ", " ", "x", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", "o", " ", " ", " ", " ", " ",
			" ", " ", " ", "o", "o", "o", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", "x",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
			"o", "o", "o", "o", "o", "o", "o", "o"
		});
	}

	private void SetMap3()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", "x", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", "o", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", "o", "o", " ",
			" ", " ", " ", " ", "o", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", "x", " ", " ", "o", "o", "x",
			" ", " ", " ", "x", "o", "x", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", "o", "o", "o", " ", "o", "o", "o",
			"o", " ", "o", "o", "o", "o", "o", "o"
		});
	}

	private void SetMap4()
	{
		map = new List<string[]>();
		map.Add(new string[10] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", " ", "x", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", "o", "o", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
		map.Add(new string[10] { " ", " ", " ", " ", " ", "x", " ", " ", " ", " " });
		map.Add(new string[10] { "o", "o", "o", "o", "o", "o", " ", " ", "o", "o" });
	}

	private void SetMap5()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", " ",
			" ", " ", " ", " ", " ", " ", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", " ", " ",
			" ", " ", "x", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", "x", " ", " ", " ", " ",
			" ", " ", "o", "o", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", "x", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
			"o", "o", " ", " ", " ", " ", "o", "o"
		});
	}

	private void SetMap6()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", " ",
			" ", " ", "x", " ", " ", " ", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", "x", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", "o", "o", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", "o",
			"o", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", "o",
			"o", " ", " ", " ", " ", "x", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", "o",
			"o", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "o", "o", " ", " ", "o", "o", "o",
			"o", " ", " ", " ", " ", "x", "x", " "
		});
		map.Add(new string[18]
		{
			" ", "o", "o", "o", "o", " ", " ", "o", "o", "o",
			"o", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", "o", "o", " ", " ", "o", "o", "o",
			"o", " ", " ", " ", " ", "o", "o", "o"
		});
	}

	private void SetMap7()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", "x", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", "o", "o", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", " ",
			" ", " ", "o", "o", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", "o", "o", " ",
			" ", " ", "o", "o", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", "o", "o", " ",
			" ", " ", "o", "o", " ", " ", "x", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", "o", " ", " ", " ", "o", "o", " ",
			" ", " ", "o", "o", " ", " ", "o", "o"
		});
	}

	private void SetMap8()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", "x", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", "x", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", " ",
			" ", " ", "o", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", "o",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", " ", " ", " ", "o", "o", " ", " ", " ",
			"o", "o", "o", " ", " ", " ", "o", "o"
		});
	}

	private void SetMap9()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", "x", " ",
			" ", " ", " ", " ", "x", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "x", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", " ",
			" ", " ", " ", "o", "o", " ", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", "o", "o", " ",
			" ", " ", " ", "o", "o", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", "x", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", "o", " ", "o", "o", "o", "o", "o",
			" ", " ", "o", "o", "o", "o", "o", "o"
		});
	}

	private void SetMap10()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", "x", " ", " ", "x",
			"x", " ", " ", " ", "x", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", "x", "o", " ", " ", " ",
			" ", " ", " ", "x", "o", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", "o", "o", "o", " ", " ", " ",
			" ", " ", "o", "o", "o", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", "x",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", "o",
			"x", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", " ", " ", " ", " ", " ", " ", " ", "o",
			"o", " ", " ", " ", " ", " ", "o", "o"
		});
	}

	private void SetMap11()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", "x", " ", "x", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", "x", " ", "x", " ",
			" ", " ", " ", " ", " ", "o", "o", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", "x", " ", " ", " ", " ", " ", " ",
			"o", "o", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", "o", "o", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", "o"
		});
	}

	private void SetMap12()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", "x", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "x", " ", " ", "x", " ", " ", "x", " ",
			" ", " ", " ", " ", " ", "x", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", "o", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", "o", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", " ", " ", "o", " ", " ", "o", " ",
			" ", " ", "o", " ", " ", "o", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", " ", " ", "o", " ", " ", "o", " ",
			" ", " ", "o", " ", " ", "o", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", " ", "x", "o", " ", "x", "o", " ",
			" ", " ", "o", " ", "x", "o", " ", " "
		});
		map.Add(new string[18]
		{
			"o", "o", "o", " ", "o", "o", " ", "o", "o", " ",
			" ", " ", "o", " ", "o", "o", "o", "o"
		});
	}

	private void SetMap13()
	{
		map = new List<string[]>();
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", "x", " ", " ", " ", " ",
			"x", " ", " ", " ", " ", " ", "x", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "x", " ", " ", "o", " ", " ", " ", " ",
			"o", " ", "x", " ", " ", "o", "o", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", " ", " ", " ", " ", " ",
			" ", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", "o", "o", " ", " ", " ", "o", "o", " ",
			" ", " ", "o", "o", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", "x", " ", " ", " ", " ",
			"x", " ", " ", " ", " ", " ", " ", " "
		});
		map.Add(new string[18]
		{
			" ", " ", " ", " ", " ", "x", " ", " ", " ", " ",
			"x", " ", " ", " ", " ", "x", " ", " "
		});
		map.Add(new string[18]
		{
			"o", " ", " ", " ", " ", "o", " ", " ", " ", " ",
			"o", " ", " ", " ", " ", "o", "o", "o"
		});
	}
}
