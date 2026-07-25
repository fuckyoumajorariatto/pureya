using UnityEngine;

public class PCK_QueueLightsController : MonoBehaviour
{
	public static PCK_QueueLightsController PCK_queueLightsController;

	public GameObject queueLight1;

	public GameObject queueLight2;

	public GameObject queueLight3;

	private void Start()
	{
		PCK_queueLightsController = this;
	}

	public void AddQueueLight()
	{
		if (!queueLight1.activeSelf)
		{
			queueLight1.SetActive(true);
		}
		else if (!queueLight2.activeSelf)
		{
			queueLight2.SetActive(true);
		}
		else if (!queueLight3.activeSelf)
		{
			queueLight3.SetActive(true);
		}
	}

	public void RemoveQueueLight()
	{
		if (queueLight3.activeSelf)
		{
			queueLight3.SetActive(false);
		}
		else if (queueLight2.activeSelf)
		{
			queueLight2.SetActive(false);
		}
		else if (queueLight1.activeSelf)
		{
			queueLight1.SetActive(false);
		}
	}
}
