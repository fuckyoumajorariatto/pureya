using UnityEngine;

public class CRA_CoralController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(0f, CRA_Difficulty.CRA_difficulty.speed * 0.75f * Time.deltaTime, 0f);
		ManageBonds();
	}

	private void ManageBonds()
	{
		if (tr.position.y > 6.5f)
		{
			Deactivate();
		}
	}

	public void Deactivate()
	{
		base.gameObject.SetActive(false);
	}
}
