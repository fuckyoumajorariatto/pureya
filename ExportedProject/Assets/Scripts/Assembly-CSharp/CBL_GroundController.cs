using UnityEngine;

public class CBL_GroundController : MonoBehaviour
{
	private Transform tr;

	private SpriteRenderer sr;

	private void Start()
	{
		tr = base.transform;
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		tr.Translate(0f, (0f - Time.deltaTime) * CBL_Difficulty.CBL_difficulty.scrollSpeed, 0f);
		if (tr.position.y < -7f)
		{
			tr.position = new Vector3(0f, tr.position.y + 15f, tr.position.z);
			sr.color = new Color32((byte)Random.Range(107, 146), (byte)Random.Range(165, 190), (byte)Random.Range(12, 40), byte.MaxValue);
		}
	}
}
