using UnityEngine;

public class FLA_Reset : MonoBehaviour
{
	public static FLA_Reset FLA_reset;

	public GameObject player;

	public bool reset;

	private float timer;

	private void Awake()
	{
		FLA_reset = this;
	}

	private void Update()
	{
		if (reset)
		{
			timer += Time.deltaTime;
			if (timer >= GlobalControl.globalControl.minigamesResetTime && !SceneChangeController.sceneChangeController.transitioning)
			{
				DoReset();
			}
		}
	}

	private void DoReset()
	{
		reset = false;
		timer = 0f;
		ResetPlayer();
		DisableByTag("enemy");
		DisableByTag("marble");
		MarbleController.ResetMarblePitch();
	}

	private void ResetPlayer()
	{
		player.transform.position = new Vector3(-2.5f, player.transform.position.y, player.transform.position.z);
		FLA_PlayerController component = player.GetComponent<FLA_PlayerController>();
		component.ResetPose();
		component.foot_leftTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		component.foot_leftRB.velocity = new Vector2(0f, 0f);
		component.foot_leftRB.angularVelocity = 0f;
		component.foot_rightTR.rotation = new Quaternion(0f, 0f, 0f, 0f);
		component.foot_rightRB.velocity = new Vector2(0f, 0f);
		component.foot_rightRB.angularVelocity = 0f;
		FLA_ShellSpawner.FLA_shellSpawner.noMarbleCombo = 0;
		FLA_ShellSpawner.FLA_shellSpawner.marbleCombo = 0;
		component.hitParticles.SetActive(false);
		component.hit = false;
	}

	private void DisableByTag(string tag)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			gameObject.SetActive(false);
		}
	}
}
