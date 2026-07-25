using UnityEngine;

public class SavingIconsController : MonoBehaviour
{
	public static SavingIconsController savingIconsController;

	public Animator savingIconAnim;

	public Animator cloudIconAnim;

	public SpriteRenderer cloudIconSR;

	public Sprite[] cloudIcons;

	private float timer;

	private void Start()
	{
		savingIconsController = this;
	}

	private void Update()
	{
		if (timer > 0f)
		{
			timer -= Time.unscaledDeltaTime;
			if (timer <= 0f)
			{
				savingIconAnim.SetBool("in", false);
			}
		}
	}

	public void ShowAndHideSaving()
	{
		timer = 0.9f;
		savingIconAnim.SetBool("in", true);
	}

	public void ChangeCloudToUploading()
	{
		cloudIconSR.sprite = cloudIcons[0];
	}

	public void ChangeCloudToDownloading()
	{
		cloudIconSR.sprite = cloudIcons[1];
	}

	public void ShowCloud()
	{
		cloudIconAnim.SetBool("in", true);
	}

	public void HideCloud()
	{
		cloudIconAnim.SetBool("in", false);
	}
}
