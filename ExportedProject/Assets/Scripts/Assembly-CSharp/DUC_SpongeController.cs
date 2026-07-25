using UnityEngine;

public class DUC_SpongeController : MonoBehaviour
{
	public Transform tr;

	private void Start()
	{
	}

	private void Update()
	{
		tr.Translate(-1f * DUC_Difficulty.DUC_difficulty.speed * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -5.375f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
