using UnityEngine;

/// <summary>
/// Reproduce un sonido al pulsar un botón. Añade este componente al mismo GameObject del Button
/// (o a un objeto hijo) y en el inspector del Button añade el método ButtonSound.Play al evento OnClick().
/// </summary>
public class ButtonSound : MonoBehaviour
{
    [Tooltip("Clip que sonará cuando se pulse el botón")]
    public AudioClip clickClip;

    [Range(0f, 1f)]
    public float volume = 1f;

    // Si prefieres usar un AudioSource compartido, puedes asignarlo aquí. Si es null, el script
    // creará/usarará un AudioSource en este GameObject.
    public AudioSource audioSource;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        audioSource.volume = volume;
    }

    void OnValidate()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.volume = volume;
    }

    /// <summary>
    /// Llama a este método desde el OnClick del Button para reproducir el sonido.
    /// </summary>
    public void Play()
    {
        if (clickClip == null || audioSource == null) return;
        audioSource.PlayOneShot(clickClip, volume);
    }
}
