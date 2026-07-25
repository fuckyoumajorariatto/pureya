using UnityEngine;

public class GOT_RockController : MonoBehaviour
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
		speed = GOT_Difficulty.GOT_difficulty.speed;
		tr.Translate(0f, speed * Time.deltaTime * -1f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.y < -8f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
