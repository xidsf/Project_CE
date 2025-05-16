public class PlayerStat
{
    public Stat MoveSpeed { get; protected set; }
    public Stat AttackRange { get; protected set; }
    public Stat AttackDamage { get; protected set; }
    public Stat AttackSpeed { get; protected set; }
    public Stat CritChance { get; protected set; }
    public Stat CritDamage { get; protected set; }

    public PlayerStat (float moveSpeed, float attackRange, float attackDamage, float attackSpeed, float critChange = 0.1f, float critDamage = 1.5f)
    {
        MoveSpeed = new Stat(moveSpeed);
        AttackRange = new Stat(attackRange);
        AttackDamage = new Stat(attackDamage);
        AttackSpeed = new Stat(attackSpeed);
        CritChance = new Stat(critChange);
        CritDamage = new Stat(critDamage);
    }

}
