using UnityEngine;

public class TransitionPlanesTransformController : MonoBehaviour
{
	private float aspectRatio;

	private float scale;

	private float pixelsPerUnit;

	private float screenSizeX;

	private float panelCenter;

	private float safeCameraSize;

	public void UpdateTransform()
	{
		aspectRatio = (float)Screen.width / (float)Screen.height;
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
		panelCenter = 4f - screenSizeX / 2f;
		scale = ScreenResolutionController.screenResolutionController.GetSafeAreaResolution().x / (float)Screen.height;
		base.transform.localScale = new Vector2(scale, scale);
		base.transform.position = new Vector3(-100f + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, panelCenter - ScreenResolutionController.screenResolutionController.GetScreenOrigin().y, base.transform.position.z);
	}

	private void HorizontalScreen()
	{
		base.transform.position = new Vector3(-100f + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, 0f - ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().y, base.transform.position.z);
		scale = ScreenResolutionController.screenResolutionController.GetSafeAreaResolution().y / (float)Screen.height;
		base.transform.localScale = new Vector3(scale, scale);
	}
}
