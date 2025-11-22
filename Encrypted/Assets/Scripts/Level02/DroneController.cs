using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveUpDistance = 2f;
    [SerializeField] private float moveDownDistance = 2f;
    [SerializeField] private float moveSpeed = 1f;
    
    [Header("Wait Times")]
    [SerializeField] private float waitTimeAtTop = 2f;
    [SerializeField] private float scannerActiveTime = 3f;
    
    [Header("References")]
    [SerializeField] private GameObject scanner;
    [SerializeField] private Animator animator;
    
    private Vector3 originalPosition;
    private Vector3 topPosition;
    private Vector3 bottomPosition;
    
    private enum DroneState
    {
        MovingUp,
        WaitingAtTop,
        MovingDown,
        Scanning
    }
    
    private DroneState currentState = DroneState.MovingUp;

    private void Awake()
    {
        originalPosition = transform.position;
        topPosition = originalPosition + Vector3.up * moveUpDistance;
        bottomPosition = originalPosition - Vector3.up * moveDownDistance;
        
        if (scanner != null)
        {
            scanner.SetActive(false);
        }
        
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        StartCoroutine(DroneMovementCycle());
    }

    private IEnumerator DroneMovementCycle()
    {
        while (true)
        {
            currentState = DroneState.MovingUp;
            SetIdleAnimation();
            yield return StartCoroutine(MoveToPosition(topPosition));

            currentState = DroneState.WaitingAtTop;
            yield return new WaitForSeconds(waitTimeAtTop);

            currentState = DroneState.MovingDown;
            SetIdleAnimation();
            yield return StartCoroutine(MoveToPosition(bottomPosition));

            currentState = DroneState.Scanning;
            if (scanner != null)
            {
                scanner.SetActive(true);
            }
            SetScanAnimation();
            yield return new WaitForSeconds(scannerActiveTime);

            if (scanner != null)
            {
                scanner.SetActive(false);
            }

            SetIdleAnimation();
            yield return StartCoroutine(MoveToPosition(originalPosition));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void SetIdleAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isScanning", false);
        }
    }

    private void SetScanAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isScanning", true);
        }
    }
}


