using System.Collections;
using UnityEngine;

public class ExLifeSkillHandler : MonoBehaviour, IInitializable
{
    public float currentSkillCooldown { get; private set; }
    public int ExLifeSkillCount { get; private set; }

    ExLifeSkillSO exLifeSkillSO;
    PlayerAnimationHandler playerAnimationHandler;

    public void Initialize(Player player)
    {
        exLifeSkillSO = player.ExLifeSkillSO;
        playerAnimationHandler = player.PlayerAnimationHandler;
        currentSkillCooldown = 0f;
        ExLifeSkillCount = 3;
    }

    public bool TryUseExLiseSkill()
    {
        if (ExLifeSkillCount > 0 && currentSkillCooldown <= 0)
        {
            ExLifeSkillCount--;
            currentSkillCooldown = exLifeSkillSO.exLifeSkillCooldown;
            StartCoroutine(exLifeSkillSO.ExLifeSkillCoroutine(playerAnimationHandler));
            StartCoroutine(CalcSkillCoolDown());
            return true;
        }
        else return false;
    }

    private IEnumerator CalcSkillCoolDown()
    {
        while (currentSkillCooldown > 0)
        {
            currentSkillCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    public void AddExLifeSkill()
    {
        ExLifeSkillCount++;
    }
}
