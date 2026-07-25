using UnityEngine;

public class CNN_SeaSplashParticleController : MonoBehaviour
{
	private float timer;

	private void OnEnable()
	{
		timer = 1f;
	}

	private void Update()
	{
		if (timer <= 0f)
		{
			base.gameObject.SetActive(false);
		}
		timer -= Time.deltaTime;
	}
}
