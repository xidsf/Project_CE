using System;

public class EveryNAttackConditionSkill : BaseSkillCondition, IResetable
{
    protected int requireAttackCount;
    public int CurrentAttackCount { get; protected set; }

    public EveryNAttackConditionSkill(int killCountCondition)
    {
        requireAttackCount = killCountCondition;
        CurrentAttackCount = 0;
    }

    public override Type GetConditionType()
    {
        return typeof(EveryNAttackConditionSkill);
    }

    public virtual void AddAttackCount()
    {
        CurrentAttackCount += 1;
    }

    public void Reset()
    {
        CurrentAttackCount = 0;
    }

    public override void OnRegister(Action<StatusContext> onConditionMet)
    {
        onMet = onConditionMet;
        EventConditionManager.Instance.Player.OnAttack += Trigger;
    }

    public override void OnUnregister()
    {
        EventConditionManager.Instance.Player.OnAttack -= Trigger;
    }

    public override void Trigger(StatusContext onConditionMet)
    {
        CurrentAttackCount += 1;
        if (CurrentAttackCount >= requireAttackCount)
        {
            CurrentAttackCount = requireAttackCount - CurrentAttackCount;
            onMet?.Invoke(onConditionMet);
        }
    }
}
