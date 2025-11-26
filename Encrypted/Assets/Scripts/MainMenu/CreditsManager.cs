using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject creditsPanel;

    public void OpenCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }
}
