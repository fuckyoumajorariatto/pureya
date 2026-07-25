using UnityEngine;

public class HEL_CraneArmController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate((0f - HEL_Difficulty.HEL_difficulty.speed) * 0.875f * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -9f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
