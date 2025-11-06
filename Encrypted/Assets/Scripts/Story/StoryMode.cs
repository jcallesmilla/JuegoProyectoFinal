using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryMode : MonoBehaviour
{
	[Header("Story Images")]
	[Tooltip("Asigna 'The Last Guardian of The Net.png'")]
	[SerializeField] private Sprite guardianImage;
	[Tooltip("Asigna 'Amid the ruins of Firewall City.png'")]
	[SerializeField] private Sprite firewallImage;
	[Tooltip("Asigna 'Mutant viruses.png'")]
	[SerializeField] private Sprite virusesImage;
	[Tooltip("Asigna 'Recovered fragments.png'")]
	[SerializeField] private Sprite fragmentsImage;

	[Header("UI References")]
	[SerializeField] private Image storyImage;
	[SerializeField] private Button nextButton;

	private int currentImageIndex = 0;
	private Sprite[] storySequence;

	private void Start()
	{
		// Definir el orden específico de las imágenes
		storySequence = new Sprite[] { guardianImage, // 1. The Last Guardian of The Net
										firewallImage, // 2. Amid the ruins of Firewall City
										virusesImage, // 3. Mutant viruses
										fragmentsImage // 4. Recovered Fragments
									  };

		// Mostrar la primera imagen
		if (storySequence.Length > 0)
		{
			storyImage.sprite = storySequence[0];
		}

		nextButton.onClick.AddListener(NextImage);
	}

	private void NextImage()
	{
		currentImageIndex++;
		if (currentImageIndex < storySequence.Length)
		{
			storyImage.sprite = storySequence[currentImageIndex];
		}
		else
		{
			// Cuando se terminan las imágenes, cargar el nivel del juego
			SceneManager.LoadSceneAsync(2); // Ajustar este número según donde esté tu nivel en el build settings
		}
	}
}

