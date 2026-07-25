using UnityEngine;

public class SQD_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	public GameObject player;

	public GameObject playerCenter;

	private bool difficultyLoaded;

	private float marbleFrequency;

	private int marbleMax;

	private float marbleSpeed;

	private float marbleTimer;

	private Color32 shadowColor = new Color32(50, 50, 50, byte.MaxValue);

	public GameObject bgParticles_1;

	public GameObject bgParticles_2;

	public AudioClip sting;

	public SQD_UrchinSpawner urchinSpawner;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(0, 90, 191, byte.MaxValue);
		GlobalAudio.globalAudio.Play(sting, 1f, 1f, 0f);
	}

	private void Update()
	{
		ManageDifficultyInit();
		ManageMarblesCreation();
	}

	private void ManageDifficultyInit()
	{
		if (!difficultyLoaded && SQD_Difficulty.SQD_difficulty.loaded)
		{
			difficultyLoaded = true;
			SetDifficultyParas();
			bgParticles_1.SetActive(true);
			bgParticles_2.SetActive(true);
			urchinSpawner.enabled = true;
		}
	}

	public void SetDifficultyParas()
	{
		marbleFrequency = SQD_Difficulty.SQD_difficulty.marble_frequency;
		marbleMax = SQD_Difficulty.SQD_difficulty.marble_max;
		marbleSpeed = SQD_Difficulty.SQD_difficulty.marble_speed;
		marbleTimer = 0f;
	}

	private void ManageMarblesCreation()
	{
		if (!difficultyLoaded || SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		marbleTimer += Time.deltaTime;
		pos = new Vector3(5f, 0f, 1f);
		if (!(marbleTimer >= marbleFrequency))
		{
			return;
		}
		marbleTimer = 0f;
		switch (Random.Range(1, marbleMax + 1))
		{
		case 1:
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.y = Random.Range(-3f, 3f);
			marble.transform.position = pos;
			SetMarbleConstants(marble);
			break;
		case 2:
			marble = MarbleCreator.marbleCreator.GetMarble();
			pos.y = Random.Range(-3f, 3f);
			marble.transform.position = pos;
			SetMarbleConstants(marble);
			marble = MarbleCreator.marbleCreator.GetMarble();
			if (pos.y <= 1f)
			{
				pos.y = Random.Range(pos.y + 2f, 3f);
			}
			else if (pos.y > -1f)
			{
				pos.y = Random.Range(-3f, pos.y - 2f);
			}
			pos.x += Random.Range(0f, 2f);
			marble.GetComponent<MarbleController>().maxOut = 7f;
			marble.transform.position = pos;
			SetMarbleConstants(marble);
			break;
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = marbleSpeed;
		marbleController.direction = MarbleController.Direction.left;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
		marble.SetActive(true);
	}
}
