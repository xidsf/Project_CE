using System;

public enum SkillTriggerCondition
{
    AttackChance,
    EveryNTime,
    EveryNAttack,
}

public abstract class SkillCondition : IDisposable
{
    public SkillTriggerCondition ConditionType { get; protected set; }
    protected Action<PlayerContext> OnConditionMet;
    protected Player currentPlayer;

    public SkillCondition(Player inGamePlayer)
    {
        currentPlayer = inGamePlayer;
    }

    public void SubscribeConditionMet(Action<PlayerContext> action)
    {
        OnConditionMet = action;
    }

    public void UnsubscribeConditionMet()
    {
        OnConditionMet = null;
    }

    protected abstract void CheckConditionMet(PlayerContext stat);
    public abstract void UpgradeCondition(float amount);

    public abstract void OnRegister();
    public abstract void OnUnregister();
    public virtual void Dispose()
    {
        OnUnregister();
        OnConditionMet = null;
        currentPlayer = null;
    }
}
