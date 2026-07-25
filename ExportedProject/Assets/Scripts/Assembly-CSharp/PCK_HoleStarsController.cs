using UnityEngine;

public class PCK_HoleStarsController : MonoBehaviour
{
	public static PCK_HoleStarsController PCK_holeStarsController;

	public Animator[] starsAnim;

	public Transform[] starsT;

	public Transform holeT;

	public Transform cornerStarT;

	private int showTrigger;

	private bool[] raised;

	private bool[] showing;

	private float[] timers;

	private Vector3[] origins;

	private Vector3 destination;

	private void Start()
	{
		PCK_holeStarsController = this;
		showTrigger = Animator.StringToHash("show");
		showing = new bool[3];
		raised = new bool[3];
		timers = new float[3];
		origins = new Vector3[3];
	}

	private void Update()
	{
		for (int i = 0; i < showing.Length; i++)
		{
			if (showing[i])
			{
				destination = cornerStarT.position;
				destination.z -= 0.01f;
				starsT[i].position = Vector3.Lerp(origins[i], destination, timers[i]);
				timers[i] += Time.deltaTime * 2f;
				if (timers[i] >= 1f && !raised[i])
				{
					raised[i] = true;
					PCK_RankController.PCK_rankController.RaiseRankCornerWithSound();
				}
				if (timers[i] > 1.34f)
				{
					showing[i] = false;
				}
			}
		}
	}

	public void ShowStar()
	{
		for (int i = 0; i < showing.Length; i++)
		{
			if (!showing[i])
			{
				showing[i] = true;
				raised[i] = false;
				origins[i] = new Vector3(holeT.position.x, holeT.position.y, starsT[i].position.z);
				starsT[i].position = origins[i];
				starsAnim[i].SetTrigger(showTrigger);
				timers[i] = 0f;
				break;
			}
		}
	}
}
