using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerMove player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMove>();

    }

    private void DisableMovementAndJump()
    {
        player.EnableMovementAndJump(false);
    }

    private void EnableMovementAndJump()
    {
        player.EnableMovementAndJump(true);
    }
}
