using System.Collections.Generic;
using UnityEngine;

public class PiecesArray : MonoBehaviour
{
	public Dictionary<string, Sprite[]> sprites;

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

	public Sprite[] BEE;

	public Sprite[] DUC;

	public Sprite[] CRA;

	public Sprite[] WGN;

	public Sprite[] SHI;

	public Sprite[] HEL;

	public Sprite[] RAT;

	public Sprite[] PLA;

	public Sprite[] SNK;

	public Sprite[] SKI;

	public Sprite[] BWL;

	public Sprite[] TOP;

	private bool initializeDictionariesNextFrame;

	private void Awake()
	{
		InitializeDictionaries();
	}

	private void Update()
	{
		if (initializeDictionariesNextFrame)
		{
			initializeDictionariesNextFrame = false;
			InitializeDictionaries();
		}
	}

	private void InitializeDictionaries()
	{
		if (GlobalControl.globalControl == null)
		{
			initializeDictionariesNextFrame = true;
			return;
		}
		sprites = new Dictionary<string, Sprite[]>();
		foreach (string minigames in GlobalControl.globalControl.minigamesList)
		{
			AddToDictionaries(minigames, (Sprite[])GetType().GetField(minigames).GetValue(this));
		}
	}

	private void AddToDictionaries(string minigame, Sprite[] sentSprites)
	{
		sprites.Add(minigame, sentSprites);
	}

	public Dictionary<string, Sprite[]> GetPiecesDictionary()
	{
		return sprites;
	}
}
