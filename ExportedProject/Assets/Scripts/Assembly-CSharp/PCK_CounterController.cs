using UnityEngine;

public class PCK_CounterController : MonoBehaviour
{
	public static PCK_CounterController PCK_counterController;

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

	public SpriteRenderer[] digits;

	public Animator counterSignaler;

	public AudioClip signalSound;

	private Color originalColor;

	private string numberString;

	private int[] d;

	private void Start()
	{
		PCK_counterController = this;
		numbers = new Sprite[10] { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };
		originalColor = digits[0].color;
		d = new int[3];
	}

	public void ShowNumber(int n)
	{
		digits[0].sprite = numbers[0];
		digits[1].sprite = numbers[0];
		digits[0].color = originalColor;
		digits[1].color = originalColor;
		digits[2].color = originalColor;
		if (n < 10)
		{
			if (n < 0)
			{
				n = 0;
			}
			digits[2].sprite = numbers[n];
			digits[0].color = Color.red;
			digits[1].color = Color.red;
			digits[2].color = Color.red;
		}
		else if (n < 100)
		{
			numberString = n + string.Empty;
			d[0] = int.Parse(numberString.ToCharArray()[0] + string.Empty);
			d[1] = int.Parse(numberString.ToCharArray()[1] + string.Empty);
			digits[1].sprite = numbers[d[0]];
			digits[2].sprite = numbers[d[1]];
		}
		else
		{
			numberString = n + string.Empty;
			d[0] = int.Parse(numberString.ToCharArray()[0] + string.Empty);
			d[1] = int.Parse(numberString.ToCharArray()[1] + string.Empty);
			d[2] = int.Parse(numberString.ToCharArray()[2] + string.Empty);
			digits[0].sprite = numbers[d[0]];
			digits[1].sprite = numbers[d[1]];
			digits[2].sprite = numbers[d[2]];
		}
	}

	public void SignalCounter()
	{
		counterSignaler.SetTrigger("in");
		PlaySignal();
	}

	private void PlaySignal()
	{
		GlobalAudio.globalAudio.Play(signalSound, 1f, 1f, 0f);
	}
}
