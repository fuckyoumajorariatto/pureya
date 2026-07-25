using UnityEngine;

public class Ending1CreditsController : MonoBehaviour
{
	public SET_CreditsController creditsController;

	public GameObject creditsElements;

	public GameObject title;

	private float timer;

	private float originalTime = 2f;

	private int index;

	private bool creditsShowing;

	private void Update()
	{
		if (!creditsShowing)
		{
			return;
		}
		timer -= Time.deltaTime;
		if (timer < 0f && creditsController.currentPage < creditsController.credits.Count - 1)
		{
			creditsController.NextPage();
			index++;
			if (originalTime - (float)index * 0.125f > 0.2f)
			{
				timer = originalTime - (float)index * 0.125f;
			}
			else
			{
				timer = 0.2f;
			}
		}
		else if (timer < 0f)
		{
			LastCredit();
		}
	}

	public void ShowTitle()
	{
		title.SetActive(true);
	}

	public void HideTitle()
	{
		title.SetActive(false);
	}

	public void StartCredits()
	{
		creditsElements.SetActive(true);
		timer = 2.5f;
		creditsShowing = true;
		creditsController.InitializeCredits();
		creditsController.InitializeIcons();
		creditsController.InitializePageNumbers();
		creditsController.NextPage();
	}

	private void LastCredit()
	{
		creditsShowing = false;
		creditsController.NextPage();
	}

	public void EndCredits()
	{
		creditsElements.SetActive(false);
	}
}
