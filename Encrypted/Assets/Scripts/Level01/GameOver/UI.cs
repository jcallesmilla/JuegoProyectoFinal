using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("Game Over ")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;



    private void Awake()
    {
        gameOverUI.SetActive(false);
        instance = this;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    #region Game Over

    public void EnableGameOverUI()
    {
        Time.timeScale = 0.5f;
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        GameManager.Instance.ResetGameProgress();

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    #region Pause Menu
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
            }
    #endregion

}

