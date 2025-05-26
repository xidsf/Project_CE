using UnityEngine;

public abstract class SkillEffectSO : ScriptableObject, ISkillEffect
{
    protected static readonly string damageableString = "Damageable";

    [SerializeField] protected GameObject attackParticle;
    public GameObject AttackParticle { get => attackParticle; }
    protected ParticleSystem.MainModule mainParticle;

    public abstract void Activate(StatusContext context);
}
