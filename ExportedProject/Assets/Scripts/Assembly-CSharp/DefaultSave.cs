using System;
using System.IO;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("DefaultSave")]
public class DefaultSave
{
	public int defaultSave;

	public int timesSaved;

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
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(DefaultSave));
		using (StreamWriter textWriter = new StreamWriter(path))
		{
			xmlSerializer.Serialize(textWriter, this);
		}
	}

	public static DefaultSave Load(string path)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(DefaultSave));
		using (StreamReader textReader = new StreamReader(path))
		{
			return xmlSerializer.Deserialize(textReader) as DefaultSave;
		}
	}
}
