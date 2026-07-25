using UnityEngine;

public class BEE_PlantController : MonoBehaviour
{
	public Transform tr;

	public SpriteRenderer sr;

	public SpriteRenderer sr_s;

	public SpriteRenderer teethSR;

	public Sprite[] sprites;

	public Sprite[] sprites_s;

	private void Update()
	{
		tr.Translate(0f, (0f - BEE_Difficulty.BEE_difficulty.speed) * Time.deltaTime, 0f);
		ManageBounds();
	}

	private void ManageBounds()
	{
		if (tr.position.y < -5f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		sr.sprite = sprites[1];
		teethSR.sprite = sprites[2];
		sr_s.sprite = sprites_s[1];
	}

	public void DoReset()
	{
		sr.sprite = sprites[0];
		teethSR.sprite = null;
		sr_s.sprite = sprites_s[0];
	}
}
