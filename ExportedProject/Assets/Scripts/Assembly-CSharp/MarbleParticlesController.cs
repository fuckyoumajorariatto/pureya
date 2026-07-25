using UnityEngine;

public class MarbleParticlesController : MonoBehaviour
{
	public float timer;

	private void Update()
	{
		if (timer > 0.5f)
		{
			Disable();
		}
		timer += Time.deltaTime;
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}
}
