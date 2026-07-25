using UnityEngine;

public class SNK_SandWaveController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
		tr.position = new Vector3(Random.Range(-4f, 4f), tr.position.y, tr.position.z);
	}

	private void Update()
	{
		tr.Translate(0f, (0f - SNK_Difficulty.SNK_difficulty.scroll) * Time.deltaTime, 0f);
		if (tr.position.y < -4.5f)
		{
			tr.position = new Vector3(Random.Range(-4f, 4f), 4.5f, tr.position.z);
		}
	}
}
