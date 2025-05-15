using System.Security.Cryptography;
using UnityEngine;

public class WanderingWarrior : Player
{
    [SerializeField] int cnt = 0;
    [SerializeField] bool isContinuousAttack = false;
    object continuousAttackSource = new object();

    protected override void Awake()
    {
        MoveSpeed = new Stat(3f);
        AttackRange = new Stat(1f);
        AttackDamage = new Stat(5f);
        AttackSpeed = new Stat(5f);
        CritChance = new Stat(0.1f);
        CritDamage = new Stat(1.5f);
        base.Awake();
    }

    protected override void Attack()
    {
        float calcCrit = Random.Range(0, 1000) / 10;
        float damage;

        if (calcCrit < CritChance.GetFinalValue())
        {
            damage = AttackDamage.GetFinalValue() * CritDamage.GetFinalValue();
        }
        else
        {
            damage = AttackDamage.GetFinalValue();
        }

        Debug.Log("Attack: Damage: " + damage + " AttSpeed: " + AttackSpeed.GetRoundedFloat());
        
        if (cnt > 0 && !isContinuousAttack)
        {
            AttackSpeed.AddModifier(new StatModifier(10, ModifierType.Percent, continuousAttackSource));
            isContinuousAttack = true;
        }
        if(isContinuousAttack && cnt <= 0)
        {
            AttackSpeed.RemoveModifier(continuousAttackSource);
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
