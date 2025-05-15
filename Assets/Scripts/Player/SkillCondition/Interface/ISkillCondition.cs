using System;

public interface ISkillCondition
{
    Type GetConditionType();
    public void Trigger(StatusContext onConditionMet);
    public void OnRegister(Action<StatusContext> onConditionMet);
    public void OnUnregister();
}
