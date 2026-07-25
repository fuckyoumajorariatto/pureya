using UnityEngine;

public class NOT_Controller : MonoBehaviour
{
	public Camera RenderCamera;

	private void Start()
	{
		RenderCamera.backgroundColor = new Color32(69, 69, 69, byte.MaxValue);
	}
}
