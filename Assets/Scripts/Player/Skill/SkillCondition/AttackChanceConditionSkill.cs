using System;

public class AttackChanceConditionSkill : BaseSkillCondition
{
    public float Chance { get; protected set; }

    public AttackChanceConditionSkill(float chance)
    {
        Chance = chance;
    }

    public override Type GetConditionType()
    {
        return typeof(AttackChanceConditionSkill);
    }

    private float GetRandomFloat() => UnityEngine.Random.Range(0f, 1f);

    public override void Trigger(StatusContext onConditionMet)
    {
        if(GetRandomFloat() <= Chance)
        {
            onMet?.Invoke(null);
        }
    }

    public override void OnRegister(Action<StatusContext> onConditionMet)
    {
        onMet = onConditionMet;
        //SkillConditionManager.Instance.Player.OnAttack += Trigger;
    }

    public override void OnUnregister()
    {
        //SkillConditionManager.Instance.Player.OnAttack -= Trigger;
    }
}
