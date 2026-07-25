using UnityEngine;

public class TOP_BallController : MonoBehaviour
{
	private Rigidbody2D rb;

	private Transform tr;

	private SpriteRenderer sr;

	private Collider2D coll;

	public SpriteRenderer sr_s;

	public ParticleSystem ps;

	public Transform maskTR;

	public bool loseVelocity;

	private bool disabling;

	private float previousY;

	public GameObject parentGO;

	private Transform parentTR;

	private float speedModifier = 1.15f;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		tr = base.transform;
		sr = GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();
		parentTR = parentGO.transform;
	}

	private void FixedUpdate()
	{
		if (loseVelocity || disabling)
		{
			tr.position = new Vector3(tr.position.x + Time.deltaTime * TOP_Difficulty.TOP_difficulty.scroll, tr.position.y, tr.position.z);
		}
		else
		{
			tr.position = new Vector3(tr.position.x + Time.deltaTime * (TOP_Difficulty.TOP_difficulty.scroll * speedModifier), tr.position.y, tr.position.z);
		}
		if (tr.position.x > 7f)
		{
			Disable();
		}
		if (disabling)
		{
			tr.position = new Vector3(tr.position.x, previousY, tr.position.z);
		}
		previousY = tr.position.y;
		maskTR.position = tr.position;
	}

	public void Initialize()
	{
		disabling = false;
		loseVelocity = false;
		tr.position = parentTR.position;
		maskTR.position = tr.position;
		coll.enabled = true;
		speedModifier = 1f;
		Invoke("Initlialize2", 0.1f);
	}

	private void Initlialize2()
	{
		sr.enabled = true;
		sr_s.enabled = true;
		rb.angularVelocity = (0f - TOP_Difficulty.TOP_difficulty.scroll) * 200f;
	}

	public void AnimatedDisable()
	{
		disabling = true;
		ps.Play();
		rb.velocity = Vector2.zero;
		sr.enabled = false;
		sr_s.enabled = false;
		coll.enabled = false;
		Invoke("Disable", 0.2f);
	}

	public void Disable()
	{
		ps.Stop();
		parentGO.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
			if (collision.gameObject.GetComponent<TOP_BallController>().loseVelocity)
			{
				rb.velocity = Vector2.zero;
				rb.angularVelocity = 0f;
				loseVelocity = true;
			}
		}
		else if (collision.transform.CompareTag("wall") && collision.transform.position.y > tr.position.y - 0.1875f)
		{
			if (tr.position.x <= -4.56f)
			{
				TOP_Spawner.TOP_spawner.noBallCombo = 5;
				Disable();
			}
			else
			{
				rb.velocity = Vector2.zero;
				rb.angularVelocity = 0f;
				loseVelocity = true;
			}
		}
	}
}
