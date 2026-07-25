using UnityEngine;

public class PLU_HitParticlesController : MonoBehaviour
{
	public Transform tr;

	private void Update()
	{
		tr.Translate(0f, 2f * Time.deltaTime, 0f);
	}
}
