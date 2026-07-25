using System;
using System.IO;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("SaveData")]
public class SaveData
{
	public int profileNumber;

	public int gameLevel;

	public int storyLevel;

	public int pachinkoDesign;

	public string pachinkoHole;

	public int pckLightsLevel;

	public int marbles;

	public int goldenMarbles;

	public int goldenIndicator;

	public int jackpots;

	public string unlockedMinigames;

	public string unlockedTracks;

	public int playtime;

	public string enabledTracks;

	public string trackPieces;

	public string recentCinematics;

	public string MIX_scores;

	public string MAX_scores;

	public int _111;

	public int _222;

	public int _333;

	public int _444;

	public int _555;

	public int _666;

	public int _777;

	public int _888;

	public int _999;

	public string playedMinigames;

	public string MTO_skins { get; set; }

	public string SQD_skins { get; set; }

	public string PLU_skins { get; set; }

	public string CNN_skins { get; set; }

	public string CBL_skins { get; set; }

	public string BAT_skins { get; set; }

	public string SPR_skins { get; set; }

	public string BAL_skins { get; set; }

	public string GOT_skins { get; set; }

	public string PPR_skins { get; set; }

	public string SNW_skins { get; set; }

	public string MNK_skins { get; set; }

	public string ZIP_skins { get; set; }

	public string FTL_skins { get; set; }

	public string SQR_skins { get; set; }

	public string FLA_skins { get; set; }

	public string FRO_skins { get; set; }

	public string PNG_skins { get; set; }

	public string SEA_skins { get; set; }

	public string SWO_skins { get; set; }

	public string BEE_skins { get; set; }

	public string DUC_skins { get; set; }

	public string CRA_skins { get; set; }

	public string WGN_skins { get; set; }

	public string SHI_skins { get; set; }

	public string HEL_skins { get; set; }

	public string RAT_skins { get; set; }

	public string PLA_skins { get; set; }

	public string SNK_skins { get; set; }

	public string SKI_skins { get; set; }

	public string BWL_skins { get; set; }

	public string TOP_skins { get; set; }

	public int MTO_pieces { get; set; }

	public int SQD_pieces { get; set; }

	public int PLU_pieces { get; set; }

	public int CNN_pieces { get; set; }

	public int CBL_pieces { get; set; }

	public int BAT_pieces { get; set; }

	public int SPR_pieces { get; set; }

	public int BAL_pieces { get; set; }

	public int GOT_pieces { get; set; }

	public int PPR_pieces { get; set; }

	public int SNW_pieces { get; set; }

	public int MNK_pieces { get; set; }

	public int ZIP_pieces { get; set; }

	public int FTL_pieces { get; set; }

	public int SQR_pieces { get; set; }

	public int FLA_pieces { get; set; }

	public int FRO_pieces { get; set; }

	public int PNG_pieces { get; set; }

	public int SEA_pieces { get; set; }

	public int SWO_pieces { get; set; }

	public int BEE_pieces { get; set; }

	public int DUC_pieces { get; set; }

	public int CRA_pieces { get; set; }

	public int WGN_pieces { get; set; }

	public int SHI_pieces { get; set; }

	public int HEL_pieces { get; set; }

	public int RAT_pieces { get; set; }

	public int PLA_pieces { get; set; }

	public int SNK_pieces { get; set; }

	public int SKI_pieces { get; set; }

	public int BWL_pieces { get; set; }

	public int TOP_pieces { get; set; }

	public string MTO_scores { get; set; }

	public string SQD_scores { get; set; }

	public string PLU_scores { get; set; }

	public string CNN_scores { get; set; }

	public string CBL_scores { get; set; }

	public string BAT_scores { get; set; }

	public string SPR_scores { get; set; }

	public string BAL_scores { get; set; }

	public string GOT_scores { get; set; }

	public string PPR_scores { get; set; }

	public string SNW_scores { get; set; }

	public string MNK_scores { get; set; }

	public string ZIP_scores { get; set; }

	public string FTL_scores { get; set; }

	public string SQR_scores { get; set; }

	public string FLA_scores { get; set; }

	public string FRO_scores { get; set; }

	public string PNG_scores { get; set; }

	public string SEA_scores { get; set; }

	public string SWO_scores { get; set; }

	public string BEE_scores { get; set; }

	public string DUC_scores { get; set; }

	public string CRA_scores { get; set; }

	public string WGN_scores { get; set; }

	public string SHI_scores { get; set; }

	public string HEL_scores { get; set; }

	public string RAT_scores { get; set; }

	public string PLA_scores { get; set; }

	public string SNK_scores { get; set; }

	public string SKI_scores { get; set; }

	public string BWL_scores { get; set; }

	public string TOP_scores { get; set; }

	public string MTO_enabledSkins { get; set; }

	public string SQD_enabledSkins { get; set; }

	public string PLU_enabledSkins { get; set; }

	public string CNN_enabledSkins { get; set; }

	public string CBL_enabledSkins { get; set; }

	public string BAT_enabledSkins { get; set; }

	public string SPR_enabledSkins { get; set; }

	public string BAL_enabledSkins { get; set; }

	public string GOT_enabledSkins { get; set; }

	public string PPR_enabledSkins { get; set; }

	public string SNW_enabledSkins { get; set; }

	public string MNK_enabledSkins { get; set; }

	public string ZIP_enabledSkins { get; set; }

	public string FTL_enabledSkins { get; set; }

	public string SQR_enabledSkins { get; set; }

	public string FLA_enabledSkins { get; set; }

	public string FRO_enabledSkins { get; set; }

	public string PNG_enabledSkins { get; set; }

	public string SEA_enabledSkins { get; set; }

	public string SWO_enabledSkins { get; set; }

	public string BEE_enabledSkins { get; set; }

	public string DUC_enabledSkins { get; set; }

	public string CRA_enabledSkins { get; set; }

	public string WGN_enabledSkins { get; set; }

	public string SHI_enabledSkins { get; set; }

	public string HEL_enabledSkins { get; set; }

	public string RAT_enabledSkins { get; set; }

	public string PLA_enabledSkins { get; set; }

	public string SNK_enabledSkins { get; set; }

	public string SKI_enabledSkins { get; set; }

	public string BWL_enabledSkins { get; set; }

	public string TOP_enabledSkins { get; set; }

	public int MTO_enabledMinigame { get; set; }

	public int SQD_enabledMinigame { get; set; }

	public int PLU_enabledMinigame { get; set; }

	public int CNN_enabledMinigame { get; set; }

	public int CBL_enabledMinigame { get; set; }

	public int BAT_enabledMinigame { get; set; }

	public int SPR_enabledMinigame { get; set; }

	public int BAL_enabledMinigame { get; set; }

	public int GOT_enabledMinigame { get; set; }

	public int PPR_enabledMinigame { get; set; }

	public int SNW_enabledMinigame { get; set; }

	public int MNK_enabledMinigame { get; set; }

	public int ZIP_enabledMinigame { get; set; }

	public int FTL_enabledMinigame { get; set; }

	public int SQR_enabledMinigame { get; set; }

	public int FLA_enabledMinigame { get; set; }

	public int FRO_enabledMinigame { get; set; }

	public int PNG_enabledMinigame { get; set; }

	public int SEA_enabledMinigame { get; set; }

	public int SWO_enabledMinigame { get; set; }

	public int BEE_enabledMinigame { get; set; }

	public int DUC_enabledMinigame { get; set; }

	public int CRA_enabledMinigame { get; set; }

	public int WGN_enabledMinigame { get; set; }

	public int SHI_enabledMinigame { get; set; }

	public int HEL_enabledMinigame { get; set; }

	public int RAT_enabledMinigame { get; set; }

	public int PLA_enabledMinigame { get; set; }

	public int SNK_enabledMinigame { get; set; }

	public int SKI_enabledMinigame { get; set; }

	public int BWL_enabledMinigame { get; set; }

	public int TOP_enabledMinigame { get; set; }

	public object this[string propertyName]
	{
		get
		{
			return GetType().GetProperty(propertyName).GetValue(this, null);
		}
		set
		{
			GetType().GetProperty(propertyName).SetValue(this, value, null);
		}
	}

	public void Save(string path)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveData));
		using (StreamWriter textWriter = new StreamWriter(path))
		{
			xmlSerializer.Serialize(textWriter, this);
		}
	}

	public static SaveData Load(string path)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveData));
		using (StreamReader textReader = new StreamReader(path))
		{
			return xmlSerializer.Deserialize(textReader) as SaveData;
		}
	}
}
