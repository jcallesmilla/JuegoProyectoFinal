using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.currentHealth = 0;
                player.BalaDamage(0);
            }
        }
    }
}

