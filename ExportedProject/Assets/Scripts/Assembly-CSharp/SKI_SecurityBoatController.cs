using UnityEngine;

public class SKI_SecurityBoatController : MonoBehaviour
{
	public float speed;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(new Vector2(0f, speed * Time.deltaTime));
		if (tr.position.y > 20f)
		{
			tr.position = new Vector3(tr.position.x, -20f, tr.position.z);
		}
	}
}
