using UnityEngine;

public class PCK_GachaController : MonoBehaviour
{
	public static PCK_GachaController PCK_gachaController;

	public AudioClip marble_in;

	public AudioClip release;

	public AudioClip bounce;

	public AudioClip stretch;

	public AudioClip pop;

	public AudioClip confetti_sound;

	public AudioClip text_appears;

	public AudioClip text_change;

	public AudioClip movement;

	public GameObject confetti;

	private void Start()
	{
		PCK_gachaController = this;
	}

	public void RaiseCollectedNumber()
	{
		PCK_UnlocksController.PCK_unlocksController.RaiseCollectedNumber();
	}

	public void PrizeEnded()
	{
		PCK_UnlocksController.PCK_unlocksController.PrizeEnded();
	}

	public void PlayMachineSounds()
	{
		GlobalAudio.globalAudio.Play(marble_in, 1f, 1f, 0.5f);
		GlobalAudio.globalAudio.Play(release, 1f, 1f, -0.5f);
	}

	public void PlayBounce()
	{
		GlobalAudio.globalAudio.Play(bounce, 1f, 1f, 0f);
	}

	public void PlayBounce2()
	{
		GlobalAudio.globalAudio.Play(bounce, 0.5f, 1.15f, 0f);
	}

	public void PlayStretch()
	{
		GlobalAudio.globalAudio.Play(stretch, 1f, 1f, 0f);
	}

	public void PlayPop()
	{
		GlobalAudio.globalAudio.Play(pop, 1f, 1f, 0f);
		confetti.SetActive(true);
		Invoke("DisableConfetti", 3f);
		GlobalAudio.globalAudio.Play(confetti_sound, 1f, 1f, 0f);
	}

	private void DisableConfetti()
	{
		confetti.SetActive(false);
	}

	public void PlayTextAppears()
	{
		GlobalAudio.globalAudio.Play(text_appears, 1f, 1f, 0f);
	}

	public void PlayTextChange()
	{
		GlobalAudio.globalAudio.Play(text_change, 1f, 1f, 0f);
	}

	public void PlayMovement()
	{
		GlobalAudio.globalAudio.Play(movement, 0.5f, Random.Range(0.75f, 1.25f), 0f);
	}

	public void UpdateCollection()
	{
		PCK_UnlocksController.PCK_unlocksController.UpdateCollection();
	}

	public void UpdateCollectionPieces()
	{
		PCK_UnlocksController.PCK_unlocksController.UpdateCollectionPieces();
	}

	public void UpdateCollectionTracks()
	{
		PCK_UnlocksController.PCK_unlocksController.UpdateCollectionTracks();
	}
}
