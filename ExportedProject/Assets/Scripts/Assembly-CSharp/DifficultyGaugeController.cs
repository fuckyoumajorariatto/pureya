using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyGaugeController : MonoBehaviour
{
	public static DifficultyGaugeController difficultyGaugeController;

	private int level;

	private Animator animator;

	public AudioClip difficultyUp;

	public AudioClip difficultyDown;

	private int previous;

	private int next;

	private float pitch;

	private void Start()
	{
		difficultyGaugeController = this;
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (GlobalControl.globalControl.mode == "normal")
		{
			ManageLevel();
		}
		else
		{
			ManageTemporalLevel();
		}
	}

	public void ResetLevel()
	{
		level = GlobalControl.globalControl.GetLevel();
		previous = level;
	}

	private void ManageLevel()
	{
		if (level != GlobalControl.globalControl.GetLevel() && !GlobalControl.globalControl.gameLevel.ignoreXp)
		{
			previous = level;
			next = GlobalControl.globalControl.GetLevel();
			if (SceneManager.GetActiveScene().name != "MMU")
			{
				ShowGauge();
			}
			level = GlobalControl.globalControl.GetLevel();
		}
	}

	private void ManageTemporalLevel()
	{
		if (level != GlobalControl.globalControl.GetLevel())
		{
			previous = level;
			next = GlobalControl.globalControl.GetLevel();
			if (SceneManager.GetActiveScene().name != "MMU")
			{
				ShowGauge();
			}
			level = GlobalControl.globalControl.GetLevel();
		}
	}

	private void ShowGauge()
	{
		if (!(SceneChangeController.sceneChangeController.currentScene == "SET") && !PauseController.pauseController.exiting)
		{
			if (!HeartController.heartController.animationRunning)
			{
				base.gameObject.transform.position = new Vector3(0f, 0f, base.gameObject.transform.position.z);
			}
			else
			{
				base.gameObject.transform.position = new Vector3(0f, -2.75f, base.gameObject.transform.position.z);
			}
			animator.SetInteger("previous", previous);
			animator.SetInteger("next", next);
		}
	}

	public void ForceHideGuage()
	{
		base.gameObject.SetActive(false);
		Invoke("Reactivate", 1f);
	}

	private void Reactivate()
	{
		base.gameObject.SetActive(true);
	}

	public void PlaySound()
	{
		if (next > previous)
		{
			pitch = 1f;
			if (next == 2)
			{
				pitch = 1.25f;
			}
			else if (next == 3)
			{
				pitch = 1.5f;
			}
			GlobalAudio.globalAudio.Play(difficultyUp, 1f, pitch, 0f);
		}
		else
		{
			pitch = 1.5f;
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
		animator.SetInteger("previous", -1);
		animator.SetInteger("next", -1);
	}
}
