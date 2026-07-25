using UnityEngine;

public class PCK_DispenserController : MonoBehaviour
{
	public void EndShowingDispenser()
	{
		PCK_UnlocksController.PCK_unlocksController.NextPrize();
	}

	public void EndHidingDispenser()
	{
		PCK_Controller.PCK_controller.EndJackpot();
	}
}
