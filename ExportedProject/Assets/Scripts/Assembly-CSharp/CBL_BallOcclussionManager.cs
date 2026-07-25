using UnityEngine;

public class CBL_BallOcclussionManager : MonoBehaviour
{
	public Transform tr;

	private RaycastHit2D[] hits;

	private Transform[] hitTR;

	private bool odd;

	private void FixedUpdate()
	{
		if (!odd)
		{
			odd = true;
			ManagePennatOcclusion();
		}
		else
		{
			odd = false;
		}
	}

	private void ManagePennatOcclusion()
	{
		hits = Physics2D.RaycastAll(new Vector2(tr.position.x - 0.25f, tr.position.y - 0.3f), new Vector2(0.5f, 0f));
		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].transform.CompareTag("enemy"))
			{
				tr.Translate(0f, Time.deltaTime * 2f, 0f);
				break;
			}
		}
	}
}
