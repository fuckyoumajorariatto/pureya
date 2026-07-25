using UnityEngine;

public class PCK_PieceContainerProxy : MonoBehaviour
{
	private void PlayUnlock()
	{
		PCK_UnlocksController.PCK_unlocksController.PlayPieceUnlock();
	}
}
