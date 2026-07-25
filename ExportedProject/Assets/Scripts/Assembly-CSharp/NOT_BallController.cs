using UnityEngine;

public class NOT_BallController : MonoBehaviour
{
	public NOT_PlayerController playerController;

	private Rigidbody2D rb;

	private Transform tr;

	private bool hit;

	public AudioClip playerBounce;

	public AudioClip wallBounce;

	public AudioClip hitSound;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		tr = base.transform;
		DoReset();
	}

	private void Update()
	{
		if (rb.velocity.magnitude < NOT_Difficulty.NOT_difficulty.ballSpeed)
		{
			rb.velocity += rb.velocity * Time.deltaTime;
		}
		else if (rb.velocity.magnitude > NOT_Difficulty.NOT_difficulty.ballSpeed + 0.1f)
		{
			rb.velocity -= rb.velocity * Time.deltaTime;
		}
		if (rb.velocity.y < 0f && rb.velocity.y > -5f)
		{
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - Time.deltaTime);
		}
		else if (rb.velocity.y >= 0f && rb.velocity.y < 5f)
		{
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + Time.deltaTime);
		}
		if (rb.velocity.x < 0f && rb.velocity.x > -0.05f)
		{
			rb.velocity = new Vector2(rb.velocity.x - Time.deltaTime * 0.1f, rb.velocity.y);
		}
		else if (rb.velocity.x > 0f && (double)rb.velocity.x < 0.05)
		{
			rb.velocity = new Vector2(rb.velocity.x + Time.deltaTime * 0.1f, rb.velocity.y);
		}
		else if (rb.velocity.x == 0f)
		{
			if ((double)Random.Range(0f, 1f) < 0.5)
			{
				rb.velocity = new Vector2(rb.velocity.x - Time.deltaTime, rb.velocity.y);
			}
			else
			{
				rb.velocity = new Vector2(rb.velocity.x + Time.deltaTime, rb.velocity.y);
			}
		}
		if (tr.position.y < -4.375f && !hit)
		{
			ManageCollision();
		}
	}

	private void ManageCollision()
	{
		hit = true;
		PlayHitSound();
		playerController.ManageCollision();
	}

	public void DoReset()
	{
		tr.position = new Vector3(0f, 3.5625f, 0f);
		rb.velocity = Vector2.up * (0f - NOT_Difficulty.NOT_difficulty.ballSpeed);
		hit = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("player"))
		{
			PlayPlayerBounce();
			float num = rb.velocity.y;
			if (num <= 0f && tr.position.y > collision.transform.position.y - 0.5f)
			{
				num = 0f - num;
				if (num < 1f)
				{
					num = 1f;
				}
			}
			if (rb.velocity.x < 0f)
			{
				if (playerController.currentSpeed < rb.velocity.x)
				{
					rb.velocity = new Vector2(playerController.currentSpeed, num);
				}
			}
			else if (playerController.currentSpeed > rb.velocity.x)
			{
				rb.velocity = new Vector2(playerController.currentSpeed, num);
			}
		}
		else
		{
			PlayWallBounce();
		}
	}

	private void PlayPlayerBounce()
	{
		GlobalAudio.globalAudio.Play(playerBounce, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void PlayWallBounce()
	{
		GlobalAudio.globalAudio.Play(wallBounce, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
