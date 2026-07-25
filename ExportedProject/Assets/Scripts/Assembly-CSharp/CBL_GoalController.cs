using System.Collections.Generic;
using UnityEngine;

public class CBL_GoalController : MonoBehaviour
{
	public static CBL_GoalController CBL_goalController;

	public AudioClip goalSound;

	private Color32 shadowColor = new Color32(55, 55, 55, byte.MaxValue);

	public List<GameObject> ballParticles;

	public GameObject ballParticle;

	private int ballParticlesPooledAmount = 3;

	private float marbleSpeed;

	private GameObject obj;

	private MarbleController marbleController;

	private Transform tr;

	private void Start()
	{
		CBL_goalController = this;
		tr = base.transform;
		PoolBallParticles();
		SetDifficultyParas();
	}

	public void SetDifficultyParas()
	{
		marbleSpeed = CBL_Difficulty.CBL_difficulty.marbleSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ManageCollision(collision);
	}

	private void ManageCollision(Collider2D collision)
	{
		if (collision.CompareTag("ball") && collision.GetComponent<Rigidbody2D>().velocity.y > 0f && !SceneChangeController.sceneChangeController.transitioning)
		{
			collision.transform.GetComponent<CBL_BallController>().Disable();
			ShowParticles(collision);
			SpawnMarbles(collision);
			PlayGoalSound();
		}
	}

	private void ShowParticles(Collider2D collision)
	{
		obj = GetBallParticles();
		if (obj != null)
		{
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.transform.position = collision.transform.position;
			obj.SetActive(true);
		}
	}

	private void SpawnMarbles(Collider2D collision)
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			for (float num = -3.4375f; num < 4f; num += 1.71875f)
			{
				obj = MarbleCreator.marbleCreator.GetMarble();
				marbleController = obj.GetComponent<MarbleController>();
				obj.transform.position = new Vector3(num, collision.transform.position.y, -2f);
				marbleController.direction = MarbleController.Direction.down;
				marbleController.speed = marbleSpeed;
				marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
				obj.SetActive(true);
			}
		}
	}

	private void PoolBallParticles()
	{
		ballParticles = new List<GameObject>();
		for (int i = 0; i < ballParticlesPooledAmount; i++)
		{
			obj = Object.Instantiate(ballParticle);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			ballParticles.Add(obj);
		}
	}

	public GameObject GetBallParticles()
	{
		for (int i = 0; i < ballParticlesPooledAmount; i++)
		{
			if (!ballParticles[i].activeInHierarchy)
			{
				return ballParticles[i];
			}
		}
		Debug.Log("ERROR: GetBall returned null on CBL_BallSpawner.");
		return null;
	}

	private void PlayGoalSound()
	{
		GlobalAudio.globalAudio.Play(goalSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
