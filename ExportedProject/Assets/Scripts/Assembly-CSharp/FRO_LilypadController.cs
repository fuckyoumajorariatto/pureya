using UnityEngine;

public class FRO_LilypadController : MonoBehaviour
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
		speed = FRO_Difficulty.FRO_difficulty.speed;
		tr.Translate(0f, speed * Time.deltaTime * -1f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.y < -6f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
