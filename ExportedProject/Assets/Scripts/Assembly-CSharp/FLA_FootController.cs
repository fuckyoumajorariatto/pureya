using UnityEngine;

public class FLA_FootController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("enemy") && !FLA_PlayerController.FLA_playerController.hit && (FLA_PlayerController.FLA_playerController.stepping || FLA_PlayerController.FLA_playerController.hopping))
		{
			FLA_PlayerController.FLA_playerController.ManageHit(collision);
		}
	}
}
