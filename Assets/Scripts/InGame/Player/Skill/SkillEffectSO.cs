using UnityEngine;

public abstract class SkillEffectSO : ScriptableObject, ISkillEffect
{
    protected static readonly string damageableString = "Damageable";

    [SerializeField] protected GameObject attackParticle;
    [SerializeField] protected GameObject hitParticle;

    public abstract void Activate(Player player, StatusContext context);
}
