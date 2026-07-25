using UnityEngine;

public class PCK_FadeController : MonoBehaviour
{
	public AudioClip transition;

	public void PlayTransition()
	{
		GlobalAudio.globalAudio.Play(transition, 1f, 1f, 0f);
	}
}
