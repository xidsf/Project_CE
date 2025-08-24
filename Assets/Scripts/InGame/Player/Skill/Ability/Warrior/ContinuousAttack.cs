using UnityEngine;

[CreateAssetMenu(fileName = "ContinuousAttack", menuName = "ScriptableObjects/Abilities/Warrior/ContinuousAttack")]
public class ContinuousAttack : BaseAbility
{
    public override int AbilityID { get => 2002; }
    private ISkillTargeting _targeting = new SelfTargeting();
    public override ISkillTargeting TargetingConfig => _targeting;

    public override void ExecuteSkill(PlayerContext ctx)
    {
        var context = ctx as WarriorContext;
        var caster = context?.Caster as Warrior;
        if (context == null || caster == null)
        {
            return;
        }
        
        if(!context.IsContinuousAttack)
        {
            caster.IncrementContinuousAttackCount(1);
        }
    }
}
