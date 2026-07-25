using UnityEngine;

public class FLA_CloudController : MonoBehaviour
{
	private float speed;

	private float speedModifier;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
		tr.position = new Vector3(tr.position.x + Random.Range(-0.5f, 0.5f), Random.Range(27f / 32f, 3.5625f), tr.position.z);
		speedModifier = Random.Range(-1f, 1f);
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		speed = FLA_Difficulty.FLA_difficulty.scroll_speed / (5f + speedModifier);
		tr.Translate(speed * Time.deltaTime, 0f, 0f);
		if (tr.position.x > 5.5f)
		{
			tr.position = new Vector3(Random.Range(-6f, -5.5f), Random.Range(27f / 32f, 3.5625f), tr.position.z);
			speedModifier = Random.Range(-1f, 1f);
		}
	}
}
