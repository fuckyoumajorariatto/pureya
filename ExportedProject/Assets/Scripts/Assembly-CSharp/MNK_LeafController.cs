using UnityEngine;

public class MNK_LeafController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (tr.position.x > 5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
