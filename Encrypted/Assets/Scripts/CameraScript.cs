using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;

    [Header("Vertical follow")]
    [Tooltip("If true, camera will follow player's Y when they move beyond verticalDeadZone.")]
    public bool followY = true;

    [Tooltip("Dead zone (units) for vertical movement. Camera won't move for smaller Y changes.")]
    public float verticalDeadZone = 0.3f;

    [Tooltip("Smoothing factor for vertical following (0 = no move, 1 = instant).")]
    [Range(0f, 1f)]
    public float verticalSmoothSpeed = 0.125f;

    [Header("Camera offset")]
    [Tooltip("Vertical offset relative to player (positive = camera higher).")]
    public float verticalOffset = 1.5f;  // <--- añade este valor

    void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 position = transform.position;
        position.x = player.transform.position.x;

        if (followY)
        {
            float targetY = player.transform.position.y + verticalOffset; // <--- usa el offset aquí
            float deltaY = Mathf.Abs(targetY - position.y);
            if (deltaY > verticalDeadZone)
            {
                position.y = Mathf.Lerp(position.y, targetY, verticalSmoothSpeed);
            }
        }

        transform.position = position;
    }
}
