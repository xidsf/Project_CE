using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    Dictionary<Type, List<ISkillEffect>> skillDic = new(); 

    public void RegisterSkill(ISkillTriggerCondition skillTriggerType, ISkillEffect skill)
    {
        var triggerType = skillTriggerType.GetTriggerType();
        if (skillDic.ContainsKey(triggerType))
        {
            skillDic[triggerType].Add(skill);
        }
        else
        {
            skillDic.Add(triggerType, new List<ISkillEffect> { skill });
        }
    }

}
