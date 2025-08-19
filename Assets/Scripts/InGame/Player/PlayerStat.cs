public class PlayerStat
{
    public Stat MoveSpeed { get; protected set; }
    public Stat AttackRange { get; protected set; }
    public Stat AttackDamage { get; protected set; }
    public Stat AttackSpeed { get; protected set; }
    public Stat CritChance { get; protected set; }
    public Stat CritDamage { get; protected set; }
    public const float ATTACK_SPEED_MULTIPLIER = 5;

    public PlayerStat (float moveSpeed, float attackRange, float attackDamage, float attackSpeed, float critChange = 0.1f, float critDamage = 1.5f)
    {
        MoveSpeed = new Stat(moveSpeed, 0, 100);
        AttackRange = new Stat(attackRange, 0, 100);
        AttackDamage = new Stat(attackDamage, 1, 10000);
        AttackSpeed = new Stat(attackSpeed, 0.1f, 100);
        CritChance = new Stat(critChange, 0, 100);
        CritDamage = new Stat(critDamage, 0, 1000);
    }

}
