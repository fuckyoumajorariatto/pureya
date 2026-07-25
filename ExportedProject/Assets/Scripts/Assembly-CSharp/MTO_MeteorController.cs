using UnityEngine;

public class MTO_MeteorController : MonoBehaviour
{
	private Color32 shadowColor = new Color32(0, 0, 0, byte.MaxValue);

	private float speed;

	public int life;

	private float colorTimer;

	private Color32 hitColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	public AudioClip meteorHit;

	public AudioClip meteorAgainstMeteor;

	public AudioClip explosion;

	public Transform tr;

	public SpriteRenderer sr;

	public Rigidbody2D rb;

	private GameObject marble;

	private MarbleController marbleController;

	private Transform marbleTR;

	private Vector3 pos;

	private Quaternion rot;

	private float minimeteorSpeed;

	private GameObject mini;

	private Transform miniTR;

	private GameObject particles;

	private Transform particlesTR;

	private void FixedUpdate()
	{
		ManageColor();
		DisableIfOut();
	}

	private void ManageColor()
	{
		if (colorTimer > 0f)
		{
			colorTimer -= Time.deltaTime;
			if (colorTimer <= 0f)
			{
				sr.color = Color.white;
			}
		}
	}

	private void DisableIfOut()
	{
		if (base.transform.position.x > 5f || base.transform.position.x < -5f || base.transform.position.y > 6f || base.transform.position.y < -5f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}

	public void SetSpeed()
	{
		speed = MTO_Difficulty.MTO_difficulty.meteor_speed;
		rb.AddForce(new Vector2(Random.Range((0f - speed) / 2f, speed / 2f), 0f - speed));
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("bullet") || (collision.transform.CompareTag("enemy") && base.transform.position.y < 4.3f))
		{
			ManageHit(collision);
		}
	}

	private void ManageHit(Collision2D collision)
	{
		life--;
		sr.color = hitColor;
		colorTimer = 0.25f;
		PlayHit(collision.transform.tag);
		if (life <= 0)
		{
			Explode();
		}
	}

	private void SpawnMarble()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			marble = MarbleCreator.marbleCreator.GetMarble();
			marbleController = marble.GetComponent<MarbleController>();
			marbleTR = marble.transform;
			marbleTR.position = base.transform.position;
			marbleTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
			marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
			marbleController.speed = 7f;
			marbleController.direction = MarbleController.Direction.down;
			marbleController.maxOut = 6f;
			marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_right;
			marble.SetActive(true);
		}
	}

	private void Explode()
	{
		SpawnMarble();
		SpawnMinimeteors();
		ActivateExplosionParticles();
		PlayExplosion();
		Disable();
	}

	private void SpawnMinimeteors()
	{
		if (!(tr.position.y < -2.5f) && !((double)tr.position.x < -4.2) && !((double)tr.position.x > 4.2))
		{
			pos = base.transform.position;
			rot = new Quaternion(0f, 0f, 0f, 0f);
			minimeteorSpeed = speed / 4f;
			mini = MTO_MeteorSpawner.mto_meteorSpawner.GetMinimeteor();
			miniTR = mini.transform;
			if (mini != null)
			{
				miniTR.position = pos;
				miniTR.rotation = rot;
				mini.GetComponent<MTO_MinimeteorController>().colliderTimer = 0f;
				mini.GetComponent<Collider2D>().enabled = false;
				mini.SetActive(true);
				mini.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range((0f - minimeteorSpeed) / 2f, minimeteorSpeed / 2f), minimeteorSpeed));
			}
			mini = MTO_MeteorSpawner.mto_meteorSpawner.GetMinimeteor();
			miniTR = mini.transform;
			if (mini != null)
			{
				miniTR.position = pos;
				miniTR.rotation = rot;
				mini.GetComponent<MTO_MinimeteorController>().colliderTimer = 0f;
				mini.GetComponent<Collider2D>().enabled = false;
				mini.SetActive(true);
				mini.GetComponent<Rigidbody2D>().AddForce(new Vector2((0f - minimeteorSpeed) * Random.Range(0.5f, 1.5f), (0f - minimeteorSpeed) * Random.Range(0.5f, 1.5f)));
			}
			mini = MTO_MeteorSpawner.mto_meteorSpawner.GetMinimeteor();
			miniTR = mini.transform;
			if (mini != null)
			{
				miniTR.position = pos;
				miniTR.rotation = rot;
				mini.GetComponent<MTO_MinimeteorController>().colliderTimer = 0f;
				mini.GetComponent<Collider2D>().enabled = false;
				mini.SetActive(true);
				mini.GetComponent<Rigidbody2D>().AddForce(new Vector2(minimeteorSpeed * Random.Range(0.5f, 1.5f), (0f - minimeteorSpeed) * Random.Range(0.5f, 1.5f)));
			}
		}
	}

	private void ActivateExplosionParticles()
	{
		particles = MTO_MeteorSpawner.mto_meteorSpawner.GetExplosionParticles();
		particlesTR = particles.transform;
		if (particles != null)
		{
			particlesTR.position = base.transform.position;
			particlesTR.rotation = base.transform.rotation;
			particles.GetComponent<MTO_MeteorExplosionParticlesController>().timer = 0f;
			particles.SetActive(true);
		}
	}

	private void PlayHit(string tag)
	{
		if (tag == "bullet")
		{
			GlobalAudio.globalAudio.Play(meteorHit, Random.Range(0.6f, 0.8f), Random.Range(0.75f, 1.25f), base.transform.position.x / 4f);
		}
		else
		{
			GlobalAudio.globalAudio.Play(meteorAgainstMeteor, Random.Range(0.6f, 0.8f), Random.Range(0.75f, 1.25f), base.transform.position.x / 4f);
		}
	}

	private void PlayExplosion()
	{
		GlobalAudio.globalAudio.Play(explosion, Random.Range(0.6f, 0.8f), Random.Range(0.5f, 1.5f), base.transform.position.x / 4f);
	}
}
