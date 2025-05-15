using System;

public class ChanceCondition : ISkillTriggerCondition
{
    public float Chance { get; protected set; }
    private ISkillEffect skillInstance;
    public ISkillEffect SkillInstance
    {
        get => skillInstance;
        set => skillInstance = value;
    }

    public ChanceCondition(float chance, ISkillEffect skill)
    {
        Chance = chance;
        skillInstance = skill;
    }

    public Type GetTriggerType()
    {
        return typeof(ChanceCondition);
    }

    private float GetRandomFloat() => UnityEngine.Random.Range(0f, 1f);

    public void EvaluateCondition()
    {
        if (GetRandomFloat() <= Chance)
        {
            skillInstance.Effect();
        }
    }
}
