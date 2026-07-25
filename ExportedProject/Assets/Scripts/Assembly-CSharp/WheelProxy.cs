using UnityEngine;

public class WheelProxy : MonoBehaviour
{
	public WheelController wheelController;

	private void UpdateIcons()
	{
		wheelController.UpdateIcons();
	}
}
