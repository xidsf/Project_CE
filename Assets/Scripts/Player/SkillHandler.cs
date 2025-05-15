using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    Dictionary<Type, List<Skill>> skillDic = new(); 

    public void RegisterSkill(ISkillTriggerType skillTriggerType, Skill skill)
    {
        var triggerType = skillTriggerType.GetTriggerType();
        if (skillDic.ContainsKey(triggerType))
        {
            skillDic[triggerType].Add(skill);
        }
        else
        {
            skillDic.Add(triggerType, new List<Skill> { skill });
        }
    }

}
