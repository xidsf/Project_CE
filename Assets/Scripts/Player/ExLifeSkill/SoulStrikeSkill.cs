using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ExLifeSkill/SoulStrikeSkill")]
public class SoulStrikeSkill : ExLifeSkillSO
{
    public override IEnumerator ExLifeSkillCoroutine(PlayerAnimationHandler animHandler)
    {
        float parryingAnimLength = animHandler.ParryingAnimLength;
        float exLifeSkillLength = animHandler.ExLifeSkillLength;
        yield return new WaitForSeconds(startupTime);
        animHandler.TriggerExLifeSkill();
        Debug.Log(parryingAnimLength);
        yield return new WaitForSeconds(parryingAnimLength);
        Debug.Log(exLifeSkillLength);
        yield return new WaitForSeconds(exLifeSkillLength);
        UseExLifeSkill();
        KnockbackAllEnemy();
        yield return new WaitForSeconds(recoveryTime);
    }

    protected override void UseExLifeSkill()
    {
        Debug.Log("Soul Strike Skill Used");
    }
}
