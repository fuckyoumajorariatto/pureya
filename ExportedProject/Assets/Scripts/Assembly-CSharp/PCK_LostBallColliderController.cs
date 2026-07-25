using UnityEngine;

public class PCK_LostBallColliderController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<PCK_MarbleController>().golden)
		{
			GlobalControl.globalControl.goldenMarbles++;
		}
		PCK_InputController.PCK_inputController.marblesRemaining++;
		PCK_InputController.PCK_inputController.visualMarblesRemaining++;
		PCK_InputController.PCK_inputController.UpdateMarblesCounter();
	}
}
