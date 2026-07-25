using UnityEngine;

public class TOP_BumpParticlesController : MonoBehaviour
{
	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(Time.deltaTime * 2f, 0f, 0f);
		if (tr.position.x > 7.5f)
		{
			tr.Translate(-14f, 0f, 0f);
		}
	}
}
