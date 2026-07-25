using UnityEngine;

public class CBL_BallController : MonoBehaviour
{
	private float speed;

	public AudioClip ballHit;

	public AudioClip ballVsObject;

	private bool activateCollider = true;

	public CircleCollider2D coll;

	public Transform tr;

	public Rigidbody2D rb;

	private float maxVel = 20f;

	private float extraY;

	private bool ensureVerticalSpeedNextFrame;

	private float newVelocity;

	private void Start()
	{
		speed = CBL_Difficulty.CBL_difficulty.scrollSpeed;
		ensureVerticalSpeedNextFrame = false;
	}

	private void Update()
	{
		ManageOutOfBounds();
		speed = CBL_Difficulty.CBL_difficulty.scrollSpeed;
		LimitVelocity();
		AddRollingVelocity();
		if (activateCollider && base.transform.position.y < 4f)
		{
			coll.enabled = true;
			activateCollider = false;
		}
		if (ensureVerticalSpeedNextFrame)
		{
			ensureVerticalSpeedNextFrame = false;
			if (rb.velocity.y < 10f)
			{
				rb.velocity = new Vector2(rb.velocity.x, 10f);
			}
		}
		ManageZ();
	}

	private void ManageZ()
	{
		if (rb.velocity.y > 0f)
		{
			extraY = 10f;
		}
		else
		{
			extraY = 5.12f;
		}
		tr.position = new Vector3(tr.position.x, tr.position.y, (tr.position.y - extraY) / 10f);
	}

	private void AddRollingVelocity()
	{
		if (rb.velocity.y > 0f - speed)
		{
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 10f * Time.deltaTime);
			if (rb.velocity.y < 0f - speed)
			{
				rb.velocity = new Vector2(rb.velocity.x, 0f - speed);
			}
		}
	}

	private void LimitVelocity()
	{
		maxVel = 15f;
		if (rb.velocity.x > maxVel)
		{
			rb.velocity = new Vector2(maxVel, rb.velocity.y);
		}
		else if (rb.velocity.x < 0f - maxVel)
		{
			rb.velocity = new Vector2(0f - maxVel, rb.velocity.y);
		}
		if (rb.velocity.y > maxVel)
		{
			rb.velocity = new Vector2(rb.velocity.x, maxVel);
		}
		else if (rb.velocity.y < 0f - maxVel)
		{
			rb.velocity = new Vector2(rb.velocity.x, 0f - maxVel);
		}
	}

	private void ManageOutOfBounds()
	{
		if (tr.position.y < -4.5f || tr.position.y > 4.5f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		ensureVerticalSpeedNextFrame = false;
		rb.velocity = new Vector2(0f, 0f);
		rb.rotation = 0f;
		coll.enabled = false;
		base.gameObject.SetActive(false);
	}

	public void Enable()
	{
		ensureVerticalSpeedNextFrame = false;
		coll.enabled = false;
		activateCollider = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("player"))
		{
			PlayCarHitSound();
			if (collision.transform.position.y < tr.position.y - 0.5f)
			{
				ensureVerticalSpeedNextFrame = true;
			}
			else if (collision.transform.position.x > tr.position.x)
			{
				rb.velocity = new Vector2(-10f, -10f);
			}
			else
			{
				rb.velocity = new Vector2(10f, -10f);
			}
		}
		else
		{
			PlayObjectHitSound();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("player") && collision.transform.position.y < tr.position.y - 0.5f)
		{
			ensureVerticalSpeedNextFrame = true;
		}
	}

	private void PlayCarHitSound()
	{
		GlobalAudio.globalAudio.Play(ballHit, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void PlayObjectHitSound()
	{
		GlobalAudio.globalAudio.Play(ballVsObject, 1f, Random.Range(0.75f, 1.25f), tr.position.x / 4f);
	}
}
