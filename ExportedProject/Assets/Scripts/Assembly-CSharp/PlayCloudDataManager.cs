using System;
using System.Collections;
using System.Text;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class PlayCloudDataManager : MonoBehaviour
{
	public bool googlePlay;

	private static PlayCloudDataManager instance;

	public Action<string> afterLoadAction;

	private int loginStatus;

	private bool startLoadAll;

	private int cloudLoadIndex;

	private int previousCloudLoadIndex;

	private int firstSaveFile;

	private int lastSaveFile;

	private bool loadingAll;

	public bool initialDataLoaded;

	public bool defaultSaveLoaded;

	private bool loadingDefaultSave;

	private int retries;

	private int t_cloudTimesSaved;

	private string t_dataToSave;

	private int t_saveFile;

	public bool dontUploadDefaultSave;

	private string m_saveFileName = "save";

	private bool uploadSave;

	private bool updateDefaultSave;

	private string dataToUpload;

	private int saveToUpload;

	private int timesSaved;

	public static PlayCloudDataManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType<PlayCloudDataManager>();
				if (instance == null)
				{
					instance = new GameObject("PlayGameCloudData").AddComponent<PlayCloudDataManager>();
				}
			}
			return instance;
		}
	}

	public bool isProcessing { get; private set; }

	public string loadedData { get; private set; }

	public bool isAuthenticated
	{
		get
		{
			return false;
		}
	}

	private void Awake()
	{
		defaultSaveLoaded = true;
		initialDataLoaded = true;
	}

	private void Start()
	{
		Login();
		afterLoadAction = (Action<string>)Delegate.Combine(afterLoadAction, new Action<string>(SaveLoadedData));
	}

	public void Login()
	{
		loginStatus = 2;
		defaultSaveLoaded = true;
		initialDataLoaded = true;
	}

	private void ProcessCloudData(byte[] cloudData)
	{
		if (cloudData != null)
		{
			string text = BytesToString(cloudData);
			loadedData = text;
		}
	}

	public void LoadFile(string filename)
	{
		m_saveFileName = filename;
		LoadFromCloud(afterLoadAction);
	}

	public void LoadFromCloud(Action<string> afterLoadAction)
	{
		if (isAuthenticated && !isProcessing)
		{
			StartCoroutine(LoadFromCloudRoutine(afterLoadAction));
		}
	}

	public void RetryLoadFromCloud(Action<string> afterLoadAction)
	{
		if (isAuthenticated)
		{
			StartCoroutine(LoadFromCloudRoutine(afterLoadAction));
		}
	}

	private IEnumerator LoadFromCloudRoutine(Action<string> loadAction)
	{
		isProcessing = true;
		while (SavingIconsController.savingIconsController == null)
		{
			yield return new WaitForEndOfFrame();
		}
		SavingIconsController.savingIconsController.ChangeCloudToDownloading();
		SavingIconsController.savingIconsController.ShowCloud();
		while (isProcessing)
		{
			yield return null;
		}
		loadAction(loadedData);
	}

	public void SaveToCloud(string dataToSave, int saveFile)
	{
		if (isAuthenticated)
		{
			t_dataToSave = dataToSave;
			t_saveFile = saveFile;
			loadedData = dataToSave;
			isProcessing = true;
			SavingIconsController.savingIconsController.ChangeCloudToUploading();
			SavingIconsController.savingIconsController.ShowCloud();
		}
	}

	private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			if (retries > 0)
			{
				retries--;
			}
			byte[] array = StringToBytes(loadedData);
			SavedGameMetadataUpdate savedGameMetadataUpdate = default(SavedGameMetadataUpdate.Builder).Build();
		}
		else if (retries < 3)
		{
			SaveToCloud(t_dataToSave, t_saveFile);
			retries++;
		}
		else
		{
			isProcessing = false;
			SavingIconsController.savingIconsController.HideCloud();
			retries--;
		}
	}

	private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			retries = 0;
		}
		else if (retries < 3)
		{
			retries++;
			RetryLoadFromCloud(afterLoadAction);
		}
		else
		{
			isProcessing = false;
			SavingIconsController.savingIconsController.HideCloud();
			retries = 0;
		}
	}

	private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			ProcessCloudData(bytes);
		}
		isProcessing = false;
		SavingIconsController.savingIconsController.HideCloud();
	}

	private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
	{
		if (status != SavedGameRequestStatus.Success)
		{
		}
		isProcessing = false;
		SavingIconsController.savingIconsController.HideCloud();
	}

	private byte[] StringToBytes(string stringToConvert)
	{
		return Encoding.UTF8.GetBytes(stringToConvert);
	}

	private string BytesToString(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes);
	}

	public void Save(int saveFile, string s)
	{
		if (isAuthenticated)
		{
			SaveToCloud(s, saveFile);
		}
	}

	private void Update()
	{
		if (isProcessing)
		{
			return;
		}
		if (startLoadAll)
		{
			if (loginStatus == 1)
			{
				startLoadAll = false;
				loadingAll = true;
				previousCloudLoadIndex = -1;
			}
			else
			{
				startLoadAll = false;
			}
		}
		else if (loadingAll)
		{
			if (cloudLoadIndex != previousCloudLoadIndex)
			{
				if (cloudLoadIndex <= lastSaveFile)
				{
					previousCloudLoadIndex = cloudLoadIndex;
					m_saveFileName = "save" + cloudLoadIndex;
					LoadFromCloud(afterLoadAction);
				}
				else
				{
					initialDataLoaded = true;
					loadingAll = false;
					SaveLoadManager.saveLoadManager.ReplaceDefaultSave(GlobalControl.globalControl.profile, t_cloudTimesSaved);
					dontUploadDefaultSave = false;
					SaveLoadManager.saveLoadManager.LoadDefaultSave();
				}
			}
		}
		else if (uploadSave)
		{
			Save(saveToUpload, dataToUpload);
			uploadSave = false;
			updateDefaultSave = true;
		}
		else if (updateDefaultSave)
		{
			dataToUpload = timesSaved + "-timesSaved-" + dataToUpload;
			Save(0, dataToUpload);
			updateDefaultSave = false;
		}
	}

	private bool IsLocalSaveNewer(int cloudTimesSaved)
	{
		SaveLoadManager.saveLoadManager.LoadDefaultSave();
		if (GlobalControl.globalControl.timesSaved >= cloudTimesSaved)
		{
			return true;
		}
		return false;
	}

	public void LoadDefaultSave()
	{
		loadingDefaultSave = true;
		m_saveFileName = "save0";
		LoadFromCloud(afterLoadAction);
	}

	public void StartLoadAll(int t_firstSaveFile, int t_lastSaveFile)
	{
		firstSaveFile = t_firstSaveFile;
		lastSaveFile = t_lastSaveFile;
		cloudLoadIndex = firstSaveFile;
		previousCloudLoadIndex = cloudLoadIndex;
		startLoadAll = true;
	}

	private void SaveLoadedData(string s)
	{
		if (loadingDefaultSave)
		{
			loadingDefaultSave = false;
			defaultSaveLoaded = true;
			if (s == null)
			{
				return;
			}
			if (s.Length > 0 && s != string.Empty)
			{
				string[] array = s.Split(new string[1] { "-timesSaved-" }, StringSplitOptions.None);
				string text = array[0];
				string empty = string.Empty;
				int num = 0;
				int result;
				if (text.Length > 0 && int.TryParse(text, out result))
				{
					num = result;
				}
				if (array.Length > 1)
				{
					empty = array[1];
					array = array[1].Split(new string[1] { "<profileNumber>" }, StringSplitOptions.None);
				}
				else
				{
					empty = array[0];
					array = array[0].Split(new string[1] { "<profileNumber>" }, StringSplitOptions.None);
				}
				int result2 = 1;
				if (array.Length > 1)
				{
					int.TryParse(array[1][0] + string.Empty, out result2);
				}
				else
				{
					array = new string[0];
				}
				if (array.Length > 1 && !IsLocalSaveNewer(num))
				{
					SaveLoad.saveLoad.WriteDataFromAndroidCloud(empty, result2);
					t_cloudTimesSaved = num;
					dontUploadDefaultSave = true;
					SaveLoadManager.saveLoadManager.ReplaceDefaultSave(result2, num - 10);
					SaveLoad.saveLoad.ReplaceFromAndroidCloud();
					return;
				}
			}
			initialDataLoaded = true;
		}
		else if (s == null)
		{
			SaveLoad.saveLoad.CopySaveToAndroidCloud(cloudLoadIndex, false);
			cloudLoadIndex++;
		}
		else
		{
			if (s.Length == 0)
			{
				SaveLoad.saveLoad.CopySaveToAndroidCloud(cloudLoadIndex, false);
			}
			else if (s[0] == '<')
			{
				SaveLoad.saveLoad.WriteDataFromAndroidCloud(s, cloudLoadIndex);
			}
			else
			{
				SaveLoad.saveLoad.CopySaveToAndroidCloud(cloudLoadIndex, false);
			}
			cloudLoadIndex++;
		}
	}

	public void ManageLocalSave(int saveFile, string data, int t_timesSaved)
	{
		if (!uploadSave && !updateDefaultSave)
		{
			saveToUpload = saveFile;
			dataToUpload = data;
			timesSaved = t_timesSaved;
			uploadSave = true;
		}
	}

	public void UpdateDefaultSave(string data, int t_timesSaved)
	{
		if (!uploadSave && !updateDefaultSave && !dontUploadDefaultSave)
		{
			saveToUpload = 0;
			dataToUpload = data;
			timesSaved = t_timesSaved;
			updateDefaultSave = true;
		}
	}

	public void UnlockAchievements()
	{
	}
}
