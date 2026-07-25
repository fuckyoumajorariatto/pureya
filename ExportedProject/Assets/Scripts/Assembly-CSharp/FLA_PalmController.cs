using UnityEngine;

public class FLA_PalmController : MonoBehaviour
{
	private float speed;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		speed = FLA_Difficulty.FLA_difficulty.scroll_speed / 3f;
		tr.Translate(speed * Time.deltaTime, 0f, 0f);
		if (tr.position.x > 5.5f)
		{
			tr.position = new Vector3(-5.5f, Random.Range(1f, 1.3125f), tr.position.z);
		}
	}
}
