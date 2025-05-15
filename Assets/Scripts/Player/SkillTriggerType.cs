using System;

public interface ISkillTriggerType
{
    Type GetTriggerType();
}

public interface IAttackChanceType : ISkillTriggerType
{
    float Chance { get; protected set; }
}

public interface IKillEnemyType : ISkillTriggerType
{
    int Count { get; protected set; }
}

public interface ITimeSpentType : ISkillTriggerType
{
    float Time { get; protected set; }
}

public interface ICritAttackChanceType : ISkillTriggerType
{
    float Change { get; protected set; }
}

public interface IAttackCountType : ISkillTriggerType
{
    int Count { get; protected set; }
}


