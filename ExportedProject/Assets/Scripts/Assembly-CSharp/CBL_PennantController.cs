using UnityEngine;

public class CBL_PennantController : MonoBehaviour
{
	private float speed;

	public Collider2D coll;

	public Transform tr;

	private float extraY = 5.12f;

	private void Start()
	{
		speed = CBL_Difficulty.CBL_difficulty.scrollSpeed;
	}

	private void Update()
	{
		speed = CBL_Difficulty.CBL_difficulty.scrollSpeed;
		ManageMovement();
		ManageOutOfBounds();
		ManageColliderActivation();
	}

	private void ManageColliderActivation()
	{
		if (base.transform.position.y > 4f)
		{
			coll.enabled = false;
		}
		else
		{
			coll.enabled = true;
		}
	}

	private void ManageMovement()
	{
		tr.Translate(0f, (0f - speed) * Time.deltaTime, 0f);
		ManageZ();
	}

	private void ManageZ()
	{
		tr.position = new Vector3(tr.position.x, tr.position.y, (tr.position.y - extraY) / 10f);
	}

	private void ManageOutOfBounds()
	{
		if (tr.position.y < -5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
