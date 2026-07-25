using UnityEngine;

public class HEL_CloudController : MonoBehaviour
{
	private Transform tr;

	private float velMultiplier;

	private void Start()
	{
		tr = base.transform;
		tr.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), tr.position.z);
		velMultiplier = Random.Range(0.75f, 1.25f);
	}

	private void Update()
	{
		tr.Translate((0f - HEL_Difficulty.HEL_difficulty.speed) / 2f * velMultiplier * Time.deltaTime, 0f, 0f);
		if (tr.position.x < -5.5f)
		{
			tr.position = new Vector3(6f, Random.Range(-4f, 4f), tr.position.z);
		}
	}
}
