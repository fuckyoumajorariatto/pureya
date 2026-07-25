using UnityEngine;

public class SKI_WaveController : MonoBehaviour
{
	public float speed;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
		tr.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 2f);
		speed = Random.Range(2f, 3f);
		tr.localScale = new Vector3(Random.Range(0.5f, 1f), 1f, 1f);
	}

	private void Update()
	{
		tr.Translate((0f - Time.deltaTime) * speed, 0f, 0f);
		if (tr.position.x < -5f)
		{
			tr.position = new Vector3(5f, Random.Range(-4f, 4f), tr.position.z);
			speed = Random.Range(2f, 3f);
			tr.localScale = new Vector3(Random.Range(0.5f, 1f), 1f, 1f);
		}
	}
}
