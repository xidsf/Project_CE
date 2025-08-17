using System;
using UnityEngine;

public class ElapsedTimeCondition : BaseCondition
{
    [SerializeField] private double ConditionInterval = 5f; // 예시 시간. 이후에 수정 및 인스팩터에서 조정 가능
    IDisposable conditionTimer;

    public ElapsedTimeCondition(double interval = 5f)
    {
        ConditionType = SkillCondition.EveryNTime;
        ConditionInterval = interval;
        conditionTimer = TimeScheduleManager.Instance.StartRepeatingTimerCondition(ConditionInterval, TimeScaleType.Scaled, () =>
        {
            OnConditionMet?.Invoke();
        });
    }

    public override void OnRegister()
    {
    }

    public override void OnUnregister()
    {
    }
}
