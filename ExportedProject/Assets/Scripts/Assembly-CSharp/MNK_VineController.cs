using UnityEngine;

public class MNK_VineController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (tr.position.x > 5.5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
