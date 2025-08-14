using UnityEngine;
using System.Collections;
using System;

public class NormalAttack : MonoBehaviour, IInitializable, IEventSubscriber
{
    [SerializeField] private SkillEffectSO skillEffect;
    private PlayerStat stat;
    private Player player;
    private ExLifeSkillHandler exLifeSkillHandler;

    private readonly float baseAttackSpeed = 10f;
    private float attackCooldown;
    private float currentAttackCooldown = 0;

    public event Action<StatusContext> OnAttack;
    //public event Action<StatusContext> OnAttackHit;

    public void Initialize(Player player)
    {
        this.player = player;
        stat = player.PlayerStat;
        exLifeSkillHandler = player.ExLifeSkillHandler;
        CalculateAttackCooldown();
        EnableNormalAttack();
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
        
        skillEffect.Activate(player, context);
        OnAttack?.Invoke(context);
        Debug.Log("Attack: Damage: " + damage + " AttSpeed: " + stat.AttackSpeed.GetRoundedFloat());
    }


    private void StopNormalAttackOnTime(float time)
    {
        DisableNormalAttack();
        Invoke(nameof(EnableNormalAttack), time);
    }

    private void EnableNormalAttack()
    {
        StartCoroutine(AttackCoroutine());
    }

    private void DisableNormalAttack()
    {
        StopAllCoroutines();
    }

    public void SubscribeEvent()
    {
        stat.AttackSpeed.OnStatChanged += CalculateAttackCooldown;
        exLifeSkillHandler.OnExLifeSkillUsedSuccess += StopNormalAttackOnTime;
    }

    public void UnsubscribeEvent()
    {
        stat.AttackSpeed.OnStatChanged -= CalculateAttackCooldown;
        exLifeSkillHandler.OnExLifeSkillUsedSuccess -= StopNormalAttackOnTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
