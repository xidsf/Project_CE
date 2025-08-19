using UnityEngine;

[CreateAssetMenu(fileName = "Slash", menuName = "ScriptableObjects/Abilities/Warrior/Slash")]
public class Slash : BaseAbility
{
    public GameObject SlashObject;
    private ISkillTargeting _targeting = new SelfTargeting();
    public override ISkillTargeting TargetingConfig => _targeting;
    public float skillOffset = -90f;

    public override void ExecuteSkill(PlayerContext ctx)
    {
        if(ctx.Caster as Warrior == null)
        {
            return;
        }
        var position = ctx.Caster.transform.position;
        var rotation = Quaternion.Euler(ctx.TargetingRotation.eulerAngles.x, ctx.TargetingRotation.eulerAngles.y, ctx.TargetingRotation.eulerAngles.z + skillOffset);

        Instantiate(SlashObject, position, rotation);
    }
}
