using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Antes de cargar una nueva escena, intentamos parar la música del menú si existe
    void StopMenuMusicIfAny()
    {
        var menuMusic = MenuMusic.Instance;
        if (menuMusic != null)
        {
            menuMusic.Stop();
        }
    }

    public void PlayGame()
    {
        StopMenuMusicIfAny();
        SceneManager.LoadSceneAsync(1);
    }

    public void Tutorial()
    {
        StopMenuMusicIfAny();
        SceneManager.LoadSceneAsync(3);
    }

    public void OpenCharacterSelect()
    {
        StopMenuMusicIfAny();
        SceneManager.LoadScene("CharacterSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

