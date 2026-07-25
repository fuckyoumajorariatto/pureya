using UnityEngine;

public class PLU_PlayerController : MonoBehaviour
{
	public Sprite[] sprs = new Sprite[9];

	public GameObject bgParticles1;

	public GameObject bgParticles2;

	public ParticleSystem hitParticles;

	private ParticleSystem.ColorOverLifetimeModule main;

	public SpriteRenderer sr;

	public Transform renderCameraTR;

	private AudioSource audioSource;

	public AudioClip movementSound;

	public AudioClip hitSound;

	private int maxSpeed = 12;

	private int acceleration = 70;

	private float currentSpeed;

	private float deceleration = 120f;

	private bool receivingInput;

	public bool hit;

	private float screenShakeRemainingTime;

	public int hits;

	public Transform tr;

	public Transform bgp1TR;

	public Transform bgp2TR;

	public Rigidbody2D rb;

	public Collider2D coll;

	private Vector3 pos;

	private bool bothButtons;

	private float doubleMoveSoundCooldown;

	private int skin;

	private void Start()
	{
		ChangeSkin();
	}

	private void Update()
	{
		if (screenShakeRemainingTime > 0f)
		{
			ShakeScreen();
		}
		if (!PauseController.pauseController.paused)
		{
			ManageInput();
			if (currentSpeed != 0f)
			{
				MovePlayer();
			}
			ReduceSpeed();
			PanAudio();
			if (doubleMoveSoundCooldown > 0f)
			{
				doubleMoveSoundCooldown -= Time.deltaTime;
			}
		}
	}

	private void ChangeSkin()
	{
		int[] unlockedAndEnabledSkins = GlobalControl.globalControl.GetUnlockedAndEnabledSkins("PLU");
		if (unlockedAndEnabledSkins.Length == 0)
		{
			skin = 9;
		}
		int num = GlobalControl.globalControl.lastUnlockedSkins["PLU"];
		if (num != -1 && !GlobalControl.globalControl.enabledSkins["PLU"][num])
		{
			num = -1;
		}
		if (num != -1)
		{
			skin = num;
			GlobalControl.globalControl.lastUnlockedSkins["PLU"] = -1;
		}
		else if (skin != 9)
		{
			int num2 = Random.Range(0, unlockedAndEnabledSkins.Length);
			skin = unlockedAndEnabledSkins[num2];
		}
		if (skin == 9)
		{
			sr.sprite = null;
		}
		else
		{
			sr.sprite = sprs[skin];
		}
		main = hitParticles.colorOverLifetime;
		Gradient gradient = new Gradient();
		switch (skin)
		{
		case 0:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("663715FF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("BF7E56FF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 1:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("56BC2CFF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("78DA50FF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 2:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("252525FF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("747474FF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 3:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("35322DFF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("675C4AFF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 4:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("7E7E7EFF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("B8B8B8FF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 5:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("FFFFFFFF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("FFFFFFFF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 6:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("DA78FBFF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("E59AFFFF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 7:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("252525FF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("686868FF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 8:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("663715FF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("93410FFF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		case 9:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("787878FF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("787878FF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		default:
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(GlobalControl.globalControl.HexToColor("FFCF2EFF"), 0f),
				new GradientColorKey(GlobalControl.globalControl.HexToColor("FFF3CCFF"), 1f)
			}, new GradientAlphaKey[3]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.65f),
				new GradientAlphaKey(0f, 1f)
			});
			break;
		}
		main.color = gradient;
	}

	private void PanAudio()
	{
		if (audioSource != null)
		{
			if (!audioSource.isPlaying)
			{
				audioSource = null;
			}
			else
			{
				GlobalAudio.globalAudio.UpdatePan(audioSource, tr.position.x / 4f);
			}
		}
	}

	private void ManageInput()
	{
		if (hit)
		{
			return;
		}
		receivingInput = false;
		if (!GlobalInput.globalInput.gameInputActive || (GlobalInput.globalInput.left && GlobalInput.globalInput.right))
		{
			bothButtons = true;
			if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
			{
				doubleMoveSoundCooldown = 0.25f;
				PlayMoveSound();
			}
			return;
		}
		if (GlobalInput.globalInput.left && tr.position.x > -4f)
		{
			if (currentSpeed > (float)(-maxSpeed))
			{
				currentSpeed -= (float)acceleration * Time.deltaTime;
			}
			SquishPlayer();
		}
		else if (GlobalInput.globalInput.right && tr.position.x < 4f)
		{
			if (currentSpeed < (float)maxSpeed)
			{
				currentSpeed += (float)acceleration * Time.deltaTime;
			}
			SquishPlayer();
		}
		else
		{
			tr.localScale = new Vector3(1f, 1f, tr.localScale.z);
		}
		if (GlobalInput.globalInput.left || GlobalInput.globalInput.right)
		{
			receivingInput = true;
		}
		if (GlobalInput.globalInput.leftDown || GlobalInput.globalInput.rightDown)
		{
			bothButtons = false;
			PlayMoveSound();
		}
		else if (bothButtons && ((GlobalInput.globalInput.left && !GlobalInput.globalInput.right) || (!GlobalInput.globalInput.left && GlobalInput.globalInput.right)))
		{
			bothButtons = false;
			if (!(doubleMoveSoundCooldown > 0f))
			{
				PlayMoveSound();
			}
		}
	}

	private void MovePlayer()
	{
		if (!hit)
		{
			tr.Translate(currentSpeed * Time.deltaTime, 0f, 0f);
			if (tr.position.x < -4f)
			{
				tr.position = new Vector2(-4f, tr.position.y);
			}
			else if (tr.position.x > 4f)
			{
				tr.position = new Vector2(4f, tr.position.y);
			}
		}
	}

	private void ReduceSpeed()
	{
		if (receivingInput || currentSpeed == 0f)
		{
			return;
		}
		if (currentSpeed > 0f)
		{
			currentSpeed -= Time.deltaTime * deceleration;
			if (currentSpeed < 0f)
			{
				currentSpeed = 0f;
			}
		}
		else if (currentSpeed < 0f)
		{
			currentSpeed += Time.deltaTime * deceleration;
			if (currentSpeed > 0f)
			{
				currentSpeed = 0f;
			}
		}
	}

	private void MoveBgParticles()
	{
		bgp1TR.Translate(currentSpeed * Time.deltaTime * 0.15f, 0f, 0f);
		bgp2TR.Translate(currentSpeed * Time.deltaTime * 0.1f, 0f, 0f);
	}

	private void SquishPlayer()
	{
		tr.localScale = new Vector3(0.8f, 1f, tr.localScale.z);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.transform.CompareTag("marble") && !hit)
		{
			ManageCollision(collision);
		}
	}

	private void ManageCollision(Collision2D collision)
	{
		if (!hit && !SceneChangeController.sceneChangeController.transitioning)
		{
			hit = true;
			bothButtons = true;
			if (MinigamesController.minigamesController.Hit())
			{
				PLU_Reset.PLU_reset.reset = true;
			}
			rb.velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
			hitParticles.transform.position = tr.position;
			hitParticles.Play();
			screenShakeRemainingTime = 0.1f;
			coll.enabled = false;
			PlayHitSound();
			if (hits == 0)
			{
				MinigamesController.minigamesController.survivedMinigames--;
				HeartController.heartController.ShowBreak();
			}
			hits++;
			GlobalControl.globalControl.ReduceXp();
		}
	}

	private void ShakeScreen()
	{
		if (PauseController.pauseController.paused)
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
			return;
		}
		screenShakeRemainingTime -= Time.deltaTime;
		if (screenShakeRemainingTime > 0f)
		{
			pos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
		else
		{
			pos = new Vector3(0f, 0f, renderCameraTR.position.z);
			renderCameraTR.position = pos;
		}
	}

	private void PlayMoveSound()
	{
		if (!SceneChangeController.sceneChangeController.transitioning)
		{
			audioSource = GlobalAudio.globalAudio.Play(movementSound, Random.Range(0.75f, 1f), Random.Range(0.9f, 1.1f), tr.position.x / 4f);
		}
	}

	private void PlayHitSound()
	{
		GlobalAudio.globalAudio.Play(hitSound, 1f, Random.Range(0.9f, 1.1f), tr.position.x / 4f);
	}
}
