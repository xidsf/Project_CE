using UnityEngine;

public class StatusContext
{
    public bool IsCrit { get; set; }
    public float Damage { get; set; }
    public GameObject Attacker { get; set; }
    public GameObject Target { get; set; }

    public StatusContext(bool crit, float damage, GameObject attacker, GameObject target = null)
    {
        IsCrit = crit;
        Damage = damage;
        Attacker = attacker;
        Target = target;
    }

}