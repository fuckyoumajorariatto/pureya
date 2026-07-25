using System.Collections.Generic;
using UnityEngine;

public class SkinsArrayController : MonoBehaviour
{
	public static SkinsArrayController skinsArrayController;

	private Transform thisT;

	public GameObject addToCollectionParticles;

	private Transform addToCollectionParticlesT;

	private Dictionary<string, float> scale;

	private Dictionary<string, int> wide;

	private Dictionary<string, Sprite[]> sprites;

	private Dictionary<string, SpriteRenderer[]> spriteRenderers;

	private Dictionary<string, Transform[]> spriteTransforms;

	private List<string> minigamesList = new List<string>();

	public Sprite[] MTO;

	public Sprite[] SQD;

	public Sprite[] PLU;

	public Sprite[] CNN;

	public Sprite[] CBL;

	public Sprite[] BAT;

	public Sprite[] SPR;

	public Sprite[] BAL;

	public Sprite[] GOT;

	public Sprite[] PPR;

	public Sprite[] SNW;

	public Sprite[] MNK;

	public Sprite[] ZIP;

	public Sprite[] FTL;

	public Sprite[] SQR;

	public Sprite[] FLA;

	public Sprite[] FRO;

	public Sprite[] PNG;

	public Sprite[] SEA;

	public Sprite[] SWO;

	private GameObject newSprite;

	private Transform newSpriteT;

	private SpriteRenderer newSpriteSR;

	private GameObject newSpriteS;

	private Transform newSpriteST;

	private Dictionary<string, Transform> containers;

	private float currentPosition = 8f;

	private float timer;

	private Vector2 initialPos;

	private Vector2 finalPos;

	private bool movingToContainer;

	private string givenMinigame;

	private int givenSkin;

	private void Awake()
	{
		skinsArrayController = this;
		thisT = base.gameObject.transform;
		addToCollectionParticlesT = addToCollectionParticles.transform;
		InitializeDictionaries();
		InstantiateArray();
	}

	private void Update()
	{
		if (movingToContainer)
		{
			DoMoveToContainer();
		}
	}

	private void InitializeDictionaries()
	{
		spriteRenderers = new Dictionary<string, SpriteRenderer[]>();
		spriteTransforms = new Dictionary<string, Transform[]>();
		minigamesList = GlobalControl.globalControl.minigamesList;
		containers = new Dictionary<string, Transform>();
		scale = new Dictionary<string, float>();
		wide = new Dictionary<string, int>();
		sprites = new Dictionary<string, Sprite[]>();
		AddToDictionaries("MTO", MTO, 1.5f, 12);
		AddToDictionaries("SQD", SQD, 1f, 14);
		AddToDictionaries("PLU", PLU, 1.25f, 14);
		AddToDictionaries("CNN", CNN, 0.75f, 32);
		AddToDictionaries("CBL", CBL, 1f, 20);
		AddToDictionaries("BAT", BAT, 1.25f, 20);
		AddToDictionaries("SPR", SPR, 1f, 24);
		AddToDictionaries("BAL", BAL, 1.25f, 16);
		AddToDictionaries("GOT", GOT, 1f, 20);
		AddToDictionaries("PPR", PPR, 1f, 24);
		AddToDictionaries("SNW", SNW, 1f, 12);
		AddToDictionaries("MNK", MNK, 1f, 24);
		AddToDictionaries("ZIP", ZIP, 1.25f, 12);
		AddToDictionaries("FTL", FTL, 0.5f, 48);
		AddToDictionaries("SQR", SQR, 1f, 26);
		AddToDictionaries("FLA", FLA, 1f, 20);
		AddToDictionaries("FRO", FRO, 1f, 22);
		AddToDictionaries("PNG", PNG, 1f, 16);
		AddToDictionaries("SEA", SEA, 1f, 14);
		AddToDictionaries("SWO", SWO, 0.75f, 12);
	}

	private void AddToDictionaries(string minigame, Sprite[] sentSprites, float sentScale, int sentWide)
	{
		scale.Add(minigame, sentScale);
		wide.Add(minigame, (int)Mathf.Ceil((float)sentWide * sentScale));
		sprites.Add(minigame, sentSprites);
		spriteRenderers.Add(minigame, new SpriteRenderer[9]);
		spriteTransforms.Add(minigame, new Transform[9]);
	}

	public void Unlock(string minigame, int skin)
	{
		givenMinigame = minigame;
		givenSkin = skin;
		MoveToContainer(minigame);
		Invoke("AddToCollection", 1f);
	}

	private void AddToCollection()
	{
		spriteRenderers[givenMinigame][givenSkin].enabled = true;
		addToCollectionParticlesT.position = spriteTransforms[givenMinigame][givenSkin].position;
		addToCollectionParticles.SetActive(true);
		Invoke("DisableCollectionParticles", 1f);
		Invoke("MoveOut", 1f);
	}

	private void DisableCollectionParticles()
	{
		addToCollectionParticles.SetActive(false);
	}

	private void MoveToContainer(string minigame)
	{
		initialPos = thisT.transform.position;
		finalPos = containers[minigame].position;
		movingToContainer = true;
	}

	private void MoveOut()
	{
		initialPos = thisT.transform.position;
		finalPos = new Vector2(2f + (float)containers.Count * 6f + 8f, 0f);
		movingToContainer = true;
	}

	private void DoMoveToContainer()
	{
		thisT.transform.position = Vector2.Lerp(initialPos, -finalPos, Mathf.SmoothStep(0f, 1f, timer));
		if (timer >= 1f)
		{
			movingToContainer = false;
			timer = 0f;
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	private void InstantiateArray()
	{
		for (int i = 0; i < minigamesList.Count; i++)
		{
			newSprite = new GameObject();
			newSprite.name = minigamesList[i] + " skins container";
			containers.Add(minigamesList[i], newSprite.GetComponent<Transform>());
			containers[minigamesList[i]].parent = thisT;
			containers[minigamesList[i]].position = new Vector3(currentPosition, 0f, -6f);
			currentPosition += 6f;
			for (int j = 0; j < 9; j++)
			{
				newSprite = new GameObject();
				newSprite.name = minigamesList[i] + (j + 1);
				newSprite.AddComponent<SpriteRenderer>();
				newSpriteSR = newSprite.GetComponent<SpriteRenderer>();
				spriteRenderers[minigamesList[i]][j] = newSpriteSR;
				newSpriteSR.sprite = sprites[minigamesList[i]][j];
				newSpriteT = newSprite.transform;
				spriteTransforms[minigamesList[i]][j] = newSpriteT;
				newSpriteT.localScale = new Vector3(scale[minigamesList[i]], scale[minigamesList[i]], 1f);
				newSpriteT.parent = containers[minigamesList[i]];
				newSpriteS = Object.Instantiate(newSprite);
				newSpriteS.name = minigamesList[i] + (j + 1) + "_s";
				newSpriteS.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 175);
				newSpriteST = newSpriteS.transform;
				newSpriteST.parent = newSprite.transform;
				newSpriteST.localPosition = new Vector3(0.0625f, 0.0625f, 0.1f);
				if (GlobalControl.globalControl.skinsDictionary[minigamesList[i]][j] == 0)
				{
					newSpriteSR.enabled = false;
				}
				switch (j)
				{
				case 0:
					newSpriteT.localPosition = new Vector2(-2f, 2f);
					break;
				case 1:
					newSpriteT.localPosition = new Vector2(0f, 2f);
					break;
				case 2:
					newSpriteT.localPosition = new Vector2(2f, 2f);
					break;
				case 3:
					newSpriteT.localPosition = new Vector2(-2f, 0f);
					break;
				case 4:
					newSpriteT.localPosition = new Vector2(2f, -2f);
					break;
				case 5:
					newSpriteT.localPosition = new Vector2(2f, 0f);
					break;
				case 6:
					newSpriteT.localPosition = new Vector2(-2f, -2f);
					break;
				case 7:
					newSpriteT.localPosition = new Vector2(0f, -2f);
					break;
				case 8:
					newSpriteT.localPosition = new Vector2(0f, 0f);
					break;
				}
			}
		}
	}

	public Dictionary<string, Sprite[]> GetSpritesDictionary()
	{
		return sprites;
	}
}
