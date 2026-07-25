using UnityEngine;

public class ButtonsTransformController : MonoBehaviour
{
	public Transform buttonLeftTR;

	public Transform buttonLeftArrowTR;

	public Transform buttonLeftBgTR;

	public SpriteRenderer buttonLeftBgSR;

	public Transform buttonRightTR;

	public Transform buttonRightArrowTR;

	public Transform buttonRightBgTR;

	public SpriteRenderer buttonRightBgSR;

	public Transform[] buttonSidesL;

	public Transform[] buttonSidesR;

	public Transform sidesContainerL;

	public Transform sidesContainerR;

	public GameObject sidesContainerLGO;

	public GameObject sidesContainerRGO;

	private float arrowScale;

	private bool hideMargins;

	public Sprite[] arrowsX1;

	public Sprite[] arrowsX075;

	public Sprite[] arrowsX05;

	public Sprite[] arrowsX025;

	private float scale;

	private float aspectRatio;

	private float pixelsPerUnit;

	private float screenSizeX;

	private float screenSizeY;

	private float margin;

	private float buttonY;

	private float buttonX;

	private float buttonBgScaleX;

	private float buttonBgScaleY;

	private float buttonCenterX;

	private float buttonCenterX2;

	private float buttonCenterY;

	private float buttonBgScale;

	private float panelCenter;

	public void UpdateButtonsTransform()
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
		UpdateSides(buttonSidesL, buttonLeftBgTR, buttonLeftBgSR, sidesContainerL, false);
		UpdateSides(buttonSidesR, buttonRightBgTR, buttonRightBgSR, sidesContainerR, true);
	}

	private void UpdateSides(Transform[] sides, Transform buttonTR, SpriteRenderer buttonSR, Transform sidesContainer, bool mirror)
	{
		if (mirror)
		{
			sidesContainer.localScale = new Vector3(1f, 1f, 1f);
		}
		scale = 0f;
		if (aspectRatio < 1f)
		{
			scale = aspectRatio;
		}
		else if (aspectRatio >= 1f)
		{
			scale = 1f;
		}
		for (int i = 0; i < sides.Length; i++)
		{
			Scale(sides[i], scale);
		}
		buttonLeftArrowTR.localScale = new Vector3(buttonLeftArrowTR.localScale.x * scale, buttonLeftArrowTR.localScale.y * scale, 1f);
		buttonRightArrowTR.localScale = new Vector3(buttonRightArrowTR.localScale.x * scale, buttonRightArrowTR.localScale.y * scale, 1f);
		sides[0].position = new Vector3(buttonTR.position.x - buttonSR.bounds.size.x / 2f, buttonTR.position.y + buttonSR.bounds.size.y / 2f, sides[0].position.z);
		sides[1].position = new Vector3(buttonTR.position.x, buttonTR.position.y + buttonSR.bounds.size.y / 2f, sides[1].position.z);
		sides[1].localScale = new Vector3(buttonTR.localScale.x / 2f, sides[1].localScale.y, sides[1].localScale.z);
		sides[2].position = new Vector3(buttonTR.position.x + buttonSR.bounds.size.x / 2f, buttonTR.position.y + buttonSR.bounds.size.y / 2f, sides[2].position.z);
		sides[3].position = new Vector3(buttonTR.position.x - buttonSR.bounds.size.x / 2f, buttonTR.position.y, sides[3].position.z);
		sides[3].localScale = new Vector3(sides[3].localScale.x, buttonTR.localScale.y / 2f, sides[3].localScale.z);
		sides[4].position = new Vector3(buttonTR.position.x + buttonSR.bounds.size.x / 2f, buttonTR.position.y, sides[4].position.z);
		sides[4].localScale = new Vector3(sides[4].localScale.x, buttonTR.localScale.y / 2f, sides[4].localScale.z);
		sides[5].position = new Vector3(buttonTR.position.x - buttonSR.bounds.size.x / 2f, buttonTR.position.y - buttonSR.bounds.size.y / 2f, sides[5].position.z);
		sides[6].position = new Vector3(buttonTR.position.x, buttonTR.position.y - buttonSR.bounds.size.y / 2f, sides[6].position.z);
		sides[6].localScale = new Vector3(buttonTR.localScale.x / 2f, sides[6].localScale.y, sides[6].localScale.z);
		sides[7].position = new Vector3(buttonTR.position.x + buttonSR.bounds.size.x / 2f, buttonTR.position.y - buttonSR.bounds.size.y / 2f, sides[7].position.z);
		if (mirror)
		{
			sidesContainer.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	private void Scale(Transform tr, float size)
	{
		tr.localScale = new Vector3(size, size, 1f);
	}

	private void VerticalScreen()
	{
		pixelsPerUnit = ScreenResolutionController.screenResolutionController.GetPixelsPerUnit();
		screenSizeX = ScreenResolutionController.screenResolutionController.GetSafeScreenSize().x;
		screenSizeY = ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y;
		margin = screenSizeY - screenSizeX;
		panelCenter = 4f - ScreenResolutionController.screenResolutionController.GetScreenOrigin().y - screenSizeX / 2f;
		buttonY = panelCenter - screenSizeX / 2f - margin / 2f;
		buttonX = screenSizeX / 2f / 2f;
		buttonBgScaleX = screenSizeX / 2f * ((float)Screen.height / pixelsPerUnit);
		buttonBgScaleY = 8f * margin;
		buttonLeftTR.localPosition = new Vector3(0f - buttonX + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, buttonY, 0f);
		buttonRightTR.localPosition = new Vector3(buttonX + ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x, buttonY, 0f);
		buttonLeftBgTR.localScale = new Vector3(buttonBgScaleX, buttonBgScaleY, 1f);
		buttonRightBgTR.localScale = new Vector3(buttonBgScaleX, buttonBgScaleY, 1f);
		if (margin / 2f < 0.3f)
		{
			arrowScale = 0f;
			hideMargins = true;
			ButtonsInputController.buttonsInputController.squareAspectRatio = 2;
		}
		else if (margin / 2f < 0.5f)
		{
			arrowScale = 1f;
			hideMargins = true;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX025;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX025;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 1;
		}
		else if (margin / 2f < 0.65f)
		{
			arrowScale = 1f;
			hideMargins = true;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX05;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX05;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 0.75f)
		{
			arrowScale = 1f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX05;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX05;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 1f)
		{
			arrowScale = 1f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX075;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX075;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 1.4f)
		{
			arrowScale = 1f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX1;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX1;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 2.5f)
		{
			arrowScale = 2f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX1;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX1;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 4f)
		{
			arrowScale = 3f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX1;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX1;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else
		{
			arrowScale = 4f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX1;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX1;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		buttonLeftArrowTR.localScale = new Vector3(arrowScale, arrowScale, 1f);
		buttonRightArrowTR.localScale = new Vector3(arrowScale, arrowScale, 1f);
		if (hideMargins)
		{
			sidesContainerLGO.SetActive(false);
			sidesContainerRGO.SetActive(false);
		}
		else
		{
			sidesContainerLGO.SetActive(true);
			sidesContainerRGO.SetActive(true);
		}
	}

	private void HorizontalScreen()
	{
		margin = (ScreenResolutionController.screenResolutionController.GetSafeScreenSize().x - ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y) / 2f;
		buttonCenterX = ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x + ScreenResolutionController.screenResolutionController.GetSafeScreenSize().x / 2f - margin / 2f;
		buttonCenterX2 = ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().x - ScreenResolutionController.screenResolutionController.GetSafeScreenSize().x / 2f + margin / 2f;
		buttonCenterY = 0f - ScreenResolutionController.screenResolutionController.GetScreenCenterOffset().y;
		buttonBgScale = margin * 8f;
		buttonLeftTR.localPosition = new Vector3(buttonCenterX2, buttonCenterY, 0f);
		buttonRightTR.localPosition = new Vector3(buttonCenterX, buttonCenterY, 0f);
		buttonLeftBgTR.localScale = new Vector3(buttonBgScale, 8f * ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y, 1f);
		buttonRightBgTR.localScale = new Vector3(buttonBgScale, 8f * ScreenResolutionController.screenResolutionController.GetSafeScreenSize().y, 1f);
		if (margin / 2f < 0.25f)
		{
			arrowScale = 0f;
			hideMargins = true;
			ButtonsInputController.buttonsInputController.squareAspectRatio = 2;
		}
		else if (margin / 2f < 0.6f)
		{
			arrowScale = 1f;
			hideMargins = true;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX025;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX025;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 1;
		}
		else if (margin / 2f < 0.75f)
		{
			arrowScale = 1f;
			hideMargins = true;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX05;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX05;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 1.1f)
		{
			arrowScale = 1f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX05;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX05;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 1.3f)
		{
			arrowScale = 1f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX075;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX075;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else if (margin / 2f < 3f)
		{
			arrowScale = 1f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX1;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX1;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		else
		{
			arrowScale = 2f;
			hideMargins = false;
			ButtonLeftController.buttonLeftController.arrowSprites = arrowsX1;
			ButtonLeftController.buttonLeftController.UpdateArrowSprite();
			ButtonRightController.buttonRightController.arrowSprites = arrowsX1;
			ButtonRightController.buttonRightController.UpdateArrowSprite();
			ButtonsInputController.buttonsInputController.squareAspectRatio = 0;
		}
		buttonLeftArrowTR.localScale = new Vector3(arrowScale, arrowScale, 1f);
		buttonRightArrowTR.localScale = new Vector3(arrowScale, arrowScale, 1f);
		if (hideMargins)
		{
			sidesContainerLGO.SetActive(false);
			sidesContainerRGO.SetActive(false);
		}
		else
		{
			sidesContainerLGO.SetActive(true);
			sidesContainerRGO.SetActive(true);
		}
	}
}
