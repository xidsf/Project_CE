using UnityEngine;

public class WanderingWarrior : Player
{
    protected override void Awake()
    {
        base.Awake();
        attackRange = 1f;
        attackDamage = 5f;
        AttackSpeed = 20f;
    }

    protected override void Attack()
    {
        Debug.Log("Attack");
    }

    protected override void UseExLifeSkill()
    {
        Debug.Log("Use Ex Life Skill");
    }
}
