using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [Header("Música del Nivel")]
    [Tooltip("AudioClip que se reproducirá cuando este nivel se cargue")]
    [SerializeField] private AudioClip levelMusicClip;

    [Header("Configuración de Volumen")]
    [Tooltip("Volumen de la música (0.0 a 1.0)")]
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.6f;

    [Tooltip("Si es true, cambia el volumen del MenuMusic al cambiar la música")]
    [SerializeField] private bool overrideVolume = true;

    [Header("Configuración de Reproducción")]
    [Tooltip("Si es true, cambia la música automáticamente al iniciar")]
    [SerializeField] private bool changeOnStart = true;

    [Tooltip("Si es true, detiene toda la música en lugar de cambiarla")]
    [SerializeField] private bool stopMusicInstead = false;

    void Start()
    {
        if (changeOnStart)
        {
            if (stopMusicInstead)
            {
                StopMusic();
            }
            else
            {
                ChangeLevelMusic();
            }
        }
    }

    public void ChangeLevelMusic()
    {
        if (MenuMusic.Instance != null && levelMusicClip != null)
        {
            MenuMusic.Instance.ChangeMusic(levelMusicClip);
            
            if (overrideVolume)
            {
                MenuMusic.Instance.SetVolume(volume);
            }
            
            Debug.Log($"Música cambiada a: {levelMusicClip.name} con volumen: {volume}");
        }
        else
        {
            if (MenuMusic.Instance == null)
                Debug.LogWarning("No se encontró MenuMusic.Instance en la escena.");
            if (levelMusicClip == null)
                Debug.LogWarning("No se asignó ningún AudioClip para este nivel.");
        }
    }

    public void StopMusic()
    {
        if (MenuMusic.Instance != null)
        {
            MenuMusic.Instance.Stop();
            Debug.Log("Música detenida en esta escena.");
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (MenuMusic.Instance != null)
        {
            MenuMusic.Instance.SetVolume(volume);
        }
    }
}
