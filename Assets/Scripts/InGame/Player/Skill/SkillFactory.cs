using System.Collections.Generic;

public class SkillFactory : Singleton<SkillFactory>
{
    public Player Player { get; private set; }
    public List<Skill> Skills = new List<Skill>();

    public BaseAbility[] testAbility;

    protected override void Init()
    {
        base.Init();
        Player = FindAnyObjectByType<Player>();
        if (Player == null)
        {
            Logger.LogError("Player not found in the scene. Please ensure a Player object is present.");
        }
    }

    private void Start()
    {
        //CreateSkill(new EveryNAttackCondition(Player, 5), testAbility[0]);
        CreateSkill(new AttackChanceCondition(Player, 0.5f), testAbility[1]);
        CreateSkill(new AttackChanceCondition(Player, 0.4f), testAbility[1]);
    }

    public Skill CreateSkill(SkillCondition condition, BaseAbility ability)
    {
        long serialNumber = SerialNumberGenerator.GenerateSerialNumber();
        Skill skill = new Skill(serialNumber, condition, ability);
        Skills.Add(skill);
        return skill;
    }
}
