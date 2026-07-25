using UnityEngine;

public class BEE_FlowerController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(0f, (0f - BEE_Difficulty.BEE_difficulty.speed) * Time.deltaTime, 0f);
		ManageBounds();
	}

	private void ManageBounds()
	{
		if (tr.position.y < -5f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
