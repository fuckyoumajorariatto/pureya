using UnityEngine;

public class TOP_BushShadeController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
		float num = Random.Range(-2.4375f, 0.75f);
		num += num % 0.0625f;
		tr.position = new Vector3(tr.position.x, num, tr.position.z);
	}

	private void Update()
	{
		tr.Translate(TOP_Difficulty.TOP_difficulty.scroll * 0.75f * Time.deltaTime, 0f, 0f);
		if (tr.position.x > 5.5f)
		{
			float num = Random.Range(-2.4375f, 0.75f);
			num += num % 0.0625f;
			tr.position = new Vector3(-5.5f, num, tr.position.z);
		}
	}
}
