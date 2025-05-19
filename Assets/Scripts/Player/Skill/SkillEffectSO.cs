using UnityEngine;

public abstract class SkillEffectSO : ScriptableObject, ISkillEffect
{
    protected static readonly string damageableString = "Damageable";
    public abstract void Activate(StatusContext context);
    public GameObject attackParticle;
    protected ParticleSystem particleSys;

    public void InitializeParticleSystem()
    {
        if (attackParticle != null)
        {
            particleSys = attackParticle.GetComponent<ParticleSystem>();
        }
    }
}
