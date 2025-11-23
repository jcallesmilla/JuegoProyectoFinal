using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Tutorial()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void OpenCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

