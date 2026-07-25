using UnityEngine;

public class PLA_MoonController : MonoBehaviour
{
	public Transform tr;

	public PLA_PlayerController playerController;

	private void Update()
	{
		tr.rotation = Quaternion.identity;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("enemy"))
		{
			playerController.ManageCollision();
		}
	}
}
