using UnityEngine;

public class PPR_CloudController : MonoBehaviour
{
	private float speed;

	private float speedModifier = 0.5f;

	public Transform tr;

	private void Start()
	{
		ChangeSpeedModifier();
	}

	private void Update()
	{
		DisableIfOut();
		Move();
	}

	private void Move()
	{
		speed = PPR_Difficulty.PPR_difficulty.balloon_speed / 3f;
		tr.Translate(speed / speedModifier * Time.deltaTime * -1f, 0f, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -6f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		ChangeSpeedModifier();
		base.gameObject.SetActive(false);
	}

	private void ChangeSpeedModifier()
	{
		speedModifier = Random.Range(0.75f, 1.25f);
	}
}
