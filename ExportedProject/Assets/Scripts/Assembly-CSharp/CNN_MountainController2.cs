using UnityEngine;

public class CNN_MountainController2 : MonoBehaviour
{
	private float[] ys = new float[2] { -0.25f, -0.31f };

	private float speed;

	public Transform tr;

	private Vector3 pos;

	private int ry;

	private void Update()
	{
		if (CNN_Difficulty.CNN_difficulty.loaded)
		{
			speed = CNN_Difficulty.CNN_difficulty.bg_elements_speed;
		}
		tr.Translate(speed * 0.4f * Time.deltaTime, 0f, 0f);
		RecycleIfOut();
	}

	private void RecycleIfOut()
	{
		if (tr.position.x > 6f)
		{
			pos = new Vector3(0f, 0f, tr.position.z);
			pos.x = Random.Range(-7f, -6f);
			ry = Random.Range(0, ys.Length);
			pos.y = ys[ry];
			tr.position = pos;
		}
	}
}
