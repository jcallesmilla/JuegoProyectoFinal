using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public static UI instance;
    [SerializeField] private GameObject gameOverUI;
    private void Awake()
    {
        gameOverUI.SetActive(false);
        instance = this;
        Time.timeScale = 1;
    }
    public void EnableGameOverUI()
    {
        Time.timeScale = 0.5f;
        gameOverUI.SetActive(true);

    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
