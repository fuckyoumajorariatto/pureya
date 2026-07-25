using UnityEngine;

public class PNG_IcebergController : MonoBehaviour
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
		speed = PNG_Difficulty.PNG_difficulty.speed / 2f;
		tr.Translate(speed * Time.deltaTime * -1f, 0f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -8f)
		{
			tr.position = new Vector3(8f, tr.position.y, tr.position.z);
		}
	}
}
