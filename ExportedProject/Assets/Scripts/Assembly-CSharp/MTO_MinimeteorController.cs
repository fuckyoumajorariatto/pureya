using UnityEngine;

public class MTO_MinimeteorController : MonoBehaviour
{
	public float colliderTimer;

	public GameObject minimeteorExplosionParticles;

	public AudioClip clip;

	private Transform tr;

	private GameObject explosion;

	private Transform explosionTR;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		if (colliderTimer < 0.25f)
		{
			ActivateCollider();
		}
		DisableIfOut();
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}

	private void DisableIfOut()
	{
		if (tr.position.x < -5f || tr.position.x > 5f || tr.position.y < -5f || tr.position.y > 5f)
		{
			Disable();
		}
	}

	private void ActivateCollider()
	{
		colliderTimer += Time.deltaTime;
		if (colliderTimer >= 0.25f)
		{
			GetComponent<Collider2D>().enabled = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ActivateExplosionParticles();
		Disable();
		PlaySound();
	}

	private void ActivateExplosionParticles()
	{
		explosion = MTO_MeteorSpawner.mto_meteorSpawner.GetMinimeteorExplosionParticles();
		if (explosion != null)
		{
			explosionTR = explosion.transform;
			explosionTR.position = base.transform.position;
			explosionTR.rotation = base.transform.rotation;
			explosion.GetComponent<MTO_MeteorExplosionParticlesController>().timer = 0f;
			explosion.SetActive(true);
		}
	}

	private void PlaySound()
	{
		GlobalAudio.globalAudio.Play(clip, Random.Range(0.75f, 1f), Random.Range(1.5f, 2f), base.transform.position.x / 4f);
	}
}
