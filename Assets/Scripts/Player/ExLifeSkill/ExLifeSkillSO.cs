using System.Collections;
using UnityEngine;

public abstract class ExLifeSkillSO : ScriptableObject
{
    public float sumOfCastingTime { get; protected set; } = 1f;

    public readonly float exLifeSkillCooldown = 5f;

    public abstract IEnumerator ExLifeSkillCoroutine(float parryingAnimTime, float exLifeSkillCastTime);

    protected abstract void UseExLifeSkill();

    protected virtual void KnockbackAllEnemy()
    {
        Debug.Log("KnockbackAllEnemy");
    }

}
