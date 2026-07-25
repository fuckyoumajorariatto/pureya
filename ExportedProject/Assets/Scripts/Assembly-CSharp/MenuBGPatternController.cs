using UnityEngine;

public class MenuBGPatternController : MonoBehaviour
{
	public Renderer rend;

	private float offset;

	private void Update()
	{
		offset += 0.25f * Time.deltaTime;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
	}
}
