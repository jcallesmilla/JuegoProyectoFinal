using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic Instance { get; private set; }

    [Tooltip("Clip de música a reproducir en el menú principal")]
    public AudioClip musicClip;

    [Range(0f, 1f)]
    public float volume = 0.6f;

    [Tooltip("Si es true, el objeto no se destruye al cambiar de escena (persistente)")]
    public bool persistAcrossScenes = true;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (persistAcrossScenes)
        {
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.Play();
        }
    }

    void OnValidate()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.volume = volume;
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        if (audioSource != null)
        {
            audioSource.volume = newVolume;
        }
    }

    public void Play()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = volume;
        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
    
    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource != null && newClip != null)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}

