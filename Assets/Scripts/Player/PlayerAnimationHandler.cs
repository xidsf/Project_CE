using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour, IInitializable
{
    Animator anim;

    private const string moveFrontAnimParamString = "isMoveFront";
    private const string moveBackAnimParamString = "isMoveBack";
    private const string exLifeSkillAnimParamString = "isExLifeSkill";

    private const string parryingRegexString = "_Parrying$";
    private const string exLifeSkillRegexString = "_ExLifeSkill$";

    public float ParryingAnimLength { get; private set; } = 0f;
    public float ExLifeSkillLength { get; private set; } = 0f;

    public void Initialize(Player player)
    {
        anim = player.Animator;

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

    public void SetIdleStateAnim()
    {
        anim.SetBool(moveFrontAnimParamString, false);
        anim.SetBool(moveBackAnimParamString, false);
    }

    public void SetMoveStateAnim(bool isForwardMoving)
    {
        if(isForwardMoving)
        {
            MoveForwardAnim();
        }
        else
        {
            MoveBackwardAnim();
        }
    }

    public void TriggerExLifeSkill()
    {
        anim.SetTrigger(exLifeSkillAnimParamString);
    }

    private void MoveForwardAnim()
    {
        anim.SetBool(moveFrontAnimParamString, true);
        anim.SetBool(moveBackAnimParamString, false);
    }

    private void MoveBackwardAnim()
    {
        anim.SetBool(moveFrontAnimParamString, false);
        anim.SetBool(moveBackAnimParamString, true);
    }
}
