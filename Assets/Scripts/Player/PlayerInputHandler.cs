using System;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction ItemASkillAction;
    private InputAction ItemBSkillAction;
    private InputAction pauseAction;
    private InputAction exLifeAction;

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<InputAction.CallbackContext> OnMoveCanceled;
    public event Action<InputAction.CallbackContext> OnLook;
    public event Action<InputAction.CallbackContext> OnItemASkillUsed;
    public event Action<InputAction.CallbackContext> OnItemBSkillUsed;
    public event Action<InputAction.CallbackContext> OnPause;
    public event Action<InputAction.CallbackContext> OnExLifeUsed;

    //event를 담기 위한 Rapper
    private Action<InputAction.CallbackContext> moveActionHandler;
    private Action<InputAction.CallbackContext> moveCanceledActionHandler;
    private Action<InputAction.CallbackContext> lookActionHandler;
    private Action<InputAction.CallbackContext> itemASkillActionHandler;
    private Action<InputAction.CallbackContext> itemBSkillActionHandler;
    private Action<InputAction.CallbackContext> pauseActionHandler;
    private Action<InputAction.CallbackContext> exLifeActionHandler;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        ItemASkillAction = playerInput.actions["SkillA"];
        ItemBSkillAction = playerInput.actions["SkillB"];
        pauseAction = playerInput.actions["Pause"];
        exLifeAction = playerInput.actions["ExLife"];

        moveActionHandler = ctx => OnMove?.Invoke(ctx);
        moveCanceledActionHandler = ctx => OnMoveCanceled?.Invoke(ctx);
        lookActionHandler = ctx => OnLook?.Invoke(ctx);
        itemASkillActionHandler = ctx => OnItemASkillUsed?.Invoke(ctx);
        itemBSkillActionHandler = ctx => OnItemBSkillUsed?.Invoke(ctx);
        pauseActionHandler = ctx => OnPause?.Invoke(ctx);
        exLifeActionHandler = ctx => OnExLifeUsed?.Invoke(ctx);
    }

    private void OnEnable()
    {
        moveAction.performed += moveActionHandler;
        moveAction.canceled += moveCanceledActionHandler;
        lookAction.performed += lookActionHandler;
        ItemASkillAction.performed += itemASkillActionHandler;
        ItemBSkillAction.performed += itemBSkillActionHandler;
        pauseAction.performed += pauseActionHandler;
        exLifeAction.performed += exLifeActionHandler;
    }

    private void OnDisable()
    {
        moveAction.performed -= moveActionHandler;
        moveAction.canceled -= moveCanceledActionHandler;
        lookAction.performed -= lookActionHandler;
        ItemASkillAction.performed -= itemASkillActionHandler;
        ItemBSkillAction.performed -= itemBSkillActionHandler;
        pauseAction.performed -= pauseActionHandler;
        exLifeAction.performed -= exLifeActionHandler;
    }
}