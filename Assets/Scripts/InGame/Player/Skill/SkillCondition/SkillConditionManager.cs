using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConditionManager : Singleton<SkillConditionManager>
{
    public static readonly float TickRate = 0.1f;
    public Player player;

    private readonly Dictionary<Type, List<ISkillCondition>> conditionDict = new();
    private readonly Dictionary<Type, Action> resetActionDict;


    private void Start()
    {
        StartCoroutine(ElapsedTimeCheck());
    }

    public void RegisterSkill(ConditionSkill skill)
    {
        Type conditionType = skill.SkillCondition.GetConditionType();
        if (conditionDict.ContainsKey(conditionType))
        {
            conditionDict[conditionType].Add(skill.SkillCondition);
        }
        else
        {
            conditionDict.Add(conditionType, new List<ISkillCondition> { skill.SkillCondition });
        }
        if(skill.SkillCondition is IResetable resetableSkill)
        {
            RegisterResetableEvent(conditionType, resetableSkill);
        }

        skill.SkillCondition.OnRegister(context => skill.SkillEffect.Activate(player, context));
    }

    private void RegisterResetableEvent(Type type, IResetable skillTrigger)
    {
        if (resetActionDict.ContainsKey(type))
        {
            resetActionDict[type] += skillTrigger.Reset;
        }
        else
        {
            resetActionDict.Add(type, skillTrigger.Reset);
        }
    }

    private void OnDisable()
    {
        foreach(var resetableSkill in resetActionDict.Keys)
        {
            if (resetActionDict.ContainsKey(resetableSkill))
            {
                resetActionDict[resetableSkill] -= ((IResetable)resetableSkill).Reset;
            }
        }
    }

    IEnumerator ElapsedTimeCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(TickRate);
            if (!conditionDict.ContainsKey(typeof(ElapsedTimeConditionSkill))) continue;
            foreach (var triggerList in conditionDict[typeof(ElapsedTimeConditionSkill)])
            {
                triggerList.Trigger(null);
            }
        }
    }

}
