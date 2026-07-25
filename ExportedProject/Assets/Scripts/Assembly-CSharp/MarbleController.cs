using UnityEngine;

public class MarbleController : MonoBehaviour
{
	public enum Direction
	{
		up = 0,
		down = 1,
		left = 2,
		right = 3,
		upLeft = 4,
		upRight = 5,
		downLeft = 6,
		downRight = 7,
		none = 8
	}

	public float speed;

	public float maxOut = 5f;

	public Direction direction;

	public GameObject shadow;

	public AudioClip clip;

	public AudioClip goldenClip;

	public static float pitch = 0.95f;

	private float cooldown;

	public Transform tr;

	public bool golden;

	private GameObject go;

	private bool disabled;

	private void Update()
	{
		disabled = false;
		Move();
		DisableIfOut();
		if (cooldown > 0f)
		{
			cooldown -= Time.deltaTime;
		}
	}

	public void ResetMarble()
	{
		disabled = false;
		cooldown = 0f;
	}

	private void Move()
	{
		switch (direction)
		{
		case Direction.up:
			tr.Translate(0f, speed * Time.deltaTime, 0f);
			break;
		case Direction.down:
			tr.Translate(0f, (0f - speed) * Time.deltaTime, 0f);
			break;
		case Direction.left:
			tr.Translate((0f - speed) * Time.deltaTime, 0f, 0f);
			break;
		case Direction.right:
			tr.Translate(speed * Time.deltaTime, 0f, 0f);
			break;
		case Direction.upLeft:
			tr.Translate((0f - speed) * Time.deltaTime, speed * Time.deltaTime, 0f);
			break;
		case Direction.upRight:
			tr.Translate(speed * Time.deltaTime, speed * Time.deltaTime, 0f);
			break;
		case Direction.downLeft:
			tr.Translate((0f - speed) * Time.deltaTime, (0f - speed) * Time.deltaTime, 0f);
			break;
		case Direction.downRight:
			tr.Translate(speed * Time.deltaTime, (0f - speed) * Time.deltaTime, 0f);
			break;
		}
	}

	private void DisableIfOut()
	{
		if (tr.position.x > maxOut || tr.position.x < 0f - maxOut || tr.position.y > maxOut || tr.position.y < 0f - maxOut)
		{
			Disable();
		}
	}

	public void Disable()
	{
		direction = Direction.none;
		disabled = true;
		golden = false;
		tr.localScale = new Vector3(1f, 1f, 1f);
		base.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("player") && cooldown <= 0f && !disabled)
		{
			Collected(collision);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("player") && cooldown <= 0f && !disabled)
		{
			Collected(collision);
		}
	}

	private void Collected(Collider2D collision)
	{
		cooldown = 0.2f;
		if (PauseController.pauseController.exiting)
		{
			Disable();
			return;
		}
		if (golden)
		{
			GlobalControl.globalControl.goldenMarbles++;
			PlayGoldenSound();
		}
		ActivateParticles();
		PlaySound();
		if ((GlobalControl.globalControl.mode == "normal" || GlobalControl.globalControl.mode == "endless") && GlobalControl.globalControl.marbles < 1000)
		{
			GlobalControl.globalControl.marbles++;
		}
		else if (GlobalControl.globalControl.alternateMarbles < 999)
		{
			GlobalControl.globalControl.alternateMarbles++;
		}
		if (!SceneChangeController.sceneChangeController.inTransition)
		{
			GlobalControl.globalControl.AddXp();
		}
		if (GlobalControl.globalControl.alternateMarbles >= 999 && !GlobalControl.globalControl._555)
		{
			GlobalControl.globalControl._555 = true;
			GlobalControl.globalControl.UnlockAchievements();
		}
		if (GlobalControl.globalControl.marbles >= 999 && !GlobalControl.globalControl._555)
		{
			GlobalControl.globalControl._555 = true;
			GlobalControl.globalControl.UnlockAchievements();
		}
		ActivateNumbers();
		Disable();
	}

	private void ActivateParticles()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			go = MarbleCreator.marbleCreator.GetMarbleParticles();
			if (go != null)
			{
				go.transform.position = tr.position;
				go.transform.rotation = tr.rotation;
				go.SetActive(true);
			}
		}
	}

	private void ActivateNumbers()
	{
		go = MarbleCreator.marbleCreator.GetMarbleNumbers();
		if (go != null)
		{
			go.transform.position = tr.position;
		}
	}

	private void PlaySound()
	{
		if (pitch < 1.91f)
		{
			pitch += 0.05f;
		}
		else
		{
			pitch = 1f;
		}
		GlobalAudio.globalAudio.Play(clip, 1f, pitch, tr.position.x / 4f);
	}

	private void PlayGoldenSound()
	{
		GlobalAudio.globalAudio.Play(goldenClip, 0.1f, 1f + pitch / 10f, tr.position.x / 4f);
	}

	public static void ResetMarblePitch()
	{
		pitch = 0.95f;
	}
}
