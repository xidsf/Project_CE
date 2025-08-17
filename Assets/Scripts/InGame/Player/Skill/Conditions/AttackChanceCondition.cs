public class AttackChanceCondition : BaseCondition
{
    public float ConditionChance = 0.1f; // 10% chance to trigger the skill

    public AttackChanceCondition()
    {
        ConditionType = SkillCondition.AttackChance;
    }

    public void CheckConditionMet()
    {
        var randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue <= ConditionChance)
        {
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
