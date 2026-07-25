using UnityEngine;

public class SNW_PlayerColliderController : MonoBehaviour
{
	public SNW_PlayerController playerController;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
			playerController.ManageHit();
		}
	}
}
