using UnityEngine;

public class Ending2MarbleScreenController : MonoBehaviour
{
	public Transform renderPlane;

	public Transform tr;

	private void Update()
	{
		tr.position = new Vector3(renderPlane.position.x, renderPlane.position.y, tr.position.z);
		tr.localScale = new Vector3(renderPlane.localScale.x, renderPlane.localScale.z, 1f);
	}
}
