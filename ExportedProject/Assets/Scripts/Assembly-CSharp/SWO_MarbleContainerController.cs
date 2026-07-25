using UnityEngine;

public class SWO_MarbleContainerController : MonoBehaviour
{
	public Transform target;

	public GameObject marble;

	public AudioClip hitSound;

	private GameObject fakeMarble;

	private Animator animator;

	public float moveSpeed = 10f;

	private bool hit;

	private Transform tr;

	private void Start()
	{
		fakeMarble = base.gameObject.transform.GetChild(0).gameObject;
		animator = fakeMarble.GetComponent<Animator>();
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
		tr.position = new Vector3(tr.position.x, tr.position.y, 0f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("bullet") && !hit)
		{
			PlayHitSound();
			hit = true;
			animator.SetTrigger("break");
			Invoke("Disable", 0.5f);
			marble.SetActive(false);
		}
		else if (collision.CompareTag("player") && !hit)
		{
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
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), 0f);
	}
}
