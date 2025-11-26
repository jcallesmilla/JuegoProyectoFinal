using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryMode : MonoBehaviour
{
    [Header("Story Images (Orden de la historia)")]
    [SerializeField] private Sprite[] storyImages;

    [Header("Story Audio Clips (Mismo orden que las imágenes)")]
    [SerializeField] private AudioClip[] storyAudios;

    [Header("UI References")]
    [SerializeField] private Image storyImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button speakerButton;

    private int currentIndex = 0;
    private AudioSource audioSource;

    private void Start()
    {
        // Validación
        if (storyImages.Length == 0)
        {
            Debug.LogError("No se asignaron imágenes para el Story Mode.");
            return;
        }

        if (storyAudios.Length != storyImages.Length)
        {
            Debug.LogWarning("El número de audios no coincide con el número de imágenes.");
        }

        // Mostrar primera imagen
        storyImage.sprite = storyImages[0];

        // Crear o recuperar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        // Eventos de botones
        nextButton.onClick.AddListener(NextImage);
        speakerButton.onClick.AddListener(PlayCurrentAudio);
    }

    private void NextImage()
    {
        currentIndex++;

        if (currentIndex < storyImages.Length)
        {
            storyImage.sprite = storyImages[currentIndex];
        }
        else
        {
            // Fin de la historia → cambiar escena
            SceneManager.LoadSceneAsync(2);
        }
    }

    public void PlayCurrentAudio()
    {
        if (currentIndex >= 0 && currentIndex < storyAudios.Length)
        {
            if (storyAudios[currentIndex] != null)
                audioSource.PlayOneShot(storyAudios[currentIndex]);
        }
    }

    public AudioClip GetCurrentAudio()
    {
    if (currentIndex >= 0 && currentIndex < storyAudios.Length)
        return storyAudios[currentIndex];

    return null;
    }
}