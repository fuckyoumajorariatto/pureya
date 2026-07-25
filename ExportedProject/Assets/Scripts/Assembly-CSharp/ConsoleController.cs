using UnityEngine;
using UnityEngine.UI;

public class ConsoleController : MonoBehaviour
{
	public static ConsoleController consoleController;

	public Text txt;

	public GameObject canvas;

	public bool active;

	private string[] lines = new string[20];

	private string allLines;

	private void Start()
	{
		consoleController = this;
		Object.DontDestroyOnLoad(this);
	}

	public void ShowText(string t)
	{
		if (active)
		{
			if (!canvas.activeSelf)
			{
				canvas.SetActive(true);
			}
			for (int i = 0; i < lines.Length - 1; i++)
			{
				lines[i] = string.Empty;
			}
			txt.text = t;
		}
	}

	public void AddText(string t)
	{
		if (!active)
		{
			return;
		}
		for (int i = 0; i < lines.Length - 1; i++)
		{
			lines[i] = lines[i + 1];
		}
		lines[lines.Length - 1] = t;
		if (!canvas.activeSelf)
		{
			canvas.SetActive(true);
		}
		allLines = string.Empty;
		for (int j = 0; j < lines.Length; j++)
		{
			if (lines[j] != string.Empty && lines[j] != null)
			{
				if (j == 0)
				{
					allLines += lines[j];
				}
				else
				{
					allLines = allLines + "\n" + lines[j];
				}
			}
		}
		txt.text = allLines;
	}

	public void ClearText()
	{
		if (active)
		{
			txt.text = string.Empty;
			for (int i = 0; i < lines.Length - 1; i++)
			{
				lines[i] = string.Empty;
			}
		}
	}
}
