using UnityEngine;

public class RAT_PlatformController : MonoBehaviour
{
	private Transform tr;

	private Collider2D[] col;

	private Animator anim;

	public bool fragile;

	private bool crumbling;

	private bool falling;

	public AudioClip crumbleSound;

	private void Awake()
	{
		tr = base.transform;
		anim = GetComponent<Animator>();
		col = new Collider2D[2];
		col = GetComponents<Collider2D>();
	}

	private void Update()
	{
		if (!falling)
		{
			tr.Translate(0f, RAT_Difficulty.RAT_difficulty.speed * Time.deltaTime, 0f);
		}
		else
		{
			tr.Translate(0f, -10f * Time.deltaTime, 0f);
		}
		if (tr.position.y > 6f || tr.position.y < -6f)
		{
			crumbling = false;
			falling = false;
			base.gameObject.SetActive(false);
		}
	}

	public void Fragile(bool on)
	{
		anim.ResetTrigger("crumble");
		if (on)
		{
			fragile = true;
			anim.SetTrigger("resetFragile");
		}
		else
		{
			fragile = false;
			anim.SetTrigger("resetSolid");
		}
		col[0].enabled = true;
		col[1].enabled = true;
		crumbling = false;
		falling = false;
	}

	public void DisableCollider()
	{
		if (crumbling)
		{
			col[0].enabled = false;
			col[1].enabled = false;
			falling = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("player") && fragile)
		{
			anim.SetTrigger("crumble");
			crumbling = true;
			fragile = false;
			PlayCrumbleSound();
		}
	}

	private void OnEnable()
	{
		crumbling = false;
		falling = false;
		col[0].enabled = true;
		col[1].enabled = true;
	}

	private void PlayCrumbleSound()
	{
		GlobalAudio.globalAudio.Play(crumbleSound, 1f, Random.Range(0.8f, 1.2f), tr.position.x / 4f);
	}
}
