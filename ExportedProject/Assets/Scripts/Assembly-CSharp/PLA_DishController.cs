using UnityEngine;

public class PLA_DishController : MonoBehaviour
{
	public PLA_PlayerController playerController;

	public Animator anim;

	public Transform playerTR;

	private Transform tr;

	private Vector2 target;

	private float acceleration = 1f;

	private float maxSpeed = 1f;

	private float inertia;

	public AudioClip chargeSound;

	public AudioClip shootSound;

	private AudioSource aS;

	private bool cancelShootSound;

	private float timer;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		target = playerTR.position - tr.position;
		float num = Vector2.SignedAngle(target, tr.up);
		if (num < 0f)
		{
			inertia += Time.deltaTime * acceleration;
			if (inertia > maxSpeed)
			{
				inertia = maxSpeed;
			}
		}
		else if (num > 0f)
		{
			inertia -= Time.deltaTime * acceleration;
			if (inertia < 0f - maxSpeed)
			{
				inertia = 0f - maxSpeed;
			}
		}
		timer += Time.deltaTime;
		if (timer > PLA_Difficulty.PLA_difficulty.laserFrequency)
		{
			timer = Random.Range(-3f, -2.5f);
			Shoot();
		}
	}

	private void FixedUpdate()
	{
		tr.eulerAngles += Vector3.forward * inertia;
		if (aS != null && !SceneChangeController.sceneChangeController.transitioning)
		{
			aS.panStereo = tr.position.x / 4f;
		}
	}

	private void Shoot()
	{
		if (!playerController.hit)
		{
			PlayChargeSound();
			Invoke("PlayShootSound", 1.5f);
			anim.ResetTrigger("cancel");
			anim.SetTrigger("shoot");
		}
	}

	public void DoReset()
	{
		timer = 0f;
		anim.SetTrigger("cancel");
		anim.ResetTrigger("shoot");
		tr.eulerAngles = new Vector3(0f, 0f, -90f);
		inertia = 0f;
		cancelShootSound = true;
		CancelSound();
	}

	private void PlayChargeSound()
	{
		cancelShootSound = false;
		aS = GlobalAudio.globalAudio.Play(chargeSound, 0.8f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}

	private void PlayShootSound()
	{
		if (cancelShootSound || SceneChangeController.sceneChangeController.transitioning)
		{
			cancelShootSound = false;
		}
		else
		{
			aS = GlobalAudio.globalAudio.Play(shootSound, 0.6f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	public void CancelSound()
	{
		if (aS != null && aS.clip == shootSound)
		{
			aS.Stop();
		}
	}
}
