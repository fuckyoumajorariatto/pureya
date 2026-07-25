using UnityEngine;

public class FTL_WindowController : MonoBehaviour
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
		speed = FTL_Difficulty.FTL_difficulty.speed;
		tr.Translate(speed * Time.deltaTime * -1f, 0f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -7f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
