using System;
using UnityEngine;
using System.Collections;

public class TimeSpentCondition : MonoBehaviour, ISkillTriggerCondition
{
    protected float timeCondition;
    private float currentTime;
    public static float TickRate => 0.1f;
    private ISkillEffect skillInstance;
    public ISkillEffect SkillInstance
    {
        get => skillInstance;
        set => skillInstance = value;
    }

    public TimeSpentCondition(float time, ISkillEffect skill)
    {
        timeCondition = time;
        skillInstance = skill;
        currentTime = 0;
    }

    private IEnumerator TimeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(TickRate);
            currentTime += TickRate;
            if (currentTime >= timeCondition)
            {
                skillInstance.Effect();
                currentTime = 0;
            }
        }
    }

    public Type GetTriggerType()
    {
        return typeof(TimeSpentCondition);
    }

    public void EvaluateCondition()
    {
        StartCoroutine(TimeCoroutine());
    }
}
