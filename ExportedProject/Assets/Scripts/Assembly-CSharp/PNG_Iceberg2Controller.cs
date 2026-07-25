using UnityEngine;

public class PNG_Iceberg2Controller : MonoBehaviour
{
	private float speed;

	public Transform tr;

	private void Update()
	{
		DisableIfOut();
		Move();
	}

	private void Move()
	{
		speed = PNG_Difficulty.PNG_difficulty.speed / 3f;
		tr.Translate(speed * Time.deltaTime * -1f, 0f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -7.2f)
		{
			tr.position = new Vector3(7f, tr.position.y, tr.position.z);
		}
	}
}
