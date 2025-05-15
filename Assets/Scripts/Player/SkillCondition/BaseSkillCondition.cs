using System;

public abstract class BaseSkillCondition : ISkillCondition
{
    protected Action<StatusContext> onMet;

    public virtual Type GetConditionType()
    {
        return typeof(BaseSkillCondition);
    }

    public virtual void OnRegister(Action<StatusContext> onConditionMet)
    {
        onMet = onConditionMet;
    }

    public virtual void OnUnregister()
    {
        onMet = null;
    }

    public virtual void Trigger(StatusContext context)
    {
        onMet?.Invoke(context);
    }
}