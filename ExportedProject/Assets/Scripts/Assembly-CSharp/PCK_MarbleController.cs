using UnityEngine;

public class PCK_MarbleController : MonoBehaviour
{
	public Rigidbody2D rb;

	private float timer;

	private float timer2;

	private Vector3 pos;

	public AudioClip pinHit;

	public AudioClip marbleHit;

	public AudioClip marbleOut;

	private GameObject lastCollision;

	private Transform GOtransform;

	public bool golden;

	private void Awake()
	{
		GOtransform = base.gameObject.transform;
	}

	private void FixedUpdate()
	{
		timer2 += Time.fixedDeltaTime;
		DisableIfOut();
		JumpIfStuck();
		ManageTooMuchTime();
		if (rb.velocity.y < -22f)
		{
			rb.velocity = new Vector2(rb.velocity.x, -22f);
		}
	}

	private void ManageTooMuchTime()
	{
		if (timer2 > 7f)
		{
			Jump();
			timer2 -= Random.Range(0.5f, 2f);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ManageCollision(collision);
	}

	private void ManageCollision(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy") && collision.transform.gameObject != lastCollision)
		{
			lastCollision = collision.transform.gameObject;
			PlayPinHit();
		}
		else if (collision.transform.CompareTag("wall"))
		{
			Disable();
			PlayOut();
		}
		else if (collision.transform.CompareTag("marble") && collision.transform.position.y <= GOtransform.position.y)
		{
			PlayMarbleHit();
		}
	}

	private void PlayPinHit()
	{
		if (!PauseController.pauseController.inCinematic)
		{
			GlobalAudio.globalAudio.PlayLowPriority(pinHit, Random.Range(0.75f, 1.25f), Random.Range(0.85f, 1.15f), GOtransform.position.x / 4f);
		}
	}

	private void PlayMarbleHit()
	{
		if (!PauseController.pauseController.inCinematic)
		{
			GlobalAudio.globalAudio.PlayLowPriority(marbleHit, Random.Range(0.75f, 1.25f), Random.Range(0.85f, 1.25f), GOtransform.position.x / 4f);
		}
	}

	private void JumpIfStuck()
	{
		if (Vector2.Distance(GOtransform.position, pos) < 0.05f)
		{
			if (timer > 1f)
			{
				Jump();
				timer = 0f;
			}
			timer += Time.deltaTime;
		}
		else
		{
			timer = 0f;
		}
		pos = GOtransform.position;
	}

	private void Jump()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), Random.Range(3f, 10f));
	}

	private void DisableIfOut()
	{
		if (GOtransform.position.y < -4.5f)
		{
			PlayOut();
			Disable();
		}
	}

	private void PlayOut()
	{
		if (!PauseController.pauseController.inCinematic)
		{
			GlobalAudio.globalAudio.PlayLowPriority(marbleOut, Random.Range(0.75f, 1.25f), Random.Range(0.85f, 1.15f), 0f);
		}
	}

	public void Disable()
	{
		timer = 0f;
		timer2 = 0f;
		GOtransform.localScale = new Vector3(1f, 1f, 1f);
		golden = false;
		base.gameObject.SetActive(false);
	}
}
