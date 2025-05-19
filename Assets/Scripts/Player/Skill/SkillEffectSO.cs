using UnityEngine;

public abstract class SkillEffectSO : ScriptableObject, ISkillEffect
{
    protected static readonly string damageableString = "Damageable";
    public abstract void Activate(StatusContext context);
    public GameObject attackParticle;
    private float? _particleDuration;
    public float ParticleDuration
    {
        get
        {
            if (_particleDuration == null && attackParticle != null)
            {
                var ps = attackParticle.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    _particleDuration = ps.main.duration;
                }
                else
                {
                    _particleDuration = 0f;
                }
            }
            return _particleDuration ?? 0f;
        }
    }
}
