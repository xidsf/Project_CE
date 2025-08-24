using System;
using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    public abstract int AbilityID { get; }

    public abstract ISkillTargeting TargetingConfig { get; }

    public virtual void ExecuteSkill(PlayerContext ctx)
    {
        Debug.Log("Base ability executed with no specific action.");
    }
}