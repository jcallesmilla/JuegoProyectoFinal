using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reproduce música de fondo para el menú. 
/// - Arrastra un AudioClip en el inspector.
/// - Por defecto persiste entre escenas. Desactiva `persistAcrossScenes` si quieres que sólo suene en el menú.
/// </summary>
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
        // Singleton simple: sólo una instancia reproducirá música
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

        // Aseguramos que exista un AudioSource
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
        // Mantener el volumen actualizado en el editor
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.volume = volume;
    }

    /// <summary>
    /// Permite iniciar la música desde otro script si hace falta.
    /// </summary>
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

    /// <summary>
    /// Parar la música (útil si quieres detenerla al lanzar el juego desde el menú).
    /// </summary>
    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
