using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerLookDirection
{
    Right = 1,
    Left = -1
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;
    private PlayerStat playerStat;

    private Rigidbody2D playerRigid;

    public PlayerLookDirection LookDirection { get; private set; } = PlayerLookDirection.Right;
    public Vector2 MoveDirection { get; private set; } = Vector2.zero;

    private void Start()
    {
        playerStat = player.PlayerStat;
        playerRigid = player.PlayerRigid;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        playerRigid.linearVelocity = MoveDirection * playerStat.MoveSpeed.GetFinalValue();
    }

    public void ChangeMoveDirection(Vector2 dir)
    {
        MoveDirection = dir.normalized;
        if (player.PlayerActionState == PlayerState.ATTACK)
        {
            return;
        }
        if (MoveDirection.x > 0)
        {
            LookDirection = PlayerLookDirection.Right;
        }
        else if (MoveDirection.x < 0)
        {
            LookDirection = PlayerLookDirection.Left;
        }
    }

    public void CancelMove()
    {
        MoveDirection = Vector2.zero;
    }
}
