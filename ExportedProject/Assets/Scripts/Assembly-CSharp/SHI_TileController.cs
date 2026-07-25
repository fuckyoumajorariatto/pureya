using UnityEngine;

public class SHI_TileController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(0f, (0f - SHI_Difficulty.SHI_difficulty.speed) * Time.deltaTime, 0f);
		if (tr.position.y < -5.5f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
