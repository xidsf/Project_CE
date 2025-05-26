using UnityEngine;
using System.Collections;
using System;

public class NormalAttack : MonoBehaviour, IInitializable, IEventSubscriber
{
    [SerializeField] private SkillEffectSO skillEffect;
    private ParticleSystem.MainModule skillEffectMainParticle;
    private PlayerStat stat;
    private Player player;

    private readonly float baseAttackSpeed = 10f;
    private float attackCooldown;
    private float currentAttackCooldown = 0;

    public event Action<StatusContext> OnAttack;
    //public event Action<StatusContext> OnAttackHit;

    public void Initialize(Player player)
    {
        this.player = player;
        player.onExLifeSkillUsed += StopAttackOnTime;
        stat = player.PlayerStat;

        InitParticle();
        CalculateAttackCooldown();
        EnableAttack();
    }

    protected void InitParticle()
    {
        if(skillEffect.AttackParticle != null)
        {
            skillEffectMainParticle = skillEffect.AttackParticle.GetComponent<ParticleSystem>().main;
        }
    }

    protected void CalculateAttackCooldown()
    {
        attackCooldown = baseAttackSpeed / stat.AttackSpeed.GetFinalValue();
        if (currentAttackCooldown > 0)
        {
            AdjustCurrentAttackCooldown();
        }
    }

    protected void AdjustCurrentAttackCooldown()
    {
        float remainRatio = currentAttackCooldown / attackCooldown;
        currentAttackCooldown = attackCooldown * remainRatio;
    }

    protected IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if (currentAttackCooldown <= 0)
            {
                Attack();
                currentAttackCooldown = attackCooldown;
            }
            currentAttackCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    protected void Attack()
    {
        float calcCrit = UnityEngine.Random.Range(0f, 1f);
        float damage;
        bool isCrit;

        if (calcCrit < stat.CritChance.GetFinalValue())
        {
            damage = stat.AttackDamage.GetFinalValue() * stat.CritDamage.GetFinalValue();
            isCrit = true;
        }
        else
        {
            damage = stat.AttackDamage.GetFinalValue();
            isCrit = false;
        }
        StatusContext context = new StatusContext(isCrit, damage, player);
        
        ChangeParticleSize();
        skillEffect.Activate(context);
        OnAttack?.Invoke(context);
        Debug.Log("Attack: Damage: " + damage + " AttSpeed: " + stat.AttackSpeed.GetRoundedFloat());
    }

    private void ChangeParticleSize()
    {
        skillEffectMainParticle.startSize = stat.AttackRange.GetFinalValue() * 0.5f;
    }

    private void StopAttackOnTime(float time)
    {
        DisableAttack();
        Invoke(nameof(EnableAttack), time);
    }

    private void EnableAttack()
    {
        StartCoroutine(AttackCoroutine());
    }

    private void DisableAttack()
    {
        StopAllCoroutines();
    }

    public void SubscribeEvent()
    {
        stat.AttackSpeed.OnStatChanged += CalculateAttackCooldown;
    }

    public void UnsubscribeEvent()
    {
        stat.AttackSpeed.OnStatChanged -= CalculateAttackCooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
