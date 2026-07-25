using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
	public static SaveLoad saveLoad;

	private bool portablePermissions;

	private string sharedSavesPath;

	private string persistentSavesPath;

	private string portableSavesPath;

	private bool filesOnPortable;

	private bool filesOnPersistent;

	private SteamManager steamManager;

	private int firstSaveFile = 1;

	private int lastSaveFile = 9;

	private bool initialized;

	private bool useShared;

	private void Start()
	{
		saveLoad = this;
		sharedSavesPath = Application.persistentDataPath + "/SharedSaves";
		persistentSavesPath = Application.persistentDataPath + "/Saves";
		portableSavesPath = Application.dataPath + "/../Saves";
		if (steamManager == null && GlobalControl.globalControl.steam)
		{
			steamManager = Object.FindObjectOfType<SteamManager>();
		}
		if (!IsSteamInitialized())
		{
			useShared = true;
			CheckPortablePermissions();
			EnsurePersistentPath();
			EnsureSharedPath();
			ManageMoveOldSaves();
			ManageCopyFromShared();
			UpdateSharedSaves();
		}
		initialized = true;
	}

	public bool IsInitialized()
	{
		return initialized;
	}

	private void CheckPortablePermissions()
	{
		portablePermissions = true;
		try
		{
			if (!Directory.Exists(portableSavesPath))
			{
				Directory.CreateDirectory(portableSavesPath);
			}
			string path = portableSavesPath + "/permissions_test.txt";
			SaveData saveData = new SaveData();
			saveData.Save(path);
			SaveData.Load(path);
			File.Delete(path);
		}
		catch
		{
			portablePermissions = false;
		}
	}

	private void EnsurePersistentPath()
	{
		if (!portablePermissions && !Directory.Exists(persistentSavesPath))
		{
			Directory.CreateDirectory(persistentSavesPath);
		}
	}

	private void EnsureSharedPath()
	{
		if (!Directory.Exists(sharedSavesPath))
		{
			Directory.CreateDirectory(sharedSavesPath);
		}
	}

	private void ManageMoveOldSaves()
	{
		bool flag = false;
		if (useShared)
		{
			for (int i = firstSaveFile; i <= lastSaveFile; i++)
			{
				if (File.Exists(Path.Combine(sharedSavesPath, "save" + i + ".xml")))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return;
		}
		if (useShared)
		{
			for (int j = firstSaveFile; j <= lastSaveFile; j++)
			{
				if (File.Exists(Path.Combine(persistentSavesPath + "/../", "save" + j + ".xml")))
				{
					File.Move(Path.Combine(persistentSavesPath + "/../", "save" + j + ".xml"), Path.Combine(sharedSavesPath, "save" + j + ".xml"));
				}
			}
			if (File.Exists(Path.Combine(persistentSavesPath + "/../", "defaultSave.xml")))
			{
				File.Move(Path.Combine(persistentSavesPath + "/../", "defaultSave.xml"), Path.Combine(sharedSavesPath, "defaultSave.xml"));
			}
			return;
		}
		for (int k = firstSaveFile; k <= lastSaveFile; k++)
		{
			if (File.Exists(Path.Combine(persistentSavesPath + "/../", "save" + k + ".xml")))
			{
				File.Move(Path.Combine(persistentSavesPath + "/../", "save" + k + ".xml"), Path.Combine(persistentSavesPath, "save" + k + ".xml"));
			}
		}
		if (File.Exists(Path.Combine(persistentSavesPath + "/../", "defaultSave.xml")))
		{
			File.Move(Path.Combine(persistentSavesPath + "/../", "defaultSave.xml"), Path.Combine(persistentSavesPath, "defaultSave.xml"));
		}
	}

	private void ManageCopyFromShared()
	{
		bool flag = false;
		for (int i = firstSaveFile; i <= lastSaveFile; i++)
		{
			if (File.Exists(Path.Combine(sharedSavesPath, "save" + i + ".xml")))
			{
				flag = true;
				break;
			}
		}
		for (int j = firstSaveFile; j <= lastSaveFile; j++)
		{
			if (File.Exists(Path.Combine(portableSavesPath, "save" + j + ".xml")))
			{
				filesOnPortable = true;
			}
			if (File.Exists(Path.Combine(persistentSavesPath, "save" + j + ".xml")))
			{
				filesOnPersistent = true;
			}
			if (filesOnPortable && filesOnPersistent)
			{
				break;
			}
		}
		if (portablePermissions && !filesOnPortable && flag)
		{
			for (int k = firstSaveFile; k <= lastSaveFile; k++)
			{
				if (File.Exists(Path.Combine(sharedSavesPath, "save" + k + ".xml")))
				{
					File.Copy(Path.Combine(sharedSavesPath, "save" + k + ".xml"), Path.Combine(portableSavesPath, "save" + k + ".xml"), true);
					filesOnPortable = true;
				}
			}
			if (filesOnPortable && File.Exists(Path.Combine(sharedSavesPath, "defaultSave.xml")))
			{
				File.Copy(Path.Combine(sharedSavesPath, "defaultSave.xml"), Path.Combine(portableSavesPath, "defaultSave.xml"), true);
			}
		}
		else
		{
			if (portablePermissions || filesOnPersistent || !flag)
			{
				return;
			}
			for (int l = firstSaveFile; l <= lastSaveFile; l++)
			{
				if (File.Exists(Path.Combine(sharedSavesPath, "save" + l + ".xml")))
				{
					File.Copy(Path.Combine(sharedSavesPath, "save" + l + ".xml"), Path.Combine(persistentSavesPath, "save" + l + ".xml"), true);
					filesOnPersistent = true;
				}
			}
			if (filesOnPersistent && File.Exists(Path.Combine(sharedSavesPath, "defaultSave.xml")))
			{
				File.Copy(Path.Combine(sharedSavesPath, "defaultSave.xml"), Path.Combine(persistentSavesPath, "defaultSave.xml"), true);
			}
		}
	}

	private void UpdateSharedSaves()
	{
		if (portablePermissions)
		{
			if (filesOnPortable)
			{
				ReplaceSharedSavesForPortableSaves();
			}
		}
		else if (!IsSteamInitialized() && filesOnPersistent)
		{
			ReplaceSharedSavesForPersistentSaves();
		}
	}

	private void ReplaceSharedSavesForPortableSaves()
	{
		for (int i = firstSaveFile; i <= lastSaveFile; i++)
		{
			if (File.Exists(Path.Combine(sharedSavesPath, "save" + i + ".xml")))
			{
				File.Delete(Path.Combine(sharedSavesPath, "save" + i + ".xml"));
			}
		}
		if (File.Exists(Path.Combine(sharedSavesPath, "defaultSave.xml")))
		{
			File.Delete(Path.Combine(sharedSavesPath, "defaultSave.xml"));
		}
		for (int j = firstSaveFile; j <= lastSaveFile; j++)
		{
			if (File.Exists(Path.Combine(portableSavesPath, "save" + j + ".xml")))
			{
				File.Copy(Path.Combine(portableSavesPath, "save" + j + ".xml"), Path.Combine(sharedSavesPath, "save" + j + ".xml"), true);
			}
		}
		if (File.Exists(Path.Combine(portableSavesPath, "defaultSave.xml")))
		{
			File.Copy(Path.Combine(portableSavesPath, "defaultSave.xml"), Path.Combine(sharedSavesPath, "defaultSave.xml"), true);
		}
	}

	private void ReplaceSharedSavesForPersistentSaves()
	{
		for (int i = firstSaveFile; i <= lastSaveFile; i++)
		{
			if (File.Exists(Path.Combine(sharedSavesPath, "save" + i + ".xml")))
			{
				File.Delete(Path.Combine(sharedSavesPath, "save" + i + ".xml"));
			}
		}
		if (File.Exists(Path.Combine(sharedSavesPath, "defaultSave.xml")))
		{
			File.Delete(Path.Combine(sharedSavesPath, "defaultSave.xml"));
		}
		for (int j = firstSaveFile; j <= lastSaveFile; j++)
		{
			if (File.Exists(Path.Combine(persistentSavesPath, "save" + j + ".xml")))
			{
				File.Copy(Path.Combine(persistentSavesPath, "save" + j + ".xml"), Path.Combine(sharedSavesPath, "save" + j + ".xml"), true);
			}
		}
		if (File.Exists(Path.Combine(persistentSavesPath, "defaultSave.xml")))
		{
			File.Copy(Path.Combine(persistentSavesPath, "defaultSave.xml"), Path.Combine(sharedSavesPath, "defaultSave.xml"), true);
		}
	}

	public void ReplaceFromAndroidCloud()
	{
		PlayCloudDataManager.Instance.StartLoadAll(firstSaveFile, lastSaveFile);
	}

	public void WriteData(SaveData data, int saveFile)
	{
		if (IsSteamInitialized())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, data);
				array = memoryStream.ToArray();
			}
			steamManager.SaveCloudData("save" + saveFile + ".xml", array, array.Length);
			return;
		}
		if (portablePermissions)
		{
			try
			{
				data.Save(Path.Combine(portableSavesPath, "save" + saveFile + ".xml"));
			}
			catch
			{
			}
		}
		else
		{
			data.Save(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml"));
		}
		if (useShared)
		{
			data.Save(Path.Combine(sharedSavesPath, "save" + saveFile + ".xml"));
		}
	}

	public void CopySaveToAndroidCloud(int saveFile, bool defaultSave)
	{
		if (File.Exists(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml")))
		{
			string text = ReadSave(saveFile);
			if (defaultSave)
			{
				saveFile = 0;
				text = GlobalControl.globalControl.timesSaved + "-timesSaved-" + text;
			}
			PlayCloudDataManager.Instance.Save(saveFile, text);
		}
		else
		{
			string text2 = string.Empty;
			if (saveFile == 0)
			{
				text2 = GlobalControl.globalControl.timesSaved + "-timesSaved-" + text2;
			}
			PlayCloudDataManager.Instance.Save(saveFile, text2);
		}
	}

	public void CopySaveToiCloud(int saveFile, bool defaultSave)
	{
		if (File.Exists(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml")))
		{
			string text = ReadSave(saveFile);
			if (defaultSave)
			{
				saveFile = 0;
				text = GlobalControl.globalControl.timesSaved + "-timesSaved-" + text;
			}
		}
		else
		{
			string empty = string.Empty;
			if (saveFile == 0)
			{
				empty = GlobalControl.globalControl.timesSaved + "-timesSaved-" + empty;
			}
		}
	}

	public void WriteDataFromAndroidCloud(string s, int saveFile)
	{
		StreamWriter streamWriter = new StreamWriter(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml"));
		streamWriter.Write(s);
		streamWriter.Close();
	}

	public string ReadSave(int saveFile)
	{
		if (File.Exists(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml")))
		{
			StreamReader streamReader = new StreamReader(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml"));
			string text = string.Empty;
			string empty = string.Empty;
			while ((empty = streamReader.ReadLine()) != null)
			{
				text = text + empty + "\n";
			}
			streamReader.Close();
			return text;
		}
		return null;
	}

	public SaveData Load(int saveFile)
	{
		if (IsSteamInitialized())
		{
			if (steamManager.FileExists("save" + saveFile + ".xml"))
			{
				try
				{
					byte[] array = steamManager.LoadCloudData("save" + saveFile + ".xml");
					using (MemoryStream memoryStream = new MemoryStream())
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						memoryStream.Write(array, 0, array.Length);
						memoryStream.Seek(0L, SeekOrigin.Begin);
						return (SaveData)binaryFormatter.Deserialize(memoryStream);
					}
				}
				catch
				{
				}
			}
		}
		else if (portablePermissions && File.Exists(Path.Combine(portableSavesPath, "save" + saveFile + ".xml")))
		{
			try
			{
				return SaveData.Load(Path.Combine(portableSavesPath, "save" + saveFile + ".xml"));
			}
			catch
			{
			}
		}
		else if (!portablePermissions && File.Exists(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml")))
		{
			try
			{
				return SaveData.Load(Path.Combine(persistentSavesPath, "save" + saveFile + ".xml"));
			}
			catch
			{
			}
		}
		return null;
	}

	public void WriteDefaultSave(DefaultSave data)
	{
		if (IsSteamInitialized())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, data);
				array = memoryStream.ToArray();
			}
			steamManager.SaveCloudData("defaultSave.xml", array, array.Length);
			return;
		}
		if (portablePermissions)
		{
			try
			{
				data.Save(Path.Combine(portableSavesPath, "defaultSave.xml"));
			}
			catch
			{
			}
		}
		else
		{
			data.Save(Path.Combine(persistentSavesPath, "defaultSave.xml"));
		}
		if (useShared)
		{
			data.Save(Path.Combine(sharedSavesPath, "defaultSave.xml"));
		}
	}

	public DefaultSave LoadDefaultSave()
	{
		if (IsSteamInitialized())
		{
			if (steamManager.FileExists("defaultSave.xml"))
			{
				try
				{
					byte[] array = steamManager.LoadCloudData("defaultSave.xml");
					using (MemoryStream memoryStream = new MemoryStream())
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						memoryStream.Write(array, 0, array.Length);
						memoryStream.Seek(0L, SeekOrigin.Begin);
						return (DefaultSave)binaryFormatter.Deserialize(memoryStream);
					}
				}
				catch
				{
				}
			}
		}
		else if (portablePermissions && File.Exists(Path.Combine(portableSavesPath, "defaultSave.xml")))
		{
			try
			{
				return DefaultSave.Load(Path.Combine(portableSavesPath, "defaultSave.xml"));
			}
			catch
			{
			}
		}
		else if (!portablePermissions && File.Exists(Path.Combine(persistentSavesPath, "defaultSave.xml")))
		{
			return DefaultSave.Load(Path.Combine(persistentSavesPath, "defaultSave.xml"));
		}
		return null;
	}

	private bool IsSteamInitialized()
	{
		if (GlobalControl.globalControl.steam)
		{
			return SteamManager.Initialized;
		}
		return false;
	}

	public void DeleteAll()
	{
		for (int i = firstSaveFile; i <= lastSaveFile; i++)
		{
			int num = i;
			if (portablePermissions)
			{
				try
				{
					if (File.Exists(Path.Combine(portableSavesPath, "save" + num + ".xml")))
					{
						File.Delete(Path.Combine(portableSavesPath, "save" + num + ".xml"));
					}
					if (File.Exists(Path.Combine(portableSavesPath, "defaultSave.xml")))
					{
						File.Delete(Path.Combine(portableSavesPath, "defaultSave.xml"));
					}
				}
				catch
				{
				}
			}
			else
			{
				if (File.Exists(Path.Combine(persistentSavesPath, "save" + num + ".xml")))
				{
					File.Delete(Path.Combine(persistentSavesPath, "save" + num + ".xml"));
				}
				if (File.Exists(Path.Combine(persistentSavesPath, "defaultSave.xml")))
				{
					File.Delete(Path.Combine(persistentSavesPath, "defaultSave.xml"));
				}
			}
			if (useShared)
			{
				if (File.Exists(Path.Combine(sharedSavesPath, "save" + num + ".xml")))
				{
					File.Delete(Path.Combine(sharedSavesPath, "save" + num + ".xml"));
				}
				if (File.Exists(Path.Combine(sharedSavesPath, "defaultSave.xml")))
				{
					File.Delete(Path.Combine(sharedSavesPath, "defaultSave.xml"));
				}
			}
		}
	}
}
