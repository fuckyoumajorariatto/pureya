using UnityEngine;

public class SET_DeleteSelectionController : MonoBehaviour
{
	public SETController controller;

	public SET_ProfileController profileController;

	public WheelController wheelController;

	public GameObject deleteQuestion;

	public GameObject deleteAllQuestion;

	public Transform titleMiddleIcon;

	public GameObject profileElements;

	public GameObject profileNumber;

	public GameObject profileNumberAll;

	public Sprite[] smallIcons;

	public Sprite[] smallIconsOverlays;

	public Sprite[] bigIcons;

	public Sprite[] bigIconsOverlays;

	public Color[] iconColors;

	private bool[] optionsToSkip;

	private bool inputActive;

	public AudioClip deleteSound;

	private bool deleteAllActivationCancelled;

	private bool movingLeftRegistered;

	private int leftConsecutiveMoves;

	private bool deleteAll;

	private void Start()
	{
		optionsToSkip = new bool[bigIcons.Length];
	}

	private void Update()
	{
		if (inputActive)
		{
			ManageAccept();
			ManageDeleteAllActivation();
		}
	}

	private void ManageDeleteAllActivation()
	{
		if (wheelController.movingRight && !deleteAllActivationCancelled)
		{
			deleteAllActivationCancelled = true;
		}
		if (deleteAllActivationCancelled)
		{
			return;
		}
		if (wheelController.movingLeft && !movingLeftRegistered)
		{
			movingLeftRegistered = true;
			leftConsecutiveMoves++;
			if (leftConsecutiveMoves >= 10)
			{
				ChangeToDeleteAll();
			}
		}
		else if (!wheelController.movingLeft && movingLeftRegistered)
		{
			movingLeftRegistered = false;
		}
	}

	private void ChangeToDeleteAll()
	{
		deleteAllActivationCancelled = true;
		inputActive = false;
		SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
		Invoke("DoChangeToDeleteAll", 0.25f);
	}

	private void DoChangeToDeleteAll()
	{
		deleteAll = true;
		inputActive = true;
		profileNumber.SetActive(false);
		profileNumberAll.SetActive(true);
		deleteQuestion.SetActive(false);
		deleteAllQuestion.SetActive(true);
	}

	public void Initialize()
	{
		Invoke("AtivateInput", 0.25f);
		wheelController.ReplaceSprites(smallIcons, smallIconsOverlays, bigIcons, bigIconsOverlays, null, iconColors, optionsToSkip);
		wheelController.ResetNextFrameInput();
		wheelController.ChangeSelectionAuto(0);
		deleteQuestion.SetActive(true);
		profileElements.SetActive(false);
		titleMiddleIcon.position = new Vector3(-2f, titleMiddleIcon.position.y, titleMiddleIcon.position.z);
	}

	private void ManageAccept()
	{
		if (!inputActive || wheelController.moving)
		{
			return;
		}
		if (GlobalInput.globalInput.acceptDown || GlobalInput.globalInput.startDown || GlobalInput.globalInput.touchButtonScreenMiddle || GlobalInput.globalInput.arrowsAcceptDown)
		{
			if (controller.wheelController.selection == 0)
			{
				inputActive = false;
				PlayAccept();
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
				Invoke("ShowProfileMenu", 1f / 6f);
				return;
			}
			inputActive = false;
			PlayAccept();
			if (deleteAll)
			{
				SceneChangeController.sceneChangeController.ManualTransitionInOutAndroidCloud2("title", 999f);
				Invoke("PlayDelete", 0.5f);
				Invoke("DeleteAll", 1f);
			}
			else
			{
				SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 1f);
				Invoke("ShowProfileMenu", 1f / 6f);
				Invoke("PlayDelete", 0.5f);
				Invoke("ResetSave", 0.6f);
			}
		}
		else if (GlobalInput.globalInput.cancelDown || GlobalInput.globalInput.exitDown)
		{
			inputActive = false;
			PlayCancel();
			SceneChangeController.sceneChangeController.ManualTransitionInOut("title", 0f);
			Invoke("ShowProfileMenu", 1f / 6f);
		}
	}

	private void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
		SaveLoadManager.saveLoadManager.DeleteAll();
	}

	private void AtivateInput()
	{
		inputActive = true;
	}

	private void ShowProfileMenu()
	{
		controller.ResetNextFrameInput();
		deleteQuestion.SetActive(false);
		deleteAllQuestion.SetActive(false);
		profileElements.SetActive(true);
		titleMiddleIcon.position = new Vector3(0f, titleMiddleIcon.position.y, titleMiddleIcon.position.z);
		profileNumber.SetActive(true);
		profileNumberAll.SetActive(false);
		profileController.Initialize();
		deleteAllActivationCancelled = false;
		movingLeftRegistered = false;
		deleteAll = false;
		leftConsecutiveMoves = 0;
	}

	public void ResetSave()
	{
		GlobalControl.globalControl.ResetData();
		GlobalControl.globalControl.LoadData();
		profileController.UpdateProfile();
	}

	private void PlayAccept()
	{
		GlobalAudio.globalAudio.PlayAccept();
	}

	private void PlayCancel()
	{
		GlobalAudio.globalAudio.PlayCancel();
	}

	private void PlayDelete()
	{
		GlobalAudio.globalAudio.Play(deleteSound, 0.9f, 1f, 0f);
	}
}
