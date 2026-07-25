using UnityEngine;

public class PPR_BalloonController : MonoBehaviour
{
	private float speed;

	public Transform tr;

	private RaycastHit2D hitRC;

	private Transform hitTR;

	public LayerMask layerMask;

	public Transform raycastOrigin;

	private void Update()
	{
		DisableIfOut();
		Move();
	}

	private void Move()
	{
		speed = PPR_Difficulty.PPR_difficulty.balloon_speed;
		tr.Translate(speed * Time.deltaTime * -1f, speed / 6f * Time.deltaTime, 0f);
		if (!(tr.position.x < -1.5f))
		{
			return;
		}
		hitRC = Physics2D.Raycast(new Vector2(raycastOrigin.position.x, raycastOrigin.position.y - 19f / 32f), Vector2.up, 1.1875f, layerMask);
		if (!(hitRC.collider != null))
		{
			return;
		}
		hitTR = hitRC.transform;
		if (hitTR.CompareTag("player"))
		{
			if (raycastOrigin.position.y > hitTR.position.y)
			{
				tr.Translate(0f, speed * Time.deltaTime, 0f);
			}
			else
			{
				tr.Translate(0f, speed * (0f - Time.deltaTime), 0f);
			}
		}
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -5f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
