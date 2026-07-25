using UnityEngine;

public class CBL_BallTest : MonoBehaviour
{
	private Rigidbody2D rb;

	private int x;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		x++;
		if (x == 10)
		{
			rb.velocity = new Vector2(0f, 0f - CBL_Difficulty.CBL_difficulty.scrollSpeed);
		}
		if (base.transform.position.y < -5f)
		{
			base.transform.position = new Vector2(-3f, 5f);
		}
	}
}
