using UnityEngine;

public class HEL_BuildingController : MonoBehaviour
{
	private Transform tr;

	public float[] r;

	private void Start()
	{
		tr = base.transform;
		tr.position = new Vector3(Random.Range(r[0], r[1]), Random.Range(-4f, -7f), tr.position.z);
	}

	private void Update()
	{
		tr.Translate((0f - HEL_Difficulty.HEL_difficulty.speed) * 0.75f * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -5.5f)
		{
			tr.position = new Vector3(Random.Range(5.5f, 7f), Random.Range(-4f, -7f), tr.position.z);
		}
	}
}
