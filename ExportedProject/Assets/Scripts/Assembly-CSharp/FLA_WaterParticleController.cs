using UnityEngine;

public class FLA_WaterParticleController : MonoBehaviour
{
	private float speed;

	private Transform tr;

	private SpriteRenderer sr;

	private void Start()
	{
		tr = base.transform;
		sr = tr.GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		speed = FLA_Difficulty.FLA_difficulty.scroll_speed / (2.5f + tr.position.y / 2f);
		tr.Translate(speed * Time.deltaTime, 0f, 0f);
		if (tr.position.x > 5.5f)
		{
			tr.position = new Vector3(-5.5f, Random.Range(-2f, 0f), tr.position.z);
			float num = (2f + tr.position.y) / 2f;
			num = Mathf.Floor(125f * (1f - num));
			num = 130f + num + (float)Random.Range(-10, 10);
			num = Mathf.Clamp(num, 0f, 255f);
			byte a = (byte)num;
			sr.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, a);
		}
	}
}
