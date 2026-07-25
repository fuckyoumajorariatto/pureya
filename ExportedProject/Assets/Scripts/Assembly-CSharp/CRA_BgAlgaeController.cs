using UnityEngine;

public class CRA_BgAlgaeController : MonoBehaviour
{
	public Transform tr;

	private float speedModifier = 1f;

	private void Start()
	{
		tr.position = new Vector3(Random.Range(-4f, 4f), tr.position.y, tr.position.z);
		speedModifier = Random.Range(0.75f, 1.25f);
	}

	private void Update()
	{
		tr.Translate(0f, CRA_Difficulty.CRA_difficulty.speed / 2f * speedModifier * Time.deltaTime, 0f);
		if (tr.position.y > 6f)
		{
			tr.position = new Vector3(Random.Range(-4f, 4f), -6f, tr.position.z);
			speedModifier = Random.Range(0.75f, 1.25f);
		}
	}
}
