using UnityEngine;

public class FLA_SandParticleSpawner : MonoBehaviour
{
	public Transform container;

	public GameObject sandParticle;

	private void Start()
	{
		for (int i = 0; i < 20; i++)
		{
			Object.Instantiate(sandParticle, new Vector3(Random.Range(-5.5f, 5.15f), Random.Range(-3.875f, -2.25f), 3f), Quaternion.identity, container);
		}
	}
}
