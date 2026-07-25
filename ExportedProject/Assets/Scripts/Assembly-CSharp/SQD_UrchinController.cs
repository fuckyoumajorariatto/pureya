using UnityEngine;

public class SQD_UrchinController : MonoBehaviour
{
	private float speed;

	private int level = -1;

	public Transform tr;

	private void Update()
	{
		if (SQD_Difficulty.SQD_difficulty.level != level)
		{
			level = SQD_Difficulty.SQD_difficulty.level;
			speed = SQD_Difficulty.SQD_difficulty.urchin_speed;
		}
		Move();
		DisableIfOut();
	}

	private void Move()
	{
		tr.Translate((0f - speed) * Time.deltaTime, 0f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
