using UnityEngine;

public class TOP_CloudController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
		float num = Random.Range(1.8f, 4f);
		num += num % 0.0625f;
		float num2 = Random.Range(-5.5f, 5f);
		num2 += num2 % 0.0625f;
		tr.position = new Vector3(num2, num, tr.position.z);
	}

	private void Update()
	{
		tr.Translate(TOP_Difficulty.TOP_difficulty.scroll * 0.5f * Time.deltaTime, 0f, 0f);
		if (tr.position.x > 5.5f)
		{
			float num = Random.Range(1.8f, 4f);
			num += num % 0.0625f;
			tr.position = new Vector3(-5.5f, num, tr.position.z);
		}
	}
}
