using UnityEngine;

public class ScreenResolutionController : MonoBehaviour
{
	public static ScreenResolutionController screenResolutionController;

	public ButtonsTransformController buttonsTransformController;

	public ButtonScreenTransformController buttonScreenTransformController;

	public TransitionPlanesTransformController transitionPlanesTransformController;

	public RenderPlanePositionController[] renderPlanePositionController;

	private GameObject[] gos;

	private Vector2 resolution;

	private bool fullscreen;

	private float timer;

	private float pixelsPerUnit;

	private Vector2 safeA;

	private float aspectRatio;

	private float unsafeAreaX;

	private float safeAreaX;

	private float totalX;

	private float centerX;

	private float unsafeAreaY;

	private float safeAreaY;

	private float totalY;

	private float centerY;

	public bool updateResolutionNextFrame;

	private void Start()
	{
		screenResolutionController = this;
		fullscreen = true;
		if (PlayerPrefs.GetInt("fullscreen") == 0 && PlayerPrefs.GetInt("initialized") != 0)
		{
			fullscreen = false;
		}
		resolution.x = GetSafeAreaResolution().x;
		resolution.y = GetSafeAreaResolution().y;
		Invoke("UpdateOthers", 0.05f);
	}

	private void Update()
	{
		if (updateResolutionNextFrame)
		{
			updateResolutionNextFrame = false;
			UpdateResolution();
		}
		if (timer < 1f)
		{
			timer += Time.unscaledDeltaTime;
		}
		else if (resolution.x != (float)Screen.width || resolution.y != (float)Screen.height || fullscreen != Screen.fullScreen)
		{
			updateResolutionNextFrame = true;
		}
	}

	private void UpdateResolution()
	{
		if (Screen.width < 256)
		{
			Screen.SetResolution(256, Screen.height, Screen.fullScreen);
		}
		if (Screen.height < 256)
		{
			Screen.SetResolution(Screen.width, 256, Screen.fullScreen);
		}
		resolution.x = Screen.width;
		resolution.y = Screen.height;
		fullscreen = Screen.fullScreen;
		PlayerPrefs.SetInt("resolutionX", (int)GetSafeAreaResolution().x);
		PlayerPrefs.SetInt("resolutionY", (int)GetSafeAreaResolution().y);
		if (fullscreen)
		{
			PlayerPrefs.SetInt("fullscreen", 1);
		}
		else
		{
			PlayerPrefs.SetInt("fullscreen", 0);
		}
		UpdateOthers();
	}

	private void UpdateOthers()
	{
		buttonsTransformController.UpdateButtonsTransform();
		buttonScreenTransformController.UpdateButtonTransform();
		transitionPlanesTransformController.UpdateTransform();
		if (PauseController.pauseController.inCinematic && renderPlanePositionController.Length < 2)
		{
			GetRenderPlanes();
		}
		if (renderPlanePositionController == null)
		{
			GetRenderPlanes();
		}
		if (renderPlanePositionController.Length == 0)
		{
			GetRenderPlanes();
		}
		if (renderPlanePositionController.Length > 0 && renderPlanePositionController[0] == null)
		{
			GetRenderPlanes();
		}
		for (int i = 0; i < renderPlanePositionController.Length; i++)
		{
			if (renderPlanePositionController[i] != null)
			{
				renderPlanePositionController[i].UpdateTransform();
			}
		}
	}

	private void GetRenderPlanes()
	{
		gos = GameObject.FindGameObjectsWithTag("renderPlane");
		renderPlanePositionController = new RenderPlanePositionController[gos.Length];
		for (int i = 0; i < gos.Length; i++)
		{
			renderPlanePositionController[i] = gos[i].GetComponent<RenderPlanePositionController>();
		}
	}

	public Vector2 GetSafeAreaResolution()
	{
		if (GlobalControl.globalControl.mobile)
		{
			return new Vector2(Screen.safeArea.width, Screen.safeArea.height);
		}
		return new Vector2(Screen.width, Screen.height);
	}

	public Vector2 GetScreenOrigin()
	{
		safeA = new Vector2(Screen.safeArea.x, Screen.safeArea.y);
		if (safeA.x < 1f && safeA.y < 1f)
		{
			safeA.x = (float)Screen.width * safeA.x;
			safeA.y = (float)Screen.height * safeA.y;
		}
		safeA.x /= GetPixelsPerUnit();
		safeA.y /= GetPixelsPerUnit();
		return safeA;
	}

	public Vector2 GetScreenCenterOffset()
	{
		unsafeAreaX = GetScreenOrigin().x;
		centerX = unsafeAreaX / 2f;
		unsafeAreaY = GetScreenOrigin().y;
		centerY = unsafeAreaY / 2f;
		return new Vector2(centerX, 0f - centerY);
	}

	public Vector2 GetSafeScreenSize()
	{
		return new Vector2(GetSafeAreaResolution().x / GetPixelsPerUnit(), GetSafeAreaResolution().y / GetPixelsPerUnit());
	}

	public float GetPixelsPerUnit()
	{
		pixelsPerUnit = (float)Screen.height / 8f;
		return pixelsPerUnit;
	}
}
