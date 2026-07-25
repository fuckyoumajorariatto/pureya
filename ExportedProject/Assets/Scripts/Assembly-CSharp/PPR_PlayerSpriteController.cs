using UnityEngine;

public class PPR_PlayerSpriteController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		PPR_PlayerController.PPR_playerController.ManageHit(collision);
	}
}
