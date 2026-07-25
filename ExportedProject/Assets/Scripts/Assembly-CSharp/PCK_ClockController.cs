using System;
using UnityEngine;

public class PCK_ClockController : MonoBehaviour
{
	public Sprite[] spriteNumbers;

	public SpriteRenderer[] numbersSR;

	private int n;

	private string s;

	private char[] charArray;

	private void Start()
	{
	}

	private void Update()
	{
		n = DateTime.Now.Hour;
		if (n > 9)
		{
			s = n + string.Empty;
			charArray = s.ToCharArray();
			numbersSR[0].sprite = spriteNumbers[(int)char.GetNumericValue(charArray[0])];
			numbersSR[1].sprite = spriteNumbers[(int)char.GetNumericValue(charArray[1])];
		}
		else
		{
			numbersSR[0].sprite = spriteNumbers[0];
			numbersSR[1].sprite = spriteNumbers[n];
		}
		n = DateTime.Now.Minute;
		if (n > 9)
		{
			s = n + string.Empty;
			charArray = s.ToCharArray();
			numbersSR[2].sprite = spriteNumbers[(int)char.GetNumericValue(charArray[0])];
			numbersSR[3].sprite = spriteNumbers[(int)char.GetNumericValue(charArray[1])];
		}
		else
		{
			numbersSR[2].sprite = spriteNumbers[0];
			numbersSR[3].sprite = spriteNumbers[n];
		}
	}
}
