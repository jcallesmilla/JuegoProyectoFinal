using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public static UI instance;
    [SerializeField] private GameObject gameOverUI;
    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }
    public void EnableGameOverUI()
    {
        Time.timeScale = 0.5f;
        gameOverUI.SetActive(true);

    }
    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
