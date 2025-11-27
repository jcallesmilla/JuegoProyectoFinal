using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class StoryModeBase : MonoBehaviour
{
    [Header("Story Images (Orden de la historia)")]
    [SerializeField] private Sprite[] storyImages;

    [Header("Story Audio Clips (Mismo orden que las imágenes)")]
    [SerializeField] private AudioClip[] storyAudios;

    [Header("UI References")]
    [SerializeField] private Image storyImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button speakerButton;

    [Header("Scene Settings")]
    [SerializeField] private string targetSceneName;

    private int currentIndex = 0;
    private AudioSource audioSource;

    private void Start()
    {
        if (storyImages.Length == 0)
        {
            Debug.LogError("No se asignaron imágenes para el Story Mode.");
            return;
        }

        if (storyAudios.Length != storyImages.Length)
        {
            Debug.LogWarning("El número de audios no coincide con el número de imágenes.");
        }

        if (storyImage == null)
        {
            Debug.LogError("La referencia a storyImage no está asignada.");
            return;
        }

        if (nextButton == null)
        {
            Debug.LogError("La referencia a nextButton no está asignada.");
            return;
        }

        if (speakerButton == null)
        {
            Debug.LogError("La referencia a speakerButton no está asignada.");
            return;
        }

        storyImage.sprite = storyImages[0];

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource añadido dinámicamente al GameObject " + gameObject.name);
        }

        audioSource.playOnAwake = false;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextImage);
        
        speakerButton.onClick.RemoveAllListeners();
        speakerButton.onClick.AddListener(PlayCurrentAudio);

        Debug.Log("StoryModeBase inicializado correctamente en " + gameObject.name);
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
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("Cargando escena: " + targetSceneName);
            SceneManager.LoadSceneAsync(targetSceneName);
        }
        else
        {
            Debug.LogError("No se especificó una escena de destino en el campo 'Target Scene Name'.");
        }
    }

    public void PlayCurrentAudio()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource es null. No se puede reproducir audio.");
            return;
        }

        if (storyAudios == null || storyAudios.Length == 0)
        {
            Debug.LogError("No hay audios asignados en el array storyAudios.");
            return;
        }

        if (currentIndex >= 0 && currentIndex < storyAudios.Length)
        {
            if (storyAudios[currentIndex] != null)
            {
                Debug.Log("Reproduciendo audio " + currentIndex + ": " + storyAudios[currentIndex].name);
                audioSource.PlayOneShot(storyAudios[currentIndex]);
            }
            else
            {
                Debug.LogWarning("El audio en el índice " + currentIndex + " es null.");
            }
        }
        else
        {
            Debug.LogWarning("Índice de audio fuera de rango: " + currentIndex);
        }
    }

    public AudioClip GetCurrentAudio()
    {
        if (currentIndex >= 0 && currentIndex < storyAudios.Length)
            return storyAudios[currentIndex];

        return null;
    }
}

