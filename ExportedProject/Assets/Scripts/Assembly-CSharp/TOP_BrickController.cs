using UnityEngine;

public class TOP_BrickController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(Time.deltaTime * TOP_Difficulty.TOP_difficulty.scroll, 0f, 0f);
		if (tr.position.x > 7.5f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
