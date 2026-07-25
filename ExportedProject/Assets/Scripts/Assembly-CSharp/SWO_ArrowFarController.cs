using UnityEngine;

public class SWO_ArrowFarController : MonoBehaviour
{
	public Transform tr;

	public float speed;

	private void Update()
	{
		tr.Translate(0f, speed * Time.deltaTime, 0f);
		if (tr.position.y > 4.5f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
