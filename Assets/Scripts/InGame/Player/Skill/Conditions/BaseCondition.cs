using UnityEngine;
using System;

public enum SkillCondition
{
    AttackChance,
    EveryNTime,
    EveryNAttack,
}

public abstract class BaseCondition
{
    public SkillCondition ConditionType { get; protected set; }
    public Action OnConditionMet;

    public abstract void OnRegister();
    public abstract void OnUnregister();
}
