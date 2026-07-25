using UnityEngine;

public class CRA_ShellController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(0f, CRA_Difficulty.CRA_difficulty.speed * 1.6f * Time.deltaTime, 0f);
		if (tr.position.y > 6f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("bullet"))
		{
			collision.gameObject.GetComponent<CRA_BulletController>().Deactivate();
		}
	}
}
