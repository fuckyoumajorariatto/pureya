using UnityEngine;

public class BWL_BallTextureController : MonoBehaviour
{
	private Transform tr;

	public Transform twinTr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(0f, (0f - Time.deltaTime) * BWL_Difficulty.BWL_difficulty.scroll, 0f);
	}

	private void LateUpdate()
	{
		if (tr.localPosition.y < -3f)
		{
			tr.localPosition = new Vector3(0f, twinTr.localPosition.y + 3.3125f, tr.localPosition.z);
		}
	}
}
