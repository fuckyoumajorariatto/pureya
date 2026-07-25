using UnityEngine;

public class PositionDebugger : MonoBehaviour
{
	private void Update()
	{
		Debug.Log(base.transform.position.x);
	}
}
