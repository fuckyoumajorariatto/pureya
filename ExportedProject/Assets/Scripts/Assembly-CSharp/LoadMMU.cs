using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMMU : MonoBehaviour
{
	private float timer;

	private bool end;

	private bool changedSceneName;

	private bool marbleCalled;

	public Text creditsText;

	public GameObject cam;

	private bool cameraActivated;

	private List<float> deltaTimes = new List<float>();

	private bool initialDataLoadCalled;

	private bool defaultSaveLoadCalled;

	private GameObject go;

	private void Start()
	{
		Invoke("ActivateCamera", 0.05f);
		Invoke("ActivateNumbers", 0.1f);
	}

	private void Update()
	{
		if (cameraActivated)
		{
			if (MarbleCreator.marbleCreator != null && !marbleCalled)
			{
				marbleCalled = true;
				MarbleCreator.marbleCreator.GetMarbleNumbers().transform.position = new Vector2(0f, -1000f);
				creditsText.text = "Majorariatto";
			}
			if (SceneChangeController.sceneChangeController != null && !changedSceneName)
			{
				Debug.Log("pry- GAME STARTS " + GlobalControl.globalControl.version);
				SceneChangeController.sceneChangeController.currentScene = "splash";
				changedSceneName = true;
			}
			if (timer >= 1f && !end)
			{
				end = true;
				SceneChangeController.sceneChangeController.initialPlayMenuMusic = true;
				ResolveFramerate();
				SceneChangeController.sceneChangeController.LoadAndChangeSceneWaitForAndroidCloud("MMU", "title", 0f);
			}
			timer += Time.deltaTime;
			if (end && !initialDataLoadCalled && PlayCloudDataManager.Instance.defaultSaveLoaded)
			{
				GlobalControl.globalControl.InitialLoadData();
				initialDataLoadCalled = true;
			}
			deltaTimes.Add(Time.deltaTime);
		}
	}

	private void ResolveFramerate()
	{
		float num = 999f;
		for (int i = 0; i < deltaTimes.Count; i++)
		{
			if (deltaTimes[i] < num)
			{
				num = deltaTimes[i];
			}
		}
		if (num > 1f / 60f)
		{
			Application.targetFrameRate = 60;
		}
	}

	private void ActivateCamera()
	{
		cam.SetActive(true);
		cameraActivated = true;
	}

	private void ActivateNumbers()
	{
		go = MarbleCreator.marbleCreator.GetMarbleNumbers();
		if (go != null)
		{
			go.transform.position = new Vector3(10f, 10f, 10f);
		}
	}
}
