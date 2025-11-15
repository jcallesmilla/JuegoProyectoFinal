using UnityEngine;

public class PasswordTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        Debug.Log("PasswordTrigger: Script started on " + gameObject.name);
        
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Debug.Log($"PasswordTrigger: Collider is trigger = {col.isTrigger}, enabled = {col.enabled}");
        }
        else
        {
            Debug.LogError("PasswordTrigger: No BoxCollider2D found!");
        }
        
        if (PasswordUI.Instance != null)
        {
            Debug.Log("PasswordTrigger: PasswordUI Instance found!");
        }
        else
        {
            Debug.LogError("PasswordTrigger: PasswordUI Instance is NULL!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"PasswordTrigger: Something entered! Name: {collision.gameObject.name}, Tag: {collision.tag}, Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
        
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("PasswordTrigger: Player detected! Opening password panel...");
            if (PasswordUI.Instance != null)
            {
                PasswordUI.Instance.ShowPasswordPanel();
            }
            else
            {
                Debug.LogError("PasswordTrigger: PasswordUI Instance not found in the scene!");
            }
        }
        else
        {
            Debug.Log($"PasswordTrigger: Not player. Expected tag '{playerTag}' but got '{collision.tag}'");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"PasswordTrigger: Regular collision (not trigger) detected with {collision.gameObject.name}");
    }
}

