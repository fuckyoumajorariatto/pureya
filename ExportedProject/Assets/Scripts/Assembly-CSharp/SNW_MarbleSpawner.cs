using UnityEngine;

public class SNW_MarbleSpawner : MonoBehaviour
{
	public static SNW_MarbleSpawner SNW_marbleSpawner;

	private float marbleSpeed;

	private Color32 shadowColor = new Color32(147, 147, 147, byte.MaxValue);

	private float frequency;

	private float timer;

	private bool odd;

	private Vector3 pos;

	private GameObject marble;

	private MarbleController marbleController;

	private GameObject obj;

	private void Start()
	{
		SNW_marbleSpawner = this;
		SetDifficultyParas();
	}

	private void Update()
	{
		if (timer >= frequency && !SNW_PlayerController.SNW_playerController.hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			if (odd)
			{
				CreateBatch();
				odd = false;
			}
			else
			{
				CreateRock();
				odd = true;
			}
			timer = 0f;
		}
		timer += Time.deltaTime;
	}

	public void SetDifficultyParas()
	{
		marbleSpeed = SNW_Difficulty.SNW_difficulty.scroll_speed;
		frequency = SNW_Difficulty.SNW_difficulty.frequency;
	}

	private void CreateBatch()
	{
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = Random.Range(-5f, -6f);
			pos.x = Random.Range(-4f, -1f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x *= -1f;
			}
			CreateMarble(pos.x, pos.y);
		}
		else
		{
			pos.y = Random.Range(-5f, -6f);
			pos.x = Random.Range(-4f, -1f);
			CreateMarble(pos.x, pos.y);
			pos.y = Random.Range(-5f, -6f);
			pos.x = Random.Range(1f, 4f);
			CreateMarble(pos.x, pos.y);
		}
	}

	public void CreateMarble(float x, float y)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			pos = new Vector3(0f, 5f, -1f);
			if (!SceneChangeController.sceneChangeController.inTransition)
			{
				marble = MarbleCreator.marbleCreator.GetMarble();
				pos.z = -3f;
				pos.x = x;
				pos.y = y;
				marble.transform.position = pos;
				SetMarbleConstants(marble);
			}
		}
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = marbleSpeed;
		marbleController.direction = MarbleController.Direction.up;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.up_left;
		marbleController.maxOut = 20f;
		marble.SetActive(true);
	}

	private void CreateRock()
	{
		pos.z = -4f;
		pos.y = -5f;
		pos.x = Random.Range(-4f, 4f);
		obj = SNW_RockCreator.SNW_rockCreator.GetRock();
		if (obj != null)
		{
			obj.transform.position = pos;
			obj.SetActive(true);
		}
		pos.y = -6f;
		if (pos.x < -1.1f || pos.x > 1.1f)
		{
			pos.x *= -1f;
			obj = SNW_RockCreator.SNW_rockCreator.GetRock();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		else
		{
			pos.x = Random.Range(3.3f, 5f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x *= -1f;
			}
			obj = SNW_RockCreator.SNW_rockCreator.GetRock();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
		if (Random.Range(0f, 1f) < 0.5f)
		{
			pos.y = -8f;
			pos.x = Random.Range(-4f, 4f);
		}
		for (int i = 0; i < 7; i++)
		{
			pos.y = Random.Range(-5f, -8f);
			pos.x = Random.Range(5f, 20f);
			if (Random.Range(0f, 1f) < 0.5f)
			{
				pos.x *= -1f;
			}
			obj = SNW_RockCreator.SNW_rockCreator.GetRock();
			if (obj != null)
			{
				obj.transform.position = pos;
				obj.SetActive(true);
			}
		}
	}
}
