using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour, IInitializable
{
    Animator anim;

    private const string moveFrontAnimString = "isMoveFront";
    private const string moveBackAnimString = "isMoveBack";
    private const string exLifeSkillAnimString = "isExLifeSkill";

    public void Initialize(Player player)
    {
        anim = player.Animator;
    }

    public void SetIdleStateAnim()
    {
        anim.SetBool(moveFrontAnimString, false);
        anim.SetBool(moveBackAnimString, false);
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
        anim.SetTrigger(exLifeSkillAnimString);
    }

    private void MoveForwardAnim()
    {
        anim.SetBool(moveFrontAnimString, true);
        anim.SetBool(moveBackAnimString, false);
    }

    private void MoveBackwardAnim()
    {
        anim.SetBool(moveFrontAnimString, false);
        anim.SetBool(moveBackAnimString, true);
    }
}
