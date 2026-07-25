using UnityEngine;

public class BAL_BushController : MonoBehaviour
{
	public float speedModifier;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		base.transform.Translate(BAL_Difficulty.BAL_difficulty.scrollSpeed / speedModifier * Time.deltaTime, 0f, 0f);
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (base.transform.position.x > 6f)
		{
			tr.position = new Vector3(Random.Range(-6f, -7f), base.transform.position.y, base.transform.position.z);
			tr.localScale = new Vector3(tr.localScale.x * -1f, tr.localScale.y, tr.localScale.z);
		}
	}
}
