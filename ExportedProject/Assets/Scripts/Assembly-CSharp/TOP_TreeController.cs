using UnityEngine;

public class TOP_TreeController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
		float num = Random.Range(2f, 2.375f);
		num += num % 0.0625f;
		tr.position = new Vector3(tr.position.x, num, tr.position.z);
	}

	private void Update()
	{
		tr.Translate(TOP_Difficulty.TOP_difficulty.scroll * 0.625f * Time.deltaTime, 0f, 0f);
		if (tr.position.x > 5.5f)
		{
			float num = Random.Range(2f, 2.375f);
			num += num % 0.0625f;
			tr.position = new Vector3(-5.5f, num, tr.position.z);
		}
	}
}
