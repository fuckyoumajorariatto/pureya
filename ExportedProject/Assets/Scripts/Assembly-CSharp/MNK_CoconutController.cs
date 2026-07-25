using UnityEngine;

public class MNK_CoconutController : MonoBehaviour
{
	public bool fall;

	public bool replacement;

	private float speed;

	public Transform tr;

	private void Update()
	{
		DisableIfOut();
		if (fall)
		{
			Fall();
		}
	}

	private void Fall()
	{
		speed = MNK_Difficulty.MNK_difficulty.fall_speed;
		speed *= Time.deltaTime;
		tr.Translate(0f, 0f - speed, 0f);
	}

	private void DisableIfOut()
	{
		if (tr.position.x > 5f || tr.position.y < -5f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		fall = false;
		base.gameObject.SetActive(false);
	}
}
