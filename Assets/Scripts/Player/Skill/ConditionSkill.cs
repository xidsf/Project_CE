using UnityEngine;

public class ConditionSkill : MonoBehaviour
{
    public ISkillCondition SkillCondition { get; }
    public ISkillEffect SkillEffect { get; }

    public void RegisterToEvent()
    {
        SkillConditionManager.Instance.RegisterSkill(this);
    }
}
