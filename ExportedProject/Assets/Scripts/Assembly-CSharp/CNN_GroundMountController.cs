using UnityEngine;

public class CNN_GroundMountController : MonoBehaviour
{
	private float speed;

	public Transform tr;

	private void Update()
	{
		speed = CNN_Difficulty.CNN_difficulty.bg_elements_speed;
		tr.Translate(speed * 0.75f * Time.deltaTime, 0f, 0f);
		DisableIfOut();
	}

	private void DisableIfOut()
	{
		if (tr.position.x > 7f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
