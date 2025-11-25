using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Antes de cargar una nueva escena, intentamos parar la música del menú si existe
    void StopMenuMusicIfAny()
    {
        // If the MenuMusic type isn't resolving at compile time in some IDE setups,
        // use reflection to find an instantiated component named "MenuMusic" and call Stop().
        // This avoids a hard compile dependency while still stopping the music at runtime.
        var all = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (var mb in all)
        {
            if (mb == null) continue;
            var t = mb.GetType();
            if (t.Name == "MenuMusic")
            {
                var stop = t.GetMethod("Stop");
                if (stop != null)
                {
                    stop.Invoke(mb, null);
                }
                break;
            }
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

