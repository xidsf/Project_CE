using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationHandler : MonoBehaviour, IInitializable, IEventSubscriber
{
    Animator anim;
    PlayerInputHandler inputHandler;
    ExLifeSkillHandler exLifeSkillHandler;

    private const string moveFrontAnimParamString = "isMoveFront";
    private const string moveBackAnimParamString = "isMoveBack";
    private const string exLifeSkillAnimParamString = "isExLifeSkill";

    private const string parryingRegexString = "_Parrying$";
    private const string exLifeSkillRegexString = "_ExLifeSkill$";

    public float ParryingAnimLength { get; private set; } = 0f;
    public float ExLifeSkillLength { get; private set; } = 0f;
    public float GetAnimRunningTime()
    {
        return ParryingAnimLength + ExLifeSkillLength;
    }

    private Vector2 inputVector;

    bool isIdleState = true;

    private void Update()
    {
        if(isIdleState)
            SetMoveStateAnim();
    }

    public void Initialize(Player player)
    {
        anim = player.Animator;
        inputHandler = player.PlayerInputHandler;
        exLifeSkillHandler = player.ExLifeSkillHandler;

        foreach (var animClip in anim.runtimeAnimatorController.animationClips)
        {
            if (Regex.IsMatch(animClip.name, parryingRegexString))
            {
                ParryingAnimLength = animClip.length;
            }
            else if (Regex.IsMatch(animClip.name, exLifeSkillRegexString))
            {
                ExLifeSkillLength = animClip.length;
            }
        }
    }

    private void SetInputVector(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    }

    private void SetMoveStateAnim()
    {
        int playerMovingDir = inputVector.x > 0 ? 1 : -1; // 1: right, -1: left
        int playerLookingDir = transform.localScale.x > 0 ? 1 : -1; // 1: right, -1: left

        if(inputVector == Vector2.zero)
        {
            SetIdleStateAnim();
        }
        else if (playerLookingDir == playerMovingDir)
        {
            SetMoveForwardAnim();
        }
        else
        {
            SetMoveBackwardAnim();
        }
    }

    private void SetMoveForwardAnim()
    {
        anim.SetBool(moveFrontAnimParamString, true);
        anim.SetBool(moveBackAnimParamString, false);
    }

    private void SetMoveBackwardAnim()
    {
        anim.SetBool(moveFrontAnimParamString, false);
        anim.SetBool(moveBackAnimParamString, true);
    }

    private void SetIdleStateAnim()
    {
        anim.SetBool(moveFrontAnimParamString, false);
        anim.SetBool(moveBackAnimParamString, false);
    }

    private void TriggerExLifeSkillAnim(float duration)
    {
        isIdleState = false;
        inputVector = Vector2.zero;
        anim.SetTrigger(exLifeSkillAnimParamString);
        Invoke(nameof(ResetIdleState), duration);
    }

    private void ResetIdleState()
    {
        isIdleState = true;
    }

    public void SubscribeEvent()
    {
        inputHandler.OnMove += SetInputVector;
        inputHandler.OnMoveCanceled += SetInputVector;

        exLifeSkillHandler.OnExLifeSkillUsedSuccess += TriggerExLifeSkillAnim;
    }

    public void UnsubscribeEvent()
    {
        inputHandler.OnMove -= SetInputVector;
        inputHandler.OnMoveCanceled -= SetInputVector;

        exLifeSkillHandler.OnExLifeSkillUsedSuccess -= TriggerExLifeSkillAnim;
    }
}
