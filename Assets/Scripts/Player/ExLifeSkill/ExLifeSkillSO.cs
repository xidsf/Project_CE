using System.Collections;
using UnityEngine;

public abstract class ExLifeSkillSO : ScriptableObject
{
    public readonly float startupTime = 0.5f;
    public readonly float recoveryTime = 0.5f;
    public float sumOfCastingTime { get; protected set; } = 1f;

    public readonly float exLifeSkillCooldown = 5f;

    public abstract IEnumerator ExLifeSkillCoroutine(PlayerAnimationHandler animHandler);

    protected abstract void UseExLifeSkill();

    protected void KnockbackAllEnemy()
    {
        Debug.Log("KnockbackAllEnemy");
    }

}
