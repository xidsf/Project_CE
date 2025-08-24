public class PlayerStat
{
    public Stat MoveSpeed { get; protected set; }
    public Stat AttackRange { get; protected set; }
    public Stat AttackDamage { get; protected set; }
    public Stat AttackSpeed { get; protected set; }
    public Stat CriticalChance { get; protected set; }
    public Stat CriticalDamage { get; protected set; }
    public Stat HealthPoint { get; protected set; }
    public const float ATTACK_SPEED_MULTIPLIER = 5;

    public PlayerStat (float moveSpeed, float attackRange, float attackDamage, float attackSpeed, float critChange = 0.1f, float critDamage = 1.5f, float healthPoint = 100)
    {
        MoveSpeed = new Stat(moveSpeed, 0, 100);
        AttackRange = new Stat(attackRange, 0, 100);
        AttackDamage = new Stat(attackDamage, 1, 10000);
        AttackSpeed = new Stat(attackSpeed, 0.1f, 100);
        CriticalChance = new Stat(critChange, 0, 100);
        CriticalDamage = new Stat(critDamage, 0, 1000);
        HealthPoint = new Stat(healthPoint, 0, 10000);
    }
}
