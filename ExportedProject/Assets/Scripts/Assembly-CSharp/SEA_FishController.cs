using UnityEngine;

public class SEA_FishController : MonoBehaviour
{
	public static SEA_FishController SEA_fishController;

	private float speed;

	public Transform tr;

	private void Start()
	{
		SEA_fishController = this;
		SetDifficultyParas();
	}

	private void Update()
	{
		Move();
	}

	public void SetDifficultyParas()
	{
		speed = SEA_Difficulty.SEA_difficulty.speed / 2f;
	}

	private void Move()
	{
		tr.Translate(0f, speed * Time.deltaTime, 0f);
		if (tr.position.y > 8f)
		{
			tr.position = new Vector3(tr.position.x, -8f, tr.position.y);
		}
	}
}
