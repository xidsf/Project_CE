using UnityEngine;


public abstract class Skill : MonoBehaviour
{
    protected string skillName;
    protected float coolDownTime = 0f;
    protected float currentcoolDownTime = 0f;
    protected ISkillTriggerType skillTriggerType;

    protected abstract void Effect(GameObject applyObj);
    protected abstract void SetSkillTriggerType(ISkillTriggerType triggerType);
    
}
