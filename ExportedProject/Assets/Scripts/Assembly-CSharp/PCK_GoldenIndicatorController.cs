using UnityEngine;

public class PCK_GoldenIndicatorController : MonoBehaviour
{
	public GameObject goldenBlocker;

	public Animator goldenLight;

	public Animator goldenLightOnEffect;

	public AudioClip lightOnSound;

	private bool lightIsOn;

	private void Start()
	{
		if (GlobalControl.globalControl.golden)
		{
			goldenBlocker.SetActive(false);
			if (GlobalControl.globalControl.goldenIndicator)
			{
				goldenLight.Play("lightOn");
				lightIsOn = true;
			}
		}
	}

	public void LightOn()
	{
		if (!lightIsOn)
		{
			lightIsOn = true;
			goldenLight.Play("lightOn");
			goldenLightOnEffect.Play("lightOnEffect");
			GlobalAudio.globalAudio.Play(lightOnSound, 0.3f, 1f, 0f);
			GlobalControl.globalControl.goldenIndicator = true;
		}
		else
		{
			GlobalAudio.globalAudio.Play(lightOnSound, 0.3f, 1f, 0f);
			goldenLightOnEffect.Play("lightOnEffect");
		}
	}

	public void LightOff()
	{
		goldenLight.Play("lightOff");
		lightIsOn = false;
		GlobalControl.globalControl.goldenIndicator = false;
	}
}
