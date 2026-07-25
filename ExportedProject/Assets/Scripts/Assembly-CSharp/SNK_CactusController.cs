using UnityEngine;

public class SNK_CactusController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(0f, (0f - SNK_Difficulty.SNK_difficulty.scroll) * Time.deltaTime, 0f);
		if (tr.position.y < -5.5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
