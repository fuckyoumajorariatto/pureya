using UnityEngine;

public class MTO_BulletController : MonoBehaviour
{
	public GameObject shadow;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (tr.position.y > 4.5f)
		{
			Disable();
		}
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble"))
		{
			Disable();
		}
	}
}
