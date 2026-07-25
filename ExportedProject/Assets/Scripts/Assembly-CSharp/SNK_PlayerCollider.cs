using UnityEngine;

public class SNK_PlayerCollider : MonoBehaviour
{
	public SNK_PlayerController playerController;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		playerController.ManageHit(collision);
	}
}
