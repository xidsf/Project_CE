using UnityEngine;

public class SelfTargeting : ISkillTargeting
{
    MonoTargetingType ISkillTargeting.MonoTargetingType => MonoTargetingType.Self;
    AreaTargetingType ISkillTargeting.AreaTargetingType => AreaTargetingType.None;

    public GameObject GetTargetObject(PlayerContext ctx)
    {
        return ctx.Caster.gameObject;
    }
    public GameObject[] GetAreaObject(PlayerContext ctx)
    {
        Logger.LogWarning("SelfTargeting does not support area targeting.");
        return null;
    }
}