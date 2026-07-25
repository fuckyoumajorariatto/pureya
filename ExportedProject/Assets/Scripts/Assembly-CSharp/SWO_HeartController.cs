using UnityEngine;

public class SWO_HeartController : MonoBehaviour
{
	public static SWO_HeartController SWO_heartController;

	public GameObject particles;

	private bool hit;

	private Animator animator;

	private Collider2D coll;

	private void Start()
	{
		SWO_heartController = this;
		animator = base.gameObject.GetComponent<Animator>();
		coll = base.gameObject.GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!hit && collision.CompareTag("enemy"))
		{
			SWO_PlayerController.SWO_playerController.ManageHit(collision);
			hit = true;
			particles.SetActive(true);
			animator.SetBool("hit", true);
			coll.enabled = false;
		}
	}

	public void Reset()
	{
		animator.SetBool("hit", false);
		particles.SetActive(false);
		hit = false;
		coll.enabled = true;
	}
}
