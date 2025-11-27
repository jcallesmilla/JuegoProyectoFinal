using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnterHelicopterTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject canvasToHide;
    [SerializeField] private Animator helicopterAnimator;
    [SerializeField] private GameObject helicopterObject;
    
    [Header("Animation Settings")]
    [SerializeField] private string idleAnimationName = "idle";
    [SerializeField] private string landingAnimationName = "landing";
    
    [Header("Movement Settings")]
    [SerializeField] private float landingAnimationDuration = 4f;
    [SerializeField] private float upwardDistance = 10f;
    [SerializeField] private float moveUpSpeed = 2f;
    
    private bool isPlayerInside = false;
    private bool hasEnteredHelicopter = false;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private Rigidbody2D helicopterRigidbody;
    
    private void Start()
    {
        if (helicopterObject != null)
        {
            helicopterRigidbody = helicopterObject.GetComponent<Rigidbody2D>();
        }
        
        if (helicopterAnimator != null)
        {
            helicopterAnimator.Play(idleAnimationName);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerAnimator = collision.gameObject.GetComponentInChildren<Animator>();
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasEnteredHelicopter)
        {
            ResetTrigger();
        }
    }
    
    private void Update()
    {
        if (isPlayerInside && Keyboard.current.fKey.wasPressedThisFrame && !hasEnteredHelicopter)
        {
            EnterHelicopter();
        }
    }
    
    private void EnterHelicopter()
    {
        hasEnteredHelicopter = true;
        
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
            playerRigidbody.bodyType = RigidbodyType2D.Static;
            playerRigidbody.gameObject.SetActive(false);
        }
        
        if (playerAnimator != null)
        {
            playerAnimator.enabled = false;
        }
        
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false);
        }
        
        StartCoroutine(HelicopterSequence());
    }
    
    private IEnumerator HelicopterSequence()
    {
        if (helicopterRigidbody != null)
        {
            helicopterRigidbody.linearVelocity = Vector2.zero;
            helicopterRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
        
        if (helicopterAnimator != null)
        {
            helicopterAnimator.Play(idleAnimationName);
        }
        
        yield return new WaitForSeconds(landingAnimationDuration);
        
        if (helicopterObject != null)
        {
            Vector3 startPosition = helicopterObject.transform.position;
            Vector3 targetPosition = startPosition + new Vector3(0f, upwardDistance, 0f);
            
            while (Vector3.Distance(helicopterObject.transform.position, targetPosition) > 0.01f)
            {
                helicopterObject.transform.position = Vector3.MoveTowards(
                    helicopterObject.transform.position, 
                    targetPosition, 
                    moveUpSpeed * Time.deltaTime
                );
                yield return null;
            }
        }
        
        SceneManager.LoadScene("SM Level 3");
    }
    
    private void ResetTrigger()
    {
        isPlayerInside = false;
        playerRigidbody = null;
        playerAnimator = null;
    }
}

