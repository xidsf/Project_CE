public class EveryNAttackCondition : BaseCondition
{
    public int ConditionInterval = 5;
    private int attackCount = 0;

    public EveryNAttackCondition()
    {
        ConditionType = SkillCondition.EveryNAttack;
    }

    public void CheckConditionMet()
    {
        attackCount++;
        if(attackCount >= ConditionInterval)
        {
            attackCount = 0;
            OnConditionMet?.Invoke();
        }
    }
    //TODO: 현재 플레이어의 OnAttack 이벤트가 없어서 나중에 연결해야함
    public override void OnRegister()
    {
        //InGameManager.Instance.Player.OnAttack += CheckConditionMet;
    }

    public override void OnUnregister()
    {
        //InGameManager.Instance.Player.OnAttack -= CheckConditionMet;
    }
}
