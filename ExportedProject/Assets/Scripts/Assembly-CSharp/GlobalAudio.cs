using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
	public static GlobalAudio globalAudio;

	private AudioSource[] audioSources;

	private float soundsVolume;

	private float musicVolume;

	private float musicVolumeNormalizer = 0.8f;

	private int musicMute;

	private int soundsMute;

	private float[] originalVolumes;

	private string[] audioTypes;

	private float panMax = 0.9f;

	private int index;

	private float previousVol;

	private float volReduction;

	private bool[] pauseRecord;

	public AudioClip acceptSound;

	public AudioClip cancelSound;

	public AudioClip invalidSound;

	public AudioClip moveSelectionSound;

	public AudioClip mainMenuMusic1;

	public AudioClip mainMenuMusic2;

	public AudioClip mainMenuMusicSwing1;

	public AudioClip mainMenuMusicSwing2;

	private AudioSource fadeAS;

	private float fadeTime;

	private float fadeOriginalVol;

	private bool fadingMusic;

	private bool changeToMenuMusic2;

	private AudioSource menuMusicAS;

	private float exp = 2f;

	private void Start()
	{
		if (globalAudio == null)
		{
			globalAudio = this;
		}
		else
		{
			Object.Destroy(this);
		}
		Initialize();
	}

	private void Update()
	{
		CheckConditions();
		if (fadingMusic)
		{
			FadeMusicUpdate();
		}
		if (changeToMenuMusic2 && ((float)menuMusicAS.timeSamples / (float)menuMusicAS.clip.frequency >= 7.538f || !menuMusicAS.isPlaying))
		{
			changeToMenuMusic2 = false;
			if (GlobalControl.globalControl.storyLevel < 2)
			{
				menuMusicAS.clip = mainMenuMusic2;
			}
			else
			{
				menuMusicAS.clip = mainMenuMusicSwing2;
			}
			menuMusicAS.loop = true;
			menuMusicAS.Play();
		}
	}

	public void Initialize()
	{
		InitializeAudioSources();
		InitializeOriginalVolumes();
		InitializeAudioTypes();
		pauseRecord = new bool[audioSources.Length];
	}

	public void UpdateMusicVolumeTo(int n)
	{
		musicVolume = Mathf.Pow((float)n / 100f, exp);
		musicMute = 1;
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioTypes[i] == "music")
			{
				audioSources[i].volume = originalVolumes[i] * musicVolume * musicVolumeNormalizer;
			}
		}
	}

	public void UpdateSoundsVolumeTo(int n)
	{
		soundsVolume = Mathf.Pow((float)n / 100f, exp);
		soundsMute = 1;
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioTypes[i] == "sound")
			{
				audioSources[i].volume = originalVolumes[i] * soundsVolume;
			}
		}
	}

	public void MuteMusic()
	{
		musicMute = 0;
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioTypes[i] == "music")
			{
				audioSources[i].volume = 0f;
			}
		}
	}

	public void MuteSounds()
	{
		soundsMute = 0;
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioTypes[i] == "sound")
			{
				audioSources[i].volume = 0f;
			}
		}
	}

	public float GetSoundsVolume()
	{
		return soundsVolume * (float)soundsMute;
	}

	private void CheckConditions()
	{
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioSources[i].isPlaying && audioSources[i].clip != null && !audioSources[i].loop && (float)audioSources[i].timeSamples > (float)audioSources[i].clip.samples * panMax && audioTypes[i] != "music")
			{
				ReduceVol(i);
			}
		}
	}

	private void ReduceVol(int i)
	{
		previousVol = audioSources[i].volume;
		volReduction = originalVolumes[i] / 4f;
		audioSources[i].volume -= volReduction;
		if (audioSources[i].volume < 0.01f && previousVol > 0.01f)
		{
			audioSources[i].volume = 0.01f;
		}
		else if (previousVol == 0.01f)
		{
			audioSources[i].volume = 0f;
		}
	}

	public AudioSource Play(AudioClip clip, float vol, float pitch, float pan)
	{
		index = GetAvailableAudioSource(false);
		audioSources[index].clip = clip;
		audioSources[index].volume = soundsVolume * vol * (float)soundsMute;
		originalVolumes[index] = vol;
		audioTypes[index] = "sound";
		audioSources[index].priority = 120;
		audioSources[index].pitch = pitch;
		audioSources[index].panStereo = pan * panMax;
		audioSources[index].loop = false;
		audioSources[index].timeSamples = 0;
		audioSources[index].Play();
		return audioSources[index];
	}

	public AudioSource PlayLowPriority(AudioClip clip, float vol, float pitch, float pan)
	{
		index = GetAvailableAudioSource(true);
		if (index == -1)
		{
			return null;
		}
		audioSources[index].clip = clip;
		audioSources[index].volume = soundsVolume * vol * (float)soundsMute;
		originalVolumes[index] = vol;
		audioTypes[index] = "sound";
		audioSources[index].priority = 100;
		audioSources[index].pitch = pitch;
		audioSources[index].panStereo = pan * panMax;
		audioSources[index].loop = false;
		audioSources[index].timeSamples = 0;
		audioSources[index].Play();
		return audioSources[index];
	}

	public AudioSource PlayMusic(AudioClip clip, float vol, float pitch, float pan)
	{
		index = GetAvailableAudioSource(false);
		audioSources[index].clip = clip;
		audioSources[index].volume = musicVolume * vol * (float)musicMute * musicVolumeNormalizer;
		originalVolumes[index] = vol;
		audioTypes[index] = "music";
		audioSources[index].priority = 130;
		audioSources[index].pitch = pitch;
		audioSources[index].panStereo = pan * panMax;
		audioSources[index].loop = false;
		audioSources[index].timeSamples = 0;
		audioSources[index].Play();
		changeToMenuMusic2 = false;
		fadingMusic = false;
		fadeAS = null;
		return audioSources[index];
	}

	public AudioSource PlayMusicLoop(AudioClip clip, float vol, float pitch, float pan)
	{
		AudioSource result = PlayMusic(clip, vol, pitch, pan);
		audioSources[index].loop = true;
		return result;
	}

	public void UpdatePan(AudioSource audioSource, float pan)
	{
		audioSource.panStereo = pan * panMax;
	}

	private int GetAvailableAudioSource(bool lowPrioroty)
	{
		if (PauseController.pauseController.paused)
		{
			for (int i = 0; i < pauseRecord.Length; i++)
			{
				if (!pauseRecord[i])
				{
					return i;
				}
			}
			if (lowPrioroty)
			{
				return -1;
			}
			for (int j = 0; j < audioSources.Length; j++)
			{
				if (audioTypes[j] != "music")
				{
					return j;
				}
			}
		}
		else
		{
			for (int k = 0; k < audioSources.Length; k++)
			{
				if (!audioSources[k].isPlaying)
				{
					return k;
				}
			}
			if (lowPrioroty)
			{
				return -1;
			}
			for (int l = 0; l < audioSources.Length; l++)
			{
				if (audioTypes[l] != "music")
				{
					return l;
				}
			}
		}
		Debug.Log("BIG ERROR: GetAvailableAudioSource() couldn't return anything. This shouldn't happen.");
		ConsoleController.consoleController.ShowText("BIG ERROR: GetAvailableAudioSource() couldn't return anything. This shouldn't happen.");
		return 0;
	}

	private void InitializeAudioSources()
	{
		audioSources = GetComponents<AudioSource>();
	}

	private void InitializeOriginalVolumes()
	{
		originalVolumes = new float[audioSources.Length];
		for (int i = 0; i < audioSources.Length; i++)
		{
			originalVolumes[i] = 1f;
		}
	}

	public void PauseAll()
	{
		if (pauseRecord.Length > audioSources.Length)
		{
			pauseRecord = new bool[audioSources.Length];
		}
		for (int i = 0; i < pauseRecord.Length; i++)
		{
			if (audioSources[i].isPlaying)
			{
				audioSources[i].Pause();
				pauseRecord[i] = true;
			}
		}
	}

	public void UnpauseAll()
	{
		for (int i = 0; i < pauseRecord.Length; i++)
		{
			if (pauseRecord[i])
			{
				audioSources[i].UnPause();
			}
		}
		pauseRecord = new bool[audioSources.Length];
	}

	public void ResetPaused()
	{
		pauseRecord = new bool[audioSources.Length];
	}

	private void InitializeAudioTypes()
	{
		audioTypes = new string[audioSources.Length];
		for (int i = 0; i < audioSources.Length; i++)
		{
			audioTypes[i] = "sound";
		}
	}

	public void StopAll()
	{
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioSources[i].isPlaying)
			{
				audioSources[i].Stop();
			}
		}
	}

	public void PlayAccept()
	{
		Play(acceptSound, 0.9f, 1f, 0f);
	}

	public void PlayCancel()
	{
		Play(cancelSound, 0.9f, 1f, 0f);
	}

	public void PlayMoveSelection()
	{
		Play(moveSelectionSound, 0.8f, 1f, 0f);
	}

	public void PlayInvalid()
	{
		Play(invalidSound, 0.5f, 1f, 0f);
	}

	public void PlayMainMenuMusicWithIntro()
	{
		if (GlobalControl.globalControl.storyLevel < 2)
		{
			menuMusicAS = PlayMusic(mainMenuMusic1, 0.6f, 1f, 0f);
		}
		else
		{
			menuMusicAS = PlayMusic(mainMenuMusicSwing1, 0.6f, 1f, 0f);
		}
		changeToMenuMusic2 = true;
	}

	public void PlayMainMenuMusic()
	{
		if (GlobalControl.globalControl.storyLevel < 2)
		{
			menuMusicAS = PlayMusicLoop(mainMenuMusic2, 0.6f, 1f, 0f);
		}
		else
		{
			menuMusicAS = PlayMusicLoop(mainMenuMusicSwing2, 0.6f, 1f, 0f);
		}
	}

	public void ClearMusic()
	{
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioTypes[i] == "music")
			{
				ClearAudioSource(i);
			}
		}
		fadingMusic = false;
		changeToMenuMusic2 = false;
	}

	public void FadeMusic(AudioSource fadeASx, float fadeTimex)
	{
		if (!(fadeASx == null))
		{
			fadingMusic = true;
			fadeAS = fadeASx;
			fadeTime = fadeTimex;
			fadeOriginalVol = fadeAS.volume;
		}
	}

	private void FadeMusicUpdate()
	{
		if (fadeAS == null)
		{
			fadingMusic = false;
			return;
		}
		fadeAS.volume -= fadeOriginalVol / fadeTime * Time.deltaTime;
		if (fadeAS.volume <= 0f)
		{
			fadingMusic = false;
		}
	}

	private void ClearAudioSource(int index)
	{
		audioSources[index].Stop();
		audioSources[index].clip = null;
		audioSources[index].loop = false;
		audioTypes[index] = string.Empty;
		pauseRecord[index] = false;
	}
}
