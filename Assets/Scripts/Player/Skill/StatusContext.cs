using UnityEngine;

public class StatusContext
{
    public bool IsCrit { get; set; }
    public float Damage { get; set; }
    public Player Attacker { get; set; }
    public GameObject Target { get; set; }

    public StatusContext(bool crit, float damage, Player attacker, GameObject target = null)
    {
        IsCrit = crit;
        Damage = damage;
        Attacker = attacker;
        Target = target;
    }

}