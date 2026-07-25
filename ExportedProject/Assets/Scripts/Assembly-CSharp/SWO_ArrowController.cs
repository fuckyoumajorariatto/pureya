using UnityEngine;

public class SWO_ArrowController : MonoBehaviour
{
	public Transform target;

	public AudioClip hitSound;

	public Animator animator;

	public float moveSpeed;

	private float rotationSpeed = 10f;

	private bool hit;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void FixedUpdate()
	{
		if (!hit)
		{
			MoveAndRotate();
		}
	}

	private void MoveAndRotate()
	{
		tr.position = Vector2.MoveTowards(tr.position, target.position, moveSpeed * Time.deltaTime);
		tr.position = new Vector2(tr.position.x, base.transform.position.y);
		tr.up = Vector2.Lerp(tr.up, target.position - tr.position, rotationSpeed * Time.deltaTime);
		tr.rotation = Quaternion.Euler(0f, 0f, tr.rotation.eulerAngles.z);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("bullet") && !hit)
		{
			PlayHitSound();
			hit = true;
			animator.SetTrigger("break");
			Invoke("Disable", 0.5f);
		}
	}

	private void Disable()
	{
		hit = false;
		base.gameObject.SetActive(false);
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.75f, 1.25f), 0f);
	}
}
