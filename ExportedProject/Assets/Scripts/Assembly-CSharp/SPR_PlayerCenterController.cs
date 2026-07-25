using UnityEngine;

public class SPR_PlayerCenterController : MonoBehaviour
{
	public Transform playerTR;

	public Transform tr;

	private void Update()
	{
		if (!SPR_Reset.SPR_reset.reset)
		{
			tr.position = new Vector3(playerTR.position.x, playerTR.position.y - 0.6f, playerTR.position.z);
		}
	}
}
