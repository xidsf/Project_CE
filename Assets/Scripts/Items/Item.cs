using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Item : MonoBehaviour
{
    public string itemName { get; protected set; }
    public float cooldownTime { get; protected set; }
    public float currentCooldownTime { get; protected set; }

    public virtual void TryExecuteSkill(InputAction.CallbackContext ctx)
    {
        if(currentCooldownTime <= 0)
        {
            ExecuteSkill();
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        currentCooldownTime = cooldownTime;
        while (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
            yield return null;
        }
        currentCooldownTime = 0f;
    }

    protected abstract void ExecuteSkill();
}
