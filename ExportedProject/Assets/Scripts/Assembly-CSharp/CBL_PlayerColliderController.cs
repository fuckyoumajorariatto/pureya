using UnityEngine;

public class CBL_PlayerColliderController : MonoBehaviour
{
	public SpriteRenderer shadow;

	private Transform tr;

	private Transform parentTR;

	private CBL_PlayerController playerController;

	private Rigidbody2D rb;

	private void FixedUpdate()
	{
		if (PauseController.pauseController.paused)
		{
			return;
		}
		if (parentTR == null)
		{
			Initialize();
			return;
		}
		CheckPush();
		if (rb.velocity.magnitude != 0f || rb.angularVelocity != 0f)
		{
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0f;
		}
	}

	private void Initialize()
	{
		tr = base.transform;
		parentTR = GetComponentInParent<Transform>();
		playerController = GetComponentInParent<CBL_PlayerController>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void CheckPush()
	{
		if (tr.position.y < -4f && !playerController.hit)
		{
			playerController.ManageHit();
		}
		else if (tr.position.y < -3.125f && !playerController.hit)
		{
			tr.position = new Vector3(tr.position.x, -3.125f, tr.position.z);
		}
		else if (tr.position.y > -3.125f && !playerController.hit)
		{
			tr.position = new Vector3(tr.position.x, -3.125f, tr.position.z);
		}
		if (tr.position.x != parentTR.position.x)
		{
			tr.position = parentTR.position;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
			playerController.ManageHit();
		}
		else if (collision.transform.CompareTag("ball"))
		{
			playerController.CallResetColliders();
		}
	}
}
