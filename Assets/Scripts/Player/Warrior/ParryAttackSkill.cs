using UnityEngine;

public class ParryAttackSkill : ExLifeSkill
{
    public override void ActiveExLifeSkill()
    {
        animHandler.TriggerExLifeSkill();
    }
}
