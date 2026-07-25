using UnityEngine;

public class PCK_PinController : MonoBehaviour
{
	private Animator animator;

	private int hitTrigger;

	private void Start()
	{
		animator = base.gameObject.GetComponentInParent<Animator>();
		hitTrigger = Animator.StringToHash("hit");
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		animator.SetTrigger(hitTrigger);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		animator.SetTrigger(hitTrigger);
	}
}
