using System;

public class ElapsedTimeConditionSkill : BaseSkillCondition, IResetable
{
    public float TimeCondition { get; }
    //private float currentTime;

    public ElapsedTimeConditionSkill(float time)
    {
        TimeCondition = time;
        //currentTime = 0;
    }

    public override Type GetConditionType()
    {
        return typeof(ElapsedTimeConditionSkill);
    }

    public void Reset()
    {
        //currentTime = 0;
    }

    public override void Trigger(StatusContext onConditionMet)
    {
        //currentTime += TimeScheduleManager.Instance.TickRate;
        //if(currentTime >= TimeCondition)
        //{
        //    currentTime = 0;
        //    onMet?.Invoke(null);
        //}
    }

}
