using UnityEngine;

public class BEE_BgFlowerController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(0f, (0f - BEE_Difficulty.BEE_difficulty.speed) / 1.1f * Time.deltaTime, 0f);
		ManageBounds();
	}

	private void ManageBounds()
	{
		if (tr.position.y < -6f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
