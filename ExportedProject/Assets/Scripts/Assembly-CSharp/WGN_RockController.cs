using UnityEngine;

public class WGN_RockController : MonoBehaviour
{
	public Transform tr;

	private bool toDisable;

	private bool soundPlayed;

	private float timer;

	private void Update()
	{
		tr.Translate(0f, (0f - WGN_Difficulty.WGN_difficulty.rockSpeed) * Time.deltaTime, 0f);
		if (tr.position.y < 4f && !soundPlayed)
		{
			WGN_RockSpawner.WGN_rockSpawner.PlayRockSound(tr);
			soundPlayed = true;
		}
		if (tr.position.y < -5f && !toDisable)
		{
			toDisable = true;
		}
		if (toDisable)
		{
			timer += Time.deltaTime;
			if (timer > 1f)
			{
				timer = 0f;
				toDisable = false;
				soundPlayed = false;
				base.gameObject.SetActive(false);
			}
		}
	}

	public void DoReset()
	{
		timer = 0f;
		toDisable = false;
		soundPlayed = false;
		base.gameObject.SetActive(false);
	}
}
