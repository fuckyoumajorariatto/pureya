using System.Collections.Generic;
using UnityEngine;

public class MTO_MeteorSpawner : MonoBehaviour
{
	public static MTO_MeteorSpawner mto_meteorSpawner;

	private List<GameObject> meteors;

	private int pooledAmount = 8;

	public GameObject meteor;

	private float frequency;

	private float timer;

	private float r;

	private bool odd = true;

	private int life;

	private GameObject obj;

	public MTO_PlayerController playerController;

	public List<GameObject> minimeteors;

	public GameObject minimeteor;

	private int minimeteorsPooledAmount = 35;

	public List<GameObject> meteorExplosionParticles;

	public GameObject meteorExplosionParticle;

	private int meteorExplosionParticlesPooledAmount = 10;

	public List<GameObject> minimeteorExplosionParticles;

	public GameObject minimeteorExplosionParticle;

	private int minimeteorExplosionParticlesPooledAmount = 20;

	private Transform tr;

	private Vector3 pos;

	private Quaternion rot = new Quaternion(0f, 0f, 0f, 0f);

	private int rainCooldown;

	private void Start()
	{
		mto_meteorSpawner = this;
		SetDifficultyParas();
		timer = frequency;
		PoolMeteors();
		PoolExplosionParticles();
		PoolMiniMeteors();
		PoolMinimeteorExplosionParticles();
	}

	private void Update()
	{
		if (!playerController.hit)
		{
			timer += Time.deltaTime;
			if (timer >= frequency)
			{
				timer = 0f;
				ActivateMeteor();
			}
		}
	}

	public void SetDifficultyParas()
	{
		frequency = MTO_Difficulty.MTO_difficulty.meteor_frequency;
		life = MTO_Difficulty.MTO_difficulty.meteor_life;
	}

	private void PoolMeteors()
	{
		meteors = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject gameObject = Object.Instantiate(meteor);
			gameObject.SetActive(false);
			gameObject.transform.parent = base.gameObject.transform;
			meteors.Add(gameObject);
		}
	}

	private void PoolMiniMeteors()
	{
		minimeteors = new List<GameObject>();
		for (int i = 0; i < minimeteorsPooledAmount; i++)
		{
			obj = Object.Instantiate(minimeteor);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			minimeteors.Add(obj);
		}
	}

	private void PoolExplosionParticles()
	{
		meteorExplosionParticles = new List<GameObject>();
		for (int i = 0; i < meteorExplosionParticlesPooledAmount; i++)
		{
			obj = Object.Instantiate(meteorExplosionParticle);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			meteorExplosionParticles.Add(obj);
		}
	}

	private void PoolMinimeteorExplosionParticles()
	{
		minimeteorExplosionParticles = new List<GameObject>();
		for (int i = 0; i < minimeteorExplosionParticlesPooledAmount; i++)
		{
			obj = Object.Instantiate(minimeteorExplosionParticle);
			obj.SetActive(false);
			obj.transform.parent = base.gameObject.transform;
			minimeteorExplosionParticles.Add(obj);
		}
	}

	private void ActivateMeteor()
	{
		RandomPosition();
		for (int i = 0; i < pooledAmount; i++)
		{
			if (!meteors[i].activeInHierarchy)
			{
				tr = meteors[i].transform;
				meteors[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				tr.position = pos;
				tr.rotation = rot;
				meteors[i].GetComponent<MTO_MeteorController>().life = life;
				meteors[i].SetActive(true);
				meteors[i].GetComponent<MTO_MeteorController>().SetSpeed();
				break;
			}
		}
	}

	private void SpawnMinimeteorRain()
	{
		pos = new Vector2(0f, 5f);
		rot = new Quaternion(0f, 0f, 0f, 0f);
		for (float num = -3.75f; num < 3.76f; num += 0.625f)
		{
			pos.x = num;
			obj = GetMinimeteor();
			if (obj == null)
			{
				break;
			}
			tr = obj.transform;
			tr.position = pos;
			tr.rotation = rot;
			obj.GetComponent<MTO_MinimeteorController>().colliderTimer = 0f;
			obj.GetComponent<Collider2D>().enabled = false;
			obj.SetActive(true);
			obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, Random.Range(MTO_Difficulty.MTO_difficulty.meteor_speed / 3f, MTO_Difficulty.MTO_difficulty.meteor_speed / 2f) * -1f));
		}
	}

	public GameObject GetMinimeteor()
	{
		for (int i = 0; i < minimeteorsPooledAmount; i++)
		{
			if (!minimeteors[i].activeInHierarchy)
			{
				return minimeteors[i];
			}
		}
		return null;
	}

	public GameObject GetExplosionParticles()
	{
		for (int i = 0; i < meteorExplosionParticlesPooledAmount; i++)
		{
			if (!meteorExplosionParticles[i].activeInHierarchy)
			{
				return meteorExplosionParticles[i];
			}
		}
		return null;
	}

	public GameObject GetMinimeteorExplosionParticles()
	{
		for (int i = 0; i < minimeteorExplosionParticlesPooledAmount; i++)
		{
			if (!minimeteorExplosionParticles[i].activeInHierarchy)
			{
				return minimeteorExplosionParticles[i];
			}
		}
		return null;
	}

	private void RandomPosition()
	{
		if (odd)
		{
			r = Random.Range(-4f, 1f);
		}
		else
		{
			r = Random.Range(-1f, 4f);
		}
		odd = !odd;
		if (Random.Range(0f, 1f) < 0.2f)
		{
			r = 0f;
		}
		pos = new Vector3(r, 5f, base.transform.position.z);
	}
}
