using UnityEngine;

public class PCK_HoleController : MonoBehaviour
{
	public static PCK_HoleController PCK_holeController;

	public PCK_SlotsProgressController slotsProgress;

	public PCK_GoldenIndicatorController PCK_goldenIndicator;

	public ParticleSystem holeParticleStars;

	public AudioClip marbleIn;

	public AudioClip holeMovement;

	public Animator animator;

	private float previousCollisionY = -10f;

	public Transform tr;

	private Vector3 pos;

	private CircleCollider2D cc;

	private GameObject particles;

	private void Start()
	{
		PCK_holeController = this;
		tr = base.transform;
		cc = base.gameObject.GetComponent<CircleCollider2D>();
	}

	private void Update()
	{
		if (Input.GetKeyUp("z") && GlobalControl.globalControl.testmode)
		{
			slotsProgress.AddProgress(false);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (Vector2.Distance(tr.localPosition, collision.transform.localPosition) < cc.radius / 4f)
		{
			BallIn(collision);
			PlayMarbleIn();
		}
		else if (previousCollisionY > tr.localPosition.y && collision.transform.localPosition.y < tr.localPosition.y)
		{
			BallIn(collision);
			PlayMarbleIn();
		}
		else
		{
			previousCollisionY = collision.transform.localPosition.y;
		}
	}

	public void ChangePosition()
	{
		if (GlobalControl.globalControl.storyLevel > 0)
		{
			animator.SetBool("change_out", true);
			GlobalAudio.globalAudio.Play(holeMovement, 1f, 1f, 0f);
		}
	}

	private void CallPlaceHole()
	{
		pos = tr.position;
		if (GlobalControl.globalControl.storyLevel > 0)
		{
			while (tr.position == pos)
			{
				PCK_BoardCreator.PCK_boardCreator.PlaceHole(true);
			}
		}
	}

	private void BallIn(Collider2D collision)
	{
		bool golden = collision.gameObject.GetComponent<PCK_MarbleController>().golden;
		particles = PCK_MarbleCreator.PCK_marbleCreator.GetMarbleParticles();
		if (particles != null)
		{
			particles.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, collision.transform.position.z);
			particles.SetActive(true);
		}
		if (golden)
		{
			PCK_goldenIndicator.LightOn();
		}
		collision.gameObject.GetComponent<PCK_MarbleController>().Disable();
		slotsProgress.AddProgress(golden);
	}

	public void StartStarParticles()
	{
		holeParticleStars.Play();
	}

	public void StopStarParticles()
	{
		holeParticleStars.Stop();
	}

	private void PlayMarbleIn()
	{
		if (!PauseController.pauseController.inCinematic)
		{
			GlobalAudio.globalAudio.Play(marbleIn, 1f, Random.Range(0.85f, 1.15f), tr.position.x / 4f);
		}
	}
}
