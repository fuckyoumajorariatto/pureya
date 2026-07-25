using UnityEngine;

public class AchievementsController : MonoBehaviour
{
	public static AchievementsController achievementsController;

	private SteamStatsAndAchievements m_StatsAndAchievements;

	private void Start()
	{
		achievementsController = this;
	}

	public void UnlockAchievements()
	{
		if (GlobalControl.globalControl.steam)
		{
			if (m_StatsAndAchievements == null)
			{
				m_StatsAndAchievements = Object.FindObjectOfType<SteamStatsAndAchievements>();
			}
			if (GlobalControl.globalControl._111)
			{
				m_StatsAndAchievements._111 = true;
			}
			if (GlobalControl.globalControl._222)
			{
				m_StatsAndAchievements._222 = true;
			}
			if (GlobalControl.globalControl._333)
			{
				m_StatsAndAchievements._333 = true;
			}
			if (GlobalControl.globalControl._444)
			{
				m_StatsAndAchievements._444 = true;
			}
			if (GlobalControl.globalControl._555)
			{
				m_StatsAndAchievements._555 = true;
			}
			if (GlobalControl.globalControl._666)
			{
				m_StatsAndAchievements._666 = true;
			}
			if (GlobalControl.globalControl._777)
			{
				m_StatsAndAchievements._777 = true;
			}
			if (GlobalControl.globalControl._888)
			{
				m_StatsAndAchievements._888 = true;
			}
			if (GlobalControl.globalControl._999)
			{
				m_StatsAndAchievements._999 = true;
			}
		}
	}
}
