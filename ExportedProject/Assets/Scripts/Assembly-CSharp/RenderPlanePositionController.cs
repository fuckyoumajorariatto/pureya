using UnityEngine;

public class RenderPlanePositionController : MonoBehaviour
{
	private float aspectRatio;

	private float pixelsPerUnit;

	private float screenSizeX;

	private float panelCenter;

	private float scale;

	private Vector2 safeA;

	private void Start()
	{
		UpdateTransform();
	}

	public void UpdateTransform()
	{
		if (ScreenResolutionController.screenResolutionController == null)
		{
			Invoke("UpdateTransform", 0.1f);
			return;
		}
		aspectRatio = ScreenResolutionController.screenResolutionController.GetSafeAreaResolution().x / ScreenResolutionController.screenResolutionController.GetSafeAreaResolution().y;
		if (aspectRatio < 1f)
		{
			VerticalScreen();
		}
		else if (aspectRatio >= 1f)
		{
			HorizontalScreen();
		}
	}

	private void VerticalScreen()
	{
		pixelsPerUnit = (float)Screen.height / 8f;
		screenSizeX = ScreenResolutionController.screenResolutionController.GetSafeAreaResolution().x / pixelsPerUnit;
		panelCenter = 4f - ScreenResolutionController.screenResolutionController.GetScreenOrigin().y - screenSizeX / 2f;
		scale = aspectRatio * ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y;
		scale *= 0.1f;
		base.transform.localScale = new Vector3(scale, 1f, scale);
		base.transform.position = new Vector3(-100f + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, panelCenter, base.transform.position.z);
	}

	private void HorizontalScreen()
	{
		scale = ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y * 0.1f;
		base.transform.localScale = new Vector3(scale, 1f, scale);
		base.transform.position = new Vector3(-100f + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, 0f - ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().y, base.transform.position.z);
	}
}
