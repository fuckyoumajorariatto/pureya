using UnityEngine;

public class SWO_BG4Controller : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(new Vector3(SWO_Difficulty.SWO_difficulty.bgSpeed / 4f * Time.deltaTime, 0f, 0f));
		if (tr.localPosition.x >= 6f)
		{
			tr.localPosition = new Vector2(tr.localPosition.x - 12f, Random.Range(-0.5f, 0f));
		}
	}
}
