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
