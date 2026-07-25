using UnityEngine;

public class DifficultyLightsController : MonoBehaviour
{
	private int level;

	private GameLevel currentGameLevel;

	private Animator animator;

	public AudioClip difficultyUp;

	public AudioClip difficultyDown;

	private int previous;

	private int next;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (currentGameLevel == null)
		{
			currentGameLevel = GlobalControl.globalControl.gameLevel;
		}
		ManageLevel();
	}

	private void ManageLevel()
	{
		if (level != GlobalControl.globalControl.gameLevel.GetLevel() && currentGameLevel == GlobalControl.globalControl.gameLevel)
		{
			previous = level;
			next = GlobalControl.globalControl.gameLevel.GetLevel();
			ShowLights();
			currentGameLevel = GlobalControl.globalControl.gameLevel;
			level = GlobalControl.globalControl.gameLevel.GetLevel();
		}
		else if (currentGameLevel != GlobalControl.globalControl.gameLevel)
		{
			currentGameLevel = GlobalControl.globalControl.gameLevel;
		}
	}

	private void ShowLights()
	{
		animator.SetInteger("previous", previous);
		animator.SetInteger("next", next);
	}

	public void PlaySound()
	{
		if (next > previous)
		{
			float pitch = 1.5f;
			if (next == 2)
			{
				pitch = 1.25f;
			}
			else if (next == 3)
			{
				pitch = 1f;
			}
			GlobalAudio.globalAudio.Play(difficultyUp, 1f, pitch, 0f);
		}
		else
		{
			float pitch = 1.5f;
			if (next == 1)
			{
				pitch = 1.25f;
			}
			else if (next == 2)
			{
				pitch = 1f;
			}
			GlobalAudio.globalAudio.Play(difficultyDown, 1f, pitch, 0f);
		}
	}

	public void ResetAnimatorParas()
	{
		animator.SetInteger("previous", 0);
		animator.SetInteger("next", 0);
	}
}
