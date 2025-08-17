using System;
using UnityEngine;

public class ElapsedTimeCondition : BaseCondition
{
    [SerializeField] private double ConditionInterval = 5f; // ���� �ð�. ���Ŀ� ���� �� �ν����Ϳ��� ���� ����
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
