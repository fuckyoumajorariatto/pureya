using UnityEngine;

public class MTO_MeteorExplosionParticlesController : MonoBehaviour
{
	public float timer;

	private void Update()
	{
		if (timer >= 1f)
		{
			Disable();
			base.gameObject.SetActive(false);
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
