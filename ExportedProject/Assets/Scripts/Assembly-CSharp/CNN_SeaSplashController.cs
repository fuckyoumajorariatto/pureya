using UnityEngine;

public class CNN_SeaSplashController : MonoBehaviour
{
	public GameObject splashParticles1;

	public GameObject splashParticles2;

	public GameObject splashParticles3;

	private GameObject[] splashParticles = new GameObject[3];

	public AudioClip splash;

	private Transform tr;

	private void Start()
	{
		InitializeSplashParticles();
	}

	private void InitializeSplashParticles()
	{
		splashParticles[0] = splashParticles1;
		splashParticles[1] = splashParticles2;
		splashParticles[2] = splashParticles3;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ManageCollision(collision);
	}

	private void ManageCollision(Collider2D collision)
	{
		if (!collision.transform.CompareTag("enemy") && !collision.transform.CompareTag("marble"))
		{
			return;
		}
		for (int i = 0; i < splashParticles.Length; i++)
		{
			if (!splashParticles[i].activeInHierarchy)
			{
				tr = splashParticles[i].transform;
				tr.position = new Vector3(collision.transform.position.x, tr.position.y, tr.position.z);
				splashParticles[i].SetActive(true);
				PlaySplash(collision.transform.position.x);
				break;
			}
		}
	}

	private void PlaySplash(float posX)
	{
		GlobalAudio.globalAudio.Play(splash, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), posX / 4f);
	}
}
