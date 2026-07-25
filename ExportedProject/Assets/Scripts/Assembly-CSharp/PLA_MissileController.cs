using UnityEngine;

public class PLA_MissileController : MonoBehaviour
{
	public bool chase;

	private Transform tr;

	private float speed = 0.5f;

	private float speedModifier;

	public Transform playerTR;

	private Vector2 target;

	private float acceleration = 100f;

	private float inertia;

	private float maxInertia = 1f;

	private float moveSpeed;

	public AudioClip launchSound;

	public AudioClip appearSound;

	private AudioSource aS;

	private bool soundPlayed;

	private bool soundPlayed2;

	private void Start()
	{
		tr = base.transform;
	}

	private void OnEnable()
	{
		speedModifier = 1f;
		inertia = 0f;
		playerTR = GameObject.FindGameObjectWithTag("player").transform;
		moveSpeed = PLA_Difficulty.PLA_difficulty.missileSpeed;
		soundPlayed = false;
		soundPlayed2 = false;
	}

	private void FixedUpdate()
	{
		Move();
		ManageOutOfBounds();
		tr.eulerAngles += Vector3.forward * inertia;
		if (aS != null && !SceneChangeController.sceneChangeController.transitioning)
		{
			aS.panStereo = tr.position.x / 4f;
		}
	}

	private void Move()
	{
		if (Vector2.Distance(Vector2.zero, tr.position) > 1.25f)
		{
			PlayLaunchSound();
			if (chase)
			{
				speedModifier += Time.deltaTime * (moveSpeed * 0.8f);
				Rotate();
			}
			else
			{
				speedModifier += Time.deltaTime * moveSpeed;
			}
		}
		else if (Vector2.Distance(Vector2.zero, tr.position) > 0.7f)
		{
			PlayAppearSound();
		}
		tr.position += tr.up * Time.deltaTime * speed * speedModifier;
	}

	private void Rotate()
	{
		target = playerTR.position - tr.position;
		float num = Vector2.SignedAngle(target, tr.up);
		if (num < 0f)
		{
			inertia += Time.deltaTime * acceleration;
			if (inertia > maxInertia)
			{
				inertia = maxInertia;
			}
		}
		else if (num > 0f)
		{
			inertia -= Time.deltaTime * acceleration;
			if (inertia < 0f - maxInertia)
			{
				inertia = 0f - maxInertia;
			}
		}
	}

	private void ManageOutOfBounds()
	{
		if (Vector2.Distance(Vector2.zero, tr.position) > 6.5f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void PlayLaunchSound()
	{
		if (!soundPlayed && !SceneChangeController.sceneChangeController.transitioning)
		{
			soundPlayed = true;
			aS = GlobalAudio.globalAudio.Play(launchSound, 0.7f, Random.Range(0.7f, 1.3f), tr.position.x / 4f);
		}
	}

	private void PlayAppearSound()
	{
		if (!soundPlayed2 && !SceneChangeController.sceneChangeController.transitioning)
		{
			soundPlayed2 = true;
			GlobalAudio.globalAudio.Play(appearSound, 0.5f, Random.Range(0.8f, 1.2f), 0f);
		}
	}
}
