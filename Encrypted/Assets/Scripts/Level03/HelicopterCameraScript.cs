using UnityEngine;

public class HelicopterCameraScript : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform helicopterTarget;
    [SerializeField] private string helicopterName = "Helicoptero";
    
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool useFixedUpdate = true;
    
    private void Start()
    {
        if (helicopterTarget == null)
        {
            GameObject helicopter = GameObject.Find(helicopterName);
            if (helicopter != null)
            {
                helicopterTarget = helicopter.transform;
            }
            else
            {
                Debug.LogWarning($"Helicopter '{helicopterName}' not found in scene!");
            }
        }
    }
    
    private void LateUpdate()
    {
        if (!useFixedUpdate)
        {
            FollowTarget();
        }
    }
    
    private void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            FollowTarget();
        }
    }
    
    private void FollowTarget()
    {
        if (helicopterTarget == null) return;
        
        Vector3 desiredPosition = helicopterTarget.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
