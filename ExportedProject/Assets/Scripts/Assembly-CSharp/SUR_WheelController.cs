using System.Collections.Generic;
using UnityEngine;

public class SUR_WheelController : MonoBehaviour
{
	public WheelController wheelController;

	public GameObject wheel;

	public SUR_ScoresController scoresController;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Sprite[] smallRotationOverlays;

	public Sprite[] bigRotationOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	public Sprite circleBig_s;

	public Sprite circleSmall_s;

	public Sprite mixBig_s;

	public Sprite mixSmall_s;

	public Sprite maxBig_s;

	public Sprite maxSmall_s;

	public SpriteRenderer titleIconSR;

	public SpriteRenderer titleIconSR_s;

	public SpriteRenderer titleIconOverlaySR;

	public Sprite settingsIcon;

	public Color titleColor;

	public Sprite backIconSmall;

	public Sprite backIconBig;

	public Color backIconColor;

	public List<string> minigames;

	public Color white;

	public Dictionary<string, Sprite[]> minigamesIconsDictionary;

	public Sprite[] MIX_icon;

	public Sprite[] MTO_icon;

	public Sprite[] SQD_icon;

	public Sprite[] PLU_icon;

	public Sprite[] CNN_icon;

	public Sprite[] CBL_icon;

	public Sprite[] BAT_icon;

	public Sprite[] SPR_icon;

	public Sprite[] BAL_icon;

	public Sprite[] GOT_icon;

	public Sprite[] PPR_icon;

	public Sprite[] SNW_icon;

	public Sprite[] MNK_icon;

	public Sprite[] ZIP_icon;

	public Sprite[] FTL_icon;

	public Sprite[] SQR_icon;

	public Sprite[] FLA_icon;

	public Sprite[] FRO_icon;

	public Sprite[] PNG_icon;

	public Sprite[] SEA_icon;

	public Sprite[] SWO_icon;

	public Sprite[] BEE_icon;

	public Sprite[] DUC_icon;

	public Sprite[] CRA_icon;

	public Sprite[] WGN_icon;

	public Sprite[] SHI_icon;

	public Sprite[] HEL_icon;

	public Sprite[] RAT_icon;

	public Sprite[] PLA_icon;

	public Sprite[] SNK_icon;

	public Sprite[] SKI_icon;

	public Sprite[] BWL_icon;

	public Sprite[] TOP_icon;

	public Sprite[] MAX_icon;

	private int selection = -1;

	private void Start()
	{
		InitializeDictionaries();
		Initialize();
	}

	private void Update()
	{
		if (selection != wheelController.selection)
		{
			selection = wheelController.selection;
			if (selection < minigames.Count)
			{
				scoresController.UpdateScores(minigames[selection]);
			}
			else
			{
				scoresController.HideScores();
			}
		}
	}

	private void InitializeDictionaries()
	{
		minigamesIconsDictionary = new Dictionary<string, Sprite[]>();
		minigamesIconsDictionary.Add("MIX", MIX_icon);
		minigamesIconsDictionary.Add("MTO", MTO_icon);
		minigamesIconsDictionary.Add("SQD", SQD_icon);
		minigamesIconsDictionary.Add("PLU", PLU_icon);
		minigamesIconsDictionary.Add("CNN", CNN_icon);
		minigamesIconsDictionary.Add("CBL", CBL_icon);
		minigamesIconsDictionary.Add("BAT", BAT_icon);
		minigamesIconsDictionary.Add("SPR", SPR_icon);
		minigamesIconsDictionary.Add("BAL", BAL_icon);
		minigamesIconsDictionary.Add("GOT", GOT_icon);
		minigamesIconsDictionary.Add("PPR", PPR_icon);
		minigamesIconsDictionary.Add("SNW", SNW_icon);
		minigamesIconsDictionary.Add("MNK", MNK_icon);
		minigamesIconsDictionary.Add("ZIP", ZIP_icon);
		minigamesIconsDictionary.Add("FTL", FTL_icon);
		minigamesIconsDictionary.Add("SQR", SQR_icon);
		minigamesIconsDictionary.Add("FLA", FLA_icon);
		minigamesIconsDictionary.Add("FRO", FRO_icon);
		minigamesIconsDictionary.Add("PNG", PNG_icon);
		minigamesIconsDictionary.Add("SEA", SEA_icon);
		minigamesIconsDictionary.Add("SWO", SWO_icon);
		minigamesIconsDictionary.Add("BEE", BEE_icon);
		minigamesIconsDictionary.Add("DUC", DUC_icon);
		minigamesIconsDictionary.Add("CRA", CRA_icon);
		minigamesIconsDictionary.Add("WGN", WGN_icon);
		minigamesIconsDictionary.Add("SHI", SHI_icon);
		minigamesIconsDictionary.Add("HEL", HEL_icon);
		minigamesIconsDictionary.Add("RAT", RAT_icon);
		minigamesIconsDictionary.Add("PLA", PLA_icon);
		minigamesIconsDictionary.Add("SNK", SNK_icon);
		minigamesIconsDictionary.Add("SKI", SKI_icon);
		minigamesIconsDictionary.Add("BWL", BWL_icon);
		minigamesIconsDictionary.Add("TOP", TOP_icon);
		minigamesIconsDictionary.Add("MAX", MAX_icon);
	}

	public void Initialize()
	{
		if (wheelController == null)
		{
			base.enabled = false;
			return;
		}
		optionsToSkip = new bool[GlobalControl.globalControl.minigamesList.Count + 3];
		smallIcons = new Sprite[optionsToSkip.Length];
		smallIconsOverlays = new Sprite[smallIcons.Length];
		bigIcons = new Sprite[smallIcons.Length];
		bigIconsOverlays = new Sprite[smallIcons.Length];
		iconColors = new Color[smallIcons.Length];
		minigames = new List<string>();
		minigames.Add("MIX");
		smallIcons[0] = mixSmall_s;
		smallIconsOverlays[0] = minigamesIconsDictionary["MIX"][1];
		bigIcons[0] = mixBig_s;
		bigIconsOverlays[0] = minigamesIconsDictionary["MIX"][0];
		iconColors[0] = white;
		for (int i = 0; i < GlobalControl.globalControl.minigamesList.Count; i++)
		{
			minigames.Add(GlobalControl.globalControl.minigamesList[i]);
			if (!GlobalControl.globalControl.unlockedMinigames.Contains(GlobalControl.globalControl.minigamesList[i]))
			{
				optionsToSkip[i + 1] = true;
			}
			smallIcons[i + 1] = circleSmall_s;
			smallIconsOverlays[i + 1] = minigamesIconsDictionary[GlobalControl.globalControl.minigamesList[i]][1];
			bigIcons[i + 1] = circleBig_s;
			bigIconsOverlays[i + 1] = minigamesIconsDictionary[GlobalControl.globalControl.minigamesList[i]][0];
			iconColors[i + 1] = white;
		}
		minigames.Add("MAX");
		smallIcons[optionsToSkip.Length - 2] = maxSmall_s;
		smallIconsOverlays[optionsToSkip.Length - 2] = minigamesIconsDictionary["MAX"][1];
		bigIcons[optionsToSkip.Length - 2] = maxBig_s;
		bigIconsOverlays[optionsToSkip.Length - 2] = minigamesIconsDictionary["MAX"][0];
		iconColors[optionsToSkip.Length - 2] = white;
		if (!GlobalControl.globalControl.AreAllMinigamesUnlocked())
		{
			optionsToSkip[optionsToSkip.Length - 2] = true;
		}
		smallIcons[smallIcons.Length - 1] = backIconSmall;
		bigIcons[smallIcons.Length - 1] = backIconBig;
		iconColors[smallIcons.Length - 1] = backIconColor;
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, bigRotationOverlays, iconColors, optionsToSkip);
		wheelController.ChangeSelectionAuto(GlobalControl.globalControl.survivalSelection);
	}
}
