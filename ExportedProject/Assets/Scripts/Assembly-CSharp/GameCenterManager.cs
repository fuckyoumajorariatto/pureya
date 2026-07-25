using UnityEngine;

public class GameCenterManager : MonoBehaviour
{
	private static GameCenterManager instance;

	public iCloudManager cloudManager;

	public int loginStatus;

	public bool initialDataLoaded;

	public bool defaultSaveLoaded;

	private string m_saveFileName = "save";

	public static GameCenterManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<GameCenterManager>();
				if (instance == null)
				{
					instance = new GameObject("GameCenterManager").AddComponent<GameCenterManager>();
				}
			}
			return instance;
		}
	}

	private void Start()
	{
		Authenticate();
	}

	private void Authenticate()
	{
		loginStatus = 2;
		defaultSaveLoaded = true;
		initialDataLoaded = true;
	}

	private void ProcessAuthentication(bool success)
	{
		if (success)
		{
			loginStatus = 1;
		}
		else
		{
			loginStatus = 2;
		}
	}

	public void UnlockAchievements()
	{
	}
}
