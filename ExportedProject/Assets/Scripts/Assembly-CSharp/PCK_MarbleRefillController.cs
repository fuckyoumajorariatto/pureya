using UnityEngine;

public class PCK_MarbleRefillController : MonoBehaviour
{
	private float timer;

	private bool activated;

	public GameObject particles;

	public GameObject digit1;

	public GameObject digit2;

	public GameObject survival_digit;

	public GameObject survival_digit_s;

	private Sprite[] numbers;

	public Sprite n0;

	public Sprite n1;

	public Sprite n2;

	public Sprite n3;

	public Sprite n4;

	public Sprite n5;

	public Sprite n6;

	public Sprite n7;

	public Sprite n8;

	public Sprite n9;

	public AudioClip bounce;

	public AudioClip refill;

	public AudioClip fusion;

	private void Update()
	{
		if (GlobalControl.globalControl != null && GlobalControl.globalControl.storyLevel == 0)
		{
			PCK_Controller.PCK_controller.marbleRefilling = false;
			GlobalControl.globalControl.marbles = 10;
			UpdateCounter();
			End();
		}
		if (timer >= 0.5f && !activated)
		{
			numbers = new Sprite[10] { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };
			GetComponent<Animator>().SetTrigger("action");
			UpdateDigits();
			activated = true;
		}
		timer += Time.deltaTime;
	}

	private void UpdateDigits()
	{
		int num = GlobalControl.globalControl.marbles;
		if (num < 10)
		{
			if (num < 0)
			{
				num = 0;
			}
			digit1.GetComponent<SpriteRenderer>().sprite = numbers[0];
			digit1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numbers[0];
			digit2.GetComponent<SpriteRenderer>().sprite = numbers[num];
			digit2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numbers[num];
		}
		else
		{
			string text = num + string.Empty;
			int num2 = int.Parse(text.ToCharArray()[0] + string.Empty);
			int num3 = int.Parse(text.ToCharArray()[1] + string.Empty);
			digit1.GetComponent<SpriteRenderer>().sprite = numbers[num2];
			digit1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numbers[num2];
			digit2.GetComponent<SpriteRenderer>().sprite = numbers[num3];
			digit2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numbers[num3];
		}
		int num4 = MinigamesController.minigamesController.survivedMinigames;
		if (num4 < 0)
		{
			num4 = 0;
		}
		survival_digit.GetComponent<SpriteRenderer>().sprite = numbers[num4];
		survival_digit_s.GetComponent<SpriteRenderer>().sprite = numbers[num4];
	}

	private void UpdateCounter()
	{
		int num = MinigamesController.minigamesController.survivedMinigames;
		if (num < 0)
		{
			num = 0;
		}
		PCK_CounterController.PCK_counterController.ShowNumber(GlobalControl.globalControl.marbles + num);
	}

	private void ActivateParticles()
	{
		particles.SetActive(true);
	}

	private void End()
	{
		if (GlobalControl.globalControl.remainingPrizes > 0)
		{
			PCK_Controller.PCK_controller.ResumeUnlocks();
		}
		else
		{
			PCK_InputController.PCK_inputController.inputActive = true;
		}
		PCK_Controller.PCK_controller.marbleRefilling = false;
		particles.SetActive(false);
		base.gameObject.SetActive(false);
	}

	private void PlayBounce()
	{
		GlobalAudio.globalAudio.Play(bounce, 1f, 1f, 0f);
	}

	private void PlayRefill()
	{
		GlobalAudio.globalAudio.Play(refill, 1f, 1f, 0f);
	}

	private void PlayFusion()
	{
		GlobalAudio.globalAudio.Play(fusion, 1f, 1f, 0f);
	}
}
