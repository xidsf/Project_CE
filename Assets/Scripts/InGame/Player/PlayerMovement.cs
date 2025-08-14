using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IInitializable, IEventSubscriber
{
    private PlayerInputHandler inputHandler;
    private PlayerStat playerStat;
    private ExLifeSkillHandler exLifeSkillHandler;

    private Rigidbody2D playerRigid;
    private GameObject controlPlayer;

    private int lookDir = 1; // 1: right, -1: left Move함수에서의 float연산 방지를 위한 변수

    public void Initialize(Player player)
    {
        inputHandler = player.PlayerInputHandler;
        playerStat = player.PlayerStat;
        exLifeSkillHandler = player.ExLifeSkillHandler;

        playerRigid = player.PlayerRigid;
        controlPlayer = player.gameObject;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        int dir;
        if (inputValue.x > 0) dir = 1;
        else dir = -1;
        playerRigid.linearVelocityX = dir * playerStat.MoveSpeed.GetFinalValue();
    }

    private void CancelMove(InputAction.CallbackContext ctx)
    {
        CancelMove(0);
    }

    private void CancelMove(float dur)
    {
        playerRigid.linearVelocityX = 0;
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        lookDir = inputValue.x > 0 ? 1 : -1;
        controlPlayer.transform.localScale = new Vector3(lookDir, 1, 1);
    }

    public Vector2 GetLookDirection()
    {
        return new Vector2(lookDir, 0);
    }

    public void SubscribeEvent()
    {
        inputHandler.OnMove += Move;
        inputHandler.OnLook += Look;
        inputHandler.OnMoveCanceled += CancelMove;

        exLifeSkillHandler.OnExLifeSkillUsedSuccess += CancelMove;
        exLifeSkillHandler.OnExLifeSkillUsedSuccess += SetVelocityZero;
    }

    public void UnsubscribeEvent()
    {
        inputHandler.OnMove -= Move;
        inputHandler.OnLook -= Look;
        inputHandler.OnMoveCanceled -= CancelMove;

        exLifeSkillHandler.OnExLifeSkillUsedSuccess -= CancelMove;
        exLifeSkillHandler.OnExLifeSkillUsedSuccess -= SetVelocityZero;
    }

    private void SetVelocityZero(float duration)
    {
        playerRigid.linearVelocityX = 0;
    }
}
