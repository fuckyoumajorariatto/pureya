using UnityEngine;

public class iCloudManager : MonoBehaviour
{
	private static iCloudManager instance;

	private string m_saveFileName = "save";

	private bool dontUploadDefaultSave;

	public bool initialDataLoaded;

	public static iCloudManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<iCloudManager>();
				if (instance == null)
				{
					instance = new GameObject("iCloudManager").AddComponent<iCloudManager>();
				}
			}
			return instance;
		}
	}

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		if (GlobalControl.globalControl == null || GameCenterManager.Instance == null)
		{
			Invoke("Initialize", 0.1f);
		}
		else if (GameCenterManager.Instance.loginStatus == 0)
		{
			Invoke("Initialize", 0.1f);
		}
		else
		{
			initialDataLoaded = true;
		}
	}
}
