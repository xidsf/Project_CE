using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public abstract class Item : MonoBehaviour
{
    ItemData m_ItemData;


    //public virtual void TryExecuteSkill(InputAction.CallbackContext ctx)
    //{
    //    if(HasSkill && CurrentCooldownTime <= 0)
    //    {
    //        ExecuteSkill();
    //        StartCoroutine(CooldownCoroutine());
    //    }
    //}

    //private IEnumerator CooldownCoroutine()
    //{
    //    CurrentCooldownTime = CooldownTime;
    //    while (CurrentCooldownTime > 0)
    //    {
    //        CurrentCooldownTime -= Time.deltaTime;
    //        yield return null;
    //    }
    //    CurrentCooldownTime = 0f;
    //}

    protected abstract void ExecuteSkill();
}
