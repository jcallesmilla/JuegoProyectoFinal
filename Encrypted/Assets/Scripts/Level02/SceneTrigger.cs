using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string targetSceneName = "Helicopter";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadSceneAsync(targetSceneName);
        }
        else
        {
            Debug.LogError("No se especificó una escena de destino en SceneTrigger.");
        }
    }
}
