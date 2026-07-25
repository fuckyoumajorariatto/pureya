using UnityEngine;

public class PauseButtonsCanvasTransformController : MonoBehaviour
{
	private int previousWidth;

	private int previousHeight;

	private void Update()
	{
		ManageResolutionChange();
	}

	private void ManageResolutionChange()
	{
		if (previousWidth != Screen.width || previousHeight != Screen.height)
		{
			UpdateCanvasTransform();
		}
	}

	private void UpdateCanvasTransform()
	{
		if (Camera.main.aspect < 1f)
		{
			VerticalScreen();
		}
		else if (Camera.main.aspect >= 1f)
		{
			HorizontalScreen();
		}
		previousWidth = Screen.width;
		previousHeight = Screen.height;
	}

	private void VerticalScreen()
	{
		float num = (float)Screen.height / 8f;
		float num2 = (float)Screen.width / num;
		float y = 4f - num2 / 2f;
		base.transform.position = new Vector3(-100f, y, base.transform.position.z);
		base.transform.localScale = new Vector3(num2 / 8f, num2 / 8f, base.transform.localScale.z);
	}

	private void HorizontalScreen()
	{
		base.transform.position = new Vector3(-100f, 0f, base.transform.position.z);
		base.transform.localScale = new Vector3(1f, 1f, base.transform.localScale.z);
	}
}
