using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ExLifeSkill/SoulStrikeSkill")]
public class SoulStrikeSkill : ExLifeSkillSO
{
    public override IEnumerator ExLifeSkillCoroutine(float parryingAnimTime, float exLifeSkillCastTime)
    {
        Debug.Log(parryingAnimTime);
        yield return new WaitForSeconds(parryingAnimTime);
        Debug.Log(exLifeSkillCastTime);
        yield return new WaitForSeconds(exLifeSkillCastTime);
        UseExLifeSkill();
        KnockbackAllEnemy();
    }

    protected override void UseExLifeSkill()
    {
        Debug.Log("Soul Strike Skill Used");
    }
}
