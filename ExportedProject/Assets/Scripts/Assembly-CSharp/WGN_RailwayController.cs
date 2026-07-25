using UnityEngine;

public class WGN_RailwayController : MonoBehaviour
{
	public Transform[] railwaysTR;

	private void Update()
	{
		if (WGN_Difficulty.WGN_difficulty == null)
		{
			return;
		}
		for (int i = 0; i < railwaysTR.Length; i++)
		{
			railwaysTR[i].Translate(WGN_Difficulty.WGN_difficulty.speed * 1.25f * Time.deltaTime, 0f, 0f);
			if (railwaysTR[i].position.x > 6f)
			{
				railwaysTR[i].Translate(-12f, 0f, 0f);
			}
		}
	}
}
