using System.Collections.Generic;
using UnityEngine;

public class PCK_UnlockAssets : MonoBehaviour
{
	public Dictionary<string, Sprite[]> skinsDictionary;

	public Sprite[] MTOskins;

	public Sprite[] SQDskins;

	public Sprite[] PLUskins;

	public Sprite[] CNNskins;

	public Sprite[] CBLskins;

	public Sprite[] BATskins;

	public Sprite[] SPRskins;

	public Sprite[] BALskins;

	public Sprite[] GOTskins;

	public Sprite[] PPRskins;

	public Sprite[] SNWskins;

	public Sprite[] MNKskins;

	public Sprite[] ZIPskins;

	public Sprite[] FTLskins;

	public Sprite[] SQRskins;

	public Sprite[] FLAskins;

	public Sprite[] FROskins;

	public Sprite[] PNGskins;

	public Sprite[] SEAskins;

	public Sprite[] SWOskins;

	public Sprite[] BEEskins;

	public Sprite[] DUCskins;

	public Sprite[] CRAskins;

	public Sprite[] WGNskins;

	public Sprite[] SHIskins;

	public Sprite[] HELskins;

	public Sprite[] RATskins;

	public Sprite[] PLAskins;

	public Sprite[] SNKskins;

	public Sprite[] SKIskins;

	public Sprite[] BWLskins;

	public Sprite[] TOPskins;

	public Dictionary<string, float> skinsScale;

	public Sprite[] tracks_big;

	public Sprite[] tracks_big_L;

	public Sprite[] tracks_big_R;

	public Sprite[] tracks_small;

	public Dictionary<string, Sprite[]> GetSkinsDictionary()
	{
		skinsDictionary = new Dictionary<string, Sprite[]>();
		skinsDictionary.Add("MTO", MTOskins);
		skinsDictionary.Add("SQD", SQDskins);
		skinsDictionary.Add("PLU", PLUskins);
		skinsDictionary.Add("CNN", CNNskins);
		skinsDictionary.Add("CBL", CBLskins);
		skinsDictionary.Add("BAT", BATskins);
		skinsDictionary.Add("SPR", SPRskins);
		skinsDictionary.Add("BAL", BALskins);
		skinsDictionary.Add("GOT", GOTskins);
		skinsDictionary.Add("PPR", PPRskins);
		skinsDictionary.Add("SNW", SNWskins);
		skinsDictionary.Add("MNK", MNKskins);
		skinsDictionary.Add("ZIP", ZIPskins);
		skinsDictionary.Add("FTL", FTLskins);
		skinsDictionary.Add("SQR", SQRskins);
		skinsDictionary.Add("FLA", FLAskins);
		skinsDictionary.Add("FRO", FROskins);
		skinsDictionary.Add("PNG", PNGskins);
		skinsDictionary.Add("SEA", SEAskins);
		skinsDictionary.Add("SWO", SWOskins);
		skinsDictionary.Add("BEE", BEEskins);
		skinsDictionary.Add("DUC", DUCskins);
		skinsDictionary.Add("CRA", CRAskins);
		skinsDictionary.Add("WGN", WGNskins);
		skinsDictionary.Add("SHI", SHIskins);
		skinsDictionary.Add("HEL", HELskins);
		skinsDictionary.Add("RAT", RATskins);
		skinsDictionary.Add("PLA", PLAskins);
		skinsDictionary.Add("SNK", SNKskins);
		skinsDictionary.Add("SKI", SKIskins);
		skinsDictionary.Add("BWL", BWLskins);
		skinsDictionary.Add("TOP", TOPskins);
		return skinsDictionary;
	}

	public Dictionary<string, float> GetSkinsScale()
	{
		skinsScale = new Dictionary<string, float>();
		skinsScale.Add("MTO", 2f);
		skinsScale.Add("SQD", 1.5f);
		skinsScale.Add("PLU", 1.5f);
		skinsScale.Add("CNN", 1f);
		skinsScale.Add("CBL", 1.5f);
		skinsScale.Add("BAT", 1.5f);
		skinsScale.Add("SPR", 1.5f);
		skinsScale.Add("BAL", 1.5f);
		skinsScale.Add("GOT", 1.5f);
		skinsScale.Add("PPR", 1.5f);
		skinsScale.Add("SNW", 1.5f);
		skinsScale.Add("MNK", 1.5f);
		skinsScale.Add("ZIP", 2f);
		skinsScale.Add("FTL", 0.75f);
		skinsScale.Add("SQR", 1f);
		skinsScale.Add("FLA", 0.75f);
		skinsScale.Add("FRO", 1.5f);
		skinsScale.Add("PNG", 1.5f);
		skinsScale.Add("SEA", 1.5f);
		skinsScale.Add("SWO", 1f);
		skinsScale.Add("BEE", 1.5f);
		skinsScale.Add("DUC", 1.5f);
		skinsScale.Add("CRA", 1.5f);
		skinsScale.Add("WGN", 1.5f);
		skinsScale.Add("SHI", 1.5f);
		skinsScale.Add("HEL", 1.5f);
		skinsScale.Add("RAT", 2f);
		skinsScale.Add("PLA", 2f);
		skinsScale.Add("SNK", 1f);
		skinsScale.Add("SKI", 1.5f);
		skinsScale.Add("BWL", 1.5f);
		skinsScale.Add("TOP", 1.5f);
		return skinsScale;
	}
}
