using UnityEngine;

public class MarbleNumbersController : MonoBehaviour
{
	public Animator anim;

	public SpriteRenderer[] numbers;

	private string s;

	private char[] charArray;

	private int trigger1;

	private int trigger2;

	private int trigger3;

	private void Start()
	{
		trigger1 = Animator.StringToHash("show1");
		trigger2 = Animator.StringToHash("show2");
		trigger3 = Animator.StringToHash("show3");
	}

	public void UpdateNumbers()
	{
		if (GlobalControl.globalControl.mode == "normal" || GlobalControl.globalControl.mode == "endless")
		{
			s = GlobalControl.globalControl.marbles + string.Empty;
		}
		else
		{
			s = GlobalControl.globalControl.alternateMarbles + string.Empty;
		}
		charArray = s.ToCharArray();
		if (charArray.Length == 1)
		{
			numbers[0].sprite = MarbleCreator.marbleCreator.numberSprites[(int)char.GetNumericValue(charArray[0])];
			anim.SetTrigger(trigger1);
		}
		else if (charArray.Length == 2)
		{
			numbers[0].sprite = MarbleCreator.marbleCreator.numberSprites[(int)char.GetNumericValue(charArray[0])];
			numbers[1].sprite = MarbleCreator.marbleCreator.numberSprites[(int)char.GetNumericValue(charArray[1])];
			anim.SetTrigger(trigger2);
		}
		else if (charArray.Length == 3)
		{
			numbers[0].sprite = MarbleCreator.marbleCreator.numberSprites[(int)char.GetNumericValue(charArray[0])];
			numbers[1].sprite = MarbleCreator.marbleCreator.numberSprites[(int)char.GetNumericValue(charArray[1])];
			numbers[2].sprite = MarbleCreator.marbleCreator.numberSprites[(int)char.GetNumericValue(charArray[2])];
			anim.SetTrigger(trigger3);
		}
	}
}
