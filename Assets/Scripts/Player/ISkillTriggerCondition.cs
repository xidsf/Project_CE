using System;

public interface ISkillTriggerCondition
{
    Type GetTriggerType();
    public void EvaluateCondition();
    public ISkillEffect SkillInstance { get; }
}
