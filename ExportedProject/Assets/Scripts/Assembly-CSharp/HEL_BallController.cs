using UnityEngine;

public class HEL_BallController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate((0f - HEL_Difficulty.HEL_difficulty.speed) * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -5f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
