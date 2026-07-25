using UnityEngine;

public class ZIP_TreeController : MonoBehaviour
{
	public Transform tr;

	private float speed;

	private void Update()
	{
		Move();
		DisableIfOut();
	}

	private void Move()
	{
		speed = ZIP_Difficulty.ZIP_difficulty.scroll_speed;
		tr.Translate(0f, (0f - speed) * Time.deltaTime, (0f - Time.deltaTime) / 100f);
	}

	private void DisableIfOut()
	{
		if (tr.position.y < -6.5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
