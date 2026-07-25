using UnityEngine;

public class PCK_CinematicNumbersController : MonoBehaviour
{
	public static PCK_CinematicNumbersController PCK_cinematicNumbersController;

	public GameObject numberLeft;

	public GameObject numberLeft_s;

	public GameObject numberRight;

	public GameObject numberRight_s;

	public GameObject numberMiddle;

	public GameObject numberMiddle_s;

	private Sprite[] spriteNumbers;

	public Sprite spr1;

	public Sprite spr2;

	public Sprite spr3;

	public Sprite spr4;

	public Sprite spr5;

	public Sprite spr6;

	public Sprite spr7;

	public Sprite spr8;

	public Sprite spr9;

	public AudioClip resultSound;

	private void Start()
	{
		PCK_cinematicNumbersController = this;
	}

	public void ChangeNumbers()
	{
		numberLeft.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[PCK_SlotsController.PCK_slotsController.resultLeft - 1];
		numberLeft_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[PCK_SlotsController.PCK_slotsController.resultLeft - 1];
		numberRight.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[PCK_SlotsController.PCK_slotsController.resultRight - 1];
		numberRight_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[PCK_SlotsController.PCK_slotsController.resultRight - 1];
		numberMiddle.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[PCK_SlotsController.PCK_slotsController.resultMiddle - 1];
		numberMiddle_s.GetComponent<SpriteRenderer>().sprite = PCK_SlotsController.PCK_slotsController.numberSprites[PCK_SlotsController.PCK_slotsController.resultMiddle - 1];
	}

	private void PlayResultSound()
	{
		GlobalAudio.globalAudio.Play(resultSound, 1f, 1f, 0f);
	}
}
