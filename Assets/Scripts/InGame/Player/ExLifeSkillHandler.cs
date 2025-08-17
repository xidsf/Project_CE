using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExLifeSkillHandler : MonoBehaviour, IInitializable, IEventSubscriber
{
    public float currentSkillCooldown { get; private set; }
    public int ExLifeSkillCount { get; private set; }

    ExLifeSkillSO exLifeSkillSO;
    //PlayerAnimationHandler playerAnimationHandler;

    public readonly float startupTime = 0.5f;

    //public event Action<float> OnExLifeSkillUsedSuccess;

    public void Initialize(Player player)
    {
        currentSkillCooldown = 0f;
        ExLifeSkillCount = 3;
    }

    public void TryUseExLiseSkill(InputAction.CallbackContext ctx)
    {
        if (ExLifeSkillCount > 0 && currentSkillCooldown <= 0)
        {
            StartCoroutine(UseExLifeSkill());
            //OnExLifeSkillUsedSuccess?.Invoke(playerAnimationHandler.GetAnimRunningTime());
        }
    }

    IEnumerator UseExLifeSkill()
    {
        //OnExLifeSkillUsedSuccess.Invoke(playerAnimationHandler.GetAnimRunningTime());

        ExLifeSkillCount--;
        currentSkillCooldown = exLifeSkillSO.exLifeSkillCooldown;
        StartCoroutine(CalcSkillCoolDown());

        yield return new WaitForSeconds(startupTime);

        //StartCoroutine(exLifeSkillSO.ExLifeSkillCoroutine(
        //    playerAnimationHandler.ParryingAnimLength, playerAnimationHandler.ExLifeSkillLength));
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

    public void SubscribeEvent()
    {
        
    }

    public void UnsubscribeEvent()
    {
        
    }
}
