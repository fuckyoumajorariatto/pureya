using UnityEngine;

public class SQR_BgBranch2Controller : MonoBehaviour
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
		speed = SQR_Difficulty.SQR_difficulty.scroll_speed / 2f;
		tr.Translate(0f, speed * Time.deltaTime, 0f);
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
		base.gameObject.SetActive(false);
	}
}
