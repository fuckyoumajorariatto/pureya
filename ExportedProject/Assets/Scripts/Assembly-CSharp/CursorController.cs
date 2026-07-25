using UnityEngine;

public class CursorController : MonoBehaviour
{
	private Vector3 cursorPosition;

	private float visibleTimer;

	private void Start()
	{
		cursorPosition = Input.mousePosition;
		Cursor.visible = false;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) || cursorPosition != Input.mousePosition)
		{
			MakeCursorVisible();
		}
		if (visibleTimer > 0f)
		{
			visibleTimer -= Time.deltaTime;
			if (visibleTimer <= 0f)
			{
				Cursor.visible = false;
			}
		}
	}

	private void MakeCursorVisible()
	{
		cursorPosition = Input.mousePosition;
		Cursor.visible = true;
		visibleTimer = 3f;
	}
}
