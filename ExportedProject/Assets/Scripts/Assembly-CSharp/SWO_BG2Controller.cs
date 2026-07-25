using UnityEngine;

public class SWO_BG2Controller : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(new Vector3(SWO_Difficulty.SWO_difficulty.bgSpeed / 2f * Time.deltaTime, 0f, 0f));
		if (tr.localPosition.x >= 6f)
		{
			tr.localPosition = new Vector2(tr.localPosition.x - 12f, 0f);
		}
	}
}
