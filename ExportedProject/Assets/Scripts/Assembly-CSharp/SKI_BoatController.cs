using UnityEngine;

public class SKI_BoatController : MonoBehaviour
{
	public float speed;

	private Transform tr;

	public AudioClip movementSound;

	public float movementSoundCooldown;

	public float vol;

	private float soundTimer;

	private bool canPlayMovement = true;

	private void Start()
	{
		tr = base.transform;
	}

	private void Update()
	{
		tr.Translate(new Vector2(0f, speed * Time.deltaTime * SKI_Difficulty.SKI_difficulty.boatSpeedModifier));
		soundTimer += Time.deltaTime;
		if (soundTimer > 0.05f)
		{
			canPlayMovement = true;
		}
		PlayMovementSound();
		if (tr.position.y > 6f || tr.position.y < -6f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void PlayMovementSound()
	{
		if (canPlayMovement)
		{
			soundTimer = 0f;
			canPlayMovement = false;
			GlobalAudio.globalAudio.PlayLowPriority(movementSound, vol * (1f - Mathf.Abs(tr.position.y) / 5f), Random.Range(0.95f, 1.05f), tr.position.x / 4f);
		}
	}
}
