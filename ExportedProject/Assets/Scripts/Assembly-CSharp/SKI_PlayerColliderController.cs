using UnityEngine;

public class SKI_PlayerColliderController : MonoBehaviour
{
	public SKI_PlayerController playerController;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("enemy"))
		{
			playerController.collisionUp = collision.transform.up;
			playerController.ManageCollision();
		}
	}

	private void ChangeToSprite1()
	{
		playerController.ChangeToSprite1();
	}

	private void ChangeToSprite2()
	{
		playerController.ChangeToSprite2();
	}
}
