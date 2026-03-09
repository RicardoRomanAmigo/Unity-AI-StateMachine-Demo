using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio & Groups")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup uiGroup;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float defaultMusicFade = 0.5f;

    [Header("SFX Pool")]
    [SerializeField, Min(1)] private int sfxPoolSize = 12;
    private readonly List<AudioSource> sfxPool = new();

    [Header("UI")]
    [SerializeField] private AudioSource uiSource;

    //"Last Clip"
    private AudioClip lastMusicClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Music source
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0f;

        //UI source
        if (uiSource == null)
            uiSource = gameObject.AddComponent<AudioSource>();
        uiSource.outputAudioMixerGroup = uiGroup;
        uiSource.playOnAwake = false;
        uiSource.spatialBlend = 0f;

        //SFX pool
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var src = new GameObject($"SFX_Source_{i}").AddComponent<AudioSource>();
            src.transform.SetParent(transform, false);
            src.outputAudioMixerGroup = sfxGroup;
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            src.dopplerLevel = 0f;
            sfxPool.Add(src);
        }
    }

    //-------------Music -------------
    public void PlayMusic(AudioClip clip, float fade = -1f, bool loop = true)
    {
        if (clip == null) return;
        float f = fade < 0 ? defaultMusicFade : fade;
        StopAllCoroutines();
        StartCoroutine(FadeToMusic(clip, f, loop));
    }

    public void PushMusic(AudioClip newClip)
    {
        SaveCurrentState();
        PlayMusic(newClip);
    }

    public void PopMusic()
    {
        RestorePreviousState();
    }

    private void SaveCurrentState()
    {
        lastMusicClip = musicSource.clip;
    }

    private void RestorePreviousState()
    {
        if (lastMusicClip != null)
        {
            PlayMusic(lastMusicClip);
            lastMusicClip = null;
        }
    }

    private System.Collections.IEnumerator FadeToMusic(AudioClip next, float fade, bool loop)
    {
        if (fade <= 0f)
        {
            musicSource.clip = next;
            musicSource.loop = loop;
            musicSource.volume = 1f;
            musicSource.Play();
            yield break;
        }

        float t = 0f;
        float startVol = musicSource.isPlaying ? musicSource.volume : 0f;

        // Fade out
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / fade);
            yield return null;
        }
        musicSource.Stop();

        // Switch clip
        musicSource.clip = next;
        musicSource.loop = loop;
        musicSource.volume = 0f;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, 1f, t / fade);
            yield return null;
        }
        musicSource.volume = 1f;
    }

    //-------------SFX -------------
    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f, Vector3? worldPos = null, float spatial = 0f)
    {
        if (clip == null) return;
        var src = GetFreeSFX();
        if (worldPos.HasValue) src.transform.position = worldPos.Value;
        src.spatialBlend = Mathf.Clamp01(spatial);
        src.pitch = pitch;
        src.volume = Mathf.Clamp01(volume);
        src.clip = clip;
        src.Play();
    }

    public void PlayerUISFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        uiSource.pitch = pitch;
        uiSource.volume = Mathf.Clamp01(volume);
        uiSource.PlayOneShot(clip);
    }

    private AudioSource GetFreeSFX()
    {
        foreach (var s in sfxPool)
        {
            if (!s.isPlaying) return s;
        }
        // If all are busy, reuse the first one
        return sfxPool[0];
    }

    // ----------------- MIXER UTILS -----------------
    // linear 0..1 -> dB (-80..0 aprox)
    public void SetVolume(string exposedParam, float linear01)
    {
        float dB = (linear01 <= 0.0001f) ? -80f : Mathf.Log10(Mathf.Clamp01(linear01)) * 20f;
        mixer.SetFloat(exposedParam, dB);
    }

    public void MuteMusic(bool mute) => mixer.SetFloat("MusicVol", mute ? -80f : 0f);
    public void MuteSFX(bool mute) => mixer.SetFloat("SFXVol", mute ? -80f : 0f);

}
