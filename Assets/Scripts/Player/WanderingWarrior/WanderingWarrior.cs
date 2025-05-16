using UnityEngine;

public class WanderingWarrior : Player
{
    [SerializeField] int cnt = 0;
    [SerializeField] bool isContinuousAttack = false;

    protected override void Awake()
    {
        PlayerStat = new PlayerStat(3f, 5f, 5f, 5f);
        base.Awake();
    }



    //protected override void Attack()
    //{
    //    float calcCrit = Random.Range(0, 1000) / 10;
    //    float damage;

    //    if (calcCrit < PlayerStat.CritChance.GetFinalValue())
    //    {
    //        damage = PlayerStat.AttackDamage.GetFinalValue() * PlayerStat.CritDamage.GetFinalValue();
    //    }
    //    else
    //    {
    //        damage = PlayerStat.AttackDamage.GetFinalValue();
    //    }

    //    Debug.Log("Attack: Damage: " + damage + " AttSpeed: " + PlayerStat.AttackSpeed.GetRoundedFloat());
        
    //    if (cnt > 0 && !isContinuousAttack)
    //    {
    //        PlayerStat.AttackSpeed.AddModifier(new StatModifier(10, ModifierType.Percent, continuousAttackSource));
    //        isContinuousAttack = true;
    //    }
    //    if(isContinuousAttack && cnt <= 0)
    //    {
    //        PlayerStat.AttackSpeed.RemoveModifier(continuousAttackSource);
    //        isContinuousAttack = false;
    //    }
    //    else if (isContinuousAttack)
    //    {
    //        cnt--;
    //    }
    //}
}
