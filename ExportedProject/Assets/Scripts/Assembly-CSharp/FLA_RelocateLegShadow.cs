using UnityEngine;

public class FLA_RelocateLegShadow : MonoBehaviour
{
	private void Update()
	{
		Vector3 position = new Vector3(base.transform.parent.position.x, base.transform.parent.position.y, base.transform.position.z);
		position.x -= 0.1875f;
		position.y += 0.0625f;
		base.transform.position = position;
	}
}
