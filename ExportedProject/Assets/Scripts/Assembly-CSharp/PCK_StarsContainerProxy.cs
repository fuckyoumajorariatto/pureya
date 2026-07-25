using UnityEngine;

public class PCK_StarsContainerProxy : MonoBehaviour
{
	private void PlayStar()
	{
		PCK_RankController.PCK_rankController.PlayStar();
	}

	private void PlayTransition()
	{
		PCK_RankController.PCK_rankController.PlayTransition();
	}

	private void RaiseStars()
	{
		PCK_RankController.PCK_rankController.RaiseRankCorner();
	}
}
