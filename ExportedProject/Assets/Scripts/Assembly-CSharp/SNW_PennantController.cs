using UnityEngine;

public class SNW_PennantController : MonoBehaviour
{
	public Transform tr;

	private float speed;

	private void Update()
	{
		Move();
		ManageOut();
	}

	private void Move()
	{
		speed = SNW_Difficulty.SNW_difficulty.scroll_speed;
		tr.Translate(new Vector3(0f, speed * Time.deltaTime, 0f));
	}

	private void ManageOut()
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
