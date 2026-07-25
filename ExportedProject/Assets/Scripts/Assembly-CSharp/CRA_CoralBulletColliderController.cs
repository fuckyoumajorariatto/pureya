using UnityEngine;

public class CRA_CoralBulletColliderController : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("bullet"))
		{
			collision.gameObject.GetComponent<CRA_BulletController>().Deactivate();
		}
	}
}
