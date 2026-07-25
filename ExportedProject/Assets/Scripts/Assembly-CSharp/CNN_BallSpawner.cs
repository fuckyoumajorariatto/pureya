using UnityEngine;

public class CNN_BallSpawner : MonoBehaviour
{
	public Transform big_shipTR;

	public GameObject cannonball;

	public GameObject marblePrefab;

	private int pooledAmount = 3;

	private GameObject[] cannonballs = new GameObject[3];

	private GameObject marble;

	private GameObject[] balls = new GameObject[4];

	private float[] speedModifiers = new float[3];

	private Color32 shadowColor = new Color32(30, 30, 30, byte.MaxValue);

	private float[] cannonsPosX = new float[4] { -1.5f, -0.5f, 0.5f, 1.5f };

	public bool newBatch = true;

	private float timer;

	private Vector3 pos = new Vector3(0f, 0f, -3f);

	private int cannonballMax;

	private float speedUp;

	private float speedDown;

	private bool ascending;

	public bool firstBall = true;

	public GameObject cannonParticles1;

	public GameObject cannonParticles2;

	public GameObject cannonParticles3;

	public GameObject cannonParticles4;

	private GameObject[] cannonParticles = new GameObject[4];

	public AudioClip cannonShoot;

	public AudioClip ballFlying;

	public CNN_Reset CNN_reset;

	private GameObject obj;

	private float variation;

	private Transform marbleTR;

	private Transform marble_sTR;

	private int marbleCannon;

	private int[] usedCannons;

	private int r;

	private bool alreadyTaken;

	private Transform cannonballTR;

	private void Start()
	{
		CreatePool();
		InitializeCannonParticles();
		SetDifficultyParas();
		pos.y = big_shipTR.position.y - 2f;
		for (int i = 0; i < speedModifiers.Length; i++)
		{
			speedModifiers[i] = Random.Range(0.75f, 1.5f);
		}
	}

	private void Update()
	{
		if (CNN_reset.reset)
		{
			timer = 0f;
		}
		else if (timer > 0.5f && !SceneChangeController.sceneChangeController.transitioning)
		{
			ManageBallCreation();
		}
		else
		{
			timer += Time.deltaTime;
		}
		MoveCannonBalls();
		MoveMarble();
		ControlDirection();
	}

	public void SetDifficultyParas()
	{
		cannonballMax = CNN_Difficulty.CNN_difficulty.cannonballMax;
		speedUp = CNN_Difficulty.CNN_difficulty.speedUp;
		speedDown = CNN_Difficulty.CNN_difficulty.speedDown;
	}

	private void CreatePool()
	{
		for (int i = 0; i < pooledAmount; i++)
		{
			obj = Object.Instantiate(cannonball);
			obj.transform.parent = big_shipTR;
			obj.SetActive(false);
			cannonballs[i] = obj;
		}
	}

	private void InitializeCannonParticles()
	{
		cannonParticles[0] = cannonParticles1;
		cannonParticles[1] = cannonParticles2;
		cannonParticles[2] = cannonParticles3;
		cannonParticles[3] = cannonParticles4;
	}

	private void ControlDirection()
	{
		if (AreAllTheBallsOut() && ascending)
		{
			ascending = false;
			if (SceneChangeController.sceneChangeController.transitioning)
			{
				marbleTR.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				marbleTR.localScale = new Vector3(1.25f, 1.25f, 1f);
			}
			for (int i = 0; i < cannonballs.Length; i++)
			{
				cannonballs[i].transform.localScale = new Vector3(1.25f, 1.25f, 1f);
			}
			variation = Random.Range(-0.25f, 0.25f);
			cannonballTR = balls[0].transform;
			cannonballTR.position = new Vector3(-5.5f + variation, 7f, cannonballTR.position.z);
			balls[1].transform.position = new Vector3(-3.5f + variation, 7f, cannonballTR.position.z);
			balls[2].transform.position = new Vector3(-1.5f + variation, 7f, cannonballTR.position.z);
			balls[3].transform.position = new Vector3(0.5f + variation, 7f, cannonballTR.position.z);
			cannonParticles1.SetActive(false);
			cannonParticles2.SetActive(false);
			cannonParticles3.SetActive(false);
			cannonParticles4.SetActive(false);
		}
		else if (AreAllTheBallsOut() && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (marble.GetComponent<SpriteRenderer>().enabled)
			{
				marble.SetActive(false);
			}
			for (int j = 0; j < cannonballs.Length; j++)
			{
				cannonballs[j].SetActive(false);
			}
			newBatch = true;
			ascending = true;
		}
	}

	private bool AreAllTheBallsOut()
	{
		if (balls[0] == null)
		{
			return false;
		}
		bool result = true;
		for (int i = 0; i < balls.Length; i++)
		{
			cannonballTR = balls[i].transform;
			if ((cannonballTR.position.y > -5f && !ascending) || (cannonballTR.position.y < 6f && ascending))
			{
				result = false;
			}
		}
		return result;
	}

	private void MoveCannonBalls()
	{
		if (ascending)
		{
			for (int i = 0; i < cannonballs.Length; i++)
			{
				cannonballs[i].transform.Translate((0f - speedUp) * Time.deltaTime * speedModifiers[i], speedUp * 1.5f * Time.deltaTime * speedModifiers[i], 0f);
			}
		}
		else
		{
			for (int j = 0; j < cannonballs.Length; j++)
			{
				cannonballs[j].transform.Translate(speedDown * 0.5f * Time.deltaTime * speedModifiers[j], (0f - speedDown) * 2f * Time.deltaTime * speedModifiers[j], 0f);
			}
		}
	}

	private void MoveMarble()
	{
		if (!(marble == null))
		{
			if (ascending)
			{
				marbleTR.Translate((0f - speedUp) * Time.deltaTime, speedUp * 1.5f * Time.deltaTime, 0f);
			}
			else
			{
				marbleTR.Translate(speedDown * 0.5f * Time.deltaTime, (0f - speedDown) * 2f * Time.deltaTime, 0f);
			}
		}
	}

	private void ManageBallCreation()
	{
		if (GetComponent<CNN_Reset>().reset || !newBatch)
		{
			return;
		}
		newBatch = false;
		ascending = true;
		for (int i = 0; i < speedModifiers.Length; i++)
		{
			speedModifiers[i] = Random.Range(0.75f, 1.5f);
		}
		marbleCannon = Random.Range(0, 4);
		if (firstBall)
		{
			marbleCannon = Random.Range(1, 4);
			firstBall = false;
		}
		CreateMarble(cannonsPosX[marbleCannon]);
		balls[marbleCannon] = marble;
		usedCannons = new int[4] { 4, 4, 4, 4 };
		usedCannons[3] = marbleCannon;
		for (int j = 0; j < 3; j++)
		{
			r = Random.Range(0, 4);
			alreadyTaken = true;
			while (alreadyTaken)
			{
				for (int k = 0; k < usedCannons.Length; k++)
				{
					alreadyTaken = false;
					if (usedCannons[k] == r)
					{
						alreadyTaken = true;
						break;
					}
				}
				if (alreadyTaken)
				{
					r++;
					if (r > 3)
					{
						r = 0;
					}
				}
			}
			usedCannons[j] = r;
			CreateCannonball(j, cannonsPosX[r]);
			if (j < cannonballMax)
			{
				cannonballs[j].SetActive(true);
			}
			balls[r] = cannonballs[j];
		}
		PlayCannonShoot();
		PlayBallFlying();
		for (int l = 0; l < balls.Length; l++)
		{
			if (balls[l].activeInHierarchy)
			{
				cannonParticles[l].SetActive(true);
			}
		}
	}

	private void CreateCannonball(int i, float posX)
	{
		pos.x = posX;
		cannonballTR = cannonballs[i].transform;
		cannonballTR.position = pos;
		cannonballTR.localScale = new Vector3(0.75f, 0.75f, 1f);
	}

	private void CreateMarble(float posX)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			marble = MarbleCreator.marbleCreator.GetMarble();
			marbleTR = marble.transform;
			marbleTR.parent = big_shipTR;
			marble.GetComponent<MarbleController>().maxOut = 10f;
			marble_sTR = marbleTR.GetChild(0);
			marble_sTR.GetComponent<SpriteRenderer>().color = shadowColor;
			marble_sTR.GetComponent<RelocateShadow>().direction = RelocateShadow.Direction.up_right;
			pos.x = posX;
			marbleTR.position = pos;
			marbleTR.localScale = new Vector3(0.75f, 0.75f, 1f);
			marble.SetActive(true);
		}
	}

	private void PlayCannonShoot()
	{
		GlobalAudio.globalAudio.Play(cannonShoot, 1f, Random.Range(0.9f, 1.1f), 0f);
	}

	private void PlayBallFlying()
	{
		GlobalAudio.globalAudio.Play(ballFlying, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), 0f);
	}
}
