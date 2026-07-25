using UnityEngine;

public class BAL_CactusController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(BAL_Difficulty.BAL_difficulty.scrollSpeed * Time.deltaTime, 0f, 0f);
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (tr.position.x > 7f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
