using UnityEngine;

public class SEA_BubbleController : MonoBehaviour
{
	private float speed;

	public Rigidbody2D rb;

	public Transform tr;

	private void Start()
	{
		SetDifficultyParas();
	}

	private void Update()
	{
		DisableIfOut();
		Move();
	}

	public void SetDifficultyParas()
	{
		speed = SEA_Difficulty.SEA_difficulty.speed;
		speed = Random.Range(speed - speed / 4f, speed + speed / 4f);
	}

	private void Move()
	{
		rb.velocity = new Vector2(0f, speed);
	}

	private void DisableIfOut()
	{
		if (tr.position.y > 5f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		SetDifficultyParas();
		base.gameObject.SetActive(false);
	}
}
