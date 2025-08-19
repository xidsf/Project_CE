public class Skill
{
    public long SkillSerialNumber { get; private set; }
    SkillCondition condition;
    IAbility effect;

    public SkillCondition Condition => condition;
    public IAbility Effect => effect;

    public Skill(long serialNum, SkillCondition condition, IAbility effect)
    {
        SkillSerialNumber = serialNum;
        this.condition = condition;
        this.effect = effect;

        condition.OnRegister();
        condition.SubscribeConditionMet(effect.ExecuteSkill);
    }

    public void Remove()
    {
        condition.UnsubscribeConditionMet();
        condition.Dispose();
        effect = null;
        condition = null;
    }
}
