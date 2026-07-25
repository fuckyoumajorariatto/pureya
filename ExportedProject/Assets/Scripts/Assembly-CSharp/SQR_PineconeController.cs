using UnityEngine;

public class SQR_PineconeController : MonoBehaviour
{
	private float speed;

	public Rigidbody2D rb;

	public Transform tr;

	private void Update()
	{
		DisableIfOut();
		Move();
	}

	private void Move()
	{
		speed = SQR_Difficulty.SQR_difficulty.scroll_speed;
		tr.Translate(0f, speed * Time.deltaTime, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.y > 5f || tr.position.y < -8f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
		rb.gravityScale = 0f;
		rb.velocity = new Vector2(0f, 0f);
		rb.angularVelocity = 0f;
		base.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		rb.gravityScale = 5f;
	}
}
