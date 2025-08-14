using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Item : MonoBehaviour
{
    public string ItemID { get; protected set; }
    public string ItemName { get; protected set; }
    public float CooldownTime { get; protected set; }
    public float CurrentCooldownTime { get; protected set; }
    public bool HasSkill { get; protected set; }

    public virtual void TryExecuteSkill(InputAction.CallbackContext ctx)
    {
        if(HasSkill && CurrentCooldownTime <= 0)
        {
            ExecuteSkill();
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        CurrentCooldownTime = CooldownTime;
        while (CurrentCooldownTime > 0)
        {
            CurrentCooldownTime -= Time.deltaTime;
            yield return null;
        }
        CurrentCooldownTime = 0f;
    }

    protected abstract void ExecuteSkill();
}
