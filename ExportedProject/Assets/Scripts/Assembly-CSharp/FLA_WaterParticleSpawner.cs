using UnityEngine;

public class FLA_WaterParticleSpawner : MonoBehaviour
{
	public Transform container;

	public GameObject waterParticle;

	private Transform tr;

	private void Start()
	{
		for (int i = 0; i < 20; i++)
		{
			tr = Object.Instantiate(waterParticle, new Vector3(Random.Range(-5.5f, 5.15f), Random.Range(-2f, 0f), 3f), Quaternion.identity, container).transform;
			float num = (2f + tr.position.y) / 2f;
			num = Mathf.Floor(125f * (1f - num));
			num = 130f + num + (float)Random.Range(-10, 10);
			num = Mathf.Clamp(num, 0f, 255f);
			byte a = (byte)num;
			tr.GetComponent<SpriteRenderer>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, a);
		}
	}
}
