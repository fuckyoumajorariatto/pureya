using UnityEngine;

public class CRA_AlgaeController : MonoBehaviour
{
	public Animator anim;

	public Transform tr;

	public Collider2D coll;

	private bool hit;

	private GameObject obj;

	private MarbleController marbleController;

	public Color32 shadowColor;

	public AudioClip hitSound;

	private void Update()
	{
		tr.Translate(0f, CRA_Difficulty.CRA_difficulty.speed * Time.deltaTime, 0f);
		if (tr.position.y > 5f && !hit)
		{
			Deactivate();
		}
	}

	public void Initialize()
	{
		anim.ResetTrigger("hit");
		anim.SetBool("hit", false);
		coll.enabled = true;
		hit = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.CompareTag("bullet") || hit)
		{
			return;
		}
		PlayHitSound();
		anim.ResetTrigger("hit");
		collision.gameObject.GetComponent<CRA_BulletController>().Deactivate();
		anim.SetBool("hit", true);
		coll.enabled = false;
		hit = true;
		Invoke("Deactivate", 0.1f);
		if (SceneChangeController.sceneChangeController.transitioning)
		{
			return;
		}
		if (Random.Range(0f, 1f) < 0.65f)
		{
			SetMarbleConstants(MarbleCreator.marbleCreator.GetMarble());
			return;
		}
		obj = CRA_ObjectSpawner.CRA_objectSpawner.GetObj2();
		if (obj != null)
		{
			obj.transform.position = tr.position;
			obj.SetActive(true);
		}
	}

	private void Deactivate()
	{
		base.gameObject.SetActive(false);
	}

	private void SetMarbleConstants(GameObject marble)
	{
		marble.transform.localScale = new Vector3(1f, 1f, 1f);
		marble.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		marble.transform.position = new Vector3(tr.position.x, tr.position.y, 0f);
		marbleController = marble.GetComponent<MarbleController>();
		marbleController.shadow.GetComponent<SpriteRenderer>().color = shadowColor;
		marbleController.speed = CRA_Difficulty.CRA_difficulty.speed * 1.5f;
		marbleController.direction = MarbleController.Direction.up;
		marble.GetComponentInChildren<RelocateShadow>().direction = RelocateShadow.Direction.down_left;
		marbleController.maxOut = 5f;
		marble.SetActive(true);
	}

	private void PlayHitSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			GlobalAudio.globalAudio.Play(hitSound, Random.Range(0.5f, 0.6f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}
}
