using UnityEngine;

public class BWL_PinController : MonoBehaviour
{
	private Transform tr;

	public Animator anim;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(0f, Time.deltaTime * BWL_Difficulty.BWL_difficulty.scroll, 0f);
		if (tr.position.y > 5f)
		{
			Disble();
		}
	}

	private void Disble()
	{
		anim.ResetTrigger("L");
		anim.ResetTrigger("R");
		base.gameObject.SetActive(false);
	}

	public void Animate(bool left)
	{
		if (left)
		{
			anim.SetTrigger("L");
		}
		else
		{
			anim.SetTrigger("R");
		}
	}
}
