using UnityEngine;

public abstract class ExLifeSkill : IInitializable
{
    protected PlayerAnimationHandler animHandler;
    protected PlayerStat playerStat;
    protected float skillCooldown;


    public void Initialize(Player player)
    {
        animHandler = player.PlayerAnimationHandler;
        playerStat = player.PlayerStat;
    }

    public abstract void ActiveExLifeSkill();

    protected void KnockbackAllEnemy()
    {
        Debug.Log("KnockbackAllEnemy");
    }
}
