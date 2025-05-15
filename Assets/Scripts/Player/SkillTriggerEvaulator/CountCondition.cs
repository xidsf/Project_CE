using System;

public class CountCondition : ISkillTriggerCondition
{
    protected int maxCount;
    public int Count { get; protected set; }
    private ISkillEffect skillInstance;
    public ISkillEffect SkillInstance
    {
        get => skillInstance;
        set => skillInstance = value;
    }

    public CountCondition(int killCountCondition, ISkillEffect skill)
    {
        this.maxCount = killCountCondition;
        skillInstance = skill;
        Count = 0;
    }

    public Type GetTriggerType()
    {
        return typeof(CountCondition);
    }

    public virtual void AddCount(int count)
    {
        Count += count;
        EvaluateCondition();
    }

    public void EvaluateCondition()
    {
        if (Count >= maxCount)
        {
            skillInstance.Effect();
            Count = maxCount - Count;
        }
    }

}
