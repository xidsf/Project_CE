using System;
using UnityEngine;

public class ElapsedTimeCondition : SkillCondition
{
    public double ConditionInterval { get; private set; } = 5f;
    IDisposable conditionTimer;

    public ElapsedTimeCondition(Player player , double interval = 5f) : base(player)
    {
        ConditionType = SkillTriggerCondition.EveryNTime;
        ConditionInterval = interval;
    }

    protected override void CheckConditionMet(PlayerContext ctx)
    {
    }

    public override void UpgradeCondition(float amount)
    {
        ConditionInterval -= amount;
        if (ConditionInterval < 0.5f)
        {
            ConditionInterval = 0.5f; // 최소 0.5초로 제한
        }
    }

    public override void OnRegister()
    {
        conditionTimer = TimeScheduleManager.Instance.StartRepeatingTimerCondition(ConditionInterval, TimeScaleType.Scaled, () =>
        {
            var ctx = currentPlayer.GetCurrentPlayerContext(PlayerContextType.ElapsedTime);
            OnConditionMet?.Invoke(ctx);
        });
    }

    public override void OnUnregister()
    {
        conditionTimer?.Dispose();
        conditionTimer = null;
    }

}
