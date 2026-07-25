using System.Collections.Generic;
using UnityEngine;

public class PCK_UnlocksController : MonoBehaviour
{
	public static PCK_UnlocksController PCK_unlocksController;

	public GameObject gachaGO;

	public GameObject timelineSkin;

	public GameObject timelinePiece;

	public GameObject timelineSkinFast;

	public GameObject timelinePieceFast;

	public GameObject timelinePieceFastRepeated;

	private GameObject timeline;

	private GameObject unlockAssets;

	private PCK_UnlockAssets unlockAssetsScript;

	public GameObject unlockAssetsPrefab;

	private string sMinigame;

	private int sSkin;

	private Dictionary<string, Sprite[]> skinSpritesDictionary;

	private Dictionary<string, int[]> skinsDictionary;

	private Dictionary<string, Sprite[]> piecesSpritesDictionary;

	private List<string> unlockedMinigames;

	public Dictionary<string, int> minigamesPieces;

	private List<string> minigamesList;

	private List<string> minigamesWithUnlockedSkins;

	private List<string> minigamesWithRemainingSkins;

	private Dictionary<string, int[]> lockedSkinsDictionary;

	private Dictionary<string, int[]> remainingSkinsDictionary;

	private List<string> remainingGoldenSkins;

	private List<string> newGoldenSkins;

	private string selectedMinigame;

	private int selectedSkin;

	private int completedSkins;

	private bool repeatedSkin;

	private int selectedTrack;

	private bool goldenSkinJustUnlocked;

	public Animator boardContainer;

	public Animator gachaAnimator;

	public Animator dispenserNumber;

	public SpriteRenderer dispenserNumber_n0;

	public SpriteRenderer dispenserNumber_n1;

	public Sprite[] abcSprites = new Sprite[26];

	public Sprite[] abcNumberSprites = new Sprite[10];

	public Sprite[] pixelNumberSprites = new Sprite[10];

	public Sprite[] slotNumberSprites = new Sprite[10];

	public SpriteRenderer skin;

	public SpriteRenderer skin_s;

	public GameObject collectedNumber;

	public GameObject collectedNumber_s;

	public GameObject char1;

	public GameObject char1_s;

	public GameObject char2;

	public GameObject char2_s;

	public GameObject char3;

	public GameObject char3_s;

	public GameObject charNumber;

	public GameObject charNumber_s;

	public GameObject medal;

	public GameObject progressNumberLeft1;

	public GameObject progressNumberLeft1_s;

	public GameObject progressNumberLeft2;

	public GameObject progressNumberLeft2_s;

	public GameObject progressNumberLeft3;

	public GameObject progressNumberLeft3_s;

	public GameObject progressNumberRight1;

	public GameObject progressNumberRight1_s;

	public GameObject progressNumberRight2;

	public GameObject progressNumberRight2_s;

	public GameObject progressNumberRight3;

	public GameObject progressNumberRight3_s;

	public GameObject smallMedal;

	public GameObject gashaponUp;

	public GameObject gashaponDown;

	private Color32 gashaponUpColor;

	private Color32 gashaponDownColor;

	public Animator[] pieceContainers;

	public SpriteRenderer[] piecesSR;

	private List<int> lockedTracks;

	public Animator[] trackPieceContainers;

	public Animator[] trackNumberContainers;

	public SpriteRenderer[] trackNumbers;

	public Sprite[] halfTrackIcon;

	public SpriteRenderer trackPieceL;

	public SpriteRenderer trackPieceR;

	public AudioClip new_unlock;

	public AudioClip repeated_unlock;

	public AudioClip medal_sound;

	public AudioClip backToPachinko;

	public Animator[] skinContainer = new Animator[9];

	public GameObject[] skinContainerSkin = new GameObject[9];

	public GameObject[] skinContainerNumber = new GameObject[9];

	public Dictionary<string, float> skinsScale;

	private bool firstPrize;

	private List<int> skins;

	private List<int> newSkins;

	private int r;

	private int[] skinsArray;

	private string number;

	private int count;

	private int collectedN;

	private string abecedary;

	private int[] skinsA;

	private int collected;

	private int triggerUp;

	private int triggerDown;

	private int triggerLower;

	private int triggerUnlocked;

	private int triggerMedal;

	private int triggerNumberUp;

	public int remainingPrizes;

	private bool noSkinsLeft;

	private bool noMinigamesLeft;

	private bool noTracksLeft;

	private int minigamesLeftToUnlock;

	private float chanceOfMinigame;

	private int tracksLeftToUnlock;

	private float chanceOfTrack;

	private string previousUnlock;

	private string rePreviousUnlock;

	public bool firstJackpot;

	private void Start()
	{
		PCK_unlocksController = this;
		triggerUp = Animator.StringToHash("up");
		triggerDown = Animator.StringToHash("down");
		triggerLower = Animator.StringToHash("lower");
		triggerUnlocked = Animator.StringToHash("unlocked");
		triggerMedal = Animator.StringToHash("medal");
		triggerNumberUp = Animator.StringToHash("numberUp");
		abecedary = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	}

	public void ShowDispenser()
	{
		dispenserNumber_n0.sprite = slotNumberSprites[remainingPrizes];
		boardContainer.SetTrigger(triggerUp);
		gachaGO.SetActive(true);
		firstPrize = true;
		goldenSkinJustUnlocked = false;
	}

	public void HideDispenser()
	{
		boardContainer.SetTrigger(triggerDown);
		gachaGO.SetActive(false);
		if (!noSkinsLeft)
		{
			PlayBackToPachinko();
		}
	}

	public void NextPrize()
	{
		dispenserNumber_n0.sprite = slotNumberSprites[remainingPrizes];
		if (remainingPrizes < 1)
		{
			remainingPrizes = 1;
		}
		dispenserNumber_n1.sprite = slotNumberSprites[remainingPrizes - 1];
		ChoosePrize();
		if (noSkinsLeft && noMinigamesLeft && noTracksLeft)
		{
			dispenserNumber.SetTrigger(triggerLower);
			PCK_GachaController.PCK_gachaController.PlayMachineSounds();
			Invoke("PrizeEnded", 0.5f);
			Invoke("PlayBackToPachinko", 0.25f);
		}
		else
		{
			dispenserNumber.SetTrigger(triggerLower);
			if (firstPrize)
			{
				gachaAnimator.SetBool("fast", false);
			}
			else
			{
				gachaAnimator.SetBool("fast", true);
			}
			if (repeatedSkin)
			{
				gachaAnimator.SetBool("repeated", true);
			}
			else
			{
				gachaAnimator.SetBool("repeated", false);
			}
			if (previousUnlock == "track")
			{
				gachaAnimator.SetInteger("kind", 0);
			}
			else if (previousUnlock == "minigame")
			{
				gachaAnimator.SetInteger("kind", 1);
			}
			else if (previousUnlock == "skin")
			{
				gachaAnimator.SetInteger("kind", 2);
			}
			gachaAnimator.SetTrigger("start");
		}
		medal.SetActive(false);
		collectedNumber.SetActive(true);
		if (firstPrize)
		{
			firstPrize = false;
		}
	}

	public void PrizeEnded()
	{
		remainingPrizes--;
		GlobalControl.globalControl.remainingPrizes--;
		if (remainingPrizes > 0)
		{
			NextPrize();
		}
		else
		{
			HideDispenser();
		}
	}

	private void ChoosePrize()
	{
		if (unlockAssets == null)
		{
			unlockAssets = Object.Instantiate(unlockAssetsPrefab);
			skinSpritesDictionary = unlockAssets.GetComponent<PCK_UnlockAssets>().GetSkinsDictionary();
			skinsScale = unlockAssets.GetComponent<PCK_UnlockAssets>().GetSkinsScale();
			piecesSpritesDictionary = unlockAssets.GetComponent<PiecesArray>().GetPiecesDictionary();
		}
		selectedMinigame = string.Empty;
		skinsDictionary = GlobalControl.globalControl.skinsDictionary;
		minigamesList = GlobalControl.globalControl.minigamesList;
		noSkinsLeft = true;
		foreach (string minigames in minigamesList)
		{
			for (int i = 0; i < 9; i++)
			{
				if (skinsDictionary[minigames][i] < 9)
				{
					noSkinsLeft = false;
					break;
				}
			}
			if (!noSkinsLeft)
			{
				break;
			}
		}
		minigamesPieces = GlobalControl.globalControl.minigamesPieces;
		noMinigamesLeft = true;
		minigamesLeftToUnlock = 0;
		foreach (string minigames2 in minigamesList)
		{
			if (GlobalControl.globalControl.unlockedMinigames.Contains(minigames2))
			{
				minigamesPieces[minigames2] = 4;
			}
			if (minigamesPieces[minigames2] < 4)
			{
				minigamesLeftToUnlock++;
				noMinigamesLeft = false;
			}
		}
		if (!noMinigamesLeft)
		{
			minigamesPieces = GlobalControl.globalControl.minigamesPieces;
			foreach (string minigames3 in minigamesList)
			{
				if (minigamesPieces[minigames3] > 0 && minigamesPieces[minigames3] < 4)
				{
					selectedMinigame = minigames3;
					break;
				}
			}
		}
		noTracksLeft = true;
		for (int j = 0; j < GlobalControl.globalControl.tracks.Length; j++)
		{
			if (GlobalControl.globalControl.trackPieces[j] < 2)
			{
				tracksLeftToUnlock++;
				noTracksLeft = false;
			}
		}
		if (minigamesLeftToUnlock == 1)
		{
			chanceOfMinigame = 0.05f + (float)GlobalControl.globalControl.rank / 1000f;
		}
		else if (minigamesLeftToUnlock < 5)
		{
			chanceOfMinigame = 0.08f + (float)GlobalControl.globalControl.rank / 800f;
		}
		else if (minigamesLeftToUnlock < 10)
		{
			chanceOfMinigame = 0.1f + (float)GlobalControl.globalControl.rank / 500f;
		}
		else if (minigamesLeftToUnlock < 15)
		{
			chanceOfMinigame = 0.2f + (float)GlobalControl.globalControl.rank / 400f;
		}
		else if (minigamesLeftToUnlock < 20)
		{
			chanceOfMinigame = 0.35f + (float)GlobalControl.globalControl.rank / 300f;
		}
		else
		{
			chanceOfMinigame = 0.6f + (float)GlobalControl.globalControl.rank / 300f;
		}
		if (chanceOfMinigame > 0.9f)
		{
			chanceOfMinigame = 0.9f;
		}
		if (previousUnlock == "minigame")
		{
			chanceOfMinigame = 0f;
		}
		else if (previousUnlock == "track" && chanceOfMinigame > 0.05f)
		{
			chanceOfMinigame = 0.05f;
		}
		if (rePreviousUnlock == "minigame" && chanceOfMinigame > 0.3f)
		{
			chanceOfMinigame = 0.3f;
		}
		if (tracksLeftToUnlock > 22)
		{
			chanceOfTrack = 0.5f + (float)GlobalControl.globalControl.rank / 300f;
		}
		else if (tracksLeftToUnlock > 15)
		{
			chanceOfTrack = 0.3f + (float)GlobalControl.globalControl.rank / 300f;
		}
		else if (tracksLeftToUnlock > 10)
		{
			chanceOfTrack = 0.2f + (float)GlobalControl.globalControl.rank / 400f;
		}
		else if (tracksLeftToUnlock > 6)
		{
			chanceOfTrack = 0.1f + (float)GlobalControl.globalControl.rank / 500f;
		}
		else if (tracksLeftToUnlock > 3)
		{
			chanceOfTrack = 0.08f + (float)GlobalControl.globalControl.rank / 800f;
		}
		else
		{
			chanceOfTrack = 0.05f + (float)GlobalControl.globalControl.rank / 1000f;
		}
		if (previousUnlock == "track")
		{
			chanceOfTrack = 0f;
		}
		else if (previousUnlock == "minigame" && chanceOfTrack > 0.05f)
		{
			chanceOfTrack = 0.05f;
		}
		if (rePreviousUnlock == "track" && chanceOfTrack > 0.3f)
		{
			chanceOfTrack = 0.3f;
		}
		if (!noMinigamesLeft && !noSkinsLeft && !noTracksLeft)
		{
			if (firstJackpot && previousUnlock != "minigame" && rePreviousUnlock != "minigame")
			{
				ChooseMinigame();
			}
			else if (Random.Range(0f, 1f) < chanceOfMinigame)
			{
				ChooseMinigame();
			}
			else if (Random.Range(0f, 1f) < chanceOfTrack)
			{
				ChooseTrack();
			}
			else
			{
				ChooseSkin();
			}
		}
		else if (!noMinigamesLeft && !noSkinsLeft && noTracksLeft)
		{
			if (Random.Range(0f, 1f) < chanceOfMinigame)
			{
				ChooseMinigame();
			}
			else
			{
				ChooseSkin();
			}
		}
		else if (noMinigamesLeft && !noSkinsLeft && !noTracksLeft)
		{
			if (Random.Range(0f, 1f) < chanceOfTrack)
			{
				ChooseTrack();
			}
			else
			{
				ChooseSkin();
			}
		}
		else if (!noMinigamesLeft && noSkinsLeft && !noTracksLeft)
		{
			if (Random.Range(0f, 1f) < chanceOfMinigame)
			{
				ChooseMinigame();
			}
			else
			{
				ChooseTrack();
			}
		}
		else if (!noMinigamesLeft)
		{
			ChooseMinigame();
		}
		else if (!noSkinsLeft)
		{
			ChooseSkin();
		}
		else if (!noTracksLeft)
		{
			ChooseTrack();
		}
		GlobalControl.globalControl.marbles = PCK_InputController.PCK_inputController.marblesRemaining;
		GlobalControl.globalControl.CalculateProgress();
	}

	private void ChooseTrack()
	{
		if (unlockAssets == null)
		{
			unlockAssets = Object.Instantiate(unlockAssetsPrefab);
			unlockAssetsScript = unlockAssets.GetComponent<PCK_UnlockAssets>();
		}
		if (unlockAssetsScript == null)
		{
			unlockAssetsScript = unlockAssets.GetComponent<PCK_UnlockAssets>();
		}
		rePreviousUnlock = previousUnlock;
		previousUnlock = "track";
		repeatedSkin = false;
		lockedTracks = new List<int>();
		selectedTrack = -1;
		for (int i = 0; i < GlobalControl.globalControl.tracks.Length; i++)
		{
			if (GlobalControl.globalControl.trackPieces[i] == 1)
			{
				selectedTrack = i;
				break;
			}
			if (!GlobalControl.globalControl.unlockedTracks[i])
			{
				lockedTracks.Add(i);
			}
		}
		if (selectedTrack == -1 && lockedTracks.Count > 0)
		{
			selectedTrack = lockedTracks[Random.Range(0, lockedTracks.Count)];
		}
		else if (selectedTrack == -1)
		{
			Debug.Log("ERROR: Unable to select valid track to unlock");
			selectedTrack = 0;
		}
		if (selectedTrack != -1)
		{
			if (GlobalControl.globalControl.trackPieces[selectedTrack] == 0)
			{
				trackPieceContainers[0].SetBool("unlocked", false);
				trackPieceContainers[1].SetBool("unlocked", false);
				skin.sprite = unlockAssetsScript.tracks_big_L[selectedTrack];
				skin_s.sprite = halfTrackIcon[0];
				trackPieceL.sprite = unlockAssetsScript.tracks_big_L[selectedTrack];
				trackPieceR.sprite = null;
			}
			else
			{
				trackPieceContainers[0].SetBool("unlocked", true);
				trackPieceContainers[1].SetBool("unlocked", false);
				skin.sprite = unlockAssetsScript.tracks_big_R[selectedTrack];
				skin_s.sprite = halfTrackIcon[1];
				trackPieceL.sprite = unlockAssetsScript.tracks_big_L[selectedTrack];
				trackPieceR.sprite = unlockAssetsScript.tracks_big_R[selectedTrack];
			}
		}
	}

	private void ChooseMinigame()
	{
		rePreviousUnlock = previousUnlock;
		previousUnlock = "minigame";
		repeatedSkin = false;
		minigamesPieces = GlobalControl.globalControl.minigamesPieces;
		selectedMinigame = string.Empty;
		for (int i = 0; i < minigamesList.Count; i++)
		{
			if (minigamesPieces[minigamesList[i]] > 0 && minigamesPieces[minigamesList[i]] < 4)
			{
				selectedMinigame = minigamesList[i];
			}
		}
		if (selectedMinigame == string.Empty)
		{
			r = Random.Range(0, minigamesList.Count);
			while (minigamesPieces[minigamesList[r]] > 0)
			{
				r++;
				if (r >= minigamesList.Count)
				{
					r = 0;
				}
			}
			selectedMinigame = minigamesList[r];
		}
		skin.sprite = piecesSpritesDictionary[selectedMinigame][minigamesPieces[selectedMinigame]];
		skin_s.sprite = skin.sprite;
		for (int j = 0; j < 4; j++)
		{
			pieceContainers[j].SetBool("unlocked", false);
			piecesSR[j].sprite = piecesSpritesDictionary[selectedMinigame][j];
		}
		for (int k = 0; k < minigamesPieces[selectedMinigame]; k++)
		{
			pieceContainers[k].SetBool("unlocked", true);
		}
		minigamesPieces[selectedMinigame]++;
	}

	private void ChooseSkin()
	{
		rePreviousUnlock = previousUnlock;
		previousUnlock = "skin";
		repeatedSkin = false;
		skinsDictionary = GlobalControl.globalControl.skinsDictionary;
		minigamesList = GlobalControl.globalControl.minigamesList;
		unlockedMinigames = GlobalControl.globalControl.unlockedMinigames;
		minigamesWithUnlockedSkins = new List<string>();
		minigamesWithRemainingSkins = new List<string>();
		lockedSkinsDictionary = new Dictionary<string, int[]>();
		remainingSkinsDictionary = new Dictionary<string, int[]>();
		remainingGoldenSkins = new List<string>();
		newGoldenSkins = new List<string>();
		selectedMinigame = string.Empty;
		selectedSkin = -1;
		foreach (string unlockedMinigame in unlockedMinigames)
		{
			skins = new List<int>();
			newSkins = new List<int>();
			for (int i = 0; i < 8; i++)
			{
				if (skinsDictionary[unlockedMinigame][i] < 9)
				{
					skins.Add(i);
				}
				if (skinsDictionary[unlockedMinigame][i] == 0)
				{
					newSkins.Add(i);
				}
			}
			if (skinsDictionary[unlockedMinigame][8] < 9)
			{
				remainingGoldenSkins.Add(unlockedMinigame);
				if (skinsDictionary[unlockedMinigame][8] == 0)
				{
					newGoldenSkins.Add(unlockedMinigame);
				}
			}
			if (skins.Count > 0)
			{
				minigamesWithRemainingSkins.Add(unlockedMinigame);
				int[] array = new int[skins.Count];
				for (int j = 0; j < skins.Count; j++)
				{
					array[j] = skins[j];
				}
				remainingSkinsDictionary.Add(unlockedMinigame, array);
			}
			if (newSkins.Count > 0)
			{
				minigamesWithUnlockedSkins.Add(unlockedMinigame);
				int[] array2 = new int[newSkins.Count];
				for (int k = 0; k < newSkins.Count; k++)
				{
					array2[k] = newSkins[k];
				}
				lockedSkinsDictionary.Add(unlockedMinigame, array2);
			}
		}
		if (GlobalControl.globalControl.goldenIndicator && newGoldenSkins.Count > 0 && !goldenSkinJustUnlocked)
		{
			r = Random.Range(0, newGoldenSkins.Count);
			selectedMinigame = newGoldenSkins[r];
			selectedSkin = 8;
		}
		else if (GlobalControl.globalControl.goldenIndicator && remainingGoldenSkins.Count > 0 && !goldenSkinJustUnlocked)
		{
			r = Random.Range(0, remainingGoldenSkins.Count);
			selectedMinigame = remainingGoldenSkins[r];
			selectedSkin = 8;
		}
		else if (minigamesWithUnlockedSkins.Count > 0 && Random.Range(0f, 1f) < 0.9f + (float)GlobalControl.globalControl.rank / 900f)
		{
			r = Random.Range(0, minigamesWithUnlockedSkins.Count);
			selectedMinigame = minigamesWithUnlockedSkins[r];
			r = Random.Range(0, lockedSkinsDictionary[selectedMinigame].Length);
			selectedSkin = lockedSkinsDictionary[selectedMinigame][r];
		}
		else if (minigamesWithRemainingSkins.Count > 0)
		{
			r = Random.Range(0, minigamesWithRemainingSkins.Count);
			selectedMinigame = minigamesWithRemainingSkins[r];
			r = Random.Range(0, remainingSkinsDictionary[selectedMinigame].Length);
			selectedSkin = remainingSkinsDictionary[selectedMinigame][r];
		}
		if (selectedMinigame == string.Empty)
		{
			minigamesWithRemainingSkins = new List<string>();
			remainingSkinsDictionary = new Dictionary<string, int[]>();
			remainingGoldenSkins = new List<string>();
			foreach (string minigames in minigamesList)
			{
				skins = new List<int>();
				for (int l = 0; l < 8; l++)
				{
					if (skinsDictionary[minigames][l] < 9)
					{
						skins.Add(l);
					}
				}
				if (skins.Count > 0)
				{
					minigamesWithRemainingSkins.Add(minigames);
					int[] array3 = new int[skins.Count];
					for (int m = 0; m < skins.Count; m++)
					{
						array3[m] = skins[m];
					}
					remainingSkinsDictionary.Add(minigames, array3);
				}
				if (skinsDictionary[minigames][8] < 9)
				{
					remainingGoldenSkins.Add(minigames);
				}
			}
			if (minigamesWithRemainingSkins.Count > 0)
			{
				r = Random.Range(0, minigamesWithRemainingSkins.Count);
				selectedMinigame = minigamesWithRemainingSkins[r];
				r = Random.Range(0, remainingSkinsDictionary[selectedMinigame].Length);
				selectedSkin = remainingSkinsDictionary[selectedMinigame][r];
			}
			if (GlobalControl.globalControl.goldenIndicator && remainingGoldenSkins.Count > 0)
			{
				r = Random.Range(0, remainingGoldenSkins.Count);
				selectedMinigame = remainingGoldenSkins[r];
				selectedSkin = 8;
			}
			if (selectedMinigame == string.Empty && remainingGoldenSkins.Count > 0)
			{
				r = Random.Range(0, remainingGoldenSkins.Count);
				selectedMinigame = remainingGoldenSkins[r];
				selectedSkin = 8;
			}
		}
		if (selectedSkin == 8)
		{
			goldenSkinJustUnlocked = true;
		}
		else
		{
			goldenSkinJustUnlocked = false;
		}
		if (selectedSkin == 8 && !GlobalControl.globalControl._777)
		{
			GlobalControl.globalControl._777 = true;
			GlobalControl.globalControl.UnlockAchievements();
		}
		if (selectedMinigame == string.Empty)
		{
			noSkinsLeft = true;
			return;
		}
		if (skinsDictionary[selectedMinigame][selectedSkin] > 0 && skinsDictionary[selectedMinigame][selectedSkin] < 9)
		{
			repeatedSkin = true;
		}
		sMinigame = selectedMinigame;
		sSkin = selectedSkin;
		SetCollection(selectedMinigame);
		skin.sprite = skinSpritesDictionary[selectedMinigame][selectedSkin];
		skin_s.sprite = skinSpritesDictionary[selectedMinigame][selectedSkin];
		skinsDictionary[selectedMinigame][selectedSkin]++;
		GlobalControl.globalControl.skinsDictionary = skinsDictionary;
		collectedN = skinsDictionary[selectedMinigame][selectedSkin];
		collectedNumber.GetComponent<SpriteRenderer>().sprite = abcNumberSprites[collectedN - 1];
		collectedNumber_s.GetComponent<SpriteRenderer>().sprite = abcNumberSprites[collectedN - 1];
		WriteMinigameName(selectedMinigame);
		charNumber.GetComponent<SpriteRenderer>().sprite = abcNumberSprites[selectedSkin + 1];
		charNumber_s.GetComponent<SpriteRenderer>().sprite = abcNumberSprites[selectedSkin + 1];
		ColorGashapon(selectedSkin);
	}

	private void ChangeCollectedNumbers(int n1, int n2)
	{
		number = string.Empty;
		if (n1 < 10)
		{
			number = "00" + n1;
		}
		else if (n1 < 99)
		{
			number = "0" + n1;
		}
		else
		{
			number = string.Empty + n1;
		}
		progressNumberLeft1.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[0].ToString())];
		progressNumberLeft1_s.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[0].ToString())];
		progressNumberLeft2.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[1].ToString())];
		progressNumberLeft2_s.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[1].ToString())];
		progressNumberLeft3.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[2].ToString())];
		progressNumberLeft3_s.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[2].ToString())];
		if (n2 < 10)
		{
			number = "00" + n2;
		}
		else if (n2 < 99)
		{
			number = "0" + n2;
		}
		else
		{
			number = string.Empty + n2;
		}
		progressNumberRight1.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[0].ToString())];
		progressNumberRight1_s.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[0].ToString())];
		progressNumberRight2.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[1].ToString())];
		progressNumberRight2_s.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[1].ToString())];
		progressNumberRight3.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[2].ToString())];
		progressNumberRight3_s.GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[int.Parse(number[2].ToString())];
	}

	private void WriteMinigameName(string minigame)
	{
		count = 0;
		foreach (char c in minigame)
		{
			count++;
			GameObject gameObject;
			GameObject gameObject2;
			switch (count)
			{
			case 1:
				gameObject = char1;
				gameObject2 = char1_s;
				break;
			case 2:
				gameObject = char2;
				gameObject2 = char2_s;
				break;
			case 3:
				gameObject = char3;
				gameObject2 = char3_s;
				break;
			default:
				gameObject = char1;
				gameObject2 = char1_s;
				break;
			}
			for (int j = 0; j < abecedary.Length; j++)
			{
				if (c == abecedary[j])
				{
					gameObject.GetComponent<SpriteRenderer>().sprite = abcSprites[j];
					gameObject2.GetComponent<SpriteRenderer>().sprite = abcSprites[j];
					break;
				}
			}
		}
	}

	public void RaiseCollectedNumber()
	{
		if (collectedN < 10)
		{
			medal.SetActive(false);
			collectedNumber.SetActive(true);
			collectedNumber.GetComponent<SpriteRenderer>().sprite = abcNumberSprites[collectedN];
			collectedNumber_s.GetComponent<SpriteRenderer>().sprite = abcNumberSprites[collectedN];
			if (collectedN == 1)
			{
				PlayNewUnlock();
			}
			else
			{
				PlayRepeatedUnlock();
			}
		}
		else
		{
			medal.SetActive(true);
			collectedNumber.SetActive(false);
			PlayMedal();
			if (!GlobalControl.globalControl._444)
			{
				GlobalControl.globalControl._444 = true;
				GlobalControl.globalControl.UnlockAchievements();
			}
		}
	}

	public void DispenserNumberChangeEnd()
	{
		dispenserNumber_n0.sprite = slotNumberSprites[remainingPrizes];
		if (remainingPrizes < 1)
		{
			remainingPrizes = 1;
		}
		dispenserNumber_n0.sprite = slotNumberSprites[remainingPrizes - 1];
	}

	private void ColorGashapon(int n)
	{
		gashaponUpColor = new Color32(81, 186, 238, byte.MaxValue);
		gashaponDownColor = new Color32(28, 131, 182, byte.MaxValue);
		switch (Random.Range(0, 4))
		{
		case 0:
			gashaponUpColor = new Color32(81, 186, 238, byte.MaxValue);
			gashaponDownColor = new Color32(28, 131, 182, byte.MaxValue);
			break;
		case 1:
			gashaponUpColor = new Color32(byte.MaxValue, 141, 181, byte.MaxValue);
			gashaponDownColor = new Color32(byte.MaxValue, 77, 140, byte.MaxValue);
			break;
		case 2:
			gashaponUpColor = new Color32(236, 201, 0, byte.MaxValue);
			gashaponDownColor = new Color32(193, 164, 1, byte.MaxValue);
			break;
		case 3:
			gashaponUpColor = new Color32(38, 234, 234, byte.MaxValue);
			gashaponDownColor = new Color32(0, 191, 191, byte.MaxValue);
			break;
		}
		if (n == 8)
		{
			gashaponUpColor = new Color32(byte.MaxValue, 244, 101, byte.MaxValue);
			gashaponDownColor = new Color32(238, 188, 26, byte.MaxValue);
		}
		gashaponUp.GetComponent<SpriteRenderer>().color = gashaponUpColor;
		gashaponDown.GetComponent<SpriteRenderer>().color = gashaponDownColor;
	}

	private void SetCollection(string minigame)
	{
		skinsA = GlobalControl.globalControl.skinsDictionary[minigame];
		collected = 0;
		for (int i = 0; i < 9; i++)
		{
			collected = skinsA[i];
			skinContainer[i].Rebind();
			skinContainerSkin[i].GetComponent<SpriteRenderer>().sprite = skinSpritesDictionary[minigame][i];
			skinContainerSkin[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = skinSpritesDictionary[minigame][i];
			skinContainerNumber[i].GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[GlobalControl.globalControl.skinsDictionary[minigame][i]];
			if (collected > 0 && collected < 9)
			{
				skinContainer[i].SetTrigger(triggerUnlocked);
			}
			else if (collected == 9)
			{
				skinContainer[i].SetTrigger(triggerMedal);
			}
		}
		float newScale = skinsScale[minigame];
		ChangeCollectionScale(newScale);
	}

	public void UpdateCollection()
	{
		skinsA = GlobalControl.globalControl.skinsDictionary[sMinigame];
		int num = skinsA[sSkin];
		if (num < 9)
		{
			skinContainer[sSkin].SetTrigger(triggerNumberUp);
		}
		else
		{
			skinContainer[sSkin].SetTrigger(triggerMedal);
		}
	}

	public void UpdateCollectionPieces()
	{
		pieceContainers[minigamesPieces[selectedMinigame] - 1].SetTrigger("unlock");
		if (minigamesPieces[selectedMinigame] == 4)
		{
			GlobalControl.globalControl.UnlockMinigame(selectedMinigame);
		}
	}

	public void UpdateCollectionTracks()
	{
		GlobalControl.globalControl.trackPieces[selectedTrack]++;
		if (GlobalControl.globalControl.trackPieces[selectedTrack] == 1)
		{
			trackPieceContainers[0].SetTrigger("unlock");
		}
		else
		{
			trackPieceContainers[1].SetTrigger("unlock");
		}
		if (GlobalControl.globalControl.trackPieces[selectedTrack] == 2)
		{
			GlobalControl.globalControl.UnlockTrack(selectedTrack);
			Invoke("PlayUnlockedTrack", 1f);
		}
	}

	private void PlayUnlockedTrack()
	{
		GlobalAudio.globalAudio.ClearMusic();
		PCK_Controller.PCK_controller.PlayingTrack(GlobalAudio.globalAudio.PlayMusic(GlobalControl.globalControl.tracks[selectedTrack], 0.9f, 1f, 0f));
	}

	private void ChangeCollectionScale(float newScale)
	{
		for (int i = 0; i < 9; i++)
		{
			skinContainerSkin[i].transform.localScale = new Vector3(newScale, newScale, newScale);
			skinContainerSkin[i].transform.GetChild(0).localPosition = new Vector3(0.0625f, -0.0625f, 0.1f);
		}
	}

	public void UpdateCollectedNumber()
	{
		skinsA = GlobalControl.globalControl.skinsDictionary[sMinigame];
		collected = skinsA[sSkin];
		if (collected < 9)
		{
			skinContainerNumber[sSkin].GetComponent<SpriteRenderer>().sprite = pixelNumberSprites[collected];
		}
		if (collected == 1)
		{
			PlayNewUnlock();
		}
		else if (collected < 9)
		{
			PlayRepeatedUnlock();
		}
		else
		{
			PlayMedal();
		}
	}

	public void PlayPieceUnlock()
	{
		if (previousUnlock == "minigame")
		{
			if (minigamesPieces[selectedMinigame] == 4)
			{
				PlayMedal();
			}
			else
			{
				PlayNewUnlock();
			}
		}
		else if (previousUnlock == "track")
		{
			if (GlobalControl.globalControl.trackPieces[selectedTrack] == 2)
			{
				PlayMedal();
			}
			else
			{
				PlayNewUnlock();
			}
		}
	}

	public void PlayNewUnlock()
	{
		GlobalAudio.globalAudio.Play(new_unlock, 1f, 1f, 0f);
	}

	public void PlayRepeatedUnlock()
	{
		GlobalAudio.globalAudio.Play(repeated_unlock, 1f, 1f, 0f);
	}

	public void PlayMedal()
	{
		GlobalAudio.globalAudio.Play(medal_sound, 1f, 1f, 0f);
	}

	public void PlayBackToPachinko()
	{
		GlobalAudio.globalAudio.Play(backToPachinko, 1f, 1f, 0f);
	}
}
