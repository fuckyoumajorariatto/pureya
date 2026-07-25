using UnityEngine;

public class RelocateShadow : MonoBehaviour
{
	public enum Direction
	{
		up_left = 0,
		up_right = 1,
		down_left = 2,
		down_right = 3,
		down = 4
	}

	public Direction direction;

	private Vector3 pos;

	private Transform tr;

	private void Start()
	{
		tr = base.transform;
	}

	private void LateUpdate()
	{
		float num = 0.0625f;
		pos = new Vector3(tr.parent.position.x, tr.parent.position.y, tr.position.z);
		if (direction == Direction.up_left)
		{
			pos.x -= num;
			pos.y += num;
		}
		else if (direction == Direction.up_right)
		{
			pos.x += num;
			pos.y += num;
		}
		else if (direction == Direction.down_left)
		{
			pos.x -= num;
			pos.y -= num;
		}
		else if (direction == Direction.down_right)
		{
			pos.x += num;
			pos.y -= num;
		}
		else if (direction == Direction.down)
		{
			pos.y -= num;
		}
		tr.position = pos;
	}
}
