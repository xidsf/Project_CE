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
    //TODO: ���� �÷��̾��� OnAttack �̺�Ʈ�� ��� ���߿� �����ؾ���
    public override void OnRegister()
    {
        //InGameManager.Instance.Player.OnAttack += CheckConditionMet;
    }

    public override void OnUnregister()
    {
        //InGameManager.Instance.Player.OnAttack -= CheckConditionMet;
    }
}
