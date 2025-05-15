using UnityEngine;

public interface ISkillEffect
{
    public GameObject TargetObject { get; }

    public abstract void Effect();
}