using System;
using UnityEngine;

public interface IAbility
{
    public ISkillTargeting TargetingConfig { get; }
    public void ExecuteSkill(PlayerContext ctx);
}

public abstract class BaseAbility : ScriptableObject, IAbility
{
    public abstract ISkillTargeting TargetingConfig { get; }

    public virtual void ExecuteSkill(PlayerContext ctx)
    {
        Debug.Log("Base ability executed with no specific action.");
    }
}