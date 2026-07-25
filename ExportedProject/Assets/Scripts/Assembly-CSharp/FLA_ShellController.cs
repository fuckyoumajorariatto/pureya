using UnityEngine;

public class FLA_ShellController : MonoBehaviour
{
	public Transform tr;

	private float speed;

	private void Update()
	{
		DisableIfOut();
		Move();
	}

	private void Move()
	{
		speed = FLA_Difficulty.FLA_difficulty.scroll_speed;
		tr.Translate(speed * Time.deltaTime, 0f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x > 6.5f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
