using System;

public class EveryNAttackCondition : SkillCondition
{
    private int attackCount = 0;
    public int ConditionInterval { get; private set; }

    public EveryNAttackCondition(Player player, int attackCountCondition = 5) : base(player)
    {
        ConditionType = SkillTriggerCondition.EveryNAttack;
        ConditionInterval = attackCountCondition > 0 ? attackCountCondition : 5;
    }

    protected override void CheckConditionMet(PlayerContext ctx)
    {
        attackCount++;
        if(attackCount >= ConditionInterval)
        {
            attackCount = 0;
            OnConditionMet?.Invoke(ctx);
        }
    }

    public override void UpgradeCondition(float amount)
    {
        int step = (int)amount;
        ConditionInterval -= step;
        if (ConditionInterval < 1)
        {
            ConditionInterval = 1; // 최소 1로 제한
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
