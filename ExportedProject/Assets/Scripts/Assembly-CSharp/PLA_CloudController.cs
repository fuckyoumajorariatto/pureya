using UnityEngine;

public class PLA_CloudController : MonoBehaviour
{
	private Transform tr;

	public float speed;

	public float[] rangeY;

	public float resetX;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(Time.deltaTime * speed, 0f, 0f);
		if (tr.localPosition.x > 0f - resetX)
		{
			tr.localPosition = new Vector2(resetX, Random.Range(rangeY[0], rangeY[1]));
		}
	}
}
