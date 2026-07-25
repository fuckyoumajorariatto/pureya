using Steamworks;
using UnityEngine;

internal class SteamStatsAndAchievements : MonoBehaviour
{
	private enum Achievement
	{
		_111 = 0,
		_222 = 1,
		_333 = 2,
		_444 = 3,
		_555 = 4,
		_666 = 5,
		_777 = 6,
		_888 = 7,
		_999 = 8
	}

	private class Achievement_t
	{
		public Achievement m_eAchievementID;

		public string m_strName;

		public string m_strDescription;

		public bool m_bAchieved;

		public Achievement_t(Achievement achievementID, string name, string desc)
		{
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}

	public bool _111;

	public bool _222;

	public bool _333;

	public bool _444;

	public bool _555;

	public bool _666;

	public bool _777;

	public bool _888;

	public bool _999;

	private Achievement_t[] m_Achievements = new Achievement_t[9]
	{
		new Achievement_t(Achievement._111, string.Empty, string.Empty),
		new Achievement_t(Achievement._222, string.Empty, string.Empty),
		new Achievement_t(Achievement._333, string.Empty, string.Empty),
		new Achievement_t(Achievement._444, string.Empty, string.Empty),
		new Achievement_t(Achievement._555, string.Empty, string.Empty),
		new Achievement_t(Achievement._666, string.Empty, string.Empty),
		new Achievement_t(Achievement._777, string.Empty, string.Empty),
		new Achievement_t(Achievement._888, string.Empty, string.Empty),
		new Achievement_t(Achievement._999, string.Empty, string.Empty)
	};

	private CGameID m_GameID;

	private bool m_bRequestedStats;

	private bool m_bStoreStats;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			m_GameID = new CGameID(SteamUtils.GetAppID());
			m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
			m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
			m_bRequestedStats = false;
		}
	}

	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!m_bRequestedStats)
		{
			if (!SteamManager.Initialized)
			{
				m_bRequestedStats = true;
				return;
			}
			bool bRequestedStats = SteamUserStats.RequestCurrentStats();
			m_bRequestedStats = bRequestedStats;
		}
		Achievement_t[] achievements = m_Achievements;
		foreach (Achievement_t achievement_t in achievements)
		{
			if (achievement_t.m_bAchieved)
			{
				continue;
			}
			switch (achievement_t.m_eAchievementID)
			{
			case Achievement._111:
				if (_111)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._222:
				if (_222)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._333:
				if (_333)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._444:
				if (_444)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._555:
				if (_555)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._666:
				if (_666)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._777:
				if (_777)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._888:
				if (_888)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			case Achievement._999:
				if (_999)
				{
					UnlockAchievement(achievement_t);
				}
				break;
			}
		}
		if (m_bStoreStats)
		{
			bool flag = SteamUserStats.StoreStats();
			m_bStoreStats = !flag;
		}
	}

	private void UnlockAchievement(Achievement_t achievement)
	{
		achievement.m_bAchieved = true;
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
		m_bStoreStats = true;
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized || (ulong)m_GameID != pCallback.m_nGameID)
		{
			return;
		}
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log("Received stats and achievements from Steam\n");
			Achievement_t[] achievements = m_Achievements;
			foreach (Achievement_t achievement_t in achievements)
			{
				if (SteamUserStats.GetAchievement(achievement_t.m_eAchievementID.ToString(), out achievement_t.m_bAchieved))
				{
					achievement_t.m_strName = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "name");
					achievement_t.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "desc");
				}
				else
				{
					Debug.LogWarning(string.Concat("SteamUserStats.GetAchievement failed for Achievement ", achievement_t.m_eAchievementID, "\nIs it registered in the Steam Partner site?"));
				}
			}
		}
		else
		{
			Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
		}
	}

	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_nMaxProgress == 0)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
				return;
			}
			Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
		}
	}
}
