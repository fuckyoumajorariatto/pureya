using UnityEngine;

public class SPR_NailController : MonoBehaviour
{
	public AudioClip clip;

	public Transform tr;

	private void Update()
	{
		tr.Translate(0f, SPR_Difficulty.SPR_difficulty.scrollSpeed * Time.deltaTime, 0f);
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (tr.position.y > 6f)
		{
			Disable();
		}
	}

	private void Disable()
	{
		base.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		PlayClip();
	}

	private void PlayClip()
	{
		GlobalAudio.globalAudio.Play(clip, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
