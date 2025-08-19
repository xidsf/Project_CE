using System.Linq;
using UnityEngine;

public enum MonoTargetingType
{
    None,
    Self,
    NearestEnemy,
}

public enum AreaTargetingType
{
    None,
    Around,
    Point,
    Cone,
}

public interface ISkillTargeting
{
    public MonoTargetingType MonoTargetingType { get; }
    public AreaTargetingType AreaTargetingType { get; }
    public GameObject GetTargetObject(PlayerContext ctx);
    public GameObject[] GetAreaObject(PlayerContext ctx);
}
