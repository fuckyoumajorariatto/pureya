using UnityEngine;

public class CRA_BulletController : MonoBehaviour
{
	public Transform tr;

	private float speed = 10f;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	private bool deactivating;

	private float timer;

	public ParticleSystem ps;

	public Collider2D coll;

	private void Update()
	{
		tr.Translate(0f, (0f - speed) * Time.deltaTime, 0f);
		if (tr.position.y < -4.3125f && !deactivating)
		{
			Deactivate();
		}
		if (deactivating)
		{
			timer += Time.deltaTime;
		}
		if (timer > 1f)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void Deactivate()
	{
		coll.enabled = false;
		deactivating = true;
		sr.enabled = false;
		sr_s.enabled = false;
		ps.Stop();
	}

	public void Initialize()
	{
		coll.enabled = true;
		sr.enabled = true;
		sr_s.enabled = true;
		deactivating = false;
		timer = 0f;
	}
}
