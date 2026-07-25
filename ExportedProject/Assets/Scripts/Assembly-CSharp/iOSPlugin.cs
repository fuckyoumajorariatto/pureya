using UnityEngine;

public class iOSPlugin : MonoBehaviour
{
	public enum BatteryStatus
	{
		UIDeviceBatteryStateUnknown = 0,
		UIDeviceBatteryStateUnplugged = 1,
		UIDeviceBatteryStateCharging = 2,
		UIDeviceBatteryStateFull = 3
	}

	private const string NOT_SUPPORTED = "not supported on this platform";

	public static void ShowAlert(string title, string message)
	{
	}

	public static void ShowAlertConfirmation(string title, string message)
	{
	}

	public static void ShareMessage(string title, string url = "")
	{
	}

	public static int GetBatteryStatus()
	{
		return 0;
	}

	public static void GetBatteryLevel()
	{
	}

	public static string iCloudGetValue(string key)
	{
		return string.Empty;
	}

	public static bool iCloudSaveValue(string key, string value)
	{
		return false;
	}

	public static int iCloudGetIntValue(string key)
	{
		return 0;
	}

	public static bool iCloudSaveIntValue(string key, int value)
	{
		return false;
	}

	public static bool iCloudGetBoolValue(string key)
	{
		return false;
	}

	public static bool iCloudSaveBoolValue(string key, bool value)
	{
		return false;
	}
}
