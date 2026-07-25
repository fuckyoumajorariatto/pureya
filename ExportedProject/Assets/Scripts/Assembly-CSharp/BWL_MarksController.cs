using UnityEngine;

public class BWL_MarksController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(0f, Time.deltaTime * BWL_Difficulty.BWL_difficulty.scroll, 0f);
		if (tr.position.y > 7f)
		{
			tr.position = new Vector3(0f, -7f, tr.position.z);
		}
	}
}
