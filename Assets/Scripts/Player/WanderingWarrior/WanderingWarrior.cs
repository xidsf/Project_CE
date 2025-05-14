using System.Security.Cryptography;
using UnityEngine;

public class WanderingWarrior : Player
{
    [SerializeField] int cnt = 0;
    [SerializeField] bool isContinuousAttack = false;
    object continuousAttackSource = new object();

    protected override void Awake()
    {
        attackRange = new Stat(1f);
        attackDamage = new Stat(5f);
        attackSpeed = new Stat(5f);
        critChance = new Stat(0.1f);
        critDamage = new Stat(1.5f);
        base.Awake();
    }

    protected override void Attack()
    {
        float calcCrit = Random.Range(0, 1000) / 10;
        float damage;

        if (calcCrit < critChance.GetFinalValue())
        {
            damage = attackDamage.GetFinalValue() * critDamage.GetFinalValue();
        }
        else
        {
            damage = attackDamage.GetFinalValue();
        }

        Debug.Log("Attack: Damage: " + damage + " AttSpeed: " + attackSpeed.GetRoundedFloat());
        
        if (cnt > 0 && !isContinuousAttack)
        {
            attackSpeed.AddModifier(new StatModifier(10, ModifierType.Percent, continuousAttackSource));
            isContinuousAttack = true;
        }
        if(isContinuousAttack && cnt <= 0)
        {
            attackSpeed.RemoveModifier(continuousAttackSource);
            isContinuousAttack = false;
        }
        else if (isContinuousAttack)
        {
            cnt--;
        }
    }

    protected override void UseExLifeSkill()
    {
        //Debug.Log("Use Ex Life Skill");
    }
}
