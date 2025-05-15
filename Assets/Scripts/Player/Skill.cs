using UnityEngine;

public class Skill : MonoBehaviour
{
    public ISkillCondition SkillCondition { get; }
    public ISkillEffect SkillEffect { get; }

    public void RegisterToEvent()
    {
        EventConditionManager.Instance.RegisterSkill(this);
    }
}
