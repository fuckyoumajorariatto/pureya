using UnityEngine;

public class BAL_RockController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(BAL_Difficulty.BAL_difficulty.scrollSpeed / 5f * Time.deltaTime, 0f, 0f);
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (base.transform.position.x > 6f)
		{
			tr.position = new Vector3(-6f, Random.Range(-1.75f, -0.75f), tr.position.z);
			tr.localScale = new Vector3(tr.localScale.x * -1f, tr.localScale.y, tr.localScale.z);
		}
	}
}
