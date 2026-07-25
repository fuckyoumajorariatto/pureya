using System.Collections.Generic;
using UnityEngine;

public class TOP_MarblesController : MonoBehaviour
{
	private List<GameObject> marblesGO = new List<GameObject>();

	private List<Transform> marblesTR = new List<Transform>();

	private void Update()
	{
		for (int num = marblesGO.Count - 1; num >= 0; num--)
		{
			if (marblesGO[num] == null)
			{
				marblesGO.RemoveAt(num);
				marblesTR.RemoveAt(num);
			}
			else if (!marblesGO[num].activeSelf)
			{
				marblesGO.RemoveAt(num);
				marblesTR.RemoveAt(num);
			}
			else
			{
				marblesTR[num].Translate(Time.deltaTime * TOP_Difficulty.TOP_difficulty.scroll, 0f, 0f);
			}
		}
	}

	public void AddMarble(GameObject go, Transform tr)
	{
		marblesGO.Add(go);
		marblesTR.Add(tr);
	}
}
