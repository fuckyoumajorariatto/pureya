using UnityEngine;

public class NumberWheelController : MonoBehaviour
{
	public Animator wheelAnimator;

	public SpriteRenderer[] n1;

	public SpriteRenderer[] n2;

	public SpriteRenderer[] n3;

	public Transform[] numbersTR;

	public GameObject plus;

	public GameObject minus;

	public SpriteRenderer plusSR;

	public SpriteRenderer plusSR_s;

	public SpriteRenderer minusSR;

	public SpriteRenderer minusSR_s;

	public Sprite[] minusPlus;

	public Color[] minusPlusC;

	public Sprite[] leftRight;

	public Color[] leftRightC;

	public Sprite[] numberSprites;

	private string s;

	public void ChangeToMinusPlus()
	{
		minusSR.sprite = minusPlus[0];
		minusSR_s.sprite = minusPlus[0];
		minusSR.color = minusPlusC[0];
		plusSR.sprite = minusPlus[1];
		plusSR_s.sprite = minusPlus[1];
		plusSR.color = minusPlusC[1];
	}

	public void ChangeToLeftRight()
	{
		minusSR.sprite = leftRight[0];
		minusSR_s.sprite = leftRight[0];
		minusSR.color = leftRightC[0];
		plusSR.sprite = leftRight[1];
		plusSR_s.sprite = leftRight[1];
		plusSR.color = leftRightC[1];
	}

	public void ChangeNumberTo(int n)
	{
		if (n < 10)
		{
			n2[0].sprite = numberSprites[n];
			n2[1].sprite = numberSprites[n];
			n1[0].sprite = null;
			n1[1].sprite = null;
			n3[0].sprite = null;
			n3[1].sprite = null;
			numbersTR[1].localPosition = new Vector3(0f, 0f, 0f);
		}
		else if (n < 100)
		{
			s = n + string.Empty;
			n1[0].sprite = null;
			n1[1].sprite = null;
			n2[0].sprite = numberSprites[int.Parse(s[0].ToString())];
			n2[1].sprite = numberSprites[int.Parse(s[0].ToString())];
			n3[0].sprite = numberSprites[int.Parse(s[1].ToString())];
			n3[1].sprite = numberSprites[int.Parse(s[1].ToString())];
			numbersTR[1].localPosition = new Vector3(-0.4375f, 0f, 0f);
			numbersTR[2].localPosition = new Vector3(0.4375f, 0f, 0f);
		}
		else
		{
			s = n + string.Empty;
			n1[0].sprite = numberSprites[int.Parse(s[0].ToString())];
			n1[1].sprite = numberSprites[int.Parse(s[0].ToString())];
			n2[0].sprite = numberSprites[int.Parse(s[1].ToString())];
			n2[1].sprite = numberSprites[int.Parse(s[1].ToString())];
			n3[0].sprite = numberSprites[int.Parse(s[2].ToString())];
			n3[1].sprite = numberSprites[int.Parse(s[2].ToString())];
			numbersTR[0].localPosition = new Vector3(-0.875f, 0f, 0f);
			numbersTR[1].localPosition = new Vector3(0f, 0f, 0f);
			numbersTR[2].localPosition = new Vector3(0.875f, 0f, 0f);
		}
	}

	public void HideNumber()
	{
		n1[0].sprite = null;
		n1[1].sprite = null;
		n2[0].sprite = null;
		n2[1].sprite = null;
		n3[0].sprite = null;
		n3[1].sprite = null;
	}

	public void TogglePlus(bool on)
	{
		plus.SetActive(on);
	}

	public void ToggleMinus(bool on)
	{
		minus.SetActive(on);
	}

	public void Plus()
	{
		wheelAnimator.SetTrigger("plus");
		PlayMoveSelection();
	}

	public void Minus()
	{
		wheelAnimator.SetTrigger("minus");
		PlayMoveSelection();
	}

	private void PlayMoveSelection()
	{
		GlobalAudio.globalAudio.PlayMoveSelection();
	}
}
