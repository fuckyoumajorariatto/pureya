using UnityEngine;

public class CBL_BallParticlesController : MonoBehaviour
{
	private float timer;

	private void Update()
	{
		if (timer >= 1f)
		{
			Disable();
		}
		timer += Time.deltaTime;
	}

	private void Disable()
	{
		timer = 0f;
		base.gameObject.SetActive(false);
	}
}
