using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds;
    public string backgroundMusicName = "BackgroundMusic";
    public float fadeDuration = 1f;

    private AudioSource bgSource;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        bgSource = GetSound(backgroundMusicName)?.source;
        if (bgSource != null)
        {
            bgSource.loop = true;
            bgSource.Play();
            //StartCoroutine(FadeIn(bgSource));
        }
    }

    public void Play(string name, bool oneShot = false, float pitch = 1f)
    {
        Sound s = GetSound(name);
        if (s != null)
        {
            if (!s.source.isPlaying)
            {
                if (pitch != 1)
                {
                    s.source.pitch = pitch;
                }
                if (oneShot)
                {
                    s.source.PlayOneShot(s.source.clip);
                }
                else
                {
                    s.source.Play();
                }
            }
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }


    public void Stop(string name)
    {
        Sound s = GetSound(name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void FadeOutMusic()
    {
        if (bgSource != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeOut(bgSource));
        }
    }

    public void FadeInMusic()
    {
        if (bgSource != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeIn(bgSource));
        }
    }

    private Sound GetSound(string name)
    {
        return System.Array.Find(sounds, s => s.name == name);
    }

    private IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0f;
        source.Play();
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        source.volume = 1f;
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        source.Stop();
        source.volume = startVolume;
    }
}
