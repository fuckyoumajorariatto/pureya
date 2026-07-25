using UnityEngine;

public class BAT_BgDarknessSquareController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		Move();
		ManageOutOfBounds();
	}

	private void Move()
	{
		tr.Translate((0f - BAT_Difficulty.BAT_difficulty.bgSquaresSpeed) * Time.deltaTime, 0f, 0f);
	}

	private void ManageOutOfBounds()
	{
		if (tr.position.x < -6f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
