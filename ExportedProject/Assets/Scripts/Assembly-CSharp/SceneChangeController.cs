using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeController : MonoBehaviour
{
	public static SceneChangeController sceneChangeController;

	public Animator transition;

	public TransitionController transitionController;

	private int inTrigger;

	private int outTrigger;

	private int intShow;

	private AsyncOperation async;

	public bool transitioning;

	public bool inTransition;

	public bool outTransition;

	private bool transitionOutIn2Frames;

	private bool transitionOutNextFrame;

	private GameObject renderPlane;

	private GameObject foundRenderPlane;

	private bool activateOnTransitionClosed;

	private bool manualInOut;

	public string previousScene = string.Empty;

	public string currentScene = string.Empty;

	public string loadingScene = string.Empty;

	public bool pausableScene;

	private bool manualTransition;

	private bool doubleLoad;

	private string savedScene;

	private string savedTransitionType;

	private float savedPause;

	private float pauseTimer;

	private bool waitForAndroidCloud1;

	private bool waitForAndroidCloud2;

	private bool countTime;

	public bool initialPlayMenuMusic;

	private bool marblesResetted;

	private void Start()
	{
		sceneChangeController = this;
		inTrigger = Animator.StringToHash("transitionIn");
		outTrigger = Animator.StringToHash("transitionOut");
		intShow = Animator.StringToHash("show");
	}

	private void Update()
	{
		if (countTime)
		{
			GlobalControl.globalControl.CountPlayTime();
		}
		if (transitioning)
		{
			if (waitForAndroidCloud1)
			{
				if (PlayCloudDataManager.Instance.defaultSaveLoaded && GlobalControl.globalControl.initialDataLoaded && iCloudManager.Instance.initialDataLoaded)
				{
					waitForAndroidCloud1 = false;
				}
			}
			else if (activateOnTransitionClosed && !inTransition && !outTransition)
			{
				if (pauseTimer > 0f)
				{
					pauseTimer -= Time.deltaTime;
					return;
				}
				if (!marblesResetted)
				{
					ResetMarbles();
				}
				else
				{
					transitionOutIn2Frames = true;
					ActivateScene();
				}
			}
		}
		if (manualInOut)
		{
			if (pauseTimer > 0f)
			{
				pauseTimer -= Time.deltaTime;
				return;
			}
			if (waitForAndroidCloud2)
			{
				if (PlayCloudDataManager.Instance.initialDataLoaded)
				{
					waitForAndroidCloud2 = false;
					manualInOut = false;
					ManualTransitionOut();
				}
			}
			else
			{
				manualInOut = false;
				ManualTransitionOut();
			}
		}
		if (transitionOutNextFrame)
		{
			transitionOutNextFrame = false;
			transitionOutIn2Frames = false;
			if (doubleLoad)
			{
				doubleLoad = false;
				LoadSceneInBackground(savedScene);
				async.allowSceneActivation = true;
				transitionOutNextFrame = true;
				if (savedScene == "SET")
				{
					pausableScene = false;
					countTime = false;
				}
				else if (savedScene == "MMU" || savedScene == "SUR" || savedScene == "COL")
				{
					pausableScene = false;
					countTime = true;
				}
				else
				{
					pausableScene = true;
					countTime = true;
				}
			}
			else
			{
				ManualTransitionOut();
			}
		}
		if (transitionOutIn2Frames && async.isDone)
		{
			foundRenderPlane = GameObject.FindGameObjectWithTag("renderPlane");
			if (foundRenderPlane != null && renderPlane != foundRenderPlane)
			{
				transitionOutIn2Frames = false;
				transitionOutNextFrame = true;
				renderPlane = foundRenderPlane;
			}
		}
	}

	public void LoadAndChangeScene(string scene, string transitionType, float pause)
	{
		if (transitioning)
		{
			doubleLoad = true;
			savedScene = scene;
			savedTransitionType = transitionType;
			savedPause = pause;
			return;
		}
		transitioning = true;
		activateOnTransitionClosed = true;
		LoadSceneInBackground(scene);
		UpdateTransitionType(transitionType);
		inTransition = true;
		ResetTransitionTriggers();
		transition.SetTrigger(inTrigger);
		pauseTimer = pause;
	}

	public void LoadAndChangeSceneWaitForAndroidCloud(string scene, string transitionType, float pause)
	{
		waitForAndroidCloud1 = true;
		LoadAndChangeScene(scene, transitionType, pause);
	}

	public AsyncOperation LoadSceneInBackground(string scene)
	{
		loadingScene = scene;
		async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
		async.allowSceneActivation = false;
		marblesResetted = false;
		return async;
	}

	public AsyncOperation DirectlyLoadSceneInBackground(string scene)
	{
		pauseTimer = 0f;
		return LoadSceneInBackground(scene);
	}

	public void ActivateScene()
	{
		if (async.allowSceneActivation)
		{
			activateOnTransitionClosed = false;
			return;
		}
		activateOnTransitionClosed = false;
		async.allowSceneActivation = true;
		previousScene = currentScene;
		currentScene = loadingScene;
		PauseController.pauseController.exiting = false;
		if (currentScene == "SET")
		{
			pausableScene = false;
			countTime = false;
		}
		else if (currentScene == "MMU" || currentScene == "SUR" || currentScene == "COL")
		{
			pausableScene = false;
			countTime = true;
		}
		else
		{
			pausableScene = true;
			countTime = true;
		}
		loadingScene = string.Empty;
	}

	private void ResetMarbles()
	{
		marblesResetted = true;
		MarbleCreator.marbleCreator.ResetMarbles();
	}

	private void UpdateTransitionType(string type)
	{
		switch (type)
		{
		case "minigame":
			transitionController.UpdateMinigameCounter();
			transition.SetInteger(intShow, 1);
			break;
		case "title":
			transition.SetInteger(intShow, 0);
			break;
		case "marbles":
			transitionController.StartUpdateMarbleCounter();
			transition.SetInteger(intShow, 2);
			break;
		}
	}

	public void ManualTransitionIn(string transitionType)
	{
		transitioning = true;
		inTransition = true;
		UpdateTransitionType(transitionType);
		transition.SetTrigger(inTrigger);
	}

	public void ManualTransitionOut()
	{
		transitioning = true;
		inTransition = false;
		outTransition = true;
		transition.SetTrigger(outTrigger);
		GlobalInput.globalInput.gameInputActive = false;
		if (initialPlayMenuMusic)
		{
			initialPlayMenuMusic = false;
			Invoke("PlayMenuMusic", 0.1f);
		}
	}

	private void PlayMenuMusic()
	{
		GlobalAudio.globalAudio.PlayMainMenuMusicWithIntro();
	}

	public void ManualTransitionInOut(string transitionType, float pause)
	{
		manualInOut = true;
		UpdateTransitionType(transitionType);
		ManualTransitionIn(transitionType);
		if (pause < 0.25f)
		{
			pause = 0.25f;
		}
		pauseTimer = pause;
	}

	public void ManualTransitionInOutAndroidCloud2(string transitionType, float pause)
	{
		manualInOut = true;
		UpdateTransitionType(transitionType);
		ManualTransitionIn(transitionType);
		if (pause < 0.25f)
		{
			pause = 0.25f;
		}
		pauseTimer = pause;
		waitForAndroidCloud2 = true;
	}

	public void TransitionEnd()
	{
		if (inTransition)
		{
			transitioning = true;
			inTransition = false;
			outTransition = false;
			activateOnTransitionClosed = true;
			pauseTimer = 0.25f;
		}
		else if (doubleLoad)
		{
			doubleLoad = false;
			transitioning = false;
			inTransition = false;
			outTransition = false;
			LoadAndChangeScene(savedScene, savedTransitionType, savedPause);
		}
		else
		{
			transitioning = false;
			inTransition = false;
			outTransition = false;
			GlobalInput.globalInput.gameInputActive = true;
		}
		transitionOutIn2Frames = false;
		transitionOutNextFrame = false;
	}

	private void ResetTransitionTriggers()
	{
		transition.ResetTrigger(inTrigger);
		transition.ResetTrigger(outTrigger);
	}

	public void ActivateOnTransitionClosed()
	{
		activateOnTransitionClosed = true;
	}

	public void RestartGame()
	{
		async = SceneManager.LoadSceneAsync("_Initial", LoadSceneMode.Single);
		Object.Destroy(GameObject.Find("GlobalScripter"));
		async.allowSceneActivation = true;
	}
}
