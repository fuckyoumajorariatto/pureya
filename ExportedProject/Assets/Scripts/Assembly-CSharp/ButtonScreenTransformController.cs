using UnityEngine;

public class ButtonScreenTransformController : MonoBehaviour
{
	private float pixelsPerUnit;

	private float screenSizeX;

	private float panelCenter;

	private float aspectRatio;

	private float scale;

	public void UpdateButtonTransform()
	{
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
		pixelsPerUnit = ScreenResolutionController.screenResolutionController.GetPixelsPerUnit();
		screenSizeX = ScreenResolutionController.screenResolutionController.GetSafeAreaResolution().x / pixelsPerUnit;
		panelCenter = 4f - ScreenResolutionController.screenResolutionController.GetScreenOrigin().y - screenSizeX / 2f;
		base.transform.position = new Vector3(-100f, panelCenter, base.transform.position.z);
		base.transform.localScale = new Vector3(screenSizeX, screenSizeX, base.transform.localScale.z);
		scale = aspectRatio * ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y;
		base.transform.localScale = new Vector2(scale, scale);
		base.transform.position = new Vector3(-100f + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, panelCenter, base.transform.position.z);
	}

	private void HorizontalScreen()
	{
		scale = ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y;
		base.transform.localScale = new Vector2(scale, scale);
		base.transform.position = new Vector3(-100f + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, 0f - ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().y, base.transform.position.z);
	}
}
