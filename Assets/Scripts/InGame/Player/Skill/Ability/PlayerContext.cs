using UnityEngine;

public enum PlayerContextType
{
    None,
    NormalAttack,
    CriticalNormalAttack,
    ElapsedTime,
}

public class PlayerContext
{
    public Player Caster { get; set; }
    public Vector3 MousePosition { get; set; }
    public Quaternion TargetingRotation { get; set; }
    public PlayerContextType ContextType { get; set; } = PlayerContextType.None;

    public PlayerContext(Player caster, Vector3 position, Quaternion lookRotation, PlayerContextType contextEmittionType)
    {
        Caster = caster;
        MousePosition = position;
        TargetingRotation = lookRotation;
        ContextType = contextEmittionType;
    }
}

public class WarriorContext : PlayerContext
{
    public bool IsContinuousAttack { get; set; } = false;
    public WarriorContext(Player caster, Vector3 position, Quaternion lookRotation, PlayerContextType contextEmittionType, bool IsContinuousAttack = false) : base(caster, position, lookRotation, contextEmittionType)
    {
        this.IsContinuousAttack = IsContinuousAttack;
    }
}
