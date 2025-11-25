using UnityEngine;

public class IgnoreHelicopterCollision : MonoBehaviour
{
    private void Start()
    {
        GameObject helicopter = GameObject.Find("Helicoptero");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject entrar = GameObject.Find("Entrar");
        
        if (helicopter != null)
        {
            Collider2D helicopterCollider = helicopter.GetComponent<Collider2D>();
            
            if (helicopterCollider != null)
            {
                if (entrar != null)
                {
                    Collider2D entrarCollider = entrar.GetComponent<Collider2D>();
                    if (entrarCollider != null)
                    {
                        Physics2D.IgnoreCollision(entrarCollider, helicopterCollider, true);
                        Debug.Log("Ignoring collision between Helicopter and Entrar");
                    }
                }
                
                if (player != null)
                {
                    Collider2D playerCollider = player.GetComponent<Collider2D>();
                    if (playerCollider != null)
                    {
                        Physics2D.IgnoreCollision(playerCollider, helicopterCollider, true);
                        Debug.Log("Ignoring collision between Helicopter and Player");
                    }
                }
            }
        }
    }
}


