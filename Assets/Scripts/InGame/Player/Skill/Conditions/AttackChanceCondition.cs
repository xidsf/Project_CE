using System;

public class AttackChanceCondition : SkillCondition
{
    public float Chance { get; private set; }

    public AttackChanceCondition(Player player, float change = 0.1f) : base(player)
    {
        ConditionType = SkillTriggerCondition.AttackChance;
        Chance = change;
    }

    protected override void CheckConditionMet(PlayerContext ctx)
    {
        var randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue <= Chance)
        {
            OnConditionMet?.Invoke(ctx);
        }
    }

    public override void UpgradeCondition(float amount)
    {
        Chance += amount;
        if (Chance > 1f)
        {
            Chance = 1f;
        }
    }

    public override void OnRegister()
    {
        currentPlayer.OnPlayerAttack += CheckConditionMet;
    }

    public override void OnUnregister()
    {
        currentPlayer.OnPlayerAttack -= CheckConditionMet;
    }
}
