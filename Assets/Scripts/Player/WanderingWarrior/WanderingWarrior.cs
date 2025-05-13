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
        base.Awake();
    }

    private void Update()
    {
        
    }

    protected override void Attack()
    {
        Debug.Log("Attack: Damage: " + attackDamage.GetFinalValue() + " AttSpeed: " + attackSpeed.GetRoundedFloat());
        
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
