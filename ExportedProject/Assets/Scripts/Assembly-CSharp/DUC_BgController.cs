using UnityEngine;

public class DUC_BgController : MonoBehaviour
{
	public Material mat;

	private float offset;

	private void Update()
	{
		offset += DUC_Difficulty.DUC_difficulty.speed / 5f * Time.deltaTime;
		mat.mainTextureOffset = new Vector2(offset, 0f);
	}
}
